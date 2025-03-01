using System.Diagnostics;
using System.DirectoryServices;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace MediaTools
{
    public partial class MainForm : Form
    {
        private readonly bool _missingKeyPaths;
        private readonly string _configPath;
        private readonly CacheHandler _cacheHandler;
        private readonly FileUtils _fileUtils;
        private ColumnName _columnName = ColumnName.Duration;
        private List<MediaFileEntry> _fileEntries = [];
        private Point _contextMenuLocation;
        private SortDirection _sortOrder = SortDirection.Ascending;
        private bool _isConsoleShown = true;
        private bool _isDirty;
        private bool _isSearchOpen;
        private bool _mediaListUpdatingBlocked;
        private bool _needsRestart;
        private bool _playlistBuilderBlocked;
        private string _lastTrashedFilePath = "";

        public MainForm()
        {
            IconModifier.SetFormIcon(this);

            var settings = Program.appSettings;

            var mediaDirectory = FileUtils.FullyResolvePath(settings.MediaDirectory);
            if (Directory.Exists(mediaDirectory))
            {
                _fileUtils = new FileUtils(mediaDirectory);
                // The cache file will be held in the root media directory.
                _cacheHandler = new CacheHandler(Path.Combine(mediaDirectory, "cache.dat"));
                _isDirty = !_cacheHandler.Exists();
            }
            else
            {
                _fileUtils = new FileUtils(AppContext.BaseDirectory);
                _cacheHandler = new CacheHandler(AppContext.BaseDirectory);
                _missingKeyPaths = true;
            }

            var ytDlpPath = FileUtils.FullyResolvePath(settings.YtDlpPath);
            if (File.Exists(ytDlpPath))
            {
                var ytDlpDirectory = new FileInfo(ytDlpPath).DirectoryName!;
                if (!string.IsNullOrWhiteSpace(ytDlpDirectory))
                {
                    _configPath = Path.Combine(ytDlpDirectory, "yt-dlp.conf");
                }
                else
                {
                    // This will never be used because we will require the data is updated
                    // and the app restarted before it will be used.
                    _configPath = AppContext.BaseDirectory;
                    _missingKeyPaths = true;
                }
            }
            else
            {
                // This will never be used because we will require the data is updated
                // and the app restarted before it will be used.
                _configPath = AppContext.BaseDirectory;
                _missingKeyPaths = true;
            }

            InitializeComponent();

            if (_missingKeyPaths)
            {
                // The paths will need to be updated and the application restarted
                // before normal functions can continue.
                DisabledFunctionsDueToMissingComponents();
            }

            if (settings.RememberDownloadOptions)
            {
                RestoreDownloadOptions();
            }
            else
            {
                optionResolution.SelectedIndex = 5;
                optionDownloadRateLimitType.SelectedIndex = 1;
            }

            actionOnComplete.SelectedIndex = 1;
            toolStripStatusLabel1.Text = "";

            SetFoldersColumnVisibility(settings.ShowFolders);

            Interop.AllocConsole();
            Interop.SetConsoleMode();

            Console.SetOut(new StreamWriter(Console.OpenStandardOutput()) { AutoFlush = true });
            Console.SetError(new StreamWriter(Console.OpenStandardError()) { AutoFlush = true });
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            Program.appSettings = AppSettings.ReadSettings();

            if (_missingKeyPaths)
            {
                MessageBox.Show(
                    DisplayBuilders.ErrorMissingKeyPaths.BuildPlain(),
                    DisplayBuilders.ErrorMissingKeyPathsTitle.BuildPlain(),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );

                return;
            }

            LoadCachedData();
            await UpdateMediaTable(true);
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            UpdateDownloadOptions();
            Program.appSettings.WriteSettings();

            WriteCacheData();
        }

        private void MediaFilesTable_CellContentDoubleClick(
            object sender,
            DataGridViewCellEventArgs e
        )
        {
            if (e.RowIndex < 0)
            {
                return;
            }

            var file = mediaFilesTable.Rows[e.RowIndex].Cells["FullPath"].Value!.ToString();
            ProcessUtils.RunMediaPlayer(file);
        }

        private async void MediaFilesTable_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e)
            {
                case { Control: true, KeyCode: Keys.Z }:
                    if (_lastTrashedFilePath.Length == 0)
                    {
                        break;
                    }

                    if (FileUtils.TryUntrashPath(_lastTrashedFilePath))
                    {
                        UpdateStatus(DisplayBuilders.SuccessRestoreFile, [_lastTrashedFilePath]);
                        await UpdateMediaTable(false);
                    }
                    else
                    {
                        UpdateStatus(DisplayBuilders.ErrorRestoreFile, [_lastTrashedFilePath]);
                    }

                    _lastTrashedFilePath = "";
                    break;
            }
        }

        private async void ReloadMediaFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            await UpdateMediaTable(false);
        }

        private async void Download_Click(object sender, EventArgs e)
        {
            var urls = UrlProcessor.BuildDownloadUrlList(downloadIds.Lines);
            if (urls.Length == 0)
            {
                UpdateStatus(DisplayBuilders.ErrorNoValidUrls);
                return;
            }

            SetOptionsEnabled(false);
            SetDownloadsEnabled(false);

            for (var i = 0; i < urls.Length; i++)
            {
                FileUtils.EnsureTempExists();

                var downloadType = urls[i].DownloadType;

                UpdateStatus(DisplayBuilders.InfoAttemptWriteConfig, [], true);
                SetupConfigFile(downloadType);
                UpdateStatus(DisplayBuilders.SuccessConfigWrite, [], true);

                object[] args = [i + 1, urls.Length];
                UpdateStatus(DisplayBuilders.InfoAttemptDownload, args);
                await ProcessUtils.RunDownloader(urls[i].Url, FileUtils.GetTempPath());
                UpdateStatus(DisplayBuilders.SuccessDownload, args);

                // We could move all the files at the end instead, but if
                // something went wrong then some of the files would be stuck
                // in the temporary folder.
                // It seems to make more sense to move them as needed.
                // We won't update the media list until the end to avoid
                // the overhead and await locking.
                UpdateStatus(DisplayBuilders.InfoAttemptMoveDownloads);
                _fileUtils.MoveTempFiles(downloadFolder.Text);
                UpdateStatus(DisplayBuilders.SuccessMoveDownloads);
            }

            // We don't need to delete the config file each time, we can overwrite
            // it and just delete it after the tasks have ended.
            File.Delete(_configPath);

            await UpdateMediaTable(true);

            SetDownloadsEnabled(true);
            SetOptionsEnabled(true);

            switch (actionOnComplete.SelectedItem)
            {
                case "0":
                    // Do nothing.
                    break;
                case 1:
                    // Close the application.
                    Close();
                    break;
                case 2:
                    // Shutdown.
                    UpdateStatus(DisplayBuilders.InfoShuttingDownComputer);
                    Shutdown();
                    break;
            }
        }

        private void OptionAudioOnly_CheckedChanged(object sender, EventArgs e)
        {
            if (optionAddThumbnails.Checked)
            {
                optionAddThumbnails.Checked = false;
            }

            optionAddThumbnails.Enabled = !optionAddThumbnails.Checked;
        }

        private void OptionLogin_CheckedChanged(object sender, EventArgs e)
        {
            optionMarkWatched.Enabled = !optionCookieLogin.Checked;
        }

        private void ShowConsoleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToggleConsole();
        }

        private void DeleteToolStripMenuItem_Click(object? sender, EventArgs e)
        {
            HandleDeleteFile(false);
        }

        private void TrashToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HandleDeleteFile(true);
        }

        private void RenameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HandleFileRename();
        }

        private void MediaFilesTable_MouseClick(object sender, MouseEventArgs e)
        {
            var hit = mediaFilesTable.HitTest(e.X, e.Y);
            var hitType = hit.Type;

            if (e.Button != MouseButtons.Right)
            {
                return;
            }

            if (hitType == DataGridViewHitTestType.ColumnHeader)
            {
                return;
            }

            _contextMenuLocation = e.Location;
            contextMenuStrip.Show(mediaFilesTable, e.Location);
        }

        private async void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e)
            {
                case { Control: true, KeyCode: Keys.F }:
                    {
                        // We don't want to have multiple search forms open at once.
                        if (_isSearchOpen)
                        {
                            e.Handled = true;
                            return;
                        }

                        tabControl1.SelectedIndex = 1;

                        SetSearchOpen(true);

                        var searchForm = new SearchForm(this);
                        searchForm.Show();
                        break;
                    }
                case { Control: true, KeyCode: Keys.I }:
                    ShowMediaInfoDialog();
                    break;
                case { Control: true, Shift: true, KeyCode: Keys.P }:
                    ShowPlaylistBuilderDialog();
                    e.Handled = true;
                    break;
                case { KeyCode: Keys.F2 }:
                    HandleFileRename();
                    break;
                case { Control: true, KeyCode: Keys.R }:
                case { KeyCode: Keys.F5 }:
                    await UpdateMediaTable(false);
                    break;
            }
        }

        private void MediaFilesTable_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                return;
            }

            var column = mediaFilesTable.Columns[e.ColumnIndex];
            var validColumn =
                Enum.TryParse(column.Name, out ColumnName newColumn);
            if (!validColumn)
            {
                return;
            }

            if (newColumn == _columnName)
            {
                // Swap the sorting order.
                _sortOrder =
                    (_sortOrder == SortDirection.Ascending) ? SortDirection.Descending : SortDirection.Ascending;
            }

            // Get the header title.
            _columnName = newColumn;

            SortEntries();
        }

        private void OptionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var optionsForm = new OptionsForm(this);
            optionsForm.ShowDialog();

            if (!_needsRestart)
            {
                return;
            }

            DisabledFunctionsDueToMissingComponents();

            var result = MessageBox.Show(
                DisplayBuilders.InfoRestartRequired.BuildPlain(),
                DisplayBuilders.InfoRestartRequiredTitle.BuildPlain(),
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Error,
                MessageBoxDefaultButton.Button1
            );
            if (result == DialogResult.Yes)
            {
                Application.Restart();
            }
        }

        private void OptionAddSubtitles_CheckedChanged(object sender, EventArgs e)
        {
            subtitleGroupBox.Enabled = optionAddSubtitles.Checked;
        }

        private void PlaylistBuilderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowPlaylistBuilderDialog();
        }

        private static void Shutdown()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Process.Start("shutdown", "/s /t 120");
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ||
                     RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                Process.Start("shutdown", $"--wait {120} now");
            }
            else
            {
                // We don't know which OS this might be and so can't determine
                // the correct shutdown command to execute.
            }
        }

        private void ShowPlaylistBuilderDialog()
        {
            if (_playlistBuilderBlocked)
            {
                return;
            }

            var playlistBuilderForm = new PlaylistBuilderForm();
            playlistBuilderForm.ShowDialog();
        }

        public void SetFoldersColumnVisibility(bool visible)
        {
            if (mediaFilesTable.Columns["Folder"] is null)
            {
                return;
            }

            mediaFilesTable.Columns["Folder"]!.Visible = visible;
        }

        public void SetNeedsRestart(bool state)
        {
            _needsRestart = state;
        }

        private void SetUpColumns()
        {
            if (mediaFilesTable.Columns["RawDuration"] == null ||
                mediaFilesTable.Columns["FullPath"] == null ||
                mediaFilesTable.Columns["Duration"] == null ||
                mediaFilesTable.Columns["LastModified"] == null ||
                mediaFilesTable.Columns["Folder"] == null ||
                mediaFilesTable.Columns["Title"] == null ||
                mediaFilesTable.Columns["Hash"] == null)
            {
                return;
            }

            mediaFilesTable.Columns["RawDuration"]!.HeaderText = "Raw Duration";
            mediaFilesTable.Columns["RawDuration"]!.Visible = false;

            mediaFilesTable.Columns["FullPath"]!.HeaderText = "Full Path";
            mediaFilesTable.Columns["FullPath"]!.Visible = false;

            mediaFilesTable.Columns["Duration"]!.AutoSizeMode =
                DataGridViewAutoSizeColumnMode.AllCells;
            mediaFilesTable.Columns["Duration"]!.Width = 96;

            mediaFilesTable.Columns["LastModified"]!.HeaderText = "Last Modified";
            mediaFilesTable.Columns["LastModified"]!.Width = 150;

            mediaFilesTable.Columns["Folder"]!.AutoSizeMode =
                DataGridViewAutoSizeColumnMode.AllCells;
            mediaFilesTable.Columns["Folder"]!.Width = 80;

            mediaFilesTable.Columns["Title"]!.AutoSizeMode =
                DataGridViewAutoSizeColumnMode.Fill;

            mediaFilesTable.Columns["Hash"]!.Visible = false;
        }

        private void SetDownloadsEnabled(bool state)
        {
            download.Enabled = state;
        }

        private void SetMediaListUpdateBlocked(bool state)
        {
            reloadMediaFilesToolStripMenuItem.Enabled = !state;
            reloadMediaListToolStripMenuItem.Enabled = !state;
            _mediaListUpdatingBlocked = state;
        }

        private void SetPlaylistBuilderEnabled(bool state)
        {
            _playlistBuilderBlocked = state;
            playlistBuilderToolStripMenuItem.Enabled = state;
        }

        private void SetOptionsEnabled(bool state)
        {
            optionsToolStripMenuItem.Enabled = state;
        }

        private void DisabledFunctionsDueToMissingComponents()
        {
            SetDownloadsEnabled(false);
            SetPlaylistBuilderEnabled(false);
            SetMediaListUpdateBlocked(true);
        }

        public bool IsMediaListUpdatingBlocked()
        {
            return _mediaListUpdatingBlocked;
        }

        private async void HandleFileRename()
        {
            var pos = _contextMenuLocation;
            var hitTest = mediaFilesTable.HitTest(pos.X, pos.Y);
            if (hitTest.Type == DataGridViewHitTestType.ColumnHeader ||
                hitTest.RowIndex == -1 || hitTest.ColumnX == -1)
            {
                return;
            }

            var index = hitTest.RowIndex;
            var oldPath = mediaFilesTable.Rows[index].Cells["FullPath"].Value!.ToString()!;
            var oldName = mediaFilesTable.Rows[index].Cells["Title"].Value!.ToString()!;

            var rename = new RenameFileForm(oldName);
            rename.ShowDialog();

            // Build the new file path.
            var oldFi = new FileInfo(oldPath);
            var newName = $"{rename.NewFileName}{oldFi.Extension}";
            var newPath = Path.Combine(oldFi.DirectoryName!, newName);

            // Attempt to move the file.
            var success = false;
            try
            {
                File.Move(oldPath, newPath);
                success = true;
                _isDirty = true;
            }
            catch
            {
                // Do nothing.
            }

            if (success)
            {
                UpdateStatus(DisplayBuilders.SuccessRenameFile, [oldName, rename.NewFileName]);
            }
            else
            {
                UpdateStatus(DisplayBuilders.ErrorRenameFile, [oldName]);
                return;
            }

            await UpdateMediaTable(true);
        }

        private void HandleDeleteFile(bool trash)
        {
            var pos = _contextMenuLocation;
            var hitTest = mediaFilesTable.HitTest(pos.X, pos.Y);
            if (hitTest.Type == DataGridViewHitTestType.ColumnHeader ||
                hitTest.RowIndex == -1 || hitTest.ColumnX == -1)
            {
                return;
            }

            var success = true;
            var path =
                mediaFilesTable.Rows[hitTest.RowIndex].Cells["FullPath"].Value!.ToString();
            if (Path.Exists(path))
            {
                success = HandleDelete(path, trash);

                // Store the path, just in case we want to undo this later.
                _lastTrashedFilePath = path;
            }

            if (!success)
            {
                return;
            }

            // Remove the table row.
            _fileEntries.RemoveAll(r => r.FullPath == path);

            RebindMediaEntries();
        }

        private bool HandleDelete(string path, bool trash)
        {
            var fileName = Path.GetFileNameWithoutExtension(path);

            var success = true;
            if (trash)
            {
                success = (FileUtils.TrashPath(path) == 0);
            }
            else
            {
                var result = MessageBox.Show(
                    DisplayBuilders.ConfirmDeleteFile.BuildPlain([path]),
                    DisplayBuilders.ConfirmDeleteFileTitle.BuildPlain(),
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button1
                );
                if (result != DialogResult.OK)
                {
                    UpdateStatus(DisplayBuilders.InfoAbortDeleteFile, [path], true);
                    return false;
                }

                try
                {
                    File.Delete(path);
                }
                catch
                {
                    success = false;
                }
            }

            if (success)
            {
                _isDirty = true;
            }

            if (trash)
            {
                UpdateStatus(success ? DisplayBuilders.SuccessTrashFile : DisplayBuilders.ErrorTrashFile, [path]);
            }
            else
            {
                UpdateStatus(success ? DisplayBuilders.SuccessDeleteFile : DisplayBuilders.ErrorDeleteFile, [fileName]);
            }

            return success;
        }

        private void ToggleConsole()
        {
            HideShowConsole(!_isConsoleShown);
        }

        public void HideShowConsole(bool newState)
        {
            var state = newState ? Interop.SW_SHOW : Interop.SW_HIDE;
            Interop.ShowWindow(Interop.GetConsoleWindow(), state);
            _isConsoleShown = newState;
            showConsoleToolStripMenuItem.Text = _isConsoleShown
                ? "Hide &Console..."
                : "Show &Console...";
        }

        public async Task UpdateMediaTable(bool suppressMessages)
        {
            // We do not want to try to update the list, if there is an update in progress.
            if (IsMediaListUpdatingBlocked())
            {
                return;
            }

            SetMediaListUpdateBlocked(true);

            // Disable downloading while the list is being refreshed.
            SetDownloadsEnabled(false);

            // Store the number of entries in the list.
            var oldEntryCount = _fileEntries.Count;

            if (!suppressMessages)
            {
                UpdateStatus(DisplayBuilders.InfoMediaListReloaded);
            }

            // Preserve any selected rows.
            var selectedPaths = (
                from DataGridViewCell cell in mediaFilesTable.SelectedCells
                select mediaFilesTable.Rows[cell.RowIndex] into row
                select row.Cells["FullPath"].Value!.ToString() into path
                select Utils.ComputeMd5Hash(path)
            ).ToArray();

            await UpdateSourceList();

            RebindMediaEntries();
            SetUpColumns();

            // Did we do any updating? If so, we'll need to rewrite the
            // cache file on exit.
            _isDirty = _fileEntries.Count != oldEntryCount;

            // Restore the selected rows.
            RestoreRowSelections(selectedPaths);

            if (!suppressMessages)
            {
                UpdateStatus(DisplayBuilders.InfoMediaListReloading);
            }

            SetMediaListUpdateBlocked(false);

            SetDownloadsEnabled(true);
        }

        public enum ColumnName
        {
            Duration,
            LastModified,
            Folder,
            Title
        }

        private void SortEntries()
        {
            _fileEntries = [.. SortByColumn(_columnName, _sortOrder)];
            RebindMediaEntries();
        }

        private IOrderedEnumerable<MediaFileEntry> SortByColumn(ColumnName columnName, SortDirection sortOrder)
        {
            return columnName switch
            {
                ColumnName.Duration => SortBy(_fileEntries, f => f.RawDuration, sortOrder),
                ColumnName.LastModified => SortBy(_fileEntries, f => f.LastModified, sortOrder),
                ColumnName.Folder => SortBy(_fileEntries, f => f.Folder, sortOrder),
                ColumnName.Title => SortBy(_fileEntries, f => f.Title, sortOrder),
                _ => throw new ArgumentOutOfRangeException(nameof(columnName), columnName, null)
            };
        }

        private static IOrderedEnumerable<T> SortBy<T, TKey>(
            IEnumerable<T> source,
            Func<T, TKey> keySelector,
            SortDirection sortOrder)
        {
            return sortOrder == SortDirection.Ascending
                ? source.OrderBy(keySelector)
                : source.OrderByDescending(keySelector);
        }

        public void RebindMediaEntries()
        {
            mediaFilesTable.DataSource = new BindingSource
            {
                DataSource = _fileEntries
            };
        }

        public enum FindType
        {
            Regex,
            Text
        }

        public void FindEntry(
            string searchString,
            ColumnName column,
            FindType findType,
            bool single,
            bool exactMatch,
            bool ignoreCase,
            bool scrollToEntry = true,
            bool findNextEntry = false
        )
        {
            if (searchString == "")
            {
                return;
            }

            if (column != ColumnName.Title)
            {
                throw new NotImplementedException();
            }

            var i = 0;
            if (findNextEntry)
            {
                if (mediaFilesTable.SelectedRows.Count > 0)
                {
                    // Start on the index after the first selection.
                    // This isn't intended to work with find all, what would be the point?
                    i = mediaFilesTable.SelectedRows[0].Index + 1;
                }
            }

            mediaFilesTable.ClearSelection();

            var hasChangedRow = false;
            for (; i < _fileEntries.Count; i++)
            {
                // TODO - allow other columns to be used in the search.
                if (!IsMatch(_fileEntries[i].Title, searchString, findType, exactMatch, ignoreCase))
                {
                    continue;
                }

                mediaFilesTable.Rows[i].Selected = true;

                if (!hasChangedRow && scrollToEntry)
                {
                    mediaFilesTable.FirstDisplayedScrollingRowIndex = i;
                    hasChangedRow = true;
                }

                if (single)
                {
                    break;
                }
            }

            if (!hasChangedRow)
            {
                MessageBox.Show(
                    DisplayBuilders.NoFurtherMatchesTitle.BuildPlain(),
                    DisplayBuilders.NoFurtherMatches.BuildPlain(),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
        }

        private static bool IsMatch(
            string tester,
            string searchFor,
            FindType findType,
            bool exactMatch,
            bool ignoreCase
        )
        {
            var regExSearchOptions = ignoreCase ?
                RegexOptions.IgnoreCase : RegexOptions.None;
            var textSearchOptions = ignoreCase ?
                StringComparison.CurrentCultureIgnoreCase : StringComparison.Ordinal;

            bool success;
            switch (findType)
            {
                case FindType.Regex:
                    var match = Regex.Match(tester, searchFor, regExSearchOptions);
                    success = match.Success;
                    break;
                case FindType.Text:
                    success = exactMatch ?
                        string.Equals(tester, searchFor, textSearchOptions) :
                        tester.Contains(searchFor, textSearchOptions);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(findType), findType, null);
            }

            return success;
        }

        private void RestoreRowSelections(ReadOnlySpan<string> items)
        {
            if (items.Length == 0)
            {
                return;
            }

            mediaFilesTable.ClearSelection();
            mediaFilesTable.FirstDisplayedScrollingRowIndex = 0;

            var itemsClone = items.ToArray().ToList();

            var lowestIndex = int.MaxValue;
            foreach (DataGridViewRow row in mediaFilesTable.Rows)
            {
                var entryHash = row.Cells["Hash"].Value!.ToString()!;
                if (!itemsClone.Contains(entryHash))
                {
                    continue;
                }

                mediaFilesTable.Rows[row.Index].Selected = true;
                lowestIndex = Math.Min(row.Index, lowestIndex);

                itemsClone.RemoveAll(i => i == entryHash);
                if (itemsClone.Count == 0)
                {
                    break;
                }
            }

            // Did we find an entry to select?
            if (lowestIndex < int.MaxValue)
            {
                mediaFilesTable.FirstDisplayedScrollingRowIndex = lowestIndex;
            }
        }

        private async Task UpdateSourceList()
        {
            var directoryInfo = new DirectoryInfo(_fileUtils.GetMediaPath());

            await ProcessDirectory(directoryInfo);

            // Remove any entries for files that no longer exist and
            // if we are working with files in the root directory only
            // then we need to remove any files from subdirectories too.
            PruneUnwantedEntries();

            // Finally, sort the list.
            SortEntries();
        }

        private void PruneUnwantedEntries()
        {
            var entries =
                _fileEntries.Where(entry => File.Exists(entry.FullPath))
                    .Where(entry => Program.appSettings.ShowMediaInSubFolders || _fileUtils.IsMediaInRoot(entry))
                    .ToList();

            _fileEntries = entries;
        }

        private async Task ProcessDirectory(DirectoryInfo directoryInfo)
        {
            // Iterate over each file in the directory.
            foreach (var file in directoryInfo.GetFiles())
            {
                var entry =
                    _fileEntries
                        .FirstOrDefault(e => e.Hash == Utils.ComputeMd5Hash(file.FullName));
                if (entry is not null)
                {
                    continue;
                }

                var info = await ProcessUtils.RunMediaInfo(file.FullName);
                if (info is null)
                {
                    continue;
                }

                var duration = (int)Math.Round(info.Format?.Duration ?? 0);
                if (duration == 0)
                {
                    continue;
                }

                _fileEntries.Add(new MediaFileEntry(duration, file.LastWriteTime, file));
            }

            // Recursively process subdirectories, if needed.
            if (!Program.appSettings.ShowMediaInSubFolders)
            {
                return;
            }

            foreach (var subdirectory in directoryInfo.GetDirectories())
            {
                await ProcessDirectory(subdirectory);
            }
        }

        private void SetupConfigFile(DownloadType downloadType)
        {
            var lines = new List<string>();

            // Build the config file.
            if (optionAutoUpdate.Checked)
            {
                lines.Add("-U");
            }
            if (optionDownloadChat.Checked)
            {
                lines.Add("--sub-langs live_chat");
            }
            if (optionAddSubtitles.Checked)
            {
                lines.Add("--sub-format best");
                //lines.Add("--convert-subs srt");
                lines.Add("--write-sub");
                lines.Add("--write-auto-subs");

                if (optionEmbedSubs.Checked)
                {
                    lines.Add("--embed-subs");
                }

                lines.Add($"--sub-langs {optionSubtleLangs.Text}");
            }
            if (optionAddMetadata.Checked)
            {
                lines.Add("--add-metadata");
            }
            if (optionAddChapters.Checked)
            {
                lines.Add("--embed-chapters");
            }
            if (optionForceKeyframeAtCuts.Checked)
            {
                lines.Add("--force-keyframes-at-cuts");
            }
            if (optionAddThumbnails.Checked)
            {
                lines.Add("--embed-thumbnail");
            }
            if (optionCookieLogin.Checked &&
                Program.appSettings.CookiePath.Length > 0)
            {
                lines.Add($"--cookies-from-browser {Program.appSettings.CookiePath}");

                if (optionMarkWatched.Checked)
                {
                    lines.Add("--mark-watched");
                }
            }
            if (optionSponsorBlock.Checked)
            {
                // TODO - allow the options to be selected.
                lines.Add("--sponsorblock-remove sponsor,selfpromo");
            }
            if (optionDownloadRateLimitVal.Value > 0)
            {
                var targetRateType =
                    optionDownloadRateLimitType
                        .Items[optionDownloadRateLimitType.SelectedIndex]!
                        .ToString()!;

                lines.Add($"-r {optionDownloadRateLimitVal.Value}{targetRateType}");
            }
            if (downloadType == DownloadType.Multiple)
            {
                lines.Add("-o \"%(playlist)s/%(playlist_index)s - %(title)s [%(id)s].%(ext)s\"");
            }

            var targetResolution =
                optionResolution
                    .Items[optionResolution.SelectedIndex]!
                    .ToString()!
                    .Replace("p", "");
            lines.Add(optionAudioOnly.Checked ? "-f ba" : $"-S \"res:{targetResolution}\"");
            lines.Add($"--ffmpeg-location \"{Program.appSettings.FfmpegDirectory}\"");


            // Write the config file.
            File.WriteAllLines(_configPath, lines);
        }

        private void UpdateStatus(OutputFormatBuilder fmt)
        {
            UpdateStatus(fmt, []);
        }

        private void UpdateStatus(OutputFormatBuilder fmt, object[] binds, bool consoleOnly = false)
        {
            if (!consoleOnly)
            {
                toolStripStatusLabel1.Text = fmt.BuildPlain(binds);
            }

            Console.WriteLine(fmt.Build(binds));
        }

        private void WriteCacheData()
        {
            if (!_isDirty)
            {
                return;
            }

            var entries = (
                from entry in _fileEntries
                let date = entry.LastModified.ToBinary()
                select new CacheEntry(date, entry.RawDuration, entry.FullPath)
            ).ToArray();

            if (!_cacheHandler.Write(entries))
            {
                MessageBox.Show(
                    DisplayBuilders.ErrorWritingCacheTitle.BuildPlain(),
                    DisplayBuilders.ErrorWritingCache.BuildPlain(),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void LoadCachedData()
        {
            foreach (var row in _cacheHandler.Read())
            {
                var path = row.Path;
                if (!Path.Exists(path))
                {
                    continue;
                }

                var modifiedDate = DateTime.FromBinary(row.LastModified);

                _fileEntries.Add(new MediaFileEntry(row.Duration, modifiedDate, new FileInfo(path)));
            }
        }

        private void ShowMediaInfoDialog()
        {
            if (mediaFilesTable.RowCount == 0)
            {
                return;
            }

            var totalDuration = _fileEntries.Sum(entry => entry.RawDuration);
            var totalMediaFiles = mediaFilesTable.RowCount;
            var averageDur = totalDuration / totalMediaFiles;
            var message = DisplayBuilders.MediaInfoDuration.BuildPlain(
                [
                    totalMediaFiles,
                    Utils.SecondsToDuration(averageDur, false),
                    Utils.SecondsToDuration(totalDuration, true)
                ]
            );
            MessageBox.Show(
                message,
                DisplayBuilders.MediaInfoDurationTitle.BuildPlain(),
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }

        private void UpdateDownloadOptions()
        {
            if (!Program.appSettings.RememberDownloadOptions)
            {
                return;
            }

            Program.appSettings.DownloadOptions = new DownloadSettings
            {
                AutoUpdate = optionAutoUpdate.Checked,
                AddSubtitles = optionAddSubtitles.Checked,
                AddMetadata = optionAddMetadata.Checked,
                AddChapters = optionAddChapters.Checked,
                AddThumbnails = optionAddThumbnails.Checked,
                CookieLogin = optionCookieLogin.Checked,
                MarkWatched = optionCookieLogin.Checked && optionMarkWatched.Checked,
                UseSponsorBlock = optionSponsorBlock.Checked,
                TargetResolutionIndex = optionResolution.SelectedIndex,
                DownloadRateLimit = optionDownloadRateLimitVal.Value,
                DownloadRateLimitTypeIndex = optionDownloadRateLimitType.SelectedIndex,
                DownloadChat = optionDownloadChat.Checked,
                ForceKeyframesAtCuts = optionForceKeyframeAtCuts.Checked,
                EmbedSubtitles = optionEmbedSubs.Checked,
                SubtitleLanguages = optionSubtleLangs.Text
            };
        }

        private void RestoreDownloadOptions()
        {
            if (!Program.appSettings.RememberDownloadOptions)
            {
                return;
            }

            var options = Program.appSettings.DownloadOptions;

            optionAutoUpdate.Checked = options.AutoUpdate;
            optionAddSubtitles.Checked = options.AddSubtitles;
            optionAddMetadata.Checked = options.AddMetadata;
            optionAddChapters.Checked = options.AddChapters;
            optionAddThumbnails.Checked = options.AddThumbnails;
            optionCookieLogin.Checked = options.CookieLogin;
            optionMarkWatched.Checked = options.MarkWatched;
            optionSponsorBlock.Checked = options.UseSponsorBlock;
            optionResolution.SelectedIndex = options.TargetResolutionIndex;
            optionDownloadRateLimitVal.Value = options.DownloadRateLimit;
            optionDownloadRateLimitType.SelectedIndex = options.DownloadRateLimitTypeIndex;
            optionForceKeyframeAtCuts.Checked = options.ForceKeyframesAtCuts;
            optionDownloadChat.Checked = options.DownloadChat;
            optionEmbedSubs.Checked = options.EmbedSubtitles;
            optionSubtleLangs.Text = options.SubtitleLanguages;
        }

        public void SetSearchOpen(bool status)
        {
            _isSearchOpen = status;
        }
    }
}

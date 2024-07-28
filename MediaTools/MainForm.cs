using System.Diagnostics;
using System.DirectoryServices;
using System.Globalization;
using System.Text.RegularExpressions;

namespace MediaTools
{
    public partial class MainForm : Form
    {
        private Point _contextMenuLocation;
        private readonly string _runFromPath;
        private readonly string _configPath;
        private SortColumn _sortColumn = SortColumn.Duration;
        private SortDirection _sortOrder = SortDirection.Ascending;
        private readonly FileUtils _fileUtils;
        private bool _consoleShown = true;
        private bool _isUpdatingMediaList;
        private bool _isDirty = !CacheFile.Exists();
        private List<MediaFileEntry> _fileEntries = [];

        public MainForm(string runFromPath)
        {
            IconModifier.SetFormIcon(this);

            _runFromPath = runFromPath;
            _configPath = Path.Combine(runFromPath, "yt-dlp.conf");
            _fileUtils = new FileUtils(runFromPath);

            InitializeComponent();

            if (Program.AppSettings.RememberDownloadOptions)
            {
                RestoreDownloadOptions();
            }
            else
            {
                optionResolution.SelectedIndex = 5;
            }

            source.SelectedIndex = 0;
            toolStripStatusLabel1.Text = "";

            SetFoldersColumnVisibility(Program.AppSettings.ShowFolders);

            Interop.AllocConsole();
            Interop.SetConsoleMode();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            UpdateDownloadOptions();
            Program.AppSettings.WriteSettings();

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

            var file = mediaFilesTable.Rows[e.RowIndex].Cells["FullPath"].Value.ToString()!;
            var processStartInfo = new ProcessStartInfo(file) { UseShellExecute = true };
            Process.Start(processStartInfo);
        }

        private async void ReloadMediaFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            await UpdateMediaTable(false);
        }

        private async void Download_Click(object sender, EventArgs e)
        {
            var urls = BuildDownloadUrlList();
            if (urls.Length == 0)
            {
                UpdateStatus(DisplayBuilders.ErrorNoValidUrls);
                return;
            }

            var downloadType = downloadSingle.Checked ? "video" : "playlist";
            download.Enabled = false;

            var subfolder = downloadFolder.Text;

            UpdateStatus(DisplayBuilders.InfoAttemptWriteConfig);
            SetupConfigFile();
            UpdateStatus(DisplayBuilders.SuccessConfigWrite);

            for (var i = 0; i < urls.Length; i++)
            {
                _fileUtils.EnsureTempExists();

                UpdateStatus(
                    DisplayBuilders.InfoAttemptDownload,
                    [downloadType, i + 1, urls.Length]
                );
                await ProcessUtils.RunDownloader(urls[i], _runFromPath, _fileUtils.GetTempPath());
                UpdateStatus(DisplayBuilders.SuccessDownload, [downloadType, i + 1]);

                // We could move all the files at the end instead, but if
                // something went wrong then some of the files would be stuck
                // in the temporary folder.
                // It seems to make more sense to move them as needed.
                UpdateStatus(DisplayBuilders.InfoAttemptMoveDownloads);
                _fileUtils.MoveTempFiles(subfolder);
                UpdateStatus(DisplayBuilders.SuccessMoveDownloads);
            }

            download.Enabled = true;

            File.Delete(_configPath);

            await UpdateMediaTable(true);
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
            if (!optionCookieLogin.Checked)
            {
                optionMarkWatched.Checked = false;
            }

            optionMarkWatched.Enabled = !optionCookieLogin.Checked;
        }

        private void ShowConsoleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HideShowConsole();
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
                        tabControl1.SelectedIndex = 1;

                        var form = new SearchForm(this)
                        {
                            StartPosition = FormStartPosition.CenterParent
                        };
                        form.Show();
                        break;
                    }
                case { Control: true, KeyCode: Keys.I }:
                    MediaInfoDialog();
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
                Enum.TryParse(column.Name, out SortColumn newColumn);
            if (!validColumn)
            {
                return;
            }

            if (newColumn == _sortColumn)
            {
                // Swap the sorting order.
                _sortOrder =
                    (_sortOrder == SortDirection.Ascending) ?
                        SortDirection.Descending : SortDirection.Ascending;
            }

            // Get the header title.
            _sortColumn = newColumn;

            SortEntries();
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

        private async void Form1_Load(object sender, EventArgs e)
        {
            Program.AppSettings = Settings.ReadSettings();

            LoadCachedData();
            await UpdateMediaTable(true);
        }

        private void OptionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var optionsForm = new OptionsForm(this);
            optionsForm.ShowDialog();
        }

        public void SetFoldersColumnVisibility(bool visible)
        {
            if (mediaFilesTable.Columns["Folder"] is null)
            {
                return;
            }

            mediaFilesTable.Columns["Folder"]!.Visible = visible;
        }

        private async void HandleFileRename()
        {
            var pos = _contextMenuLocation;
            var hitTest = mediaFilesTable.HitTest(pos.X, pos.Y);
            if (hitTest.Type == DataGridViewHitTestType.ColumnHeader)
            {
                return;
            }

            var index = hitTest.RowIndex;
            var oldPath = mediaFilesTable.Rows[index].Cells["FullPath"].Value.ToString()!;
            var oldName = mediaFilesTable.Rows[index].Cells["Title"].Value.ToString()!;

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
            if (hitTest.Type == DataGridViewHitTestType.ColumnHeader)
            {
                return;
            }

            var success = true;
            var path =
                mediaFilesTable.Rows[hitTest.RowIndex].Cells["FullPath"].Value.ToString();
            if (Path.Exists(path))
            {
                success = HandleDelete(path, trash);
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

        private void HideShowConsole()
        {
            var state = _consoleShown ? Interop.SW_HIDE : Interop.SW_SHOW;
            Interop.ShowWindow(Interop.GetConsoleWindow(), state);
            _consoleShown = !_consoleShown;
            showConsoleToolStripMenuItem.Text = _consoleShown
                ? "Hide &Console..."
                : "Show &Console...";
        }

        public string[] BuildDownloadUrlList()
        {
            var isSingle = downloadSingle.Checked;

            return (
                from id in downloadIds.Lines
                where id.Length != 0
                select isSingle
                    ? $"https://www.youtube.com/watch?v={id}"
                    : $"https://www.youtube.com/playlist?list={id}"
            ).Distinct().ToArray();
        }

        public async Task UpdateMediaTable(bool suppressMessages)
        {
            // We do not want to try to update the list, if there is an update in progress.
            if (_isUpdatingMediaList)
            {
                return;
            }

            _isUpdatingMediaList = true;

            // Disable downloading while the list is being refreshed.
            download.Enabled = false;

            if (!suppressMessages)
            {
                UpdateStatus(DisplayBuilders.InfoMediaListReloaded);
            }

            // Preserve any selected rows.
            var selectedPaths = (
                from DataGridViewCell cell in mediaFilesTable.SelectedCells
                select mediaFilesTable.Rows[cell.RowIndex] into row
                select row.Cells["FullPath"].Value.ToString()! into path
                select Utils.ComputeMd5Hash(path)
            ).ToArray();

            await UpdateSourceList();

            RebindMediaEntries();
            SetUpColumns();

            // Restore the selected rows.
            RestoreRowSelections(selectedPaths);

            if (!suppressMessages)
            {
                UpdateStatus(DisplayBuilders.InfoMediaListReloading);
            }

            _isUpdatingMediaList = false;

            download.Enabled = true;
        }

        public enum SortColumn
        {
            Duration,
            LastModified,
            Folder,
            Title
        }

        private void SortEntries()
        {
            _fileEntries = SortByColumn(_sortColumn, _sortOrder).ToList();
            RebindMediaEntries();
        }

        private IOrderedEnumerable<MediaFileEntry> SortByColumn(SortColumn column, SortDirection sortOrder)
        {
            return column switch
            {
                SortColumn.Duration => SortBy(_fileEntries, f => f.RawDuration, sortOrder),
                SortColumn.LastModified => SortBy(_fileEntries, f => f.LastModified, sortOrder),
                SortColumn.Folder => SortBy(_fileEntries, f => f.Folder, sortOrder),
                SortColumn.Title => SortBy(_fileEntries, f => f.Title, sortOrder),
                _ => throw new ArgumentOutOfRangeException(nameof(column), column, null)
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
            mediaFilesTable.DataSource = _fileEntries;
            mediaFilesTable.Refresh();
        }

        public enum FindType
        {
            Regex,
            Text
        }

        public void FindEntry(
            string searchString,
            string column,
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

            var i = 0;
            if (findNextEntry && mediaFilesTable.SelectedRows.Count > 0)
            {
                // Start on the index after the first selection.
                // This isn't intended to work with find all, what would be the point?
                i = mediaFilesTable.SelectedRows[0].Index + 1;
            }

            mediaFilesTable.ClearSelection();

            var hasChangedRow = false;
            for (; i < mediaFilesTable.Rows.Count; i++)
            {
                var title = mediaFilesTable.Rows[i].Cells[column].Value.ToString()!;
                if (!IsMatch(title, searchString, findType, exactMatch, ignoreCase))
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
                var entryHash = row.Cells["Hash"].Value.ToString()!;
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
            var entries = new List<MediaFileEntry>();

            foreach (var entry in _fileEntries)
            {
                // Skip any files that don't exist.
                if (!File.Exists(entry.FullPath))
                {
                    continue;
                }

                // Skip any files in subdirectories, if the setting to show
                // them isn't enabled.
                if (!Program.AppSettings.ShowMediaInSubFolders &&
                    !_fileUtils.IsMediaInRoot(entry))
                {
                    continue;
                }

                entries.Add(entry);
            }

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

                var duration = await ProcessUtils.RunMediaInfo(file.FullName);
                if (duration == 0)
                {
                    continue;
                }

                _fileEntries.Add(new MediaFileEntry(duration, file.LastWriteTime, file));
            }

            // Recursively process subdirectories, if needed.
            if (!Program.AppSettings.ShowMediaInSubFolders)
            {
                return;
            }

            foreach (var subdirectory in directoryInfo.GetDirectories())
            {
                await ProcessDirectory(subdirectory);
            }
        }

        private void SetupConfigFile()
        {
            var lines = new List<string>();

            // Build the config file.
            if (optionAutoUpdate.Checked)
            {
                lines.Add("-U");
            }
            if (optionAddSubtitles.Checked)
            {
                // TODO - allow the languages to be selected?
                lines.Add("--write-sub");
                lines.Add("--sub-format best");
                lines.Add("--sub-langs \"en.*\"");
            }
            if (optionAddMetadata.Checked)
            {
                lines.Add("--add-metadata");
            }
            if (optionAddChapters.Checked)
            {
                lines.Add("--embed-chapters");
            }
            if (optionAddThumbnails.Checked)
            {
                lines.Add("--embed-thumbnail");
            }
            if (optionCookieLogin.Checked && 
                Program.AppSettings.CookiePath.Length > 0)
            {
                lines.Add($"--cookies-from-browser {Program.AppSettings.CookiePath}");

                if (optionMarkWatched.Checked)
                {
                    lines.Add("--mark-watched");
                }
            }
            if (optionSponsorBlock.Checked)
            {
                // TODO - allow the options to be selected?
                lines.Add("--sponsorblock-remove sponsor,selfpromo");
            }
            if (downloadPlaylist.Checked)
            {
                lines.Add("-o \"%(playlist)s/%(playlist_index)s - %(title)s [%(id)s].%(ext)s\"");
            }

            var targetResolution = optionResolution.Items[optionResolution.SelectedIndex]!
                .ToString()!
                .Replace("p", "");
            lines.Add(optionAudioOnly.Checked ? "-f ba" : $"-S \"res:{targetResolution}\"");

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
                let date = DateTime.Parse(entry.LastModified, CultureInfo.CurrentCulture).ToBinary()
                select new CacheEntry(date, entry.RawDuration, entry.FullPath)
            ).ToArray();

            if (!CacheFile.Write(entries))
            {
                // TODO - do something to notify the user here.
                // TODO - I'm not sure what since this occurs as the application is closing...
                MessageBox.Show("Something went wrong while writing the file.");
            }
        }

        private void LoadCachedData()
        {
            foreach (var row in CacheFile.Read())
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

        private void MediaInfoDialog()
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
                MessageBoxIcon.Information,
                MessageBoxDefaultButton.Button1
            );
        }

        private void UpdateDownloadOptions()
        {
            if (!Program.AppSettings.RememberDownloadOptions)
            {
                return;
            }

            Program.AppSettings.DownloadOptions.AutoUpdate = 
                optionAutoUpdate.Checked;

            Program.AppSettings.DownloadOptions.AddSubtitles =
                optionAddSubtitles.Checked;

            Program.AppSettings.DownloadOptions.AddMetadata =
                optionAddMetadata.Checked;

            Program.AppSettings.DownloadOptions.AddChapters =
                optionAddChapters.Checked;

            Program.AppSettings.DownloadOptions.AddThumbnails =
                optionAddThumbnails.Checked;

            Program.AppSettings.DownloadOptions.CookieLogin =
                optionCookieLogin.Checked;

            Program.AppSettings.DownloadOptions.MarkWatched =
                optionCookieLogin.Checked && optionMarkWatched.Checked;

            Program.AppSettings.DownloadOptions.UseSponsorBlock =
                optionSponsorBlock.Checked;

            Program.AppSettings.DownloadOptions.TargetResolutionIndex =
                optionResolution.SelectedIndex;
        }

        private void RestoreDownloadOptions()
        {
            if (!Program.AppSettings.RememberDownloadOptions)
            {
                return;
            }

            optionAutoUpdate.Checked = 
                Program.AppSettings.DownloadOptions.AutoUpdate;

            optionAddSubtitles.Checked = 
                Program.AppSettings.DownloadOptions.AddSubtitles;

            optionAddMetadata.Checked = 
                Program.AppSettings.DownloadOptions.AddMetadata;

            optionAddChapters.Checked = 
                Program.AppSettings.DownloadOptions.AddChapters;

            optionAddThumbnails.Checked = 
                Program.AppSettings.DownloadOptions.AddThumbnails;

            optionCookieLogin.Checked = 
                Program.AppSettings.DownloadOptions.CookieLogin;

            optionMarkWatched.Checked = 
                Program.AppSettings.DownloadOptions.MarkWatched;

            optionSponsorBlock.Checked = 
                Program.AppSettings.DownloadOptions.UseSponsorBlock;

            optionResolution.SelectedIndex = 
                Program.AppSettings.DownloadOptions.TargetResolutionIndex;
        }
    }
}

using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace MediaTools
{
    public partial class MainForm : Form
    {
        private Point _contextMenuLocation;
        private readonly string _runFromPath;
        private readonly string _configTemplatePath;
        private readonly string _configPath;
        private readonly string _cachePath;
        private readonly FileUtils _fileUtils;
        private readonly Dictionary<string, (int Index, int Duration)> _cache = new();
        private bool _consoleShown = true;
        private bool _isUpdatingMediaList;
        private bool _isFirstSort = true;
        private const string SerializeDelimiter = @"//";

        public MainForm(string runFromPath)
        {
            IconModifier.SetFormIcon(this);

            _runFromPath = runFromPath;
            _configPath = Path.Combine(runFromPath, "yt-dlp.conf");
            _configTemplatePath = _configPath + "-template";
            _cachePath = Path.Combine(runFromPath, "cache.dat");
            _fileUtils = new FileUtils(runFromPath);

            InitializeComponent();

            source.SelectedIndex = 0;
            options2Resolution.SelectedIndex = 5;
            toolStripStatusLabel1.Text = "";

            Interop.AllocConsole();
            Interop.SetConsoleMode();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
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

        private void MediaFilesTable_Sorted(object sender, EventArgs e)
        {
            // Update the index table.
            UpdateCache();

            _isFirstSort = false;
        }

        private async void ReloadMediaFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            await UpdateMediaTable();
        }

        private void ClearCacheToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            FileUtils.TruncateFile(_cachePath);
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

            UpdateStatus(DisplayBuilders.AttemptWriteConfig);
            SetupConfigFile();
            UpdateStatus(DisplayBuilders.SuccessConfigWrite);

            for (var i = 0; i < urls.Length; i++)
            {
                _fileUtils.EnsureTempExists();

                UpdateStatus(
                    DisplayBuilders.AttemptingDownload,
                    [downloadType, i + 1, urls.Length]
                );
                await ProcessUtils.RunDownloader(urls[i], _runFromPath, _fileUtils.GetTempPath());
                UpdateStatus(DisplayBuilders.SuccessDownload, [downloadType, i + 1]);

                // We could move all the files at the end instead, but if
                // something went wrong then some of the files would be stuck
                // in the temporary folder.
                // It seems to make more sense to move them as needed.
                UpdateStatus(DisplayBuilders.AttemptingMoveDownloads);
                _fileUtils.MoveTempFiles(subfolder);
                UpdateStatus(DisplayBuilders.SuccessMoveDownloads);
            }

            download.Enabled = true;

            File.Delete(_configPath);

            await UpdateMediaTable();
        }

        private void OptionAudioOnly_CheckedChanged(object sender, EventArgs e)
        {
            if (optionAddThumbnails.Checked)
            {
                optionAddThumbnails.Checked = false;
            }
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
            if (e.Button != MouseButtons.Right)
            {
                return;
            }

            if (mediaFilesTable.HitTest(e.X, e.Y).Type == DataGridViewHitTestType.ColumnHeader)
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
                    await UpdateMediaTable();
                    break;
            }
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            LoadCacheData();

            await UpdateMediaTable();
        }

        private void HandleFileRename()
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
            var oldHash = Utils.ComputeMd5Hash(oldPath);

            var rename = new RenameFileForm(oldName)
            {
                StartPosition = FormStartPosition.CenterParent
            };
            rename.ShowDialog();

            // Build the new file path.
            var oldFi = new FileInfo(oldPath);
            var newName = $"{rename.NewFileName}{oldFi.Extension}";
            var newPath = Path.Combine(oldFi.DirectoryName!, newName);
            var newHash = Utils.ComputeMd5Hash(newPath);

            // Attempt to move the file.
            var success = false;
            try
            {
                File.Move(oldPath, newPath);
                success = true;
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

            // Rename the entry in the table.
            mediaFilesTable.Rows[index].Cells["FullPath"].Value = newPath;
            mediaFilesTable.Rows[index].Cells["Title"].Value = rename.NewFileName;

            // Update the cache.
            var cacheEntry = _cache[oldHash];
            _cache.Remove(oldHash);
            _cache.Add(newHash, cacheEntry);
        }

        private void HandleDeleteFile(bool trash)
        {
            var pos = _contextMenuLocation;
            var hitTest = mediaFilesTable.HitTest(pos.X, pos.Y);
            if (hitTest.Type == DataGridViewHitTestType.ColumnHeader)
            {
                return;
            }

            var index = hitTest.RowIndex;
            var path = mediaFilesTable.Rows[index].Cells["FullPath"].Value.ToString()!;

            var success = false;
            if (trash)
            {
                success = (FileUtils.TrashPath(path) == 0);
            }
            else
            {
                try
                {
                    File.Delete(path);
                    success = true;
                }
                catch
                {
                    // Do nothing.
                }
            }

            var fileName = Path.GetFileNameWithoutExtension(path);

            if (trash)
            {
                if (success)
                {
                    UpdateStatus(DisplayBuilders.SuccessTrashFile, [fileName]);
                }
                else
                {
                    UpdateStatus(DisplayBuilders.ErrorTrashFile, [fileName]);
                    return;
                }
            }
            else
            {
                if (success)
                {
                    UpdateStatus(DisplayBuilders.SuccessDeleteFile, [fileName]);
                }
                else
                {
                    UpdateStatus(DisplayBuilders.ErrorDeleteFile, [fileName]);
                    return;
                }
            }

            // Remove the cache entry for the file.
            _cache.Remove(Utils.ComputeMd5Hash(path));

            // Remove the table row.
            mediaFilesTable.Rows.RemoveAt(hitTest.RowIndex);

            // Update the cache.
            UpdateCache();
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
            ).ToArray();
        }

        private async Task UpdateMediaTable()
        {
            // We do not want to try to update the list, if there is an update in progress.
            if (_isUpdatingMediaList)
            {
                return;
            }

            _isUpdatingMediaList = true;

            // Disable downloading while the list is being refreshed.
            download.Enabled = false;

            UpdateStatus(DisplayBuilders.ReloadingMediaFiles);

            // Preserve any selected rows.
            var selectedPaths = (
                from DataGridViewCell cell in mediaFilesTable.SelectedCells
                select mediaFilesTable.Rows[cell.RowIndex] into row
                select row.Cells["FullPath"].Value.ToString()! into path
                select Utils.ComputeMd5Hash(path)
            ).ToList();

            await FillTable();

            SortTable();

            // Restore the selected rows.
            RestoreRowSelections(ref selectedPaths);

            UpdateStatus(DisplayBuilders.SuccessReload);

            _isUpdatingMediaList = false;

            download.Enabled = true;
        }

        private void SortTable()
        {
            string sortCol;
            ListSortDirection sortOrder;

            if (_isFirstSort)
            {
                sortCol = "Duration";
                sortOrder = ListSortDirection.Ascending;
            }
            else
            {
                sortCol = mediaFilesTable.SortedColumn.Name!;
                sortOrder =
                    mediaFilesTable.SortOrder == SortOrder.Ascending
                        ? ListSortDirection.Ascending
                        : ListSortDirection.Descending;
            }

            mediaFilesTable.Sort(mediaFilesTable.Columns[sortCol]!, sortOrder);
        }

        private void UpdateCache()
        {
            for (var i = 0; i < mediaFilesTable.Rows.Count; i++)
            {
                var row = mediaFilesTable.Rows[i];
                var path = row.Cells["FullPath"].Value.ToString()!;
                var hash = Utils.ComputeMd5Hash(path);

                var entry = _cache[hash];
                _cache[hash] = (i, entry.Duration);
            }
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
            bool scrollToEntry = true
        )
        {
            if (searchString == "")
            {
                return;
            }

            mediaFilesTable.ClearSelection();

            var hasChangedRow = false;
            for (var i = 0; i < mediaFilesTable.Rows.Count; i++)
            {
                var title = mediaFilesTable.Rows[i].Cells[column].Value.ToString()!;
                if (!IsMatch(title, searchString, findType, exactMatch))
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
            bool exactMatch)
        {
            bool success;
            switch (findType)
            {
                case FindType.Regex:
                    var match = Regex.Match(tester, searchFor, RegexOptions.IgnoreCase);
                    success = match.Success;
                    break;
                case FindType.Text:
                    if (exactMatch)
                    {
                        success = string.Equals(tester,
                            searchFor,
                            StringComparison.CurrentCultureIgnoreCase
                        );
                    }
                    else
                    {
                        success = tester.Contains(
                            searchFor,
                            StringComparison.CurrentCultureIgnoreCase
                        );
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(findType), findType, null);
            }

            return success;
        }

        private void RestoreRowSelections(ref List<string> items)
        {
            if (items.Count == 0)
            {
                return;
            }

            mediaFilesTable.ClearSelection();
            mediaFilesTable.FirstDisplayedScrollingRowIndex = 0;

            var lowestIndex = int.MaxValue;
            foreach (var item in items)
            {
                if (!_cache.TryGetValue(item, out var entry))
                {
                    continue;
                }

                var index = entry.Index;
                mediaFilesTable.Rows[index].Selected = true;

                if (index < lowestIndex)
                {
                    lowestIndex = index;
                }
            }

            // Did we find an entry to select?
            if (lowestIndex < int.MaxValue)
            {
                mediaFilesTable.FirstDisplayedScrollingRowIndex = lowestIndex;
            }
        }

        private async Task FillTable()
        {
            var directoryInfo = new DirectoryInfo(_fileUtils.GetMediaPath());

            var results =
                new List<(
                    int RawDuration,
                    string Duration,
                    string LastModified,
                    string Title,
                    string FullPath
                )>();

            // Iterate over each file in the directory.
            foreach (var file in directoryInfo.GetFiles())
            {
                var hash = Utils.ComputeMd5Hash(file.FullName);
                if (_cache.ContainsKey(hash))
                {
                    continue;
                }

                // Cache the durations to avoid the performance overhead of running the info
                // tool when we already have the information at hand.
                // Note that we DO NOT want to update the indices here as they will need to be
                // recomputed after sorting either way.
                int duration;
                if (_cache.TryGetValue(hash, out var entry))
                {
                    duration = entry.Duration;
                }
                else
                {
                    duration = await ProcessUtils.RunMediaInfo(file.FullName);

                    // Add the entry to the cache.
                    _cache.Add(hash, (0, duration));
                }

                if (duration == 0)
                {
                    continue;
                }

                var filePath = Path.GetFileNameWithoutExtension(file.FullName);
                var modified = file.LastWriteTime;

                results.Add(
                    (
                        duration,
                        Utils.SecondsToDuration(duration, false),
                        modified.ToString(CultureInfo.CurrentCulture),
                        filePath,
                        file.FullName
                    )
                );
            }

            // Ensure we update only on the UI thread.
            mediaFilesTable.Invoke(() =>
            {
                foreach (var result in results)
                {
                    mediaFilesTable.Rows.Add(
                        result.RawDuration,
                        result.Duration,
                        result.LastModified,
                        result.Title,
                        result.FullPath
                    );
                }
            });
        }

        private void SetupConfigFile()
        {
            var lines = new List<string>(File.ReadAllLines(_configTemplatePath));

            // Build the config file.
            if (optionSubtitles.Checked)
            {
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
            if (optionMarkWatched.Checked)
            {
                lines.Add("--mark-watched");
            }
            if (optionSponsorBlock.Checked)
            {
                lines.Add("--sponsorblock-remove sponsor,selfpromo");
            }
            if (downloadPlaylist.Checked)
            {
                lines.Add("-o \"%(playlist)s/%(playlist_index)s - %(title)s [%(id)s].%(ext)s\"");
            }

            var targetResolution = options2Resolution.Items[options2Resolution.SelectedIndex]!
                .ToString()!
                .Replace("p", "");
            lines.Add(optionAudioOnly.Checked ? "-f ba" : $"-S \"res:{targetResolution}\"");

            // Write the config file.
            File.WriteAllLines(_configPath, lines);
        }

        private void UpdateStatus(OutputFormatBuilder fmt)
        {
            toolStripStatusLabel1.Text = fmt.BuildPlain([]);
            Console.WriteLine(fmt.Build([]));
        }

        private void UpdateStatus(OutputFormatBuilder fmt, object[] binds)
        {
            toolStripStatusLabel1.Text = fmt.BuildPlain(binds);
            Console.WriteLine(fmt.Build(binds));
        }

        private void WriteCacheData()
        {
            var text = new StringBuilder();

            for (var i = 0; i < mediaFilesTable.Rows.Count; i++)
            {
                var row = mediaFilesTable.Rows[i];

                var lastModified = row.Cells["LastModified"].Value.ToString()!;
                var date = 
                    DateTime.Parse(lastModified).ToBinary().ToString("X");
                var duration = 
                    int.Parse(row.Cells["RawDuration"].Value.ToString()!).ToString("X");
                var path = row.Cells["FullPath"].Value.ToString()!;

                text.Append(path);
                text.Append(SerializeDelimiter);
                text.Append(duration);
                text.Append(SerializeDelimiter);
                text.Append(date);
                text.Append(Environment.NewLine);
            }

            var bytes = Encoding.UTF8.GetBytes(text.ToString());
            var compressedBytes = Utils.Compress(ref bytes);
            File.WriteAllBytes(_cachePath, compressedBytes);
        }

        private void LoadCacheData()
        {
            if (!Path.Exists(_cachePath))
            {
                return;
            }

            var compressed = File.ReadAllBytes(_cachePath);
            if (compressed.Length == 0)
            {
                return;
            }

            string[] rows;
            try
            {
                var decompressed = Utils.Decompress(ref compressed);
                var text = Encoding.UTF8.GetString(decompressed);
                rows = text.Split(Environment.NewLine);
            }
            catch
            {
                FileUtils.TruncateFile(_cachePath);
                return;
            }

            for (var i = 0; i < rows.Length; i++)
            {
                if (!rows[i].Contains(SerializeDelimiter))
                {
                    continue;
                }

                var bits = rows[i].Split(SerializeDelimiter);

                var path = bits[0];
                if (!Path.Exists(path))
                {
                    continue;
                }

                var hash = Utils.ComputeMd5Hash(path);
                var duration = int.Parse(bits[1], NumberStyles.HexNumber);
                var modifiedDate = DateTime.FromBinary(long.Parse(bits[2], NumberStyles.HexNumber));

                var fi = new FileInfo(path);
                var title = Path.GetFileNameWithoutExtension(fi.FullName);

                // Populate the display table.
                mediaFilesTable.Rows.Add(
                    duration,
                    Utils.SecondsToDuration(duration, false),
                    modifiedDate.ToString(CultureInfo.CurrentCulture),
                    title,
                    path
                );

                // Add the corresponding cache entry.
                _cache.Add(hash, (i, duration));
            }
        }

        private void MediaInfoDialog()
        {
            if (mediaFilesTable.RowCount == 0)
            {
                return;
            }

            var totalDuration = 0;
            foreach (var entry in _cache.Values)
            {
                var (_, dur) = entry;
                totalDuration += dur;
            }

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
                DisplayBuilders.MediaInfoDurationTitle,
                MessageBoxButtons.OK,
                MessageBoxIcon.Information,
                MessageBoxDefaultButton.Button1
            );
        }
    }
}
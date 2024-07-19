using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace MediaTools
{
    public partial class Form1 : Form
    {
        private Point _contextMenuLocation;
        private readonly string _runFromPath;
        private readonly string _configTemplatePath;
        private readonly string _configPath;
        private double _totalDuration = 0;
        private bool _consoleShown = true;
        private bool _isUpdatingMediaList = false;
        private const bool TestMode = false;
        private readonly FileUtils _fileUtils;

        private Dictionary<string, (int Index, double Duration)> _cache = new();

        public Form1(string runFromPath)
        {
            IconModifier.SetFormIcon(this);

            _runFromPath = runFromPath;
            _configPath = Path.Combine(runFromPath, "yt-dlp.conf");
            _configTemplatePath = _configPath + "-template";
            _fileUtils = new FileUtils(runFromPath);

            InitializeComponent();

            source.SelectedIndex = 0;
            options2Resolution.SelectedIndex = 5;
            toolStripStatusLabel1.Text = "";

            Interop.AllocConsole();
            Interop.SetConsoleMode();
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
            await UpdateMediaTable();
        }

        private async void Download_Click(object sender, EventArgs e)
        {
            var urls = BuildDownloadUrlList();
            if (urls.Length == 0)
            {
                var noUrlError = new OutputFormatBuilder()
                    .Foreground(ConsoleColour.Red)
                    .Text("Error:")
                    .ResetForeground()
                    .Text(" no valid target download URLs specified.");
                UpdateStatus(ref noUrlError);
                return;
            }

            var downloadType = downloadSingle.Checked ? "video" : "playlist";
            download.Enabled = false;

            var configSuccess = new OutputFormatBuilder()
                .Foreground(ConsoleColour.Green)
                .Text("Success:")
                .ResetForeground()
                .Text(" download config file successfully written!");

            var moveSuccess = new OutputFormatBuilder()
                .Foreground(ConsoleColour.Green)
                .Text("Success:")
                .ResetForeground()
                .Text(@" downloaded files successfully moved!");

            var subfolder = downloadFolder.Text;

            UpdateStatus(@"Attempting to write download config file...");
            SetupConfigFile();
            UpdateStatus(ref configSuccess);

            for (var i = 0; i < urls.Length; i++)
            {
                _fileUtils.EnsureTempExists();

                UpdateStatus($@"Downloading {downloadType} {i + 1} of {urls.Length}...");
                await ProcessUtils.RunDownloader(urls[i], _runFromPath, _fileUtils.GetTempPath());

                var downloadSuccess = new OutputFormatBuilder()
                    .Foreground(ConsoleColour.Green)
                    .Text("Success:")
                    .ResetForeground()
                    .Text($@" {downloadType} {i + 1} successfully downloaded!");
                UpdateStatus(ref downloadSuccess);

                // We could move all the files at the end instead, but if
                // something went wrong then some of the files would be stuck
                // in the temporary folder. It seems to make more sense to move
                // them as and when needed.
                UpdateStatus(@"Attempting to move files to specified folder...");
                _fileUtils.MoveTempFiles(subfolder);
                UpdateStatus(ref moveSuccess);
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

        private void DeleteItem_Click(object? sender, EventArgs e)
        {
            HandleDeleteFile(false);
        }

        private void TrashToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HandleDeleteFile(true);
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

                    var form = new Form2(this);
                    form.Show();
                    break;
                }
                case { Control: true, KeyCode: Keys.I }:
                    if (mediaFilesTable.RowCount == 0)
                    {
                        break;
                    }

                    var totalMediaFiles = mediaFilesTable.RowCount;
                    var averageDur = _totalDuration / totalMediaFiles;
                    var message =
                        $"There are a total of {totalMediaFiles} files."
                        + $"The average duration of a file is {Utils.SecondsToDuration(averageDur, false)} "
                        + $"and a total length of {Utils.SecondsToDuration(_totalDuration, true)}.";
                    MessageBox.Show(
                        message,
                        @"Total Media Duration",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information,
                        MessageBoxDefaultButton.Button1
                    );
                    break;
                case { Control: true, KeyCode: Keys.R }:
                case { KeyCode: Keys.F5 }:
                    await UpdateMediaTable();
                    break;
            }
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            await UpdateMediaTable();
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
                success = (FileUtils.TrashPath(path!) == 0);
            }
            else
            {
                try
                {
                    File.Delete(path!);
                    success = true;
                }
                catch
                {
                    // Do nothing.
                }
            }

            var fi = new FileInfo(path);
            var trashSuccess = new OutputFormatBuilder()
                .Foreground(ConsoleColour.Green)
                .Text("Success:")
                .ResetForeground()
                .Text($" file '{fi.Name}' has been sent to the trash!");
            var trashError = new OutputFormatBuilder()
                .Foreground(ConsoleColour.Red)
                .Text("Error:")
                .ResetForeground()
                .Text(" failed to send file to the trash!");
            var deleteSuccess = new OutputFormatBuilder()
                .Foreground(ConsoleColour.Green)
                .Text("Success:")
                .ResetForeground()
                .Text($" file '{fi.Name}' has been deleted!");
            var deleteError = new OutputFormatBuilder()
                .Foreground(ConsoleColour.Red)
                .Text("Error:")
                .ResetForeground()
                .Text(" failed to delete the file!");

            if (trash)
            {
                if (success)
                {
                    UpdateStatus(ref trashSuccess);
                }
                else
                {
                    UpdateStatus(ref trashError);
                    return;
                }
            }
            else
            {
                if (success)
                {
                    UpdateStatus(ref deleteSuccess);
                }
                else
                {
                    UpdateStatus(ref deleteError);
                    return;
                }
            }

            // Remove the cache entry for the file.
            _cache.Remove(Utils.ComputeMd5Hash(path));

            // Update the duration counter.
            var duration = (double)mediaFilesTable.Rows[index].Cells["RawDuration"].Value;
            _totalDuration -= duration;

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

            UpdateStatus(@"Reloading media file list...");

            // Preserve any selected rows.
            var selectedPaths = (
                from DataGridViewCell cell in mediaFilesTable.SelectedCells
                select mediaFilesTable.Rows[cell.RowIndex] into row
                select row.Cells["FullPath"].Value.ToString()! into path
                select Utils.ComputeMd5Hash(path)
            ).ToList();

            mediaFilesTable.Rows.Clear();
            _totalDuration = 0;

            await FillTable();

            mediaFilesTable.Sort(mediaFilesTable.Columns["Duration"]!, ListSortDirection.Ascending);

            // Update the index table.
            UpdateCache();

            // Restore the selected rows.
            RestoreRowSelections(ref selectedPaths);

            var updateListSuccess = new OutputFormatBuilder()
                .Foreground(ConsoleColour.Green)
                .Text("Success:")
                .ResetForeground()
                .Text(" media list successfully reloaded!");
            UpdateStatus(ref updateListSuccess);

            _isUpdatingMediaList = false;

            download.Enabled = true;
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
                bool isMatch;

                switch (findType)
                {
                    case FindType.Regex:
                        var match = Regex.Match(title, searchString, RegexOptions.IgnoreCase);
                        isMatch = match.Success;
                        break;
                    case FindType.Text:
                        isMatch = string.Equals(
                            title,
                            searchString,
                            StringComparison.CurrentCultureIgnoreCase
                        );
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(findType), findType, null);
                }

                if (!isMatch)
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
                    double RawDuration,
                    string Duration,
                    string LastModified,
                    string Title,
                    string FullPath
                )>();

            // Iterate over each file in the directory.
            var i = 0;
            foreach (var file in directoryInfo.GetFiles())
            {
                var hash = Utils.ComputeMd5Hash(file.FullName);

                // Cache the durations to avoid the performance overhead of running the info
                // tool when we already have the information at hand.
                // Note that we DO NOT want to update the indices here as they will need to be
                // recomputed after sorting either way.
                double duration = 0;
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

                if (duration < 0.1)
                {
                    continue;
                }

                _totalDuration += duration;

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

                if (TestMode && i == 10)
                {
                    break;
                }

                ++i;
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

        private void UpdateStatus(ref OutputFormatBuilder fmt)
        {
            toolStripStatusLabel1.Text = fmt.BuildPlain();
            Console.WriteLine(fmt.Build());
        }

        private void UpdateStatus(string text)
        {
            toolStripStatusLabel1.Text = text;
            Console.WriteLine(text);
        }
    }
}

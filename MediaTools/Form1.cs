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

        private void MediaFilesTable_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }

            var file = mediaFilesTable.Rows[e.RowIndex].Cells["FullPath"].Value.ToString()!;
            var processStartInfo = new ProcessStartInfo(file)
            {
                UseShellExecute = true
            };
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
            var pos = _contextMenuLocation;
            var hitTest = mediaFilesTable.HitTest(pos.X, pos.Y);
            if (hitTest.Type == DataGridViewHitTestType.ColumnHeader)
            {
                return;
            }

            if (!DeleteFile(hitTest.RowIndex, false))
            {
                return;
            }

            mediaFilesTable.Rows.RemoveAt(hitTest.RowIndex);

            var duration = (double)mediaFilesTable
                .Rows[hitTest.RowIndex]
                .Cells["RawDuration"]
                .Value;
            _totalDuration -= duration;
        }

        private void TrashToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var pos = _contextMenuLocation;
            var hitTest = mediaFilesTable.HitTest(pos.X, pos.Y);
            if (hitTest.Type == DataGridViewHitTestType.ColumnHeader)
            {
                return;
            }

            if (!DeleteFile(hitTest.RowIndex, true))
            {
                return;
            }

            mediaFilesTable.Rows.RemoveAt(hitTest.RowIndex);

            var duration = (double)mediaFilesTable
                .Rows[hitTest.RowIndex]
                .Cells["RawDuration"]
                .Value;
            _totalDuration -= duration;
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
                        $"There are a total of {totalMediaFiles} files." +
                        $"The average duration of a file is {Utils.SecondsToDuration(averageDur, false)} " +
                        $"and a total length of {Utils.SecondsToDuration(_totalDuration, true)}.";
                    MessageBox.Show(message,
                        @"Total Media Duration",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information,
                        MessageBoxDefaultButton.Button1);
                    break;
                case { Control: true, KeyCode: Keys.R }:
                    await UpdateMediaTable();
                    break;
            }
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            await UpdateMediaTable();
        }

        private bool DeleteFile(int rowId, bool trash)
        {
            var path = mediaFilesTable
                .Rows[rowId]
                .Cells["FullPath"]
                .Value
                .ToString();

            if (trash)
            {
                if (FileUtils.TrashPath(path!) == 0)
                {
                    return true;
                }

                var trashError = new OutputFormatBuilder()
                    .Foreground(ConsoleColour.Red)
                    .Text("Error:")
                    .ResetForeground()
                    .Text(" failed to send file to the trash!");
                UpdateStatus(ref trashError);

                return false;
            }
            else
            {
                var deleteError = new OutputFormatBuilder()
                    .Foreground(ConsoleColour.Red)
                    .Text("Error:")
                    .ResetForeground()
                    .Text(" failed to delete file!");

                try
                {
                    File.Delete(path!);
                }
                catch
                {
                    UpdateStatus(ref deleteError);
                    return false;
                }
            }

            return true;
        }

        private void HideShowConsole()
        {
            var state = _consoleShown ? Interop.SW_HIDE : Interop.SW_SHOW;
            Interop.ShowWindow(Interop.GetConsoleWindow(), state);
            _consoleShown = !_consoleShown;
            showConsoleToolStripMenuItem.Text = _consoleShown ? "Hide &Console..." : "Show &Console...";
        }

        public string[] BuildDownloadUrlList()
        {
            var isSingle = downloadSingle.Checked;

            return (from id in downloadIds.Lines
                    where id.Length != 0
                    select isSingle
                        ? $"https://www.youtube.com/watch?v={id}"
                        : $"https://www.youtube.com/playlist?list={id}").ToArray();
        }

        private async Task UpdateMediaTable()
        {
            // We do not want to try to update the list, if there is an update in progress.
            if (_isUpdatingMediaList)
            {
                return;
            }

            _isUpdatingMediaList = true;

            UpdateStatus(@"Reloading media file list...");

            // Preserve the selected item.
            var selectedItem = 
                (mediaFilesTable.SelectedRows.Count > 0) ? 
                    mediaFilesTable.SelectedRows[0].Cells["FullPath"].Value.ToString()! :
                    "";

            mediaFilesTable.Rows.Clear();
            _totalDuration = 0;

            await FillTable();

            mediaFilesTable.Sort(mediaFilesTable.Columns["Duration"]!, ListSortDirection.Ascending);

            // Restore the selected item.
            FindEntry(selectedItem, "FullPath", true);

            var updateListSuccess = new OutputFormatBuilder()
                .Foreground(ConsoleColour.Green)
                .Text("Success:")
                .ResetForeground()
                .Text(" media list successfully reloaded!");
            UpdateStatus(ref updateListSuccess);

            _isUpdatingMediaList = false;
        }

        public void FindEntry(string searchRegex, string column, bool single)
        {
            mediaFilesTable.ClearSelection();

            var hasChangedRow = false;
            for (var i = 0; i < mediaFilesTable.Rows.Count; i++)
            {
                var title = mediaFilesTable.Rows[i].Cells[column].Value.ToString();

                var match = Regex.Match(title!, searchRegex, RegexOptions.IgnoreCase);
                if (!match.Success)
                {
                    continue;
                }

                mediaFilesTable.Rows[i].Selected = true;

                if (!hasChangedRow)
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

        private async Task FillTable()
        {
            var directoryInfo = new DirectoryInfo(_fileUtils.GetMediaPath());

            var results = new List<(double RawDuration, string Duration, string LastModified, string Title, string FullPath)>();

            // Iterate over each file in the directory.
            var i = 0;
            foreach (var file in directoryInfo.GetFiles())
            {
                var dur = await ProcessUtils.RunMediaInfo(file.FullName);
                if (dur == 0)
                {
                    continue;
                }

                _totalDuration += dur;

                var filePath = Path.GetFileNameWithoutExtension(file.FullName);
                var modified = file.LastWriteTime;

                results.Add((
                    dur,
                    Utils.SecondsToDuration(dur, false),
                    modified.ToString(CultureInfo.CurrentCulture),
                    filePath,
                    file.FullName
                ));

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
                    mediaFilesTable.Rows.Add(result.RawDuration,
                        result.Duration,
                        result.LastModified,
                        result.Title, result.FullPath);
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

            var targetResolution = options2Resolution
                .Items[options2Resolution.SelectedIndex]!
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

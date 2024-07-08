using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace MediaTools
{
    public partial class Form1 : Form
    {
        private Point _contextMenuLocation;
        private readonly ContextMenuStrip _contextMenuStrip;
        private readonly string _runFromPath;
        private readonly string _configTemplatePath;
        private readonly string _configPath;
        private double _totalDuration = 0;
        private bool _consoleShown = true;
        private bool _isUpdatingMediaList = false;
        private const bool TestMode = false;

        private FileUtils _fileUtils;

        #region DLL Imports

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool AllocConsole();

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        private const int SW_HIDE = 0;
        private const int SW_SHOW = 5;

        #endregion

        public Form1(string runFromPath)
        {
            IconModifier.SetFormIcon(this);

            _runFromPath = runFromPath;
            _configTemplatePath = Path.Combine(runFromPath, "yt-dlp.conf-template");
            _configPath = Path.Combine(runFromPath, "yt-dlp.conf");
            _fileUtils = new FileUtils(runFromPath);

            InitializeComponent();

            toolStripStatusLabel1.Text = "";
            source.SelectedIndex = 0;
            options2Resolution.SelectedIndex = 5;

            _contextMenuStrip = new ContextMenuStrip();
            var deleteItem = new ToolStripMenuItem("Delete");
            deleteItem.Click += DeleteItem_Click;
            _contextMenuStrip.Items.Add(deleteItem);

            AllocConsole();
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
                UpdateStatus(@"No valid target download URLs specified.");
                return;
            }

            download.Enabled = false;

            var subfolder = downloadFolder.Text;

            for (var i = 0; i < urls.Length; i++)
            {
                _fileUtils.EnsureTempExists();

                UpdateStatus("Building download config file... ");
                SetupConfigFile();
                UpdateStatus(@"Download config file written!");

                UpdateStatus($@"Downloading video {i + 1} of {urls.Length}...");
                await ProcessUtils.RunDownloader(urls[i], _runFromPath, _fileUtils.GetTempPath());
                UpdateStatus($@"Video {i + 1} successfully downloaded!");

                UpdateStatus(@"Moving downloaded files to specified folder... ");
                _fileUtils.MoveTempFiles(subfolder);
                UpdateStatus(@"Downloaded files have been successfully moved!");
            }

            download.Enabled = true;

            File.Delete(_configPath);

            await UpdateMediaTable();
        }

        private void OptionAudioOnly_CheckedChanged(object sender, EventArgs e)
        {
            optionAddThumbnails.Checked = !optionAudioOnly.Checked;
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

            var path = mediaFilesTable
                .Rows[hitTest.RowIndex]
                .Cells["FullPath"]
                .Value
                .ToString();
            if (FileUtils.TrashPath(path!) != 0)
            {
                UpdateStatus("Failed to send file to the trash!");
                return;
            }
            else
            {
                Console.WriteLine(@$"File '{path}' has been sent to the trash.");
            }

            mediaFilesTable.Rows.RemoveAt(hitTest.RowIndex);
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
            _contextMenuStrip.Show(mediaFilesTable, e.Location);
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
                case { Control: true, KeyCode: Keys.T }:
                    MessageBox.Show(@$"Total Media Duration: {SecondsToDuration(_totalDuration, true)}",
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

        private void HideShowConsole()
        {
            var state = _consoleShown ? SW_HIDE : SW_SHOW;
            ShowWindow(GetConsoleWindow(), state);
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

            UpdateStatus(@"Reloading media file list... ");

            mediaFilesTable.ClearSelection();
            mediaFilesTable.Rows.Clear();
            _totalDuration = 0;

            await FillTable();

            mediaFilesTable.Sort(mediaFilesTable.Columns["Duration"]!, ListSortDirection.Ascending);

            UpdateStatus(@"Media list successfully reloaded!");

            _isUpdatingMediaList = false;
        }

        public void FindEntry(string searchStr, bool single)
        {
            mediaFilesTable.ClearSelection();

            var hasChangedRow = false;
            for (var i = 0; i < mediaFilesTable.Rows.Count; i++)
            {
                var title = mediaFilesTable.Rows[i].Cells["Title"].Value.ToString();

                var match = Regex.Match(title!, searchStr, RegexOptions.IgnoreCase);
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
            var results = new List<(string Duration, string LastModified, string Title, string FullPath)>();

            var directoryInfo = new DirectoryInfo(_fileUtils.GetMediaPath());

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
                
                results.Add((SecondsToDuration(dur, false),
                    modified.ToString(CultureInfo.CurrentCulture),
                    filePath,
                    file.FullName));

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

        private static string SecondsToDuration(double seconds, bool longFormat)
        {
            var format = longFormat ? @"dd\:hh\:mm\:ss" : @"hh\:mm\:ss";
            return TimeSpan.FromSeconds(seconds).ToString(format);
        }

        public void UpdateStatus(string status)
        {
            toolStripStatusLabel1.Text = status;
            Console.WriteLine(status);
        }
    }
}

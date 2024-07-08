using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

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
        private const string FFProbePath = @"D:\Projects\Video Encoding\ffmpeg-7.0.1\bin\ffprobe.exe";
        private const bool TestMode = false;

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

        private const int FO_DELETE = 0x0003;
        private const int FOF_ALLOWUNDO = 0x0040; // Preserve undo information, if possible.
        private const int FOF_NOCONFIRMATION = 0x0010; // Show no confirmation dialog box to the user.

        // Struct which contains information that the SHFileOperation function uses to perform file operations.
        // See: https://learn.microsoft.com/en-us/windows/win32/api/shellapi/ns-shellapi-shfileopstructw
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct SHFILEOPSTRUCTW
        {
            public IntPtr hWnd;
            [MarshalAs(UnmanagedType.U4)]
            public int wFunc;
            public string pFrom;
            public string pTo;
            public short fFlags;
            [MarshalAs(UnmanagedType.Bool)]
            public bool fAnyOperationsAborted;
            public IntPtr hNameMappings;
            public string lpszProgressTitle;
        }

        [DllImport("shell32.dll", CharSet = CharSet.Auto)]
        static extern int SHFileOperationW(ref SHFILEOPSTRUCTW fileOp);

        #endregion

        public Form1(string runFromPath)
        {
            _runFromPath = runFromPath;
            _configTemplatePath = Path.Combine(runFromPath, "yt-dlp.conf-template");
            _configPath = Path.Combine(runFromPath, "yt-dlp.conf");

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

        private void ReloadMediaFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UpdateMediaTable();
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
                EnsureTempExists();

                UpdateStatus(@"Building download config file... ");
                SetupConfigFile();
                UpdateStatus(@"Download config file written!");

                UpdateStatus($@"Downloading video {i + 1} of {urls.Length}...");
                await RunDownloader(urls[i]);
                UpdateStatus($@"Video {i + 1} successfully downloaded!");

                UpdateStatus(@"Moving downloaded files to specified folder... ");
                MoveTempFiles(subfolder);
                UpdateStatus(@"Downloaded files have been successfully moved!");
            }

            download.Enabled = true;

            File.Delete(_configPath);

            UpdateMediaTable();
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
            TrashPath(path!);

            Console.WriteLine(@$"File '{path}' has been sent to the trash.");

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

        private void Form1_KeyDown(object sender, KeyEventArgs e)
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
                    UpdateMediaTable();
                    break;
            }
        }

        public void TrashPath(string path)
        {
            // Note that the specification requires a double null termination here.
            // See: https://learn.microsoft.com/en-us/windows/win32/api/shellapi/ns-shellapi-shfileopstructa
            var fileOp = new SHFILEOPSTRUCTW
            {
                wFunc = FO_DELETE,
                pFrom = $"{path}\0\0",
                fFlags = FOF_ALLOWUNDO | FOF_NOCONFIRMATION
            };
            if (SHFileOperationW(ref fileOp) != 0)
            {
                UpdateStatus("Failed to send file to the trash!");
            }
        }

        private void HideShowConsole()
        {
            var state = _consoleShown ? SW_HIDE : SW_SHOW;
            ShowWindow(GetConsoleWindow(), state);
            _consoleShown = !_consoleShown;
            showConsoleToolStripMenuItem.Text = _consoleShown ? "Hide &Console..." : "Show &Console...";
        }

        private async Task RunDownloader(string downloadUrl)
        {
            var tcs = new TaskCompletionSource<bool>();
            var process = new Process();
            process.StartInfo.FileName = $"{_runFromPath}\\yt-dlp.exe";
            process.StartInfo.Arguments = downloadUrl;
            process.StartInfo.WorkingDirectory = GetTempPath();
            process.EnableRaisingEvents = true;
            process.Exited += (sender, args) =>
            {
                tcs.SetResult(true);
                process.Dispose();
            };

            process.Start();

            await tcs.Task;
        }

        private void MoveTempFiles(string? destFolder)
        {
            var mediaDir = GetMediaPath();
            if (destFolder is not null)
            {
                mediaDir = Path.Combine(mediaDir, destFolder);
            }

            MoveDirectoryContents(GetTempPath(), mediaDir);
        }

        private void MoveDirectoryContents(string sourceDir, string destDir)
        {
            if (!Directory.Exists(sourceDir))
            {
                UpdateStatus(@"Error! Source directory does not exist!");
                return;
            }

            if (!Directory.Exists(destDir))
            {
                Directory.CreateDirectory(destDir);
            }

            // Move subdirectories recursively.
            var directories = Directory.GetDirectories(sourceDir);
            foreach (var directory in directories)
            {
                var dirName = Path.GetFileName(directory);
                var destDirectory = Path.Combine(destDir, dirName);

                MoveDirectoryContents(directory, destDirectory);
            }

            // Move the individual files.
            var files = Directory.GetFiles(sourceDir);
            foreach (var file in files)
            {
                var fileName = Path.GetFileName(file);
                var destFile = Path.Combine(destDir, fileName);

                try
                {
                    File.Move(file, destFile, true);
                }
                catch (Exception ex)
                {
                    UpdateStatus(@$"Error! Failed to move file! {ex.Message}");
                    return;
                }
            }

            if (sourceDir != GetTempPath())
            {
                Directory.Delete(sourceDir);
            }
        }

        private string GetTempPath()
        {
            return Path.Combine(_runFromPath, "temp");
        }

        private string GetMediaPath()
        {
            return Path.GetFullPath(Path.Combine(_runFromPath, "..\\"));
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

        private void UpdateMediaTable()
        {
            UpdateStatus(@"Reloading media file list... ");

            mediaFilesTable.ClearSelection();
            mediaFilesTable.Rows.Clear();
            _totalDuration = 0;

            FillTable();

            mediaFilesTable.Sort(mediaFilesTable.Columns["Duration"]!, ListSortDirection.Ascending);

            UpdateStatus(@"Media list successfully reloaded!");
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

        private static double RunMediaInfo(string path)
        {
            var process = new Process();
            process.StartInfo.FileName = FFProbePath;
            process.StartInfo.Arguments = $"-show_entries format=duration -v quiet -of csv=\"p=0\" \"{path}\"";
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.CreateNoWindow = true;
            process.Start();

            var output = process.StandardOutput.ReadToEnd();

            process.WaitForExit();

            if (output.Length == 0)
            {
                return 0;
            }
            else
            {
                return double.Parse(output);
            }
        }

        private void FillTable()
        {
            var directoryInfo = new DirectoryInfo(GetMediaPath());

            // Iterate over each file in the directory.
            var i = 0;
            foreach (var file in directoryInfo.GetFiles())
            {
                var dur = RunMediaInfo(file.FullName);
                if (dur == 0)
                {
                    continue;
                }

                _totalDuration += dur;

                var filePath = Path.GetFileNameWithoutExtension(file.FullName);
                var modified = file.LastWriteTime;
                mediaFilesTable.Rows.Add([
                        SecondsToDuration(dur, false),
                        modified,
                        filePath,
                        file.FullName
                    ]);

                if (TestMode && i == 10)
                {
                    break;
                }

                ++i;
            }
        }

        private void EnsureTempExists()
        {
            var tempDir = GetTempPath();
            if (!Directory.Exists(tempDir))
            {
                Directory.CreateDirectory(tempDir);
            }
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

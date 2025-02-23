namespace MediaTools
{
    public partial class OptionsForm : Form
    {
        private readonly ErrorProvider _errorProvider = new();
        private readonly MainForm _parent;

        public OptionsForm(MainForm parent)
        {
            IconModifier.SetFormIcon(this);

            InitializeComponent();

            var settings = Program.appSettings;
            optionShowFolders.Checked = settings.ShowFolders;
            optionShowConsole.Checked = settings.ShowConsole;
            optionIncludeSubMedia.Checked = settings.ShowMediaInSubFolders;
            optionCookiePath.Text = settings.CookiePath;
            optionRememberDownloadOpts.Checked = settings.RememberDownloadOptions;
            optionTempDirectory.Text = settings.TempDirectory;
            optionMediaDirectory.Text = settings.MediaDirectory;
            optionFfmpegDirectory.Text = settings.FfmpegDirectory;
            optionYtdlpPath.Text = settings.YtDlpPath;
            optionPlayerPath.Text = settings.MediaPlayerPath;

            toolTip1.SetToolTip(optionShowFolders, "Should the folders column be showed in the media list?");
            toolTip1.SetToolTip(optionShowConsole, "Should the output console be shown?");
            toolTip1.SetToolTip(optionRememberDownloadOpts, "Should the downloading options persist between runs?");
            toolTip1.SetToolTip(optionIncludeSubMedia, "Should media from sub-folders be included in the media list?");
            toolTip1.SetToolTip(optionCookiePath, "The profile to be used for the YouTube cookie path.");
            toolTip1.SetToolTip(optionMediaDirectory, "The path to the target media directory.");
            toolTip1.SetToolTip(optionTempDirectory, "The path to the temporary downloads directory. If unset the system temporary path will be used.");
            toolTip1.SetToolTip(optionFfmpegDirectory, "The path to the FFmpeg directory.");
            toolTip1.SetToolTip(optionYtdlpPath, "The path to the yt-dlp binary.");
            toolTip1.SetToolTip(optionPlayerPath, "The path to the media player binary. If unset the system default will be used.");

            _parent = parent;
        }

        private async void Ok_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs())
            {
                return;
            }

            var settings = Program.appSettings;

            var mediaDirty =
                settings.ShowMediaInSubFolders != optionIncludeSubMedia.Checked;

            var consoleDirty = settings.ShowConsole != optionShowConsole.Checked;

            Program.appSettings = new AppSettings
            {
                ShowFolders = optionShowFolders.Checked,
                ShowConsole = optionShowConsole.Checked,
                ShowMediaInSubFolders = optionIncludeSubMedia.Checked,
                CookiePath = optionCookiePath.Text,
                RememberDownloadOptions = optionRememberDownloadOpts.Checked,
                MediaDirectory = FileUtils.FullyResolvePath(optionMediaDirectory.Text),
                TempDirectory = FileUtils.FullyResolvePath(optionTempDirectory.Text),
                FfmpegDirectory = FileUtils.FullyResolvePath(optionFfmpegDirectory.Text),
                YtDlpPath = FileUtils.FullyResolvePath(optionYtdlpPath.Text),
                MediaPlayerPath = FileUtils.FullyResolvePath(optionPlayerPath.Text)
            };

            // These config settings alter key components that define how the program operates.
            // A restart will be needed for them to take effect.
            var needsRestart = Program.appSettings.MediaDirectory != settings.MediaDirectory ||
                               Program.appSettings.YtDlpPath != settings.YtDlpPath;

            settings.WriteSettings();

            _parent.SetFoldersColumnVisibility(optionShowFolders.Checked);
            _parent.SetNeedsRestart(needsRestart);

            if (mediaDirty)
            {
                await _parent.UpdateMediaTable(false);
            }

            if (consoleDirty)
            {
                _parent.HideShowConsole(optionShowConsole.Checked);
            }

            Close();
        }

        private void BrowseFile_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            var name = (string)((Control)sender).Tag!;
            var control = Controls.Find(name, true).FirstOrDefault();
            if (control is not null)
            {
                ((TextBox)control).Text = openFileDialog1.FileName;
            }
        }

        private void BrowseFolder_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            var name = (string)((Control)sender).Tag!;
            var control = Controls.Find(name, true).FirstOrDefault();
            if (control is not null)
            {
                ((TextBox)control).Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private bool ValidateInputs()
        {
            ResolvePath(optionMediaDirectory);
            ResolvePath(optionTempDirectory);
            ResolvePath(optionFfmpegDirectory);
            ResolvePath(optionYtdlpPath);
            ResolvePath(optionPlayerPath);

            var mediaDirectory = optionMediaDirectory.Text;
            var mediaDirectoryValid = string.IsNullOrWhiteSpace(mediaDirectory) || Directory.Exists(mediaDirectory);
            _errorProvider.SetError(optionMediaDirectory,
                !mediaDirectoryValid ? "Please provide a valid media directory path." : "");

            var tempDirectory = optionTempDirectory.Text;
            var tempDirectoryValid = !string.IsNullOrWhiteSpace(tempDirectory) && Directory.Exists(tempDirectory);
            _errorProvider.SetError(optionTempDirectory,
                !tempDirectoryValid ? "Please provide a valid temporary directory path." : "");

            var ffmpegDirectory = optionFfmpegDirectory.Text;
            var ffmpegDirectoryValid = !string.IsNullOrWhiteSpace(ffmpegDirectory) && Directory.Exists(ffmpegDirectory);
            _errorProvider.SetError(optionFfmpegDirectory,
                !ffmpegDirectoryValid ? "Please provide a valid ffmpeg directory path." : "");

            var ytDlpPath = optionYtdlpPath.Text;
            var ytDlpPathValid = File.Exists(ytDlpPath);
            _errorProvider.SetError(optionYtdlpPath,
                !ytDlpPathValid ? "Please provide a valid path to the YT-DLP binary." : "");

            var mediaPlayerPath = optionPlayerPath.Text;
            var mediaPlayerPathValid = string.IsNullOrWhiteSpace(mediaPlayerPath) || File.Exists(mediaPlayerPath);
            _errorProvider.SetError(optionPlayerPath,
                !mediaPlayerPathValid ? "Please provide a valid path to a media player." : "");

            return
                mediaDirectoryValid && tempDirectoryValid && ffmpegDirectoryValid &&
                ytDlpPathValid && mediaPlayerPathValid;
        }

        private static void ResolvePath(TextBox control)
        {
            control.Text = FileUtils.FullyResolvePath(control.Text);
        }
    }
}

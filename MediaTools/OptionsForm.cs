namespace MediaTools
{
    public partial class OptionsForm : Form
    {
        private readonly MainForm _parent;

        public OptionsForm(MainForm parent)
        {
            IconModifier.SetFormIcon(this);

            InitializeComponent();

            var settings = Program.appSettings;
            optionIncludeFolders.Checked = settings.ShowFolders;
            optionIncludeSubMedia.Checked = settings.ShowMediaInSubFolders;
            optionCookiePath.Text = settings.CookiePath;
            optionRememberDownloadOpts.Checked = settings.RememberDownloadOptions;

            optionTempDirectory.Text = settings.TempDirectory;
            optionMediaDirectory.Text = settings.MediaDirectory;
            optionFfprobePath.Text = settings.FfprobePath;
            optionYtdlpPath.Text = settings.YtDlpPath;
            optionPlayerPath.Text = settings.MediaPlayerPath;

            toolTip1.SetToolTip(optionIncludeFolders, "Should the folders column be showed in the media list?");
            toolTip1.SetToolTip(optionRememberDownloadOpts, "Should the downloading options persist between runs?");
            toolTip1.SetToolTip(optionIncludeSubMedia, "Should media from sub-folders be included in the media list?");
            toolTip1.SetToolTip(optionCookiePath, "The profile to be used for the YouTube cookie path.");

            toolTip1.SetToolTip(optionMediaDirectory, "The path to the target media directory.");
            toolTip1.SetToolTip(optionTempDirectory, "The path to the temporary downloads directory. If unset the system temporary path will be used.");
            toolTip1.SetToolTip(optionFfprobePath, "The path to the FFprobe binary.");
            toolTip1.SetToolTip(optionYtdlpPath, "The path to the yt-dlp binary.");
            toolTip1.SetToolTip(optionPlayerPath, "The path to the media player binary. If unset the system default will be used.");

            _parent = parent;
        }

        private async void Ok_Click(object sender, EventArgs e)
        {
            var settings = Program.appSettings;

            var mediaDirty =
                settings.MediaDirectory != optionMediaDirectory.Text ||
                settings.ShowMediaInSubFolders != optionIncludeSubMedia.Checked;

            Program.appSettings = new AppSettings
            {
                ShowFolders = optionIncludeFolders.Checked,
                ShowMediaInSubFolders = optionIncludeSubMedia.Checked,
                CookiePath = optionCookiePath.Text,
                RememberDownloadOptions = optionRememberDownloadOpts.Checked,
                MediaDirectory = optionMediaDirectory.Text,
                TempDirectory = optionTempDirectory.Text,
                FfprobePath = optionFfprobePath.Text,
                YtDlpPath = optionYtdlpPath.Text,
                MediaPlayerPath = optionPlayerPath.Text
            };

            settings.WriteSettings();

            _parent.SetFoldersColumnVisibility(optionIncludeFolders.Checked);

            // If the media list is now considered dirty, we need to reload it.
            if (mediaDirty)
            {
                await _parent.UpdateMediaTable(false);
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
    }
}

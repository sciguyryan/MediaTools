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
            optionMediaDirectory.Text = settings.MediaDirectory;
            optionIncludeFolders.Checked = settings.ShowFolders;
            optionIncludeSubMedia.Checked = settings.ShowMediaInSubFolders;
            optionCookiePath.Text = settings.CookiePath;
            optionRememberDownloadOpts.Checked = settings.RememberDownloadOptions;
            optionFfprobePath.Text = settings.FfprobePath;
            optionYtdlpPath.Text = settings.YtDlpPath;
            optionPlayerPath.Text = settings.MediaPlayerPath;

            _parent = parent;
        }

        private async void Ok_Click(object sender, EventArgs e)
        {
            var settings = Program.appSettings;

            var mediaDirty =
                settings.MediaDirectory != optionMediaDirectory.Text ||
                settings.ShowMediaInSubFolders != optionIncludeSubMedia.Checked;

            settings = new AppSettings
            {
                MediaDirectory = optionMediaDirectory.Text,
                ShowFolders = optionIncludeFolders.Checked,
                ShowMediaInSubFolders = optionIncludeSubMedia.Checked,
                CookiePath = optionCookiePath.Text,
                RememberDownloadOptions = optionRememberDownloadOpts.Checked,
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

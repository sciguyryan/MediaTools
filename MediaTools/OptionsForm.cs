namespace MediaTools
{
    public partial class OptionsForm : Form
    {
        private readonly MainForm _parent;

        private readonly bool _subMediaDirty;

        public OptionsForm(MainForm parent)
        {
            IconModifier.SetFormIcon(this);

            InitializeComponent();

            var settings = Program.appAppSettings;
            optionIncludeFolders.Checked = settings.ShowFolders;
            optionIncludeSubMedia.Checked = settings.ShowMediaInSubFolders;
            optionCookiePath.Text = settings.CookiePath;
            optionRememberDownloadOpts.Checked = settings.RememberDownloadOptions;
            optionFfprobePath.Text = settings.FfprobePath;
            optionYtdlpPath.Text = settings.YtDlpPath;
            optionPlayerPath.Text = settings.MediaPlayerPath;

            _parent = parent;
            _subMediaDirty = settings.ShowMediaInSubFolders;
        }

        private async void Ok_Click(object sender, EventArgs e)
        {
            Program.appAppSettings = new AppSettings
            {
                ShowFolders = optionIncludeFolders.Checked,
                ShowMediaInSubFolders = optionIncludeSubMedia.Checked,
                CookiePath = optionCookiePath.Text,
                RememberDownloadOptions = optionRememberDownloadOpts.Checked,
                FfprobePath = optionFfprobePath.Text,
                YtDlpPath = optionYtdlpPath.Text,
                MediaPlayerPath = optionPlayerPath.Text
            };

            Program.appAppSettings.WriteSettings();

            _parent.SetFoldersColumnVisibility(optionIncludeFolders.Checked);

            // If this setting has been changed, we need to perform a full
            // reload of the media list.
            if (_subMediaDirty != Program.appAppSettings.ShowMediaInSubFolders)
            {
                await _parent.UpdateMediaTable(false);
            }

            Close();
        }

        private void Browse_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            // Get the associated control and add the value to the relevant field.
            var controlName = (string)((Button)sender).Tag!;
            var foundControls = this.Controls.Find(controlName, true);
            if (foundControls.Length > 0)
            {
                foundControls[0].Text = openFileDialog1.FileName;
            }
        }
    }
}

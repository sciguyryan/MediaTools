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

            optionIncludeFolders.Checked = Program.AppSettings.ShowFolders;
            optionIncludeSubMedia.Checked = Program.AppSettings.ShowMediaInSubFolders;
            optionCookiePath.Text = Program.AppSettings.CookiePath;
            optionRememberDownloadOpts.Checked = Program.AppSettings.RememberDownloadOptions;
            optionFfprobePath.Text = Program.AppSettings.FfprobePath;
            optionYtdlpPath.Text = Program.AppSettings.YtDlpPath;

            _parent = parent;
            _subMediaDirty = Program.AppSettings.ShowMediaInSubFolders;
        }

        private async void Ok_Click(object sender, EventArgs e)
        {
            Program.AppSettings.ShowFolders = optionIncludeFolders.Checked;
            Program.AppSettings.ShowMediaInSubFolders = optionIncludeSubMedia.Checked;
            Program.AppSettings.CookiePath = optionCookiePath.Text;
            Program.AppSettings.RememberDownloadOptions = optionRememberDownloadOpts.Checked;
            Program.AppSettings.FfprobePath = optionFfprobePath.Text;
            Program.AppSettings.YtDlpPath = optionYtdlpPath.Text;

            Program.AppSettings.WriteSettings();

            _parent.SetFoldersColumnVisibility(optionIncludeFolders.Checked);

            // If this setting has been changed, we need to perform a full
            // reload of the media list.
            if (_subMediaDirty != Program.AppSettings.ShowMediaInSubFolders)
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

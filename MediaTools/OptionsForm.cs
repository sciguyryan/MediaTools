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

            _parent = parent;
            _subMediaDirty = Program.AppSettings.ShowMediaInSubFolders;
        }

        private async void Ok_Click(object sender, EventArgs e)
        {
            Program.AppSettings.ShowFolders = optionIncludeFolders.Checked;
            Program.AppSettings.ShowMediaInSubFolders = optionIncludeSubMedia.Checked;
            Program.AppSettings.CookiePath = optionCookiePath.Text;
            Program.AppSettings.RememberDownloadOptions = optionRememberDownloadOpts.Checked;

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
    }
}

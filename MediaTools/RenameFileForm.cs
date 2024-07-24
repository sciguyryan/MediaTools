namespace MediaTools
{
    public partial class RenameFileForm : Form
    {
        public string NewFileName = "";

        public RenameFileForm(string oldName)
        {
            IconModifier.SetFormIcon(this);

            InitializeComponent();

            fileName.Text = oldName;
        }

        private void Rename_Click(object sender, EventArgs e)
        {
            if (!IsValidFileName(fileName.Text))
            {
                MessageBox.Show(
                    DisplayBuilders.InvalidFileName.BuildPlain([]),
                    DisplayBuilders.InvalidFileNameTitle.BuildPlain([]),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error,
                    MessageBoxDefaultButton.Button1
                );

                return;
            }

            NewFileName = fileName.Text;

            Close();
        }

        private void RenameFileForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Return)
            {
                return;
            }

            rename.PerformClick();
            e.Handled = true;
        }

        private static bool IsValidFileName(string name)
        {
            return !Path.GetInvalidFileNameChars().Any(name.Contains);
        }
    }
}

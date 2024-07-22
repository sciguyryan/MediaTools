namespace MediaTools
{
    public partial class SearchForm : Form
    {
        private readonly MainForm _parent;

        public SearchForm(MainForm parent)
        {
            IconModifier.SetFormIcon(this);

            this._parent = parent;

            InitializeComponent();
        }

        private void Next_Click(object sender, EventArgs e)
        {
            var findType = regularExpression.Checked ? MainForm.FindType.Regex : MainForm.FindType.Text;

            _parent.FindEntry(searchString.Text, "Title", findType, true,
                exactMatch.Checked, ignoreCase.Checked, true, true);
        }

        private void Find_Click(object sender, EventArgs e)
        {
            var findType = regularExpression.Checked ? MainForm.FindType.Regex : MainForm.FindType.Text;

            _parent.FindEntry(searchString.Text, "Title", findType, true,
                exactMatch.Checked, ignoreCase.Checked);
        }

        private void FindAll_Click(object sender, EventArgs e)
        {
            var findType = regularExpression.Checked ? MainForm.FindType.Regex : MainForm.FindType.Text;

            _parent.FindEntry(searchString.Text, "Title", findType, false,
                exactMatch.Checked, ignoreCase.Checked);
        }

        private void SearchString_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Return)
            {
                return;
            }

            find.PerformClick();
            e.Handled = true;
        }

        private void RegularExpression_CheckedChanged(object sender, EventArgs e)
        {
            if (regularExpression.Checked)
            {
                exactMatch.Checked = false;
                exactMatch.Enabled = false;

                ignoreCase.Checked = false;
                ignoreCase.Enabled = false;
            }
            else
            {
                exactMatch.Enabled = true;
                ignoreCase.Enabled = true;
            }
        }
    }
}
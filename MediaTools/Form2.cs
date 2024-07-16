namespace MediaTools
{
    public partial class Form2 : Form
    {
        private readonly Form1 _parent;

        public Form2(Form1 parent)
        {
            IconModifier.SetFormIcon(this);

            this._parent = parent;

            InitializeComponent();
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Find_Click(object sender, EventArgs e)
        {
            _parent.FindEntry(searchString.Text, "Title", true, Form1.FindType.Regex);
        }

        private void FindAll_Click(object sender, EventArgs e)
        {
            _parent.FindEntry(searchString.Text, "Title", false, Form1.FindType.Regex);
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
    }
}

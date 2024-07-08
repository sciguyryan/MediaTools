using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MediaTools
{
    public partial class Form2 : Form
    {
        private Form1 parent;

        public Form2(Form1 parent)
        {
            this.parent = parent;

            InitializeComponent();
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Find_Click(object sender, EventArgs e)
        {
            parent.FindEntry(searchString.Text, true);
        }

        private void FindAll_Click(object sender, EventArgs e)
        {
            parent.FindEntry(searchString.Text, false);
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

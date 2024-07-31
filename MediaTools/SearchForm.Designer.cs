namespace MediaTools
{
    partial class SearchForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            searchString = new TextBox();
            find = new Button();
            next = new Button();
            findAll = new Button();
            regularExpression = new CheckBox();
            exactMatch = new CheckBox();
            ignoreCase = new CheckBox();
            SuspendLayout();
            // 
            // searchString
            // 
            searchString.Location = new Point(12, 12);
            searchString.Name = "searchString";
            searchString.Size = new Size(294, 27);
            searchString.TabIndex = 0;
            searchString.KeyPress += SearchString_KeyPress;
            // 
            // find
            // 
            find.Location = new Point(12, 45);
            find.Name = "find";
            find.Size = new Size(94, 29);
            find.TabIndex = 1;
            find.Text = "&Find";
            find.UseVisualStyleBackColor = true;
            find.Click += Find_Click;
            // 
            // next
            // 
            next.Location = new Point(109, 45);
            next.Name = "next";
            next.Size = new Size(94, 29);
            next.TabIndex = 3;
            next.Text = "&Next";
            next.UseVisualStyleBackColor = true;
            next.Click += Next_Click;
            // 
            // findAll
            // 
            findAll.Location = new Point(209, 45);
            findAll.Name = "findAll";
            findAll.Size = new Size(94, 29);
            findAll.TabIndex = 2;
            findAll.Text = "Find &All";
            findAll.UseVisualStyleBackColor = true;
            findAll.Click += FindAll_Click;
            // 
            // regularExpression
            // 
            regularExpression.AutoSize = true;
            regularExpression.Location = new Point(12, 82);
            regularExpression.Name = "regularExpression";
            regularExpression.Size = new Size(191, 24);
            regularExpression.TabIndex = 4;
            regularExpression.Text = "&Use Regular Expression?";
            regularExpression.UseVisualStyleBackColor = true;
            regularExpression.CheckedChanged += RegularExpression_CheckedChanged;
            // 
            // exactMatch
            // 
            exactMatch.AutoSize = true;
            exactMatch.Location = new Point(12, 107);
            exactMatch.Name = "exactMatch";
            exactMatch.Size = new Size(118, 24);
            exactMatch.TabIndex = 5;
            exactMatch.Text = "&Exact Match?";
            exactMatch.UseVisualStyleBackColor = true;
            // 
            // ignoreCase
            // 
            ignoreCase.AutoSize = true;
            ignoreCase.Checked = true;
            ignoreCase.CheckState = CheckState.Checked;
            ignoreCase.Location = new Point(12, 135);
            ignoreCase.Name = "ignoreCase";
            ignoreCase.Size = new Size(116, 24);
            ignoreCase.TabIndex = 6;
            ignoreCase.Text = "&Ignore Case?";
            ignoreCase.UseVisualStyleBackColor = true;
            // 
            // SearchForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(318, 171);
            Controls.Add(ignoreCase);
            Controls.Add(exactMatch);
            Controls.Add(regularExpression);
            Controls.Add(findAll);
            Controls.Add(next);
            Controls.Add(find);
            Controls.Add(searchString);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            KeyPreview = true;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "SearchForm";
            ShowInTaskbar = false;
            SizeGripStyle = SizeGripStyle.Hide;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Media Tools :: Search";
            FormClosed += SearchForm_FormClosed;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox searchString;
        private Button find;
        private Button next;
        private Button findAll;
        private CheckBox regularExpression;
        private CheckBox exactMatch;
        private CheckBox ignoreCase;
    }
}
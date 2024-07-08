namespace MediaTools
{
    partial class Form2
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
            cancel = new Button();
            findAll = new Button();
            SuspendLayout();
            // 
            // searchString
            // 
            searchString.Location = new Point(12, 12);
            searchString.Name = "searchString";
            searchString.Size = new Size(402, 27);
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
            // cancel
            // 
            cancel.Location = new Point(212, 47);
            cancel.Name = "cancel";
            cancel.Size = new Size(94, 29);
            cancel.TabIndex = 3;
            cancel.Text = "&Cancel";
            cancel.UseVisualStyleBackColor = true;
            cancel.Click += Cancel_Click;
            // 
            // findAll
            // 
            findAll.Location = new Point(112, 47);
            findAll.Name = "findAll";
            findAll.Size = new Size(94, 29);
            findAll.TabIndex = 2;
            findAll.Text = "Find &All";
            findAll.UseVisualStyleBackColor = true;
            findAll.Click += FindAll_Click;
            // 
            // Form2
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(426, 88);
            Controls.Add(findAll);
            Controls.Add(cancel);
            Controls.Add(find);
            Controls.Add(searchString);
            KeyPreview = true;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Form2";
            ShowInTaskbar = false;
            SizeGripStyle = SizeGripStyle.Hide;
            Text = "Media Tools";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox searchString;
        private Button find;
        private Button cancel;
        private Button findAll;
    }
}
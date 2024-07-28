namespace MediaTools
{
    partial class OptionsForm
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
            optionIncludeFolders = new CheckBox();
            optionIncludeSubMedia = new CheckBox();
            okButton = new Button();
            groupBox1 = new GroupBox();
            groupBox2 = new GroupBox();
            optionCookiePath = new TextBox();
            label1 = new Label();
            groupBox3 = new GroupBox();
            optionRememberDownloadOpts = new CheckBox();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox3.SuspendLayout();
            SuspendLayout();
            // 
            // optionIncludeFolders
            // 
            optionIncludeFolders.AutoSize = true;
            optionIncludeFolders.Location = new Point(11, 27);
            optionIncludeFolders.Name = "optionIncludeFolders";
            optionIncludeFolders.Size = new Size(126, 24);
            optionIncludeFolders.TabIndex = 1;
            optionIncludeFolders.Text = "Show &Folders?";
            optionIncludeFolders.UseVisualStyleBackColor = true;
            // 
            // optionIncludeSubMedia
            // 
            optionIncludeSubMedia.AutoSize = true;
            optionIncludeSubMedia.Location = new Point(11, 57);
            optionIncludeSubMedia.Name = "optionIncludeSubMedia";
            optionIncludeSubMedia.Size = new Size(239, 24);
            optionIncludeSubMedia.TabIndex = 2;
            optionIncludeSubMedia.Text = "Show &Media From Sub Folders?";
            optionIncludeSubMedia.UseVisualStyleBackColor = true;
            // 
            // okButton
            // 
            okButton.Location = new Point(12, 243);
            okButton.Name = "okButton";
            okButton.Size = new Size(94, 29);
            okButton.TabIndex = 3;
            okButton.Text = "&OK";
            okButton.UseVisualStyleBackColor = true;
            okButton.Click += Ok_Click;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(optionIncludeSubMedia);
            groupBox1.Controls.Add(optionIncludeFolders);
            groupBox1.Location = new Point(12, 12);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(250, 91);
            groupBox1.TabIndex = 4;
            groupBox1.TabStop = false;
            groupBox1.Text = "Display Options";
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(optionCookiePath);
            groupBox2.Controls.Add(label1);
            groupBox2.Location = new Point(268, 12);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(357, 91);
            groupBox2.TabIndex = 5;
            groupBox2.TabStop = false;
            groupBox2.Text = "Download Options";
            // 
            // optionCookiePath
            // 
            optionCookiePath.Location = new Point(102, 27);
            optionCookiePath.Name = "optionCookiePath";
            optionCookiePath.Size = new Size(249, 27);
            optionCookiePath.TabIndex = 6;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(6, 34);
            label1.Name = "label1";
            label1.Size = new Size(90, 20);
            label1.TabIndex = 5;
            label1.Text = "&Cookie Path:";
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(optionRememberDownloadOpts);
            groupBox3.Location = new Point(12, 109);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(256, 91);
            groupBox3.TabIndex = 6;
            groupBox3.TabStop = false;
            groupBox3.Text = "Other Options";
            // 
            // optionRememberDownloadOpts
            // 
            optionRememberDownloadOpts.AutoSize = true;
            optionRememberDownloadOpts.Checked = true;
            optionRememberDownloadOpts.CheckState = CheckState.Checked;
            optionRememberDownloadOpts.Location = new Point(11, 27);
            optionRememberDownloadOpts.Name = "optionRememberDownloadOpts";
            optionRememberDownloadOpts.Size = new Size(241, 24);
            optionRememberDownloadOpts.TabIndex = 1;
            optionRememberDownloadOpts.Text = "Remember Download Settings?";
            optionRememberDownloadOpts.UseVisualStyleBackColor = true;
            // 
            // OptionsForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(637, 284);
            Controls.Add(groupBox3);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Controls.Add(okButton);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "OptionsForm";
            SizeGripStyle = SizeGripStyle.Hide;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Media Tool :: Options";
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private CheckBox optionIncludeFolders;
        private CheckBox optionIncludeSubMedia;
        private Button okButton;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private Label label1;
        private TextBox optionCookiePath;
        private GroupBox groupBox3;
        private CheckBox optionRememberDownloadOpts;
    }
}
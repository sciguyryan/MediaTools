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
            groupBox4 = new GroupBox();
            button3 = new Button();
            optionMediaDirectory = new TextBox();
            label4 = new Label();
            button2 = new Button();
            label5 = new Label();
            optionPlayerPath = new TextBox();
            optionYtdlpPath = new TextBox();
            button1 = new Button();
            label3 = new Label();
            browseFfmpeg = new Button();
            optionFfprobePath = new TextBox();
            label2 = new Label();
            openFileDialog1 = new OpenFileDialog();
            folderBrowserDialog1 = new FolderBrowserDialog();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox3.SuspendLayout();
            groupBox4.SuspendLayout();
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
            optionIncludeSubMedia.TabIndex = 1;
            optionIncludeSubMedia.Text = "Show &Media From Sub Folders?";
            optionIncludeSubMedia.UseVisualStyleBackColor = true;
            // 
            // okButton
            // 
            okButton.Location = new Point(12, 349);
            okButton.Name = "okButton";
            okButton.Size = new Size(94, 29);
            okButton.TabIndex = 10;
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
            optionCookiePath.TabIndex = 2;
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
            groupBox3.Size = new Size(256, 65);
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
            optionRememberDownloadOpts.TabIndex = 3;
            optionRememberDownloadOpts.Text = "Remember Download Settings?";
            optionRememberDownloadOpts.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            groupBox4.Controls.Add(button3);
            groupBox4.Controls.Add(optionMediaDirectory);
            groupBox4.Controls.Add(label4);
            groupBox4.Controls.Add(button2);
            groupBox4.Controls.Add(label5);
            groupBox4.Controls.Add(optionPlayerPath);
            groupBox4.Controls.Add(optionYtdlpPath);
            groupBox4.Controls.Add(button1);
            groupBox4.Controls.Add(label3);
            groupBox4.Controls.Add(browseFfmpeg);
            groupBox4.Controls.Add(optionFfprobePath);
            groupBox4.Controls.Add(label2);
            groupBox4.Location = new Point(12, 180);
            groupBox4.Name = "groupBox4";
            groupBox4.Size = new Size(607, 163);
            groupBox4.TabIndex = 7;
            groupBox4.TabStop = false;
            groupBox4.Text = "Paths";
            // 
            // button3
            // 
            button3.Location = new Point(526, 19);
            button3.Name = "button3";
            button3.Size = new Size(75, 29);
            button3.TabIndex = 17;
            button3.Tag = "optionMediaDirectory";
            button3.Text = "&Browse...";
            button3.UseVisualStyleBackColor = true;
            button3.Click += BrowseFolder_Click;
            // 
            // optionMediaDirectory
            // 
            optionMediaDirectory.Enabled = false;
            optionMediaDirectory.Location = new Point(136, 20);
            optionMediaDirectory.Name = "optionMediaDirectory";
            optionMediaDirectory.ReadOnly = true;
            optionMediaDirectory.Size = new Size(384, 27);
            optionMediaDirectory.TabIndex = 16;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(11, 23);
            label4.Name = "label4";
            label4.Size = new Size(119, 20);
            label4.TabIndex = 15;
            label4.Text = "Media Directory:";
            // 
            // button2
            // 
            button2.Location = new Point(526, 120);
            button2.Name = "button2";
            button2.Size = new Size(75, 29);
            button2.TabIndex = 9;
            button2.Tag = "optionPlayerPath";
            button2.Text = "&Browse...";
            button2.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(11, 124);
            label5.Name = "label5";
            label5.Size = new Size(98, 20);
            label5.TabIndex = 14;
            label5.Text = "Media Player:";
            // 
            // optionPlayerPath
            // 
            optionPlayerPath.Enabled = false;
            optionPlayerPath.Location = new Point(136, 121);
            optionPlayerPath.Name = "optionPlayerPath";
            optionPlayerPath.ReadOnly = true;
            optionPlayerPath.Size = new Size(384, 27);
            optionPlayerPath.TabIndex = 8;
            // 
            // optionYtdlpPath
            // 
            optionYtdlpPath.Enabled = false;
            optionYtdlpPath.Location = new Point(136, 86);
            optionYtdlpPath.Name = "optionYtdlpPath";
            optionYtdlpPath.ReadOnly = true;
            optionYtdlpPath.Size = new Size(384, 27);
            optionYtdlpPath.TabIndex = 6;
            // 
            // button1
            // 
            button1.Location = new Point(526, 85);
            button1.Name = "button1";
            button1.Size = new Size(75, 29);
            button1.TabIndex = 7;
            button1.Tag = "optionYtdlpPath";
            button1.Text = "&Browse...";
            button1.UseVisualStyleBackColor = true;
            button1.Click += BrowseFile_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(11, 89);
            label3.Name = "label3";
            label3.Size = new Size(56, 20);
            label3.TabIndex = 9;
            label3.Text = "yt-dlp: ";
            // 
            // browseFfmpeg
            // 
            browseFfmpeg.Location = new Point(526, 52);
            browseFfmpeg.Name = "browseFfmpeg";
            browseFfmpeg.Size = new Size(75, 29);
            browseFfmpeg.TabIndex = 5;
            browseFfmpeg.Tag = "optionFfprobePath";
            browseFfmpeg.Text = "&Browse...";
            browseFfmpeg.UseVisualStyleBackColor = true;
            browseFfmpeg.Click += BrowseFile_Click;
            // 
            // optionFfprobePath
            // 
            optionFfprobePath.Enabled = false;
            optionFfprobePath.Location = new Point(136, 53);
            optionFfprobePath.Name = "optionFfprobePath";
            optionFfprobePath.ReadOnly = true;
            optionFfprobePath.Size = new Size(384, 27);
            optionFfprobePath.TabIndex = 4;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(11, 56);
            label2.Name = "label2";
            label2.Size = new Size(70, 20);
            label2.TabIndex = 0;
            label2.Text = "FFprobe: ";
            // 
            // openFileDialog1
            // 
            openFileDialog1.Filter = "EXE Files (*.exe)|*.exe";
            openFileDialog1.Title = "Open Executable File";
            // 
            // OptionsForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(637, 386);
            Controls.Add(groupBox4);
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
            groupBox4.ResumeLayout(false);
            groupBox4.PerformLayout();
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
        private GroupBox groupBox4;
        private TextBox optionFfprobePath;
        private Label label2;
        private Button browseFfmpeg;
        private OpenFileDialog openFileDialog1;
        private TextBox optionYtdlpPath;
        private Button button1;
        private Label label3;
        private TextBox optionPlayerPath;
        private Button button2;
        private Label label5;
        private Button button3;
        private TextBox optionMediaDirectory;
        private Label label4;
        private FolderBrowserDialog folderBrowserDialog1;
    }
}
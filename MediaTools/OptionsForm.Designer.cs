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
            components = new System.ComponentModel.Container();
            okButton = new Button();
            openFileDialog1 = new OpenFileDialog();
            folderBrowserDialog1 = new FolderBrowserDialog();
            toolTip1 = new ToolTip(components);
            tabControl1 = new TabControl();
            tabPage1 = new TabPage();
            groupBox3 = new GroupBox();
            optionIncludeSubMedia = new CheckBox();
            optionRememberDownloadOpts = new CheckBox();
            groupBox2 = new GroupBox();
            optionCookiePath = new TextBox();
            label1 = new Label();
            groupBox1 = new GroupBox();
            optionShowConsole = new CheckBox();
            optionShowFolders = new CheckBox();
            tabPage2 = new TabPage();
            button4 = new Button();
            optionTempDirectory = new TextBox();
            label6 = new Label();
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
            optionFfmpegDirectory = new TextBox();
            label2 = new Label();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            groupBox3.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox1.SuspendLayout();
            tabPage2.SuspendLayout();
            SuspendLayout();
            // 
            // okButton
            // 
            okButton.Location = new Point(16, 256);
            okButton.Name = "okButton";
            okButton.Size = new Size(94, 29);
            okButton.TabIndex = 5;
            okButton.Text = "&OK";
            okButton.UseVisualStyleBackColor = true;
            okButton.Click += Ok_Click;
            // 
            // openFileDialog1
            // 
            openFileDialog1.Filter = "EXE Files (*.exe)|*.exe";
            openFileDialog1.Title = "Open Executable File";
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Location = new Point(12, 12);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(638, 242);
            tabControl1.TabIndex = 9;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(groupBox3);
            tabPage1.Controls.Add(groupBox2);
            tabPage1.Controls.Add(groupBox1);
            tabPage1.Location = new Point(4, 29);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(630, 209);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "Main Options";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(optionIncludeSubMedia);
            groupBox3.Controls.Add(optionRememberDownloadOpts);
            groupBox3.Location = new Point(6, 103);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(250, 87);
            groupBox3.TabIndex = 9;
            groupBox3.TabStop = false;
            groupBox3.Text = "Other Options";
            // 
            // optionIncludeSubMedia
            // 
            optionIncludeSubMedia.AutoSize = true;
            optionIncludeSubMedia.Location = new Point(9, 56);
            optionIncludeSubMedia.Name = "optionIncludeSubMedia";
            optionIncludeSubMedia.Size = new Size(167, 24);
            optionIncludeSubMedia.TabIndex = 4;
            optionIncludeSubMedia.Text = "Include &Sub Folders?";
            optionIncludeSubMedia.UseVisualStyleBackColor = true;
            // 
            // optionRememberDownloadOpts
            // 
            optionRememberDownloadOpts.AutoSize = true;
            optionRememberDownloadOpts.Checked = true;
            optionRememberDownloadOpts.CheckState = CheckState.Checked;
            optionRememberDownloadOpts.Location = new Point(9, 26);
            optionRememberDownloadOpts.Name = "optionRememberDownloadOpts";
            optionRememberDownloadOpts.Size = new Size(241, 24);
            optionRememberDownloadOpts.TabIndex = 3;
            optionRememberDownloadOpts.Text = "Remember &Download Settings?";
            optionRememberDownloadOpts.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(optionCookiePath);
            groupBox2.Controls.Add(label1);
            groupBox2.Location = new Point(262, 6);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(357, 91);
            groupBox2.TabIndex = 8;
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
            // groupBox1
            // 
            groupBox1.Controls.Add(optionShowConsole);
            groupBox1.Controls.Add(optionShowFolders);
            groupBox1.Location = new Point(6, 6);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(250, 91);
            groupBox1.TabIndex = 7;
            groupBox1.TabStop = false;
            groupBox1.Text = "Display Options";
            // 
            // optionShowConsole
            // 
            optionShowConsole.AutoSize = true;
            optionShowConsole.Checked = true;
            optionShowConsole.CheckState = CheckState.Checked;
            optionShowConsole.Location = new Point(11, 57);
            optionShowConsole.Name = "optionShowConsole";
            optionShowConsole.Size = new Size(131, 24);
            optionShowConsole.TabIndex = 1;
            optionShowConsole.Text = "Show &Console?";
            optionShowConsole.UseVisualStyleBackColor = true;
            // 
            // optionShowFolders
            // 
            optionShowFolders.AutoSize = true;
            optionShowFolders.Location = new Point(11, 27);
            optionShowFolders.Name = "optionShowFolders";
            optionShowFolders.Size = new Size(126, 24);
            optionShowFolders.TabIndex = 0;
            optionShowFolders.Text = "Show &Folders?";
            optionShowFolders.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(button4);
            tabPage2.Controls.Add(optionTempDirectory);
            tabPage2.Controls.Add(label6);
            tabPage2.Controls.Add(button3);
            tabPage2.Controls.Add(optionMediaDirectory);
            tabPage2.Controls.Add(label4);
            tabPage2.Controls.Add(button2);
            tabPage2.Controls.Add(label5);
            tabPage2.Controls.Add(optionPlayerPath);
            tabPage2.Controls.Add(optionYtdlpPath);
            tabPage2.Controls.Add(button1);
            tabPage2.Controls.Add(label3);
            tabPage2.Controls.Add(browseFfmpeg);
            tabPage2.Controls.Add(optionFfmpegDirectory);
            tabPage2.Controls.Add(label2);
            tabPage2.Location = new Point(4, 29);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(630, 209);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "Paths";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // button4
            // 
            button4.Location = new Point(532, 40);
            button4.Name = "button4";
            button4.Size = new Size(75, 29);
            button4.TabIndex = 4;
            button4.Tag = "optionTempDirectory";
            button4.Text = "&Browse...";
            button4.UseVisualStyleBackColor = true;
            button4.Click += BrowseFolder_Click;
            // 
            // optionTempDirectory
            // 
            optionTempDirectory.Location = new Point(142, 41);
            optionTempDirectory.Name = "optionTempDirectory";
            optionTempDirectory.Size = new Size(366, 27);
            optionTempDirectory.TabIndex = 3;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(6, 44);
            label6.Name = "label6";
            label6.Size = new Size(114, 20);
            label6.TabIndex = 30;
            label6.Text = "Temp Directory:";
            // 
            // button3
            // 
            button3.Location = new Point(532, 5);
            button3.Name = "button3";
            button3.Size = new Size(75, 29);
            button3.TabIndex = 2;
            button3.Tag = "optionMediaDirectory";
            button3.Text = "&Browse...";
            button3.UseVisualStyleBackColor = true;
            button3.Click += BrowseFolder_Click;
            // 
            // optionMediaDirectory
            // 
            optionMediaDirectory.Location = new Point(142, 6);
            optionMediaDirectory.Name = "optionMediaDirectory";
            optionMediaDirectory.Size = new Size(366, 27);
            optionMediaDirectory.TabIndex = 1;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(6, 9);
            label4.Name = "label4";
            label4.Size = new Size(119, 20);
            label4.TabIndex = 27;
            label4.Text = "Media Directory:";
            // 
            // button2
            // 
            button2.Location = new Point(532, 145);
            button2.Name = "button2";
            button2.Size = new Size(75, 29);
            button2.TabIndex = 10;
            button2.Tag = "optionPlayerPath";
            button2.Text = "&Browse...";
            button2.UseVisualStyleBackColor = true;
            button2.Click += BrowseFile_Click;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(6, 149);
            label5.Name = "label5";
            label5.Size = new Size(98, 20);
            label5.TabIndex = 26;
            label5.Text = "Media Player:";
            // 
            // optionPlayerPath
            // 
            optionPlayerPath.Location = new Point(142, 146);
            optionPlayerPath.Name = "optionPlayerPath";
            optionPlayerPath.Size = new Size(366, 27);
            optionPlayerPath.TabIndex = 9;
            // 
            // optionYtdlpPath
            // 
            optionYtdlpPath.Location = new Point(142, 111);
            optionYtdlpPath.Name = "optionYtdlpPath";
            optionYtdlpPath.Size = new Size(366, 27);
            optionYtdlpPath.TabIndex = 7;
            // 
            // button1
            // 
            button1.Location = new Point(532, 110);
            button1.Name = "button1";
            button1.Size = new Size(75, 29);
            button1.TabIndex = 8;
            button1.Tag = "optionYtdlpPath";
            button1.Text = "&Browse...";
            button1.UseVisualStyleBackColor = true;
            button1.Click += BrowseFile_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(6, 114);
            label3.Name = "label3";
            label3.Size = new Size(56, 20);
            label3.TabIndex = 25;
            label3.Text = "yt-dlp: ";
            // 
            // browseFfmpeg
            // 
            browseFfmpeg.Location = new Point(532, 75);
            browseFfmpeg.Name = "browseFfmpeg";
            browseFfmpeg.Size = new Size(75, 29);
            browseFfmpeg.TabIndex = 6;
            browseFfmpeg.Tag = "optionFfmpegDirectory";
            browseFfmpeg.Text = "&Browse...";
            browseFfmpeg.UseVisualStyleBackColor = true;
            browseFfmpeg.Click += BrowseFolder_Click;
            // 
            // optionFfmpegDirectory
            // 
            optionFfmpegDirectory.Location = new Point(142, 76);
            optionFfmpegDirectory.Name = "optionFfmpegDirectory";
            optionFfmpegDirectory.Size = new Size(366, 27);
            optionFfmpegDirectory.TabIndex = 5;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(6, 79);
            label2.Name = "label2";
            label2.Size = new Size(130, 20);
            label2.TabIndex = 16;
            label2.Text = "FFmpeg Directory:";
            // 
            // OptionsForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(659, 297);
            Controls.Add(tabControl1);
            Controls.Add(okButton);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "OptionsForm";
            SizeGripStyle = SizeGripStyle.Hide;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Media Tool :: Options";
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            tabPage2.ResumeLayout(false);
            tabPage2.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private Button okButton;
        private OpenFileDialog openFileDialog1;
        private FolderBrowserDialog folderBrowserDialog1;
        private ToolTip toolTip1;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private GroupBox groupBox3;
        private CheckBox optionIncludeSubMedia;
        private CheckBox optionRememberDownloadOpts;
        private GroupBox groupBox2;
        private TextBox optionCookiePath;
        private Label label1;
        private GroupBox groupBox1;
        private CheckBox optionShowFolders;
        private TabPage tabPage2;
        private Button button3;
        private TextBox optionMediaDirectory;
        private Label label4;
        private Button button2;
        private Label label5;
        private TextBox optionPlayerPath;
        private TextBox optionYtdlpPath;
        private Button button1;
        private Label label3;
        private Button browseFfmpeg;
        private TextBox optionFfmpegDirectory;
        private Label label2;
        private Button button4;
        private TextBox optionTempDirectory;
        private Label label6;
        private CheckBox optionShowConsole;
    }
}
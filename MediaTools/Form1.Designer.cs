﻿namespace MediaTools
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            tabControl1 = new TabControl();
            tabPage1 = new TabPage();
            groupBox1 = new GroupBox();
            optionSponsorBlock = new CheckBox();
            optionMarkWatched = new CheckBox();
            optionAddThumbnails = new CheckBox();
            optionAddChapters = new CheckBox();
            optionAddMetadata = new CheckBox();
            optionSubtitles = new CheckBox();
            optionAudioOnly = new CheckBox();
            downloadPlaylist = new RadioButton();
            downloadSingle = new RadioButton();
            label3 = new Label();
            downloadFolder = new TextBox();
            label2 = new Label();
            source = new ComboBox();
            label1 = new Label();
            download = new Button();
            downloadIds = new TextBox();
            tabPage2 = new TabPage();
            mediaFilesTable = new DataGridView();
            menuStrip1 = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            showConsoleToolStripMenuItem = new ToolStripMenuItem();
            toolStripMenuItem1 = new ToolStripSeparator();
            exitToolStripMenuItem = new ToolStripMenuItem();
            dataToolStripMenuItem = new ToolStripMenuItem();
            reloadMediaFilesToolStripMenuItem = new ToolStripMenuItem();
            statusStrip1 = new StatusStrip();
            toolStripStatusLabel1 = new ToolStripStatusLabel();
            Duration = new DataGridViewTextBoxColumn();
            LastModified = new DataGridViewTextBoxColumn();
            Title = new DataGridViewTextBoxColumn();
            FullPath = new DataGridViewTextBoxColumn();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            groupBox1.SuspendLayout();
            tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)mediaFilesTable).BeginInit();
            menuStrip1.SuspendLayout();
            statusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Location = new Point(12, 27);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(1232, 795);
            tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(groupBox1);
            tabPage1.Controls.Add(downloadPlaylist);
            tabPage1.Controls.Add(downloadSingle);
            tabPage1.Controls.Add(label3);
            tabPage1.Controls.Add(downloadFolder);
            tabPage1.Controls.Add(label2);
            tabPage1.Controls.Add(source);
            tabPage1.Controls.Add(label1);
            tabPage1.Controls.Add(download);
            tabPage1.Controls.Add(downloadIds);
            tabPage1.Location = new Point(4, 29);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(1224, 762);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "Downloader";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(optionSponsorBlock);
            groupBox1.Controls.Add(optionMarkWatched);
            groupBox1.Controls.Add(optionAddThumbnails);
            groupBox1.Controls.Add(optionAddChapters);
            groupBox1.Controls.Add(optionAddMetadata);
            groupBox1.Controls.Add(optionSubtitles);
            groupBox1.Controls.Add(optionAudioOnly);
            groupBox1.Location = new Point(537, 111);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(218, 235);
            groupBox1.TabIndex = 6;
            groupBox1.TabStop = false;
            groupBox1.Text = "Options";
            // 
            // optionSponsorBlock
            // 
            optionSponsorBlock.AutoSize = true;
            optionSponsorBlock.Checked = true;
            optionSponsorBlock.CheckState = CheckState.Checked;
            optionSponsorBlock.Location = new Point(18, 205);
            optionSponsorBlock.Name = "optionSponsorBlock";
            optionSponsorBlock.Size = new Size(149, 24);
            optionSponsorBlock.TabIndex = 11;
            optionSponsorBlock.Text = "Use SponsorBlock";
            optionSponsorBlock.UseVisualStyleBackColor = true;
            // 
            // optionMarkWatched
            // 
            optionMarkWatched.AutoSize = true;
            optionMarkWatched.Checked = true;
            optionMarkWatched.CheckState = CheckState.Checked;
            optionMarkWatched.Location = new Point(18, 175);
            optionMarkWatched.Name = "optionMarkWatched";
            optionMarkWatched.Size = new Size(169, 24);
            optionMarkWatched.TabIndex = 10;
            optionMarkWatched.Text = "Mark Video Watched";
            optionMarkWatched.UseVisualStyleBackColor = true;
            // 
            // optionAddThumbnails
            // 
            optionAddThumbnails.AutoSize = true;
            optionAddThumbnails.Checked = true;
            optionAddThumbnails.CheckState = CheckState.Checked;
            optionAddThumbnails.Location = new Point(18, 145);
            optionAddThumbnails.Name = "optionAddThumbnails";
            optionAddThumbnails.Size = new Size(139, 24);
            optionAddThumbnails.TabIndex = 9;
            optionAddThumbnails.Text = "Add Thumbnails";
            optionAddThumbnails.UseVisualStyleBackColor = true;
            // 
            // optionAddChapters
            // 
            optionAddChapters.AutoSize = true;
            optionAddChapters.Checked = true;
            optionAddChapters.CheckState = CheckState.Checked;
            optionAddChapters.Location = new Point(18, 55);
            optionAddChapters.Name = "optionAddChapters";
            optionAddChapters.Size = new Size(121, 24);
            optionAddChapters.TabIndex = 6;
            optionAddChapters.Text = "Add Chapters";
            optionAddChapters.UseVisualStyleBackColor = true;
            // 
            // optionAddMetadata
            // 
            optionAddMetadata.AutoSize = true;
            optionAddMetadata.Checked = true;
            optionAddMetadata.CheckState = CheckState.Checked;
            optionAddMetadata.Location = new Point(18, 115);
            optionAddMetadata.Name = "optionAddMetadata";
            optionAddMetadata.Size = new Size(127, 24);
            optionAddMetadata.TabIndex = 8;
            optionAddMetadata.Text = "Add Metadata";
            optionAddMetadata.UseVisualStyleBackColor = true;
            // 
            // optionSubtitles
            // 
            optionSubtitles.AutoSize = true;
            optionSubtitles.Location = new Point(18, 85);
            optionSubtitles.Name = "optionSubtitles";
            optionSubtitles.Size = new Size(88, 24);
            optionSubtitles.TabIndex = 7;
            optionSubtitles.Text = "Subtitles";
            optionSubtitles.UseVisualStyleBackColor = true;
            // 
            // optionAudioOnly
            // 
            optionAudioOnly.AutoSize = true;
            optionAudioOnly.Location = new Point(18, 25);
            optionAudioOnly.Name = "optionAudioOnly";
            optionAudioOnly.Size = new Size(105, 24);
            optionAudioOnly.TabIndex = 5;
            optionAudioOnly.Text = "Audio Only";
            optionAudioOnly.UseVisualStyleBackColor = true;
            optionAudioOnly.CheckedChanged += OptionAudioOnly_CheckedChanged;
            // 
            // downloadPlaylist
            // 
            downloadPlaylist.AutoSize = true;
            downloadPlaylist.Location = new Point(677, 48);
            downloadPlaylist.Name = "downloadPlaylist";
            downloadPlaylist.Size = new Size(76, 24);
            downloadPlaylist.TabIndex = 4;
            downloadPlaylist.Text = "Playlist";
            downloadPlaylist.UseVisualStyleBackColor = true;
            // 
            // downloadSingle
            // 
            downloadSingle.AutoSize = true;
            downloadSingle.Checked = true;
            downloadSingle.Location = new Point(600, 48);
            downloadSingle.Name = "downloadSingle";
            downloadSingle.Size = new Size(71, 24);
            downloadSingle.TabIndex = 3;
            downloadSingle.TabStop = true;
            downloadSingle.Text = "Single";
            downloadSingle.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(537, 81);
            label3.Name = "label3";
            label3.Size = new Size(54, 20);
            label3.TabIndex = 0;
            label3.Text = "Folder:";
            // 
            // downloadFolder
            // 
            downloadFolder.Location = new Point(604, 78);
            downloadFolder.Name = "downloadFolder";
            downloadFolder.Size = new Size(151, 27);
            downloadFolder.TabIndex = 5;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(537, 47);
            label2.Name = "label2";
            label2.Size = new Size(43, 20);
            label2.TabIndex = 0;
            label2.Text = "Type:";
            // 
            // source
            // 
            source.FormattingEnabled = true;
            source.Items.AddRange(new object[] { "YouTube" });
            source.Location = new Point(600, 14);
            source.Name = "source";
            source.Size = new Size(151, 28);
            source.TabIndex = 2;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(537, 14);
            label1.Name = "label1";
            label1.Size = new Size(57, 20);
            label1.TabIndex = 0;
            label1.Text = "Source:";
            // 
            // download
            // 
            download.Location = new Point(537, 352);
            download.Name = "download";
            download.Size = new Size(94, 29);
            download.TabIndex = 12;
            download.Text = "Download";
            download.UseVisualStyleBackColor = true;
            download.Click += Download_Click;
            // 
            // downloadIds
            // 
            downloadIds.Location = new Point(6, 6);
            downloadIds.Multiline = true;
            downloadIds.Name = "downloadIds";
            downloadIds.Size = new Size(525, 750);
            downloadIds.TabIndex = 1;
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(mediaFilesTable);
            tabPage2.Location = new Point(4, 29);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(1224, 762);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "Media";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // mediaFilesTable
            // 
            mediaFilesTable.AllowUserToAddRows = false;
            mediaFilesTable.AllowUserToDeleteRows = false;
            mediaFilesTable.AllowUserToOrderColumns = true;
            mediaFilesTable.AllowUserToResizeColumns = false;
            mediaFilesTable.AllowUserToResizeRows = false;
            mediaFilesTable.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            mediaFilesTable.Columns.AddRange(new DataGridViewColumn[] { Duration, LastModified, Title, FullPath });
            mediaFilesTable.Location = new Point(6, 6);
            mediaFilesTable.Name = "mediaFilesTable";
            mediaFilesTable.RowHeadersVisible = false;
            mediaFilesTable.RowHeadersWidth = 51;
            mediaFilesTable.Size = new Size(1212, 750);
            mediaFilesTable.TabIndex = 3;
            mediaFilesTable.CellContentDoubleClick += MediaFilesTable_CellContentDoubleClick;
            mediaFilesTable.MouseClick += MediaFilesTable_MouseClick;
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new Size(20, 20);
            menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, dataToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(1256, 28);
            menuStrip1.TabIndex = 1;
            menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { showConsoleToolStripMenuItem, toolStripMenuItem1, exitToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(46, 24);
            fileToolStripMenuItem.Text = "&File";
            // 
            // showConsoleToolStripMenuItem
            // 
            showConsoleToolStripMenuItem.Name = "showConsoleToolStripMenuItem";
            showConsoleToolStripMenuItem.Size = new Size(194, 26);
            showConsoleToolStripMenuItem.Text = "Show &Console...";
            showConsoleToolStripMenuItem.Click += ShowConsoleToolStripMenuItem_Click;
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.Size = new Size(191, 6);
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.Size = new Size(194, 26);
            exitToolStripMenuItem.Text = "E&xit";
            // 
            // dataToolStripMenuItem
            // 
            dataToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { reloadMediaFilesToolStripMenuItem });
            dataToolStripMenuItem.Name = "dataToolStripMenuItem";
            dataToolStripMenuItem.Size = new Size(65, 24);
            dataToolStripMenuItem.Text = "&Media";
            // 
            // reloadMediaFilesToolStripMenuItem
            // 
            reloadMediaFilesToolStripMenuItem.Name = "reloadMediaFilesToolStripMenuItem";
            reloadMediaFilesToolStripMenuItem.Size = new Size(218, 26);
            reloadMediaFilesToolStripMenuItem.Text = "&Reload Media Files";
            reloadMediaFilesToolStripMenuItem.Click += ReloadMediaFilesToolStripMenuItem_Click;
            // 
            // statusStrip1
            // 
            statusStrip1.ImageScalingSize = new Size(20, 20);
            statusStrip1.Items.AddRange(new ToolStripItem[] { toolStripStatusLabel1 });
            statusStrip1.Location = new Point(0, 825);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(1256, 26);
            statusStrip1.Stretch = false;
            statusStrip1.TabIndex = 5;
            statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            toolStripStatusLabel1.Size = new Size(151, 20);
            toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // Duration
            // 
            Duration.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            Duration.HeaderText = "Duration";
            Duration.MinimumWidth = 6;
            Duration.Name = "Duration";
            Duration.ReadOnly = true;
            Duration.Width = 96;
            // 
            // LastModified
            // 
            LastModified.HeaderText = "Last Modified";
            LastModified.MinimumWidth = 6;
            LastModified.Name = "LastModified";
            LastModified.ReadOnly = true;
            LastModified.Resizable = DataGridViewTriState.False;
            LastModified.Width = 150;
            // 
            // Title
            // 
            Title.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            Title.HeaderText = "Title";
            Title.MinimumWidth = 6;
            Title.Name = "Title";
            Title.ReadOnly = true;
            // 
            // FullPath
            // 
            FullPath.HeaderText = "FullPath";
            FullPath.MinimumWidth = 6;
            FullPath.Name = "FullPath";
            FullPath.Visible = false;
            FullPath.Width = 125;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1256, 851);
            Controls.Add(statusStrip1);
            Controls.Add(tabControl1);
            Controls.Add(menuStrip1);
            KeyPreview = true;
            MainMenuStrip = menuStrip1;
            Name = "Form1";
            Text = "Media Tools";
            KeyDown += Form1_KeyDown;
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tabPage1.PerformLayout();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)mediaFilesTable).EndInit();
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TabControl tabControl1;
        private TabPage tabPage1;
        private DataGridView mediaFilesTable;
        private TabPage tabPage2;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem exitToolStripMenuItem;
        private StatusStrip statusStrip1;
        private ToolStripMenuItem dataToolStripMenuItem;
        private ToolStripMenuItem reloadMediaFilesToolStripMenuItem;
        private ToolStripStatusLabel toolStripStatusLabel1;
        private Button download;
        private TextBox downloadIds;
        private Label label2;
        private ComboBox source;
        private Label label1;
        private Label label3;
        private TextBox downloadFolder;
        private RadioButton downloadPlaylist;
        private RadioButton downloadSingle;
        private GroupBox groupBox1;
        private CheckBox optionSponsorBlock;
        private CheckBox optionMarkWatched;
        private CheckBox optionAddThumbnails;
        private CheckBox optionAddChapters;
        private CheckBox optionAddMetadata;
        private CheckBox optionSubtitles;
        private CheckBox optionAudioOnly;
        private ToolStripMenuItem showConsoleToolStripMenuItem;
        private ToolStripSeparator toolStripMenuItem1;
        private DataGridViewTextBoxColumn Duration;
        private DataGridViewTextBoxColumn LastModified;
        private DataGridViewTextBoxColumn Title;
        private DataGridViewTextBoxColumn FullPath;
    }
}
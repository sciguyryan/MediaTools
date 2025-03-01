﻿namespace MediaTools
{
    partial class MainForm
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
            components = new System.ComponentModel.Container();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            tabControl1 = new TabControl();
            tabPage1 = new TabPage();
            actionOnComplete = new ComboBox();
            label1 = new Label();
            groupBox4 = new GroupBox();
            optionCookieLogin = new CheckBox();
            optionMarkWatched = new CheckBox();
            subtitleGroupBox = new GroupBox();
            optionSubtleLangs = new TextBox();
            label6 = new Label();
            optionEmbedSubs = new CheckBox();
            groupBox3 = new GroupBox();
            optionDownloadChat = new CheckBox();
            optionAutoUpdate = new CheckBox();
            optionAddMetadata = new CheckBox();
            optionSponsorBlock = new CheckBox();
            groupBox2 = new GroupBox();
            optionForceKeyframeAtCuts = new CheckBox();
            optionDownloadRateLimitType = new ComboBox();
            optionDownloadRateLimitVal = new NumericUpDown();
            label5 = new Label();
            optionResolution = new ComboBox();
            label4 = new Label();
            groupBox1 = new GroupBox();
            optionAddThumbnails = new CheckBox();
            optionAddChapters = new CheckBox();
            optionAddSubtitles = new CheckBox();
            optionAudioOnly = new CheckBox();
            label3 = new Label();
            downloadFolder = new TextBox();
            download = new Button();
            downloadIds = new TextBox();
            tabPage2 = new TabPage();
            mediaFilesTable = new DataGridView();
            menuStrip1 = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            showConsoleToolStripMenuItem = new ToolStripMenuItem();
            toolStripMenuItem1 = new ToolStripSeparator();
            exitToolStripMenuItem = new ToolStripMenuItem();
            clearCacheToolStripMenuItem = new ToolStripMenuItem();
            playlistBuilderToolStripMenuItem = new ToolStripMenuItem();
            reloadMediaListToolStripMenuItem = new ToolStripMenuItem();
            toolStripMenuItem3 = new ToolStripSeparator();
            optionsToolStripMenuItem = new ToolStripMenuItem();
            reloadMediaFilesToolStripMenuItem = new ToolStripMenuItem();
            contextMenuStrip = new ContextMenuStrip(components);
            contextDelete = new ToolStripMenuItem();
            trashToolStripMenuItem = new ToolStripMenuItem();
            toolStripMenuItem2 = new ToolStripSeparator();
            renameToolStripMenuItem = new ToolStripMenuItem();
            statusStrip1 = new StatusStrip();
            toolStripStatusLabel1 = new ToolStripStatusLabel();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            groupBox4.SuspendLayout();
            subtitleGroupBox.SuspendLayout();
            groupBox3.SuspendLayout();
            groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)optionDownloadRateLimitVal).BeginInit();
            groupBox1.SuspendLayout();
            tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)mediaFilesTable).BeginInit();
            menuStrip1.SuspendLayout();
            contextMenuStrip.SuspendLayout();
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
            tabControl1.Size = new Size(1454, 795);
            tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(actionOnComplete);
            tabPage1.Controls.Add(label1);
            tabPage1.Controls.Add(groupBox4);
            tabPage1.Controls.Add(subtitleGroupBox);
            tabPage1.Controls.Add(groupBox3);
            tabPage1.Controls.Add(groupBox2);
            tabPage1.Controls.Add(groupBox1);
            tabPage1.Controls.Add(label3);
            tabPage1.Controls.Add(downloadFolder);
            tabPage1.Controls.Add(download);
            tabPage1.Controls.Add(downloadIds);
            tabPage1.Location = new Point(4, 29);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(1446, 762);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "Downloader";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // actionOnComplete
            // 
            actionOnComplete.DropDownStyle = ComboBoxStyle.DropDownList;
            actionOnComplete.FormattingEnabled = true;
            actionOnComplete.ItemHeight = 20;
            actionOnComplete.Items.AddRange(new object[] { "Do Nothing", "Shutdown", "Exit Application" });
            actionOnComplete.Location = new Point(643, 408);
            actionOnComplete.Name = "actionOnComplete";
            actionOnComplete.Size = new Size(147, 28);
            actionOnComplete.TabIndex = 21;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(537, 411);
            label1.Name = "label1";
            label1.Size = new Size(100, 20);
            label1.TabIndex = 20;
            label1.Text = "On Complete:";
            // 
            // groupBox4
            // 
            groupBox4.Controls.Add(optionCookieLogin);
            groupBox4.Controls.Add(optionMarkWatched);
            groupBox4.Enabled = false;
            groupBox4.Location = new Point(767, 287);
            groupBox4.Name = "groupBox4";
            groupBox4.Size = new Size(299, 114);
            groupBox4.TabIndex = 17;
            groupBox4.TabStop = false;
            groupBox4.Text = "Account Options";
            // 
            // optionCookieLogin
            // 
            optionCookieLogin.AutoSize = true;
            optionCookieLogin.Location = new Point(8, 26);
            optionCookieLogin.Name = "optionCookieLogin";
            optionCookieLogin.Size = new Size(159, 24);
            optionCookieLogin.TabIndex = 16;
            optionCookieLogin.Text = "&Login With Cookies";
            optionCookieLogin.UseVisualStyleBackColor = true;
            optionCookieLogin.CheckedChanged += OptionLogin_CheckedChanged;
            // 
            // optionMarkWatched
            // 
            optionMarkWatched.AutoSize = true;
            optionMarkWatched.Enabled = false;
            optionMarkWatched.Location = new Point(8, 56);
            optionMarkWatched.Name = "optionMarkWatched";
            optionMarkWatched.Size = new Size(126, 24);
            optionMarkWatched.TabIndex = 17;
            optionMarkWatched.Text = "Mark &Watched";
            optionMarkWatched.UseVisualStyleBackColor = true;
            // 
            // subtitleGroupBox
            // 
            subtitleGroupBox.Controls.Add(optionSubtleLangs);
            subtitleGroupBox.Controls.Add(label6);
            subtitleGroupBox.Controls.Add(optionEmbedSubs);
            subtitleGroupBox.Enabled = false;
            subtitleGroupBox.Location = new Point(537, 287);
            subtitleGroupBox.Name = "subtitleGroupBox";
            subtitleGroupBox.Size = new Size(218, 114);
            subtitleGroupBox.TabIndex = 15;
            subtitleGroupBox.TabStop = false;
            subtitleGroupBox.Text = "Subtitle Options";
            // 
            // optionSubtleLangs
            // 
            optionSubtleLangs.Location = new Point(9, 76);
            optionSubtleLangs.Name = "optionSubtleLangs";
            optionSubtleLangs.Size = new Size(196, 27);
            optionSubtleLangs.TabIndex = 15;
            optionSubtleLangs.Text = "en.*";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(8, 53);
            label6.Name = "label6";
            label6.Size = new Size(135, 20);
            label6.TabIndex = 9;
            label6.Text = "Subtitle Languages";
            // 
            // optionEmbedSubs
            // 
            optionEmbedSubs.AutoSize = true;
            optionEmbedSubs.Checked = true;
            optionEmbedSubs.CheckState = CheckState.Checked;
            optionEmbedSubs.Location = new Point(8, 26);
            optionEmbedSubs.Name = "optionEmbedSubs";
            optionEmbedSubs.Size = new Size(139, 24);
            optionEmbedSubs.TabIndex = 14;
            optionEmbedSubs.Text = "&Embed Subtitles";
            optionEmbedSubs.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(optionDownloadChat);
            groupBox3.Controls.Add(optionAutoUpdate);
            groupBox3.Controls.Add(optionAddMetadata);
            groupBox3.Controls.Add(optionSponsorBlock);
            groupBox3.Location = new Point(1079, 40);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(218, 241);
            groupBox3.TabIndex = 14;
            groupBox3.TabStop = false;
            groupBox3.Text = "Other Options";
            // 
            // optionDownloadChat
            // 
            optionDownloadChat.AutoSize = true;
            optionDownloadChat.Location = new Point(9, 86);
            optionDownloadChat.Name = "optionDownloadChat";
            optionDownloadChat.Size = new Size(134, 24);
            optionDownloadChat.TabIndex = 12;
            optionDownloadChat.Text = "Download &Chat";
            optionDownloadChat.UseVisualStyleBackColor = true;
            // 
            // optionAutoUpdate
            // 
            optionAutoUpdate.AutoSize = true;
            optionAutoUpdate.Checked = true;
            optionAutoUpdate.CheckState = CheckState.Checked;
            optionAutoUpdate.Location = new Point(9, 116);
            optionAutoUpdate.Name = "optionAutoUpdate";
            optionAutoUpdate.Size = new Size(116, 24);
            optionAutoUpdate.TabIndex = 13;
            optionAutoUpdate.Text = "&Auto Update";
            optionAutoUpdate.UseVisualStyleBackColor = true;
            // 
            // optionAddMetadata
            // 
            optionAddMetadata.AutoSize = true;
            optionAddMetadata.Checked = true;
            optionAddMetadata.CheckState = CheckState.Checked;
            optionAddMetadata.Location = new Point(9, 26);
            optionAddMetadata.Name = "optionAddMetadata";
            optionAddMetadata.Size = new Size(127, 24);
            optionAddMetadata.TabIndex = 10;
            optionAddMetadata.Text = "Add &Metadata";
            optionAddMetadata.UseVisualStyleBackColor = true;
            // 
            // optionSponsorBlock
            // 
            optionSponsorBlock.AutoSize = true;
            optionSponsorBlock.Checked = true;
            optionSponsorBlock.CheckState = CheckState.Checked;
            optionSponsorBlock.Location = new Point(9, 56);
            optionSponsorBlock.Name = "optionSponsorBlock";
            optionSponsorBlock.Size = new Size(149, 24);
            optionSponsorBlock.TabIndex = 11;
            optionSponsorBlock.Text = "Use &SponsorBlock";
            optionSponsorBlock.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(optionForceKeyframeAtCuts);
            groupBox2.Controls.Add(optionDownloadRateLimitType);
            groupBox2.Controls.Add(optionDownloadRateLimitVal);
            groupBox2.Controls.Add(label5);
            groupBox2.Controls.Add(optionResolution);
            groupBox2.Controls.Add(label4);
            groupBox2.Location = new Point(761, 40);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(305, 241);
            groupBox2.TabIndex = 13;
            groupBox2.TabStop = false;
            groupBox2.Text = "Advanced Options";
            // 
            // optionForceKeyframeAtCuts
            // 
            optionForceKeyframeAtCuts.AutoSize = true;
            optionForceKeyframeAtCuts.Location = new Point(6, 86);
            optionForceKeyframeAtCuts.Name = "optionForceKeyframeAtCuts";
            optionForceKeyframeAtCuts.Size = new Size(189, 24);
            optionForceKeyframeAtCuts.TabIndex = 9;
            optionForceKeyframeAtCuts.Text = "Force &Keyframes at Cuts";
            optionForceKeyframeAtCuts.UseVisualStyleBackColor = true;
            // 
            // optionDownloadRateLimitType
            // 
            optionDownloadRateLimitType.FormattingEnabled = true;
            optionDownloadRateLimitType.Items.AddRange(new object[] { "K", "M" });
            optionDownloadRateLimitType.Location = new Point(256, 51);
            optionDownloadRateLimitType.Name = "optionDownloadRateLimitType";
            optionDownloadRateLimitType.Size = new Size(43, 28);
            optionDownloadRateLimitType.TabIndex = 8;
            // 
            // optionDownloadRateLimitVal
            // 
            optionDownloadRateLimitVal.DecimalPlaces = 1;
            optionDownloadRateLimitVal.Location = new Point(164, 52);
            optionDownloadRateLimitVal.Name = "optionDownloadRateLimitVal";
            optionDownloadRateLimitVal.Size = new Size(86, 27);
            optionDownloadRateLimitVal.TabIndex = 7;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(6, 54);
            label5.Name = "label5";
            label5.Size = new Size(152, 20);
            label5.TabIndex = 13;
            label5.Text = "Download Rate Limit:";
            // 
            // optionResolution
            // 
            optionResolution.DropDownStyle = ComboBoxStyle.DropDownList;
            optionResolution.FormattingEnabled = true;
            optionResolution.Items.AddRange(new object[] { "240p", "360p", "480p", "720p", "1080p", "1440p", "2160p", "4320p" });
            optionResolution.Location = new Point(164, 18);
            optionResolution.Name = "optionResolution";
            optionResolution.Size = new Size(86, 28);
            optionResolution.TabIndex = 6;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(6, 26);
            label4.Name = "label4";
            label4.Size = new Size(114, 20);
            label4.TabIndex = 0;
            label4.Text = "Max Resolution:";
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(optionAddThumbnails);
            groupBox1.Controls.Add(optionAddChapters);
            groupBox1.Controls.Add(optionAddSubtitles);
            groupBox1.Controls.Add(optionAudioOnly);
            groupBox1.Location = new Point(537, 40);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(218, 241);
            groupBox1.TabIndex = 6;
            groupBox1.TabStop = false;
            groupBox1.Text = "Basic Options";
            // 
            // optionAddThumbnails
            // 
            optionAddThumbnails.AutoSize = true;
            optionAddThumbnails.Location = new Point(18, 110);
            optionAddThumbnails.Name = "optionAddThumbnails";
            optionAddThumbnails.Size = new Size(139, 24);
            optionAddThumbnails.TabIndex = 5;
            optionAddThumbnails.Text = "Add &Thumbnails";
            optionAddThumbnails.UseVisualStyleBackColor = true;
            // 
            // optionAddChapters
            // 
            optionAddChapters.AutoSize = true;
            optionAddChapters.Checked = true;
            optionAddChapters.CheckState = CheckState.Checked;
            optionAddChapters.Location = new Point(18, 50);
            optionAddChapters.Name = "optionAddChapters";
            optionAddChapters.Size = new Size(121, 24);
            optionAddChapters.TabIndex = 3;
            optionAddChapters.Text = "Add &Chapters";
            optionAddChapters.UseVisualStyleBackColor = true;
            // 
            // optionAddSubtitles
            // 
            optionAddSubtitles.AutoSize = true;
            optionAddSubtitles.Location = new Point(18, 80);
            optionAddSubtitles.Name = "optionAddSubtitles";
            optionAddSubtitles.Size = new Size(120, 24);
            optionAddSubtitles.TabIndex = 4;
            optionAddSubtitles.Text = "Add &Subtitles";
            optionAddSubtitles.UseVisualStyleBackColor = true;
            optionAddSubtitles.CheckedChanged += OptionAddSubtitles_CheckedChanged;
            // 
            // optionAudioOnly
            // 
            optionAudioOnly.AutoSize = true;
            optionAudioOnly.Location = new Point(18, 25);
            optionAudioOnly.Name = "optionAudioOnly";
            optionAudioOnly.Size = new Size(105, 24);
            optionAudioOnly.TabIndex = 2;
            optionAudioOnly.Text = "&Audio Only";
            optionAudioOnly.UseVisualStyleBackColor = true;
            optionAudioOnly.CheckedChanged += OptionAudioOnly_CheckedChanged;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(537, 9);
            label3.Name = "label3";
            label3.Size = new Size(54, 20);
            label3.TabIndex = 0;
            label3.Text = "Folder:";
            // 
            // downloadFolder
            // 
            downloadFolder.Location = new Point(597, 6);
            downloadFolder.Name = "downloadFolder";
            downloadFolder.Size = new Size(151, 27);
            downloadFolder.TabIndex = 1;
            // 
            // download
            // 
            download.Location = new Point(537, 443);
            download.Name = "download";
            download.Size = new Size(94, 29);
            download.TabIndex = 19;
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
            downloadIds.TabIndex = 0;
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(mediaFilesTable);
            tabPage2.Location = new Point(4, 29);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(1446, 762);
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
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = SystemColors.Control;
            dataGridViewCellStyle4.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle4.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = DataGridViewTriState.True;
            mediaFilesTable.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            mediaFilesTable.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            mediaFilesTable.Location = new Point(6, 6);
            mediaFilesTable.Name = "mediaFilesTable";
            mediaFilesTable.RowHeadersVisible = false;
            mediaFilesTable.RowHeadersWidth = 51;
            mediaFilesTable.Size = new Size(1434, 750);
            mediaFilesTable.TabIndex = 3;
            mediaFilesTable.CellContentDoubleClick += MediaFilesTable_CellContentDoubleClick;
            mediaFilesTable.ColumnHeaderMouseClick += MediaFilesTable_ColumnHeaderMouseClick;
            mediaFilesTable.KeyDown += MediaFilesTable_KeyDown;
            mediaFilesTable.MouseClick += MediaFilesTable_MouseClick;
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new Size(20, 20);
            menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, clearCacheToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(1478, 28);
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
            showConsoleToolStripMenuItem.Size = new Size(181, 26);
            showConsoleToolStripMenuItem.Text = "Hide &Console";
            showConsoleToolStripMenuItem.Click += ShowConsoleToolStripMenuItem_Click;
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.Size = new Size(178, 6);
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.Size = new Size(181, 26);
            exitToolStripMenuItem.Text = "E&xit";
            // 
            // clearCacheToolStripMenuItem
            // 
            clearCacheToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { playlistBuilderToolStripMenuItem, reloadMediaListToolStripMenuItem, toolStripMenuItem3, optionsToolStripMenuItem });
            clearCacheToolStripMenuItem.Name = "clearCacheToolStripMenuItem";
            clearCacheToolStripMenuItem.Size = new Size(58, 24);
            clearCacheToolStripMenuItem.Text = "&Tools";
            // 
            // playlistBuilderToolStripMenuItem
            // 
            playlistBuilderToolStripMenuItem.Name = "playlistBuilderToolStripMenuItem";
            playlistBuilderToolStripMenuItem.Size = new Size(211, 26);
            playlistBuilderToolStripMenuItem.Text = "&Playlist Builder...";
            playlistBuilderToolStripMenuItem.Click += PlaylistBuilderToolStripMenuItem_Click;
            // 
            // reloadMediaListToolStripMenuItem
            // 
            reloadMediaListToolStripMenuItem.Name = "reloadMediaListToolStripMenuItem";
            reloadMediaListToolStripMenuItem.Size = new Size(211, 26);
            reloadMediaListToolStripMenuItem.Text = "&Reload Media List";
            reloadMediaListToolStripMenuItem.Click += ReloadMediaFilesToolStripMenuItem_Click;
            // 
            // toolStripMenuItem3
            // 
            toolStripMenuItem3.Name = "toolStripMenuItem3";
            toolStripMenuItem3.Size = new Size(208, 6);
            // 
            // optionsToolStripMenuItem
            // 
            optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            optionsToolStripMenuItem.Size = new Size(211, 26);
            optionsToolStripMenuItem.Text = "&Options...";
            optionsToolStripMenuItem.Click += OptionsToolStripMenuItem_Click;
            // 
            // reloadMediaFilesToolStripMenuItem
            // 
            reloadMediaFilesToolStripMenuItem.Name = "reloadMediaFilesToolStripMenuItem";
            reloadMediaFilesToolStripMenuItem.Size = new Size(224, 26);
            reloadMediaFilesToolStripMenuItem.Text = "&Reload Media Files";
            reloadMediaFilesToolStripMenuItem.Click += ReloadMediaFilesToolStripMenuItem_Click;
            // 
            // contextMenuStrip
            // 
            contextMenuStrip.ImageScalingSize = new Size(20, 20);
            contextMenuStrip.Items.AddRange(new ToolStripItem[] { contextDelete, trashToolStripMenuItem, toolStripMenuItem2, renameToolStripMenuItem });
            contextMenuStrip.Name = "contextMenuStrip1";
            contextMenuStrip.Size = new Size(142, 82);
            // 
            // contextDelete
            // 
            contextDelete.Name = "contextDelete";
            contextDelete.Size = new Size(141, 24);
            contextDelete.Text = "&Delete";
            contextDelete.Click += DeleteToolStripMenuItem_Click;
            // 
            // trashToolStripMenuItem
            // 
            trashToolStripMenuItem.Name = "trashToolStripMenuItem";
            trashToolStripMenuItem.Size = new Size(141, 24);
            trashToolStripMenuItem.Text = "&Trash";
            trashToolStripMenuItem.Click += TrashToolStripMenuItem_Click;
            // 
            // toolStripMenuItem2
            // 
            toolStripMenuItem2.Name = "toolStripMenuItem2";
            toolStripMenuItem2.Size = new Size(138, 6);
            // 
            // renameToolStripMenuItem
            // 
            renameToolStripMenuItem.Name = "renameToolStripMenuItem";
            renameToolStripMenuItem.Size = new Size(141, 24);
            renameToolStripMenuItem.Text = "&Rename...";
            renameToolStripMenuItem.Click += RenameToolStripMenuItem_Click;
            // 
            // statusStrip1
            // 
            statusStrip1.ImageScalingSize = new Size(20, 20);
            statusStrip1.Items.AddRange(new ToolStripItem[] { toolStripStatusLabel1 });
            statusStrip1.Location = new Point(0, 825);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(1478, 26);
            statusStrip1.SizingGrip = false;
            statusStrip1.TabIndex = 2;
            statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            toolStripStatusLabel1.Size = new Size(151, 20);
            toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1478, 851);
            Controls.Add(statusStrip1);
            Controls.Add(tabControl1);
            Controls.Add(menuStrip1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            KeyPreview = true;
            MainMenuStrip = menuStrip1;
            MaximizeBox = false;
            Name = "MainForm";
            SizeGripStyle = SizeGripStyle.Hide;
            Text = "Media Tools";
            FormClosed += Form1_FormClosed;
            Load += Form1_Load;
            KeyDown += Form1_KeyDown;
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tabPage1.PerformLayout();
            groupBox4.ResumeLayout(false);
            groupBox4.PerformLayout();
            subtitleGroupBox.ResumeLayout(false);
            subtitleGroupBox.PerformLayout();
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)optionDownloadRateLimitVal).EndInit();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)mediaFilesTable).EndInit();
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            contextMenuStrip.ResumeLayout(false);
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
        private ToolStripMenuItem reloadMediaFilesToolStripMenuItem;
        private Button download;
        private TextBox downloadIds;
        private Label label3;
        private TextBox downloadFolder;
        private GroupBox groupBox1;
        private CheckBox optionSponsorBlock;
        private CheckBox optionAddThumbnails;
        private CheckBox optionAddChapters;
        private CheckBox optionAddMetadata;
        private CheckBox optionAddSubtitles;
        private CheckBox optionAudioOnly;
        private ToolStripMenuItem showConsoleToolStripMenuItem;
        private ToolStripSeparator toolStripMenuItem1;
        private Label label4;
        private ComboBox optionResolution;
        private ContextMenuStrip contextMenuStrip;
        private ToolStripMenuItem contextDelete;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel toolStripStatusLabel1;
        private ToolStripMenuItem trashToolStripMenuItem;
        private ToolStripMenuItem renameToolStripMenuItem;
        private ToolStripMenuItem clearCacheToolStripMenuItem;
        private ToolStripMenuItem reloadMediaListToolStripMenuItem;
        private ToolStripSeparator toolStripMenuItem2;
        private ToolStripSeparator toolStripMenuItem3;
        private ToolStripMenuItem optionsToolStripMenuItem;
        private CheckBox optionMarkWatched;
        private CheckBox optionCookieLogin;
        private GroupBox groupBox3;
        private GroupBox groupBox2;
        private CheckBox optionAutoUpdate;
        private Label label5;
        private NumericUpDown optionDownloadRateLimitVal;
        private ComboBox optionDownloadRateLimitType;
        private GroupBox subtitleGroupBox;
        private CheckBox optionEmbedSubs;
        private Label label6;
        private TextBox optionSubtleLangs;
        private CheckBox optionDownloadChat;
        private ToolStripMenuItem playlistBuilderToolStripMenuItem;
        private GroupBox groupBox4;
        private CheckBox optionForceKeyframeAtCuts;
        private ComboBox actionOnComplete;
        private Label label1;
    }
}

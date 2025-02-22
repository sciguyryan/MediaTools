namespace MediaTools
{
    partial class PlaylistBuilderForm
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
            dataGridView1 = new DataGridView();
            export = new Button();
            exportFormat = new ComboBox();
            saveFileDialog1 = new SaveFileDialog();
            label1 = new Label();
            statusStrip1 = new StatusStrip();
            toolStripStatusLabel1 = new ToolStripStatusLabel();
            useAbsolutePaths = new CheckBox();
            LastModified = new DataGridViewTextBoxColumn();
            FullPath = new DataGridViewTextBoxColumn();
            FileName = new DataGridViewTextBoxColumn();
            Artist = new DataGridViewTextBoxColumn();
            Duration = new DataGridViewTextBoxColumn();
            RawDuration = new DataGridViewTextBoxColumn();
            Link = new DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            statusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // dataGridView1
            // 
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { LastModified, FullPath, FileName, Artist, Duration, RawDuration, Link });
            dataGridView1.Location = new Point(12, 12);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.Size = new Size(1250, 725);
            dataGridView1.TabIndex = 0;
            dataGridView1.KeyDown += DataGridView1_KeyDown;
            // 
            // export
            // 
            export.Location = new Point(364, 743);
            export.Name = "export";
            export.Size = new Size(94, 29);
            export.TabIndex = 3;
            export.Text = "&Export...";
            export.UseVisualStyleBackColor = true;
            export.Click += Export_Click;
            // 
            // exportFormat
            // 
            exportFormat.DropDownStyle = ComboBoxStyle.DropDownList;
            exportFormat.FormattingEnabled = true;
            exportFormat.Items.AddRange(new object[] { "M3U", "XSPF" });
            exportFormat.Location = new Point(77, 744);
            exportFormat.Name = "exportFormat";
            exportFormat.Size = new Size(140, 28);
            exportFormat.TabIndex = 1;
            // 
            // saveFileDialog1
            // 
            saveFileDialog1.Title = "Specify Output Path and Name";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 747);
            label1.Name = "label1";
            label1.Size = new Size(59, 20);
            label1.TabIndex = 3;
            label1.Text = "Format:";
            // 
            // statusStrip1
            // 
            statusStrip1.ImageScalingSize = new Size(20, 20);
            statusStrip1.Items.AddRange(new ToolStripItem[] { toolStripStatusLabel1 });
            statusStrip1.Location = new Point(0, 781);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(1276, 26);
            statusStrip1.TabIndex = 4;
            statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            toolStripStatusLabel1.Size = new Size(151, 20);
            toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // useAbsolutePaths
            // 
            useAbsolutePaths.AutoSize = true;
            useAbsolutePaths.Checked = true;
            useAbsolutePaths.CheckState = CheckState.Checked;
            useAbsolutePaths.Location = new Point(223, 746);
            useAbsolutePaths.Name = "useAbsolutePaths";
            useAbsolutePaths.Size = new Size(135, 24);
            useAbsolutePaths.TabIndex = 2;
            useAbsolutePaths.Text = "&Absolute Paths?";
            useAbsolutePaths.UseVisualStyleBackColor = true;
            // 
            // LastModified
            // 
            LastModified.FillWeight = 10F;
            LastModified.HeaderText = "Last Modified";
            LastModified.MinimumWidth = 6;
            LastModified.Name = "LastModified";
            LastModified.ReadOnly = true;
            // 
            // FullPath
            // 
            FullPath.FillWeight = 146.524063F;
            FullPath.HeaderText = "Path";
            FullPath.MinimumWidth = 6;
            FullPath.Name = "FullPath";
            FullPath.ReadOnly = true;
            FullPath.Visible = false;
            // 
            // FileName
            // 
            FileName.FillWeight = 47.57245F;
            FileName.HeaderText = "Name";
            FileName.MinimumWidth = 6;
            FileName.Name = "FileName";
            FileName.ReadOnly = true;
            // 
            // Artist
            // 
            Artist.FillWeight = 15F;
            Artist.HeaderText = "Artist";
            Artist.MinimumWidth = 6;
            Artist.Name = "Artist";
            Artist.ReadOnly = true;
            // 
            // Duration
            // 
            Duration.FillWeight = 10F;
            Duration.HeaderText = "Duration";
            Duration.MinimumWidth = 6;
            Duration.Name = "Duration";
            Duration.ReadOnly = true;
            // 
            // RawDuration
            // 
            RawDuration.FillWeight = 10F;
            RawDuration.HeaderText = "Raw Duration";
            RawDuration.MinimumWidth = 6;
            RawDuration.Name = "RawDuration";
            RawDuration.ReadOnly = true;
            RawDuration.Visible = false;
            // 
            // Link
            // 
            Link.HeaderText = "Link";
            Link.MinimumWidth = 6;
            Link.Name = "Link";
            Link.ReadOnly = true;
            Link.Visible = false;
            // 
            // PlaylistBuilderForm
            // 
            AllowDrop = true;
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1276, 807);
            Controls.Add(useAbsolutePaths);
            Controls.Add(statusStrip1);
            Controls.Add(label1);
            Controls.Add(exportFormat);
            Controls.Add(export);
            Controls.Add(dataGridView1);
            Name = "PlaylistBuilderForm";
            Text = "Media Tools :: Playlist Builder";
            DragDrop += PlaylistBuilderForm_DragDrop;
            DragEnter += PlaylistBuilderForm_DragEnter;
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dataGridView1;
        private Button export;
        private ComboBox exportFormat;
        private SaveFileDialog saveFileDialog1;
        private Label label1;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel toolStripStatusLabel1;
        private CheckBox useAbsolutePaths;
        private DataGridViewTextBoxColumn LastModified;
        private DataGridViewTextBoxColumn FullPath;
        private DataGridViewTextBoxColumn FileName;
        private DataGridViewTextBoxColumn Artist;
        private DataGridViewTextBoxColumn Duration;
        private DataGridViewTextBoxColumn RawDuration;
        private DataGridViewTextBoxColumn Link;
    }
}
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
            SuspendLayout();
            // 
            // optionIncludeFolders
            // 
            optionIncludeFolders.AutoSize = true;
            optionIncludeFolders.Location = new Point(12, 12);
            optionIncludeFolders.Name = "optionIncludeFolders";
            optionIncludeFolders.Size = new Size(126, 24);
            optionIncludeFolders.TabIndex = 1;
            optionIncludeFolders.Text = "Show &Folders?";
            optionIncludeFolders.UseVisualStyleBackColor = true;
            // 
            // optionIncludeSubMedia
            // 
            optionIncludeSubMedia.AutoSize = true;
            optionIncludeSubMedia.Location = new Point(12, 42);
            optionIncludeSubMedia.Name = "optionIncludeSubMedia";
            optionIncludeSubMedia.Size = new Size(239, 24);
            optionIncludeSubMedia.TabIndex = 2;
            optionIncludeSubMedia.Text = "Show &Media From Sub Folders?";
            optionIncludeSubMedia.UseVisualStyleBackColor = true;
            // 
            // okButton
            // 
            okButton.Location = new Point(12, 72);
            okButton.Name = "okButton";
            okButton.Size = new Size(94, 29);
            okButton.TabIndex = 3;
            okButton.Text = "&OK";
            okButton.UseVisualStyleBackColor = true;
            okButton.Click += Ok_Click;
            // 
            // OptionsForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(464, 111);
            Controls.Add(okButton);
            Controls.Add(optionIncludeSubMedia);
            Controls.Add(optionIncludeFolders);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "OptionsForm";
            SizeGripStyle = SizeGripStyle.Hide;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Media Tool :: Options";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private CheckBox optionIncludeFolders;
        private CheckBox optionIncludeSubMedia;
        private Button okButton;
    }
}
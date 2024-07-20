namespace MediaTools
{
    partial class RenameFileForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RenameFileForm));
            fileName = new TextBox();
            rename = new Button();
            SuspendLayout();
            // 
            // fileName
            // 
            resources.ApplyResources(fileName, "fileName");
            fileName.Name = "fileName";
            // 
            // rename
            // 
            resources.ApplyResources(rename, "rename");
            rename.Name = "rename";
            rename.UseVisualStyleBackColor = true;
            rename.Click += Rename_Click;
            // 
            // RenameFileForm
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(rename);
            Controls.Add(fileName);
            KeyPreview = true;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "RenameFileForm";
            SizeGripStyle = SizeGripStyle.Hide;
            KeyPress += RenameFileForm_KeyPress;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox fileName;
        private Button rename;
    }
}
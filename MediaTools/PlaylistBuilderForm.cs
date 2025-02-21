namespace MediaTools
{
    public partial class PlaylistBuilderForm : Form
    {
        public PlaylistBuilderForm()
        {
            IconModifier.SetFormIcon(this);

            InitializeComponent();

            exportFormat.SelectedIndex = 0;
            toolStripStatusLabel1.Text = "";
        }

        private void PlaylistBuilderForm_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect =
                e.Data!.GetDataPresent(DataFormats.FileDrop) ?
                    DragDropEffects.Copy : DragDropEffects.None;
        }

        private async void PlaylistBuilderForm_DragDrop(object sender, DragEventArgs e)
        {
            if (!e.Data!.GetDataPresent(DataFormats.FileDrop))
            {
                return;
            }

            var files = (string[])e.Data.GetData(DataFormats.FileDrop)!;
            foreach (var file in files)
            {
                var duration = await ProcessUtils.RunMediaDuration(file);
                if (duration == 0)
                {
                    // Not a media file we can recognise, we can skip this.
                    continue;
                }

                var fi = new FileInfo(file);
                dataGridView1.Rows.Add(
                    fi.LastWriteTime, file, fi.Name,
                    Utils.SecondsToDuration(duration, false), duration);
            }
        }

        private void Export_Click(object sender, EventArgs e)
        {
            switch (exportFormat.Text)
            {
                case "M3U":
                    saveFileDialog1.Filter = "M3U Files (*.m3u8)|*.m3u8";
                    break;
                default:
                    break;
            }

            if (saveFileDialog1.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            var outputPath = saveFileDialog1.FileName;

            switch (exportFormat.Text)
            {
                case "M3U":
                    WriteM3UFile(outputPath);
                    break;
            }
        }

        private const string M3UExInfoTemplate = "#EXTINF:{DURATION},{NAME}";

        private void WriteM3UFile(string path)
        {
            var lines = new List<string>() { "#EXTM3U" };

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                var duration = row.Cells["RawDuration"].Value!.ToString();
                var exInfo = 
                    M3UExInfoTemplate.Replace("{DURATION}", duration);

                var filePath = row.Cells["FullPath"].Value!.ToString()!;
                // This is to avoid breaking the syntax of the information line,
                // which uses a comma to separate values.
                var fi = new FileInfo(filePath);
                var name = Path.GetFileNameWithoutExtension(fi.Name).Replace(",", "，");
                exInfo = exInfo.Replace("{NAME}", name);

                lines.Add(exInfo);
                lines.Add(filePath);
            }

            try
            {
                File.WriteAllLines(path, lines);
                toolStripStatusLabel1.Text = "The playlist file was successfully written!";
            }
            catch (Exception ex)
            {
                toolStripStatusLabel1.Text = $"Failed to write playlist file: {ex.Message}";
            }
        }
    }
}

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
            if (e.Data is null)
            {
                return;
            }

            if (!e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                return;
            }

            var files = (string[])e.Data.GetData(DataFormats.FileDrop)!;
            foreach (var file in files)
            {
                var mi = await ProcessUtils.RunMediaInfoFull(file);
                if (mi?.Format is null)
                {
                    continue;
                }

                var duration = mi.Format.Duration;
                if (duration == 0)
                {
                    continue;
                }

                var artist = mi.Format.Tags?.GetValueOrDefault("artist", "") ?? "";
                var link = mi.Format.Tags?.GetValueOrDefault("purl", "") ?? "";

                var fi = new FileInfo(file);
                dataGridView1.Rows.Add(
                    fi.LastWriteTime, file, fi.Name, artist,
                    Utils.SecondsToDuration(duration, false), duration, link);
            }
        }

        private void DataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e)
            {
                case { Control: true, Shift: true, KeyCode: Keys.Delete }:
                    {
                        dataGridView1.Rows.Clear();
                        e.Handled = true;
                        break;
                    }
            }
        }

        private void Export_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count == 0)
            {
                toolStripStatusLabel1.Text = "Unable to create a playlist containing no items.";
                return;
            }

            switch (exportFormat.Text)
            {
                case "M3U":
                    saveFileDialog1.Filter = "M3U Files (*.m3u8)|*.m3u8";
                    break;
                case "XSPF":
                    saveFileDialog1.Filter = "XSPF Files (*.xspf)|*.xspf";
                    break;
                default:
                    break;
            }

            if (saveFileDialog1.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            var outputPath = saveFileDialog1.FileName;

            var builder = new PlaylistBuilder(ref dataGridView1, useAbsolutePaths.Checked, outputPath);

            switch (exportFormat.Text)
            {
                case "M3U":
                    builder.SerialiseM3U();
                    break;
                case "XSPF":
                    builder.SerialiseXspf();
                    break;
            }

            toolStripStatusLabel1.Text =
                builder.Write() ?
                    "The playlist file was successfully written!" :
                    "Failed to write playlist file!";
        }
    }
}

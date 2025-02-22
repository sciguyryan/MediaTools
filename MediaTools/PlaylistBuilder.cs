using System.Net;

namespace MediaTools
{
    internal class PlaylistEntry(string duration, string filePath, string fileName, string artist, string link)
    {
        public string Duration = duration;
        public string FilePath = filePath;
        public string FileName = fileName;
        public string Artist = artist;
        public string Link = link;
    }

    internal class PlaylistBuilder
    {
        private readonly bool _useAbsolutePaths;
        private readonly string _outFilePath;
        private readonly List<string> _lines = [];
        private readonly List<PlaylistEntry> _data = [];


        public PlaylistBuilder(ref DataGridView view, bool useAbsolutePaths, string outFilePath)
        {
            _useAbsolutePaths = useAbsolutePaths;
            _outFilePath = outFilePath;

            ExtractData(ref view);
        }

        private void ExtractData(ref DataGridView view)
        {
            foreach (DataGridViewRow row in view.Rows)
            {
                var duration = row.Cells["RawDuration"].Value!.ToString()!;
                var fullPath = row.Cells["FullPath"].Value!.ToString()!;

                var fi = new FileInfo(fullPath);
                var fileName = fi.Name;

                var artist = row.Cells["Artist"].Value!.ToString()!;
                var link = row.Cells["Link"].Value!.ToString()!;

                _data.Add(new PlaylistEntry(duration, fullPath, fileName, artist, link));
            }
        }

        public void SerialiseM3U()
        {
            _lines.Add("#EXTM3U");

            foreach (var entry in _data)
            {
                var metaInfoLine = $"#EXTINF:{entry.Duration},";

                // This is to avoid breaking the syntax of the information line,
                // which uses a comma to separate values.
                var name = Path.GetFileNameWithoutExtension(entry.FileName).Replace(",", "，");
                metaInfoLine += name;

                _lines.Add(metaInfoLine);
                _lines.Add(_useAbsolutePaths ? entry.FilePath : entry.FileName);
            }
        }

        public void SerialiseXspf()
        {
            _lines.Add("<?xml version=\"1.1\" encoding=\"UTF-8\"?>");
            _lines.Add("<playlist version=\"1\" xmlns=\"http://xspf.org/ns/0/\">");
            _lines.Add("\t<trackList>");

            foreach (var entry in _data)
            {
                _lines.Add("\t\t<track>");

                _lines.Add($"\t\t\t<title>{WebUtility.HtmlEncode(entry.FileName)}</title>");
                var location = _useAbsolutePaths ? $"file:///{entry.FilePath}" : entry.FileName;
                _lines.Add($"\t\t\t<location>{location}</location>");
                _lines.Add($"\t\t\t<duration>{Math.Round(double.Parse(entry.Duration) * 1000)}</duration>");
                _lines.Add($"\t\t\t<artist>{WebUtility.HtmlEncode(entry.Artist)}</artist>");
                _lines.Add($"\t\t\t<info>{entry.Link}</info>");

                _lines.Add("\t\t</track>");
            }

            _lines.Add("\t</trackList>");
            _lines.Add("</playlist>");
        }

        public bool Write()
        {
            try
            {
                File.WriteAllLines(_outFilePath, _lines);
                _lines.Clear();

                return true;
            }
            catch
            {
                _lines.Clear();
                return false;
            }
        }
    }
}

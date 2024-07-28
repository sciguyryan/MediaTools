using System.Text.Json;

namespace MediaTools
{
    internal class Settings
    {
        private const string FileName = "settings.json";

        public bool ShowFolders { get; set; } = true;

        public bool ShowMediaInSubFolders { get; set; }

        public string CookiePath { get; set; } = "";

        public bool RememberDownloadOptions { get; set; } = true;

        public DownloadSettings DownloadOptions { get; set; } = new();

        public void WriteSettings()
        {
            var json = JsonSerializer.Serialize(this);
            File.WriteAllText($".\\{FileName}", json);
        }

        public static Settings ReadSettings()
        {
            if (!File.Exists(FileName))
            {
                return new Settings();
            }

            var json = File.ReadAllText(FileName);
            var deserialized = JsonSerializer.Deserialize<Settings>(json);

            return deserialized ?? new Settings();
        }
    }

    internal class DownloadSettings
    {
        // Basic Options

        public bool AddSubtitles { get; set; }

        public bool AddMetadata { get; set; }

        public bool AddChapters { get; set; }

        public bool AddThumbnails { get; set; }

        // Advanced Options



        // Other Options

        public bool AutoUpdate { get; set; }

        public bool CookieLogin { get; set; }

        public bool MarkWatched { get; set; }

        public bool UseSponsorBlock { get; set; }

        public int TargetResolutionIndex { get; set; }
    }
}

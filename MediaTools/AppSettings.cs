﻿using System.Text.Json;

namespace MediaTools
{
    internal class AppSettings
    {
        private const string FileName = "settings.json";

        public bool ShowFolders { get; set; } = true;

        public bool ShowMediaInSubFolders { get; set; }

        public string CookiePath { get; set; } = "";

        public bool RememberDownloadOptions { get; set; } = true;

        public DownloadSettings DownloadOptions { get; set; } = new();

        public string MediaDirectory { get; set; } = "";

        public string FfprobePath { get; set; } = "";

        public string YtDlpPath { get; set; } = "";

        public string MediaPlayerPath { get; set; } = "";

        public void WriteSettings()
        {
            var json = JsonSerializer.Serialize(this);
            File.WriteAllText($".\\{FileName}", json);
        }

        public static AppSettings ReadSettings()
        {
            if (!File.Exists(FileName))
            {
                return new AppSettings();
            }

            var json = File.ReadAllText(FileName);
            var deserialized = JsonSerializer.Deserialize<AppSettings>(json);

            return deserialized ?? new AppSettings();
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

        public int TargetResolutionIndex { get; set; }

        public decimal DownloadRateLimit { get; set; }

        public int DownloadRateLimitTypeIndex { get; set; }

        public bool DownloadChat { get; set; }


        // Other Options

        public bool AutoUpdate { get; set; }

        public bool CookieLogin { get; set; }

        public bool MarkWatched { get; set; }

        public bool UseSponsorBlock { get; set; }

        // Subtitle Options

        public bool EmbedSubtitles { get; set; }

        public string? SubtitleLanguages { get; set; }
    }
}

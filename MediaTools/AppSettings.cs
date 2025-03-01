﻿using System.Text.Json;

namespace MediaTools
{
    internal class AppSettings
    {
        private const string FileName = "settings.json";

        public bool ShowFolders { get; set; } = true;

        public bool ShowConsole { get; set; } = true;

        public bool ShowMediaInSubFolders { get; set; }

        public string CookiePath { get; set; } = "";

        public bool RememberDownloadOptions { get; set; } = true;

        public DownloadSettings DownloadOptions { get; set; } = new();

        public string MediaDirectory { get; set; } = "";

        public string TempDirectory { get; set; } = "";

        public string FfmpegDirectory { get; set; } = "";

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
        public bool AddSubtitles { get; set; }

        public bool AddMetadata { get; set; }

        public bool AddChapters { get; set; }

        public bool AddThumbnails { get; set; }

        public int TargetResolutionIndex { get; set; }

        public decimal DownloadRateLimit { get; set; }

        public int DownloadRateLimitTypeIndex { get; set; }

        public bool ForceKeyframesAtCuts { get; set; }

        public bool AutoUpdate { get; set; }

        public bool CookieLogin { get; set; }

        public bool MarkWatched { get; set; }

        public bool UseSponsorBlock { get; set; }

        public bool DownloadChat { get; set; }

        public bool EmbedSubtitles { get; set; }

        public string? SubtitleLanguages { get; set; }
    }
}

using System.Text.RegularExpressions;

namespace MediaTools
{
    internal enum DownloadType
    {
        Single,
        Multiple
    }

    internal class UrlEntry(string url, DownloadType downloadType)
    {
        public string Url = url;
        public DownloadType DownloadType = downloadType;
    }

    internal partial class UrlProcessor
    {
        [GeneratedRegex(@"^\d{9,}$", RegexOptions.IgnoreCase)]
        private static partial Regex TwitchVideoRegex();

        [GeneratedRegex(@"^[a-zA-Z0-9]{10}$", RegexOptions.IgnoreCase)]
        private static partial Regex TwitchCollectionRegex();

        [GeneratedRegex(@"^[A-Za-z0-9_-]{10}[AEIMQUYcgkosw048]$", RegexOptions.IgnoreCase)]
        private static partial Regex YouTubeVideoRegex();

        [GeneratedRegex(@"^PL[A-Za-z0-9_-]{10}[A-Za-z0-9_-]{22}$", RegexOptions.IgnoreCase)]
        private static partial Regex YouTubePlaylistRegex();

        public static UrlEntry[] BuildDownloadUrlList(string[] input)
        {
            var entries = new List<UrlEntry>();

            foreach (var line in input)
            {
                var trimmed = line.Trim();
                if (trimmed.StartsWith("http"))
                {
                    entries.Add(TryGuessUrlType(trimmed));
                }

                var entry = TryAsYouTubeId(trimmed) ?? TryAsTwitchId(trimmed);

                if (entry is null)
                {
                    Console.WriteLine($"Unable to identify id {trimmed}, this entry will be skipped.");
                }

                if (entry is not null && entry.Url != "" &&
                    entries.All(obj => entry.Url != obj.Url))
                {
                    entries.Add(entry);
                }
            }

            return [.. entries];
        }

        private static UrlEntry TryGuessUrlType(string url)
        {
            var urlType = DownloadType.Single;

            var lower = url.ToLower();
            if (lower.Contains("youtube") || lower.Contains("youtu"))
            {
                if (lower.Contains("playlist"))
                {
                    urlType = DownloadType.Multiple;
                }
                // Otherwise we will consider it to be a single video.
            }
            else if (lower.Contains("twitch"))
            {
                if (lower.Contains("collections"))
                {
                    urlType = DownloadType.Multiple;
                }
                // Otherwise we will consider it to be a single video.
            }
            else
            {
                Console.WriteLine("Unknown website, the URL will be assumed as being a single download target...");
            }

            return new UrlEntry(url, urlType);
        }

        private static UrlEntry? TryAsYouTubeId(string id)
        {
            if (YouTubeVideoRegex().IsMatch(id))
            {
                return new UrlEntry($"https://www.youtube.com/watch?v={id}", DownloadType.Single);
            }
            else if (YouTubePlaylistRegex().IsMatch(id))
            {
                return new UrlEntry($"https://www.youtube.com/playlist?list={id}", DownloadType.Multiple);
            }
            else
            {
                return null;
            }
        }

        private static UrlEntry? TryAsTwitchId(string id)
        {
            if (TwitchVideoRegex().IsMatch(id))
            {
                return new UrlEntry($"https://www.twitch.tv/videos/{id}", DownloadType.Single);
            }
            else if (TwitchCollectionRegex().IsMatch(id))
            {
                return new UrlEntry($"https://www.twitch.tv/collections/{id}", DownloadType.Multiple);
            }
            else
            {
                return null;
            }
        }
    }
}

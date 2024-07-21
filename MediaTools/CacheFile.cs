using System.Text;

namespace MediaTools
{
    internal class CacheEntry(long lastModified, int duration, string path)
    {
        public long LastModified = lastModified;
        public int Duration = duration;
        public string Path = path;
    }

    internal static class CacheFile
    {
        private static readonly byte[] FileHeader = [0x4C, 0xA8, 0x3, 0x4B];

        public static bool WriteCacheFile(ReadOnlySpan<CacheEntry> entries, string filePath)
        {
            var rawDataBytes = new List<byte>();

            foreach (var entry in entries)
            {
                var pathBytes = Encoding.UTF8.GetBytes(entry.Path);

                rawDataBytes.AddRange(BitConverter.GetBytes((ushort)pathBytes.Length));
                rawDataBytes.AddRange(BitConverter.GetBytes(entry.LastModified));
                rawDataBytes.AddRange(BitConverter.GetBytes(entry.Duration));
                rawDataBytes.AddRange(pathBytes);
            }

            var rawDataBytesArr = rawDataBytes.ToArray();
            var compressed = Utils.Compress(rawDataBytesArr);

            try
            {
                using var file = File.OpenWrite(filePath);
                file.Write(FileHeader);
                file.Write(compressed);
            }
            catch
            {
                return false;
            }

            return true;
        }

        public static CacheEntry[] ReadCacheFile(string filePath)
        {
            var readBytes = File.ReadAllBytes(filePath);

            var hasHeader = readBytes[..FileHeader.Length].SequenceEqual(FileHeader);
            if (!hasHeader)
            {
                return [];
            }

            // Decompress the data.
            byte[] decompressed;
            try
            {
                decompressed = Utils.Decompress(readBytes[FileHeader.Length..]);
            }
            catch
            {
                return [];
            }

            var span = decompressed.AsSpan();

            // The size of the length indicator, the last modified and duration, in bytes.
            const int fixedEntryLength = 8 + 4;

            var results = new List<CacheEntry>();

            var index = 0;
            while (index < span.Length)
            {
                // This would indicate a malformed cache file.
                if ((index + 2) >= span.Length)
                {
                    return [];
                }

                // Get the individual entry.
                // [Path Length][Last Modified][Duration][Path]

                var pathLength = BitConverter.ToUInt16(span[index..(index + 2)]);
                index += 2;

                if ((index + fixedEntryLength + pathLength) > span.Length)
                {
                    // Insufficient space for the entry to be read.
                    return [];
                }

                var lastModified = BitConverter.ToInt64(span[index..(index + 8)]);
                index += 8;

                var duration = BitConverter.ToInt32(span[index..(index + 4)]);
                index += 4;

                var path = Encoding.UTF8.GetString(span[index..(index + pathLength)]);
                index += pathLength;

                results.Add(new CacheEntry(lastModified, duration, path));
            }

            return results.ToArray();
        }
    }
}

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
        private const byte FileVersion = 0;
        private static readonly byte[] FileHeader = [0x4C, 0xA8, 0x3, 0x4B];

        public static bool WriteCacheFile(ReadOnlySpan<CacheEntry> entries, string filePath)
        {
            using var memoryStream = new MemoryStream();

            // Store each entry.
            foreach (var entry in entries)
            {
                var pathBytes = Encoding.UTF8.GetBytes(entry.Path);

                memoryStream.Write(BitConverter.GetBytes((ushort)pathBytes.Length));
                memoryStream.Write(BitConverter.GetBytes(entry.LastModified));
                memoryStream.Write(BitConverter.GetBytes(entry.Duration));
                memoryStream.Write(pathBytes);
            }

            try
            {
                // Compress the entries.
                var compressed = Utils.Compress(memoryStream.ToArray());

                // Hash the compressed data.
                var hash = Utils.ComputeMd5ByteHash(compressed);

                // Write the file structure.
                using var file = File.OpenWrite(filePath);
                file.Write(FileHeader);
                file.WriteByte(FileVersion);
                file.Write(BitConverter.GetBytes(entries.Length));
                file.Write(hash);
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
            if (readBytes.Length < 21)
            {
                // There will be insufficient space for the file metadata.
                return [];
            }

            var hasHeader = readBytes[..4].SequenceEqual(FileHeader);
            if (!hasHeader)
            {
                return [];
            }

            return readBytes[4] switch
            {
                0 => ReadVersion1File(readBytes[5..].AsSpan()),
                _ => []
            };
        }

        private static CacheEntry[] ReadVersion1File(ReadOnlySpan<byte> bytes)
        {
            // The entry count comes before the hash.
            var entryCount = BitConverter.ToInt32(bytes[..4]);

            // Validate the hash of the compressed data.
            if (!ValidateVersion1Hash(bytes[20..], bytes[4..20]))
            {
                return [];
            }

            // The compressed data follows the hash.
            byte[] decompressed;
            try
            {
                decompressed = Utils.Decompress(bytes[20..].ToArray());
            }
            catch
            {
                return [];
            }

            // We will be unable to get the number of stored cache entries.
            var span = decompressed.AsSpan();

            // The size of the length indicator, the last modified and duration, in bytes.
            const int fixedEntryLength = 8 + 4;

            var results = new CacheEntry[entryCount];

            var entry = 0;
            var index = 0;
            while (index < span.Length)
            {
                // This would indicate a malformed cache file. We can't validate
                // the integrity of the returned data, so it's safer to start
                // over from scratch.
                if ((index + 2) >= span.Length)
                {
                    return [];
                }

                // Read an individual entry.
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

                results[entry++] = new CacheEntry(lastModified, duration, path);
            }

            return results;
        }

        private static bool ValidateVersion1Hash(
            ReadOnlySpan<byte> bytes,
            ReadOnlySpan<byte> expectedHash
        )
        {
            return expectedHash.SequenceEqual(Utils.ComputeMd5ByteHash(bytes));
        }
    }
}

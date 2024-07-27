using System.Text;
using System.Text.Json;

namespace MediaTools
{
    internal class CacheEntry(long lastModified, int duration, string path)
    {
        public long LastModified { get; set; } = lastModified;
        public int Duration { get; set; } = duration;
        public string Path { get; set; } = path;
    }

    internal static class CacheFile
    {
        private const byte FileVersion = 0;
        private static readonly byte[] FileHeader = [0x4C, 0xA8, 0x3, 0x4B];
        public const string CachePath = @"D:\Downloads\YouTube\tools\cache.dat";

        public static bool Exists()
        {
            return File.Exists(CachePath);
        }

        public static bool Write(CacheEntry[] entries)
        {
            if (FileUtils.IsFileLocked(CachePath))
            {
                MessageBox.Show(
                    DisplayBuilders.ErrorCacheFileOpen.BuildPlain(),
                    DisplayBuilders.ErrorCacheFileOpenTitle.BuildPlain(),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error,
                    MessageBoxDefaultButton.Button1
                );

                return false;
            }

            var json = JsonSerializer.Serialize(entries);
            var compressed = Utils.Compress(Encoding.UTF8.GetBytes(json));

            // Hash the compressed data.
            var hash = Utils.ComputeMd5ByteHash(compressed);

            try
            {
                using var stream = new FileStream(CachePath, FileMode.Create);
                stream.Write(FileHeader);
                stream.WriteByte(FileVersion);
                stream.Write(hash);
                stream.Write(compressed);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public static CacheEntry[] Read()
        {
            var entries = ReadInternal();
            if (entries.Length == 0 && Path.Exists(CachePath))
            {
                // The file may be corrupted and may need to be rebuilt.
                FileUtils.TruncateFile(CachePath);
            }

            return entries;
        }

        private static CacheEntry[] ReadInternal()
        {
            if (!Path.Exists(CachePath))
            {
                return [];
            }

            var readBytes = File.ReadAllBytes(CachePath);
            if (readBytes.Length < 21)
            {
                Console.WriteLine("too small");
                // There will be insufficient space for the file metadata.
                return [];
            }

            var hasHeader = readBytes[..4].SequenceEqual(FileHeader);
            if (!hasHeader)
            {
                Console.WriteLine("invalid header");
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
            // Validate the hash of the compressed data.
            if (!ValidateVersion1Hash(bytes[16..], bytes[..16]))
            {
                Console.WriteLine("bad hash");
                return [];
            }

            byte[] decompressed;
            try
            {
                decompressed = Utils.Decompress(bytes[16..].ToArray());
            }
            catch
            {
                Console.WriteLine("unable to decompress");
                return [];
            }

            var jsonString = Encoding.UTF8.GetString(decompressed);
            return JsonSerializer.Deserialize<CacheEntry[]>(jsonString) ?? [];
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

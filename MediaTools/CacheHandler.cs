﻿using System.Text;
using System.Text.Json;

namespace MediaTools
{
    internal class CacheEntry(long lastModified, int duration, string path)
    {
        public long LastModified { get; set; } = lastModified;
        public int Duration { get; set; } = duration;
        public string Path { get; set; } = path;
    }

    internal class CacheHandler(string path)
    {
        private const byte FileVersion = 0;
        private static readonly byte[] FileHeader = [0x4C, 0xA8, 0x3, 0x4B];

        public bool Exists()
        {
            return File.Exists(path);
        }

        public bool Write(CacheEntry[] entries)
        {
            if (FileUtils.IsFileLocked(path))
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
                using var stream = new FileStream(path, FileMode.Create);
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

        public CacheEntry[] Read()
        {
            var entries = ReadInternal();
            if (entries.Length == 0 && Path.Exists(path))
            {
                // The file may be corrupted and may need to be rebuilt.
                FileUtils.TruncateFile(path);
            }

            return entries;
        }

        private CacheEntry[] ReadInternal()
        {
            if (!Path.Exists(path))
            {
                return [];
            }

            var readBytes = File.ReadAllBytes(path);
            if (readBytes.Length < 17)
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
            // Validate the hash of the compressed data.
            if (!ValidateVersion1Hash(bytes[16..], bytes[..16]))
            {
                return [];
            }

            byte[] decompressed;
            try
            {
                decompressed = Utils.Decompress(bytes[16..].ToArray());
            }
            catch
            {
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

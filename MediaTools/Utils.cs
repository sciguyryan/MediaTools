using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;

namespace MediaTools
{
    internal class Utils
    {
        public static string SecondsToDuration(double seconds, bool longFormat)
        {
            var format = longFormat ? @"dd\:hh\:mm\:ss" : @"hh\:mm\:ss";
            return TimeSpan.FromSeconds(seconds).ToString(format);
        }

        public static byte[] ComputeMd5ByteHash(ReadOnlySpan<byte> bytes)
        {
            return MD5.HashData(bytes);
        }

        public static string ByteArrayToHexString(ReadOnlySpan<byte> bytes)
        {
            var hash = new StringBuilder();
            foreach (var b in bytes)
            {
                hash.Append(b.ToString("x2"));
            }

            return hash.ToString();
        }

        public static string ComputeMd5Hash(string input)
        {
            var hashBytes = MD5.HashData((Encoding.UTF8.GetBytes(input)));
            return ByteArrayToHexString(hashBytes);
        }

        public static byte[] Compress(byte[] bytes)
        {
            using var memoryStream = new MemoryStream();
            using (var gzipStream = new DeflateStream(memoryStream, CompressionLevel.Optimal))
            {
                gzipStream.Write(bytes, 0, bytes.Length);
            }

            return memoryStream.ToArray();
        }

        public static byte[] Decompress(byte[] bytes)
        {
            var compressedStream = new MemoryStream(bytes);

            using var decompressorStream = new DeflateStream(compressedStream, CompressionMode.Decompress);
            using var decompressedStream = new MemoryStream();
            decompressorStream.CopyTo(decompressedStream);

            var decompressedBytes = decompressedStream.ToArray();

            return decompressedBytes;
        }

        public static string TruncateString(string str, int maxLength = 32)
        {
            if (str.Length <= maxLength)
            {
                return str;
            }

            return string.Concat(str.Take(maxLength - 3)) + "...";
        }
    }
}

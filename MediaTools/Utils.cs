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
            using var gzipStream = new GZipStream(memoryStream, CompressionLevel.Optimal);
            gzipStream.Write(bytes, 0, bytes.Length);
            gzipStream.Flush();
            return memoryStream.ToArray();
        }

        public static byte[] Decompress(byte[] bytes)
        {
            using var memoryStream = new MemoryStream(bytes);
            using var outputStream = new MemoryStream();
            using var decompressStream = new GZipStream(memoryStream, CompressionMode.Decompress);
            decompressStream.CopyTo(outputStream);
            decompressStream.Flush();
            return outputStream.ToArray();
        }
    }
}

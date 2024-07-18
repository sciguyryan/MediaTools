using System.Security.Cryptography;
using System.Text;

namespace MediaTools
{
    internal partial class Utils
    {
        public static string SecondsToDuration(double seconds, bool longFormat)
        {
            var format = longFormat ? @"dd\:hh\:mm\:ss" : @"hh\:mm\:ss";
            return TimeSpan.FromSeconds(seconds).ToString(format);
        }

        public static string ComputeSha384Hash(string input)
        {
            using var sha384 = SHA3_384.Create();

            var hashBytes = sha384.ComputeHash(Encoding.UTF8.GetBytes(input));
            var hash = new StringBuilder();
            foreach (var b in hashBytes)
            {
                hash.Append(b.ToString("x2"));
            }

            return hash.ToString();
        }
    }
}
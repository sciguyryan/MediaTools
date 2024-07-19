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

        public static string ComputeMD5Hash(string input)
        {
            var hashBytes = MD5.HashData((Encoding.UTF8.GetBytes(input)));
            var hash = new StringBuilder();
            foreach (var b in hashBytes)
            {
                hash.Append(b.ToString("x2"));
            }

            return hash.ToString();
        }
    }
}
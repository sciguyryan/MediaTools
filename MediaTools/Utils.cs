using System.Text.RegularExpressions;

namespace MediaTools
{
    internal partial class Utils
    {
        public static string SecondsToDuration(double seconds, bool longFormat)
        {
            var format = longFormat ? @"dd\:hh\:mm\:ss" : @"hh\:mm\:ss";
            return TimeSpan.FromSeconds(seconds).ToString(format);
        }
    }
}
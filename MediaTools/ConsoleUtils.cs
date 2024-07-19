using System.Text;
using System.Text.RegularExpressions;

namespace MediaTools
{
    internal enum ConsoleColour
    {
        Black,
        DarkRed,
        DarkGreen,
        DarkYellow,
        DarkBlue,
        DarkMagenta,
        DarkCyan,
        Gray,
        DarkGray,
        Red,
        Green,
        Yellow,
        Blue,
        Magenta,
        Cyan,
        White
    }

    internal partial class OutputFormatBuilder
    {
        private readonly StringBuilder _output = new();

        public OutputFormatBuilder Text(string s)
        {
            _output.Append(s);
            return this;
        }

        public OutputFormatBuilder Bold()
        {
            _output.Append("\x1b[1m");
            return this;
        }

        public OutputFormatBuilder NotBold()
        {
            _output.Append("\x1b[22m");
            return this;
        }

        public OutputFormatBuilder Italic()
        {
            _output.Append("\x1b[3m");
            return this;
        }

        public OutputFormatBuilder NotItalic()
        {
            _output.Append("\x1b[23m");
            return this;
        }

        public OutputFormatBuilder Underline()
        {
            _output.Append("\x1b[4m");
            return this;
        }

        public OutputFormatBuilder NotUnderline()
        {
            _output.Append("\x1b[24m");
            return this;
        }

        public OutputFormatBuilder Strike()
        {
            _output.Append("\x1b[9m");
            return this;
        }

        public OutputFormatBuilder NotStrike()
        {
            _output.Append("\x1b[29m");
            return this;
        }

        public OutputFormatBuilder DoubleUnderline()
        {
            _output.Append("\x1b[21m");
            return this;
        }

        public OutputFormatBuilder NotDoubleUnderline()
        {
            _output.Append("\x1b[24m");
            return this;
        }

        public OutputFormatBuilder Foreground(ConsoleColour c)
        {
            var code = c switch
            {
                ConsoleColour.Black => "30",
                ConsoleColour.DarkRed => "31",
                ConsoleColour.DarkGreen => "32",
                ConsoleColour.DarkYellow => "33",
                ConsoleColour.DarkBlue => "34",
                ConsoleColour.DarkMagenta => "35",
                ConsoleColour.DarkCyan => "36",
                ConsoleColour.Gray => "37",
                ConsoleColour.DarkGray => "90",
                ConsoleColour.Red => "91",
                ConsoleColour.Green => "92",
                ConsoleColour.Yellow => "93",
                ConsoleColour.Blue => "94",
                ConsoleColour.Magenta => "95",
                ConsoleColour.Cyan => "96",
                ConsoleColour.White => "97",
                _ => throw new ArgumentOutOfRangeException(nameof(c), c, null)
            };

            _output.Append($"\x1b[{code}m");
            return this;
        }

        public OutputFormatBuilder Foreground(int r, int g, int b)
        {
            _output.Append($"\x1b[38;2;{r};{g};{b}m");
            return this;
        }

        public OutputFormatBuilder Foreground(Color c)
        {
            _output.Append($"\x1b[38;2;{c.R};{c.G};{c.B}m");
            return this;
        }

        public OutputFormatBuilder ResetForeground()
        {
            _output.Append($"\x1b[39m");
            return this;
        }

        public OutputFormatBuilder Background(ConsoleColour c)
        {
            var code = c switch
            {
                ConsoleColour.Black => "40",
                ConsoleColour.DarkRed => "41",
                ConsoleColour.DarkGreen => "42",
                ConsoleColour.DarkYellow => "43",
                ConsoleColour.DarkBlue => "44",
                ConsoleColour.DarkMagenta => "45",
                ConsoleColour.DarkCyan => "46",
                ConsoleColour.Gray => "47",
                ConsoleColour.DarkGray => "100",
                ConsoleColour.Red => "101",
                ConsoleColour.Green => "102",
                ConsoleColour.Yellow => "103",
                ConsoleColour.Blue => "104",
                ConsoleColour.Magenta => "105",
                ConsoleColour.Cyan => "106",
                ConsoleColour.White => "107",
                _ => throw new ArgumentOutOfRangeException(nameof(c), c, null)
            };

            _output.Append($"\x1b[{code}m");
            return this;
        }

        public OutputFormatBuilder Background(int r, int g, int b)
        {
            _output.Append($"\x1b[48;2;{r};{g};{b}m");
            return this;
        }

        public OutputFormatBuilder Background(Color c)
        {
            _output.Append($"\x1b[48;2;{c.R};{c.G};{c.B}m");
            return this;
        }

        public OutputFormatBuilder ResetBackground()
        {
            _output.Append($"\x1b[49m");
            return this;
        }

        public OutputFormatBuilder Reverse()
        {
            _output.Append("\x1b[7m");
            return this;
        }

        public OutputFormatBuilder NotReverse()
        {
            _output.Append("\x1b[27m");
            return this;
        }

        public OutputFormatBuilder Clear()
        {
            _output.Append("\x1b[0m");
            return this;
        }

        public (string, string) HandleBinds(ReadOnlySpan<object> binds)
        {
            // Ensure we always clear the formatting, so it doesn't bleed into other
            // following entries in the console window...
            Clear();

            var outFormatted = new StringBuilder(_output.ToString());
            var outPlain = new StringBuilder(StripFormatting());

            for (var i = 0; i < binds.Length; i++)
            {
                var str = binds[i].ToString();
                outFormatted = outFormatted.Replace($"[{i}]", str);
                outPlain = outPlain.Replace($"[{i}]", str);
            }

            return (outFormatted.ToString(), outPlain.ToString());
        }

        public string Build(ReadOnlySpan<object> binds)
        {
            var (outFormatted, _) = HandleBinds(binds);

            return outFormatted;
        }

        public string BuildPlain(ReadOnlySpan<object> binds)
        {
            var (_, outPlain) = HandleBinds(binds);

            return outPlain;
        }

        [GeneratedRegex(@"\x1b\[[^m]+m", RegexOptions.None, "en-US")]
        private static partial Regex StripAnsiFormattingRegex();

        private string StripFormatting()
        {
            var str = _output.ToString();
            return StripAnsiFormattingRegex().Replace(str, "");
        }
    }
}

using Miku.Core;
using System.Text;

namespace MikuLib.Console
{
    /// <summary>
    /// Provides colored console output utilities with TrueColor (24-bit RGB) and 256-color support.
    /// </summary>
    public static class MikuConsole
    {
        private static readonly object _lock = new();
        private static bool? _supportsAnsi;

        #region Properties
        public static bool SupportsAnsi => _supportsAnsi ??= CheckAnsiSupport();
        public static int WindowWidth => System.Console.WindowWidth;
        public static int WindowHeight => System.Console.WindowHeight;
        public static int CursorLeft => System.Console.CursorLeft;
        public static int CursorTop => System.Console.CursorTop;
        #endregion

        #region Initialization
        public static void Initialize()
        {
            System.Console.OutputEncoding = Encoding.UTF8;
        }
        #endregion

        #region Write Methods - Plain
        public static void Write(string text)
        {
            lock (_lock) { System.Console.Write(text); }
        }

        public static void WriteLine(string text)
        {
            lock (_lock) { System.Console.WriteLine(text); }
        }

        public static void WriteLine()
        {
            System.Console.WriteLine();
        }
        #endregion

        #region Write Methods - TrueColor
        public static void Write(string text, MikuRgbColor foreground)
        {
            lock (_lock)
            {
                System.Console.Write($"{MikuAnsiCodes.ForegroundRgb(foreground)}{text}{MikuAnsiCodes.Reset}");
            }
        }

        public static void Write(string text, MikuRgbColor foreground, MikuRgbColor background)
        {
            lock (_lock)
            {
                System.Console.Write($"{MikuAnsiCodes.ForegroundRgb(foreground)}{MikuAnsiCodes.BackgroundRgb(background)}{text}{MikuAnsiCodes.Reset}");
            }
        }

        public static void WriteLine(string text, MikuRgbColor foreground)
        {
            Write(text, foreground);
            System.Console.WriteLine();
        }

        public static void WriteLine(string text, MikuRgbColor foreground, MikuRgbColor background)
        {
            Write(text, foreground, background);
            System.Console.WriteLine();
        }
        #endregion

        #region Write Methods - 256 Color
        public static void Write256(string text, byte colorCode)
        {
            lock (_lock)
            {
                System.Console.Write($"{MikuAnsiCodes.Foreground256(colorCode)}{text}{MikuAnsiCodes.Reset}");
            }
        }

        public static void Write256(string text, byte foreground, byte background)
        {
            lock (_lock)
            {
                System.Console.Write($"{MikuAnsiCodes.Foreground256(foreground)}{MikuAnsiCodes.Background256(background)}{text}{MikuAnsiCodes.Reset}");
            }
        }

        public static void WriteLine256(string text, byte colorCode)
        {
            Write256(text, colorCode);
            System.Console.WriteLine();
        }
        #endregion

        #region Styled Write Methods
        public static void WriteBold(string text, MikuRgbColor foreground)
        {
            lock (_lock)
            {
                System.Console.Write($"{MikuAnsiCodes.Bold}{MikuAnsiCodes.ForegroundRgb(foreground)}{text}{MikuAnsiCodes.Reset}");
            }
        }

        public static void WriteUnderline(string text, MikuRgbColor foreground)
        {
            lock (_lock)
            {
                System.Console.Write($"{MikuAnsiCodes.Underline}{MikuAnsiCodes.ForegroundRgb(foreground)}{text}{MikuAnsiCodes.Reset}");
            }
        }

        public static void WriteItalic(string text, MikuRgbColor foreground)
        {
            lock (_lock)
            {
                System.Console.Write($"{MikuAnsiCodes.Italic}{MikuAnsiCodes.ForegroundRgb(foreground)}{text}{MikuAnsiCodes.Reset}");
            }
        }

        public static void WriteStyled(string text, MikuRgbColor foreground, params string[] styles)
        {
            lock (_lock)
            {
                var styleString = string.Concat(styles);
                System.Console.Write($"{styleString}{MikuAnsiCodes.ForegroundRgb(foreground)}{text}{MikuAnsiCodes.Reset}");
            }
        }
        #endregion

        #region Gradient Methods
        public static void WriteGradient(string text, MikuRgbColor from, MikuRgbColor to)
        {
            lock (_lock)
            {
                for (int i = 0; i < text.Length; i++)
                {
                    var color = MikuRgbColor.Lerp(from, to, i / (double)(text.Length - 1));
                    System.Console.Write($"{MikuAnsiCodes.ForegroundRgb(color)}{text[i]}");
                }
                System.Console.Write(MikuAnsiCodes.Reset);
            }
        }

        public static void WriteGradientLine(string text, MikuRgbColor from, MikuRgbColor to)
        {
            WriteGradient(text, from, to);
            System.Console.WriteLine();
        }

        public static void WriteRainbow(string text, double phaseOffset = 0)
        {
            lock (_lock)
            {
                for (int i = 0; i < text.Length; i++)
                {
                    var color = MikuColorHelper.GetRainbow(phaseOffset + i * 0.3);
                    System.Console.Write($"{MikuAnsiCodes.ForegroundRgb(color)}{text[i]}");
                }
                System.Console.Write(MikuAnsiCodes.Reset);
            }
        }

        public static void WriteRainbowLine(string text, double phaseOffset = 0)
        {
            WriteRainbow(text, phaseOffset);
            System.Console.WriteLine();
        }

        public static void WriteMikuRainbow(string text, double phaseOffset = 0)
        {
            lock (_lock)
            {
                for (int i = 0; i < text.Length; i++)
                {
                    var color = MikuColorHelper.GetMikuRainbow(phaseOffset + i * 0.3);
                    System.Console.Write($"{MikuAnsiCodes.ForegroundRgb(color)}{text[i]}");
                }
                System.Console.Write(MikuAnsiCodes.Reset);
            }
        }

        public static void WriteMikuRainbowLine(string text, double phaseOffset = 0)
        {
            WriteMikuRainbow(text, phaseOffset);
            System.Console.WriteLine();
        }
        #endregion

        #region Positioning Methods
        public static void SetCursorPosition(int x, int y)
        {
            lock (_lock) { System.Console.SetCursorPosition(x, y); }
        }

        public static (int Left, int Top) GetCursorPosition()
        {
            return System.Console.GetCursorPosition();
        }

        public static void WriteAt(int x, int y, string text, MikuRgbColor foreground)
        {
            lock (_lock)
            {
                System.Console.SetCursorPosition(x, y);
                Write(text, foreground);
            }
        }

        public static void WriteAt(int x, int y, string text)
        {
            lock (_lock)
            {
                System.Console.SetCursorPosition(x, y);
                System.Console.Write(text);
            }
        }

        public static void WriteCentered(string text, MikuRgbColor foreground)
        {
            int padding = (System.Console.WindowWidth - text.Length) / 2;
            lock (_lock)
            {
                System.Console.SetCursorPosition(Math.Max(0, padding), System.Console.CursorTop);
                Write(text, foreground);
            }
        }

        public static void WriteCenteredLine(string text, MikuRgbColor foreground)
        {
            WriteCentered(text, foreground);
            System.Console.WriteLine();
        }
        #endregion

        #region Block Drawing
        public const char FullBlock = '\u2588';
        public const char MediumShade = '\u2592';
        public const char LightShade = '\u2591';
        public const char DarkShade = '\u2593';
        private const char BoxTopLeft = '\u2554';
        private const char BoxTopRight = '\u2557';
        private const char BoxBottomLeft = '\u255A';
        private const char BoxBottomRight = '\u255D';
        private const char BoxHorizontal = '\u2550';
        private const char BoxVertical = '\u2551';

        public static void DrawBar(int width, MikuRgbColor color)
        {
            Write(new string(FullBlock, width), color);
        }

        public static void DrawBar(int x, int y, int width, MikuRgbColor color)
        {
            lock (_lock)
            {
                System.Console.SetCursorPosition(x, y);
                Write(new string(FullBlock, width), color);
            }
        }

        public static void DrawBar(int width, MikuRgbColor color, char character)
        {
            Write(new string(character, width), color);
        }

        public static void DrawBar(int x, int y, int width, MikuRgbColor color, char character)
        {
            lock (_lock)
            {
                System.Console.SetCursorPosition(x, y);
                Write(new string(character, width), color);
            }
        }

        public static void DrawGradientBar(int width, MikuRgbColor from, MikuRgbColor to)
        {
            DrawGradientBar(width, from, to, FullBlock);
        }

        public static void DrawGradientBar(int x, int y, int width, MikuRgbColor from, MikuRgbColor to)
        {
            DrawGradientBar(x, y, width, from, to, FullBlock);
        }

        public static void DrawGradientBar(int width, MikuRgbColor from, MikuRgbColor to, char character)
        {
            lock (_lock)
            {
                for (int i = 0; i < width; i++)
                {
                    var color = MikuRgbColor.Lerp(from, to, i / (double)(width - 1));
                    System.Console.Write($"{MikuAnsiCodes.ForegroundRgb(color)}{character}");
                }
                System.Console.Write(MikuAnsiCodes.Reset);
            }
        }

        public static void DrawGradientBar(int x, int y, int width, MikuRgbColor from, MikuRgbColor to, char character)
        {
            lock (_lock)
            {
                System.Console.SetCursorPosition(x, y);
                for (int i = 0; i < width; i++)
                {
                    var color = MikuRgbColor.Lerp(from, to, i / (double)(width - 1));
                    System.Console.Write($"{MikuAnsiCodes.ForegroundRgb(color)}{character}");
                }
                System.Console.Write(MikuAnsiCodes.Reset);
            }
        }

        public static void DrawBox(int x, int y, int width, int height, MikuRgbColor color)
        {
            lock (_lock)
            {
                System.Console.SetCursorPosition(x, y);
                Write($"{BoxTopLeft}{new string(BoxHorizontal, width - 2)}{BoxTopRight}", color);

                for (int i = 1; i < height - 1; i++)
                {
                    System.Console.SetCursorPosition(x, y + i);
                    Write(BoxVertical.ToString(), color);
                    System.Console.SetCursorPosition(x + width - 1, y + i);
                    Write(BoxVertical.ToString(), color);
                }

                System.Console.SetCursorPosition(x, y + height - 1);
                Write($"{BoxBottomLeft}{new string(BoxHorizontal, width - 2)}{BoxBottomRight}", color);
            }
        }
        #endregion

        #region Cursor and Screen Control
        public static void HideCursor() { System.Console.CursorVisible = false; }
        public static void ShowCursor() { System.Console.CursorVisible = true; }
        public static void Clear() { System.Console.Clear(); }
        
        public static void ResetColors()
        {
            lock (_lock) { System.Console.Write(MikuAnsiCodes.Reset); }
        }

        public static void ResetConsoleColors()
        {
            System.Console.ResetColor();
        }
        #endregion

        #region ANSI Support Check
        private static bool CheckAnsiSupport()
        {
            var colorTerm = Environment.GetEnvironmentVariable("COLORTERM");
            if (colorTerm == "truecolor" || colorTerm == "24bit")
                return true;

            var term = Environment.GetEnvironmentVariable("TERM");
            if (!string.IsNullOrEmpty(term) &&
                (term.Contains("xterm") || term.Contains("256color") || term.Contains("color")))
                return true;

            if (Environment.OSVersion.Platform == PlatformID.Win32NT &&
                Environment.OSVersion.Version.Major >= 10)
                return true;

            if (Environment.OSVersion.Platform == PlatformID.Unix ||
                Environment.OSVersion.Platform == PlatformID.MacOSX)
                return true;

            return false;
        }
        #endregion
    }
}

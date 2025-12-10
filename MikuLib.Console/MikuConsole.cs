using Miku.Core;
using System.Text;

namespace MikuLib.Console
{
    /// <summary>
    /// Provides colored console output utilities with TrueColor (24-bit RGB) and 256-color support.
    /// </summary>
    /// <remarks>
    /// Like Miku's vibrant stage performances, this class brings color to your console!
    /// All operations are thread-safe for concurrent logging scenarios.
    /// </remarks>
    public static class MikuConsole
    {
        private static readonly object _lock = new();
        private static bool? _supportsAnsi;

        #region Properties

        /// <summary>
        /// Gets a value indicating whether the current terminal supports ANSI escape sequences.
        /// </summary>
        public static bool SupportsAnsi => _supportsAnsi ??= CheckAnsiSupport();

        /// <summary>
        /// Gets the width of the console window in characters.
        /// </summary>
        public static int WindowWidth => System.Console.WindowWidth;

        /// <summary>
        /// Gets the height of the console window in characters.
        /// </summary>
        public static int WindowHeight => System.Console.WindowHeight;

        /// <summary>
        /// Gets the column position of the cursor within the buffer area.
        /// </summary>
        public static int CursorLeft => System.Console.CursorLeft;

        /// <summary>
        /// Gets the row position of the cursor within the buffer area.
        /// </summary>
        public static int CursorTop => System.Console.CursorTop;

        #endregion

        #region Initialization

        /// <summary>
        /// Initializes the console for UTF-8 output encoding.
        /// Call this method before using any colored output methods.
        /// </summary>
        public static void Initialize()
        {
            System.Console.OutputEncoding = Encoding.UTF8;
        }

        #endregion

        #region Write Methods - Plain

        /// <summary>
        /// Writes the specified string value to the standard output stream.
        /// </summary>
        /// <param name="text">The value to write.</param>
        public static void Write(string text)
        {
            lock (_lock) { System.Console.Write(text); }
        }

        /// <summary>
        /// Writes the specified string value, followed by the current line terminator, to the standard output stream.
        /// </summary>
        /// <param name="text">The value to write.</param>
        public static void WriteLine(string text)
        {
            lock (_lock) { System.Console.WriteLine(text); }
        }

        /// <summary>
        /// Writes the current line terminator to the standard output stream.
        /// </summary>
        public static void WriteLine()
        {
            System.Console.WriteLine();
        }

        #endregion

        #region Write Methods - TrueColor

        /// <summary>
        /// Writes the specified string value with the specified foreground color using TrueColor (24-bit RGB).
        /// </summary>
        /// <param name="text">The value to write.</param>
        /// <param name="foreground">The foreground color.</param>
        public static void Write(string text, MikuRgbColor foreground)
        {
            lock (_lock)
            {
                System.Console.Write($"{MikuAnsiCodes.ForegroundRgb(foreground)}{text}{MikuAnsiCodes.Reset}");
            }
        }

        /// <summary>
        /// Writes the specified string value with the specified foreground and background colors using TrueColor (24-bit RGB).
        /// </summary>
        /// <param name="text">The value to write.</param>
        /// <param name="foreground">The foreground color.</param>
        /// <param name="background">The background color.</param>
        public static void Write(string text, MikuRgbColor foreground, MikuRgbColor background)
        {
            lock (_lock)
            {
                System.Console.Write($"{MikuAnsiCodes.ForegroundRgb(foreground)}{MikuAnsiCodes.BackgroundRgb(background)}{text}{MikuAnsiCodes.Reset}");
            }
        }

        /// <summary>
        /// Writes the specified string value with the specified foreground color, followed by the current line terminator.
        /// </summary>
        /// <param name="text">The value to write.</param>
        /// <param name="foreground">The foreground color.</param>
        public static void WriteLine(string text, MikuRgbColor foreground)
        {
            Write(text, foreground);
            System.Console.WriteLine();
        }

        /// <summary>
        /// Writes the specified string value with the specified foreground and background colors, followed by the current line terminator.
        /// </summary>
        /// <param name="text">The value to write.</param>
        /// <param name="foreground">The foreground color.</param>
        /// <param name="background">The background color.</param>
        public static void WriteLine(string text, MikuRgbColor foreground, MikuRgbColor background)
        {
            Write(text, foreground, background);
            System.Console.WriteLine();
        }

        #endregion

        #region Write Methods - 256 Color

        /// <summary>
        /// Writes the specified string value with the specified 256-color palette foreground color.
        /// </summary>
        /// <param name="text">The value to write.</param>
        /// <param name="colorCode">The 256-color palette code (0-255).</param>
        public static void Write256(string text, byte colorCode)
        {
            lock (_lock)
            {
                System.Console.Write($"{MikuAnsiCodes.Foreground256(colorCode)}{text}{MikuAnsiCodes.Reset}");
            }
        }

        /// <summary>
        /// Writes the specified string value with the specified 256-color palette foreground and background colors.
        /// </summary>
        /// <param name="text">The value to write.</param>
        /// <param name="foreground">The foreground 256-color palette code (0-255).</param>
        /// <param name="background">The background 256-color palette code (0-255).</param>
        public static void Write256(string text, byte foreground, byte background)
        {
            lock (_lock)
            {
                System.Console.Write($"{MikuAnsiCodes.Foreground256(foreground)}{MikuAnsiCodes.Background256(background)}{text}{MikuAnsiCodes.Reset}");
            }
        }

        /// <summary>
        /// Writes the specified string value with the specified 256-color palette foreground color, followed by the current line terminator.
        /// </summary>
        /// <param name="text">The value to write.</param>
        /// <param name="colorCode">The 256-color palette code (0-255).</param>
        public static void WriteLine256(string text, byte colorCode)
        {
            Write256(text, colorCode);
            System.Console.WriteLine();
        }

        #endregion

        #region Styled Write Methods

        /// <summary>
        /// Writes the specified string value in bold with the specified foreground color.
        /// </summary>
        /// <param name="text">The value to write.</param>
        /// <param name="foreground">The foreground color.</param>
        public static void WriteBold(string text, MikuRgbColor foreground)
        {
            lock (_lock)
            {
                System.Console.Write($"{MikuAnsiCodes.Bold}{MikuAnsiCodes.ForegroundRgb(foreground)}{text}{MikuAnsiCodes.Reset}");
            }
        }

        /// <summary>
        /// Writes the specified string value with underline and the specified foreground color.
        /// </summary>
        /// <param name="text">The value to write.</param>
        /// <param name="foreground">The foreground color.</param>
        public static void WriteUnderline(string text, MikuRgbColor foreground)
        {
            lock (_lock)
            {
                System.Console.Write($"{MikuAnsiCodes.Underline}{MikuAnsiCodes.ForegroundRgb(foreground)}{text}{MikuAnsiCodes.Reset}");
            }
        }

        /// <summary>
        /// Writes the specified string value in italic with the specified foreground color.
        /// </summary>
        /// <param name="text">The value to write.</param>
        /// <param name="foreground">The foreground color.</param>
        public static void WriteItalic(string text, MikuRgbColor foreground)
        {
            lock (_lock)
            {
                System.Console.Write($"{MikuAnsiCodes.Italic}{MikuAnsiCodes.ForegroundRgb(foreground)}{text}{MikuAnsiCodes.Reset}");
            }
        }

        /// <summary>
        /// Writes the specified string value with custom ANSI styles and the specified foreground color.
        /// </summary>
        /// <param name="text">The value to write.</param>
        /// <param name="foreground">The foreground color.</param>
        /// <param name="styles">The ANSI style codes to apply (e.g., MikuAnsiCodes.Bold, MikuAnsiCodes.Underline).</param>
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

        /// <summary>
        /// Writes the specified string value with a color gradient from one color to another.
        /// </summary>
        /// <param name="text">The value to write.</param>
        /// <param name="from">The starting color of the gradient.</param>
        /// <param name="to">The ending color of the gradient.</param>
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

        /// <summary>
        /// Writes the specified string value with a color gradient, followed by the current line terminator.
        /// </summary>
        /// <param name="text">The value to write.</param>
        /// <param name="from">The starting color of the gradient.</param>
        /// <param name="to">The ending color of the gradient.</param>
        public static void WriteGradientLine(string text, MikuRgbColor from, MikuRgbColor to)
        {
            WriteGradient(text, from, to);
            System.Console.WriteLine();
        }

        /// <summary>
        /// Writes the specified string value with rainbow colors.
        /// </summary>
        /// <param name="text">The value to write.</param>
        /// <param name="phaseOffset">The phase offset for the rainbow colors (default: 0).</param>
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

        /// <summary>
        /// Writes the specified string value with rainbow colors, followed by the current line terminator.
        /// </summary>
        /// <param name="text">The value to write.</param>
        /// <param name="phaseOffset">The phase offset for the rainbow colors (default: 0).</param>
        public static void WriteRainbowLine(string text, double phaseOffset = 0)
        {
            WriteRainbow(text, phaseOffset);
            System.Console.WriteLine();
        }

        /// <summary>
        /// Writes the specified string value with Miku-themed rainbow colors (cyan/pink biased).
        /// </summary>
        /// <param name="text">The value to write.</param>
        /// <param name="phaseOffset">The phase offset for the rainbow colors (default: 0).</param>
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

        /// <summary>
        /// Writes the specified string value with Miku-themed rainbow colors, followed by the current line terminator.
        /// </summary>
        /// <param name="text">The value to write.</param>
        /// <param name="phaseOffset">The phase offset for the rainbow colors (default: 0).</param>
        public static void WriteMikuRainbowLine(string text, double phaseOffset = 0)
        {
            WriteMikuRainbow(text, phaseOffset);
            System.Console.WriteLine();
        }

        #endregion

        #region Positioning Methods

        /// <summary>
        /// Sets the position of the cursor.
        /// </summary>
        /// <param name="x">The column position of the cursor (0-based).</param>
        /// <param name="y">The row position of the cursor (0-based).</param>
        public static void SetCursorPosition(int x, int y)
        {
            lock (_lock) { System.Console.SetCursorPosition(x, y); }
        }

        /// <summary>
        /// Gets the current position of the cursor.
        /// </summary>
        /// <returns>A tuple containing the column (Left) and row (Top) position of the cursor.</returns>
        public static (int Left, int Top) GetCursorPosition()
        {
            return System.Console.GetCursorPosition();
        }

        /// <summary>
        /// Writes the specified string value at the specified position with the specified foreground color.
        /// </summary>
        /// <param name="x">The column position (0-based).</param>
        /// <param name="y">The row position (0-based).</param>
        /// <param name="text">The value to write.</param>
        /// <param name="foreground">The foreground color.</param>
        public static void WriteAt(int x, int y, string text, MikuRgbColor foreground)
        {
            lock (_lock)
            {
                System.Console.SetCursorPosition(x, y);
                Write(text, foreground);
            }
        }

        /// <summary>
        /// Writes the specified string value at the specified position.
        /// </summary>
        /// <param name="x">The column position (0-based).</param>
        /// <param name="y">The row position (0-based).</param>
        /// <param name="text">The value to write.</param>
        public static void WriteAt(int x, int y, string text)
        {
            lock (_lock)
            {
                System.Console.SetCursorPosition(x, y);
                System.Console.Write(text);
            }
        }

        /// <summary>
        /// Writes the specified string value centered on the current line with the specified foreground color.
        /// </summary>
        /// <param name="text">The value to write.</param>
        /// <param name="foreground">The foreground color.</param>
        public static void WriteCentered(string text, MikuRgbColor foreground)
        {
            int padding = (System.Console.WindowWidth - text.Length) / 2;
            lock (_lock)
            {
                System.Console.SetCursorPosition(Math.Max(0, padding), System.Console.CursorTop);
                Write(text, foreground);
            }
        }

        /// <summary>
        /// Writes the specified string value centered on the current line, followed by the current line terminator.
        /// </summary>
        /// <param name="text">The value to write.</param>
        /// <param name="foreground">The foreground color.</param>
        public static void WriteCenteredLine(string text, MikuRgbColor foreground)
        {
            WriteCentered(text, foreground);
            System.Console.WriteLine();
        }

        #endregion

        #region Block Drawing

        /// <summary>Full block character (?) for drawing solid bars.</summary>
        public const char FullBlock = '\u2588';

        /// <summary>Medium shade character (?) for drawing shaded areas.</summary>
        public const char MediumShade = '\u2592';

        /// <summary>Light shade character (?) for drawing light shaded areas.</summary>
        public const char LightShade = '\u2591';

        /// <summary>Dark shade character (?) for drawing dark shaded areas.</summary>
        public const char DarkShade = '\u2593';

        private const char BoxTopLeft = '\u2554';
        private const char BoxTopRight = '\u2557';
        private const char BoxBottomLeft = '\u255A';
        private const char BoxBottomRight = '\u255D';
        private const char BoxHorizontal = '\u2550';
        private const char BoxVertical = '\u2551';

        /// <summary>
        /// Draws a solid bar at the current cursor position with the specified color.
        /// </summary>
        /// <param name="width">The width of the bar in characters.</param>
        /// <param name="color">The color of the bar.</param>
        public static void DrawBar(int width, MikuRgbColor color)
        {
            Write(new string(FullBlock, width), color);
        }

        /// <summary>
        /// Draws a solid bar at the specified position with the specified color.
        /// </summary>
        /// <param name="x">The column position (0-based).</param>
        /// <param name="y">The row position (0-based).</param>
        /// <param name="width">The width of the bar in characters.</param>
        /// <param name="color">The color of the bar.</param>
        public static void DrawBar(int x, int y, int width, MikuRgbColor color)
        {
            lock (_lock)
            {
                System.Console.SetCursorPosition(x, y);
                Write(new string(FullBlock, width), color);
            }
        }

        /// <summary>
        /// Draws a bar at the current cursor position with the specified color and character.
        /// </summary>
        /// <param name="width">The width of the bar in characters.</param>
        /// <param name="color">The color of the bar.</param>
        /// <param name="character">The character to use for drawing the bar.</param>
        public static void DrawBar(int width, MikuRgbColor color, char character)
        {
            Write(new string(character, width), color);
        }

        /// <summary>
        /// Draws a bar at the specified position with the specified color and character.
        /// </summary>
        /// <param name="x">The column position (0-based).</param>
        /// <param name="y">The row position (0-based).</param>
        /// <param name="width">The width of the bar in characters.</param>
        /// <param name="color">The color of the bar.</param>
        /// <param name="character">The character to use for drawing the bar.</param>
        public static void DrawBar(int x, int y, int width, MikuRgbColor color, char character)
        {
            lock (_lock)
            {
                System.Console.SetCursorPosition(x, y);
                Write(new string(character, width), color);
            }
        }

        /// <summary>
        /// Draws a gradient bar at the current cursor position.
        /// </summary>
        /// <param name="width">The width of the bar in characters.</param>
        /// <param name="from">The starting color of the gradient.</param>
        /// <param name="to">The ending color of the gradient.</param>
        public static void DrawGradientBar(int width, MikuRgbColor from, MikuRgbColor to)
        {
            DrawGradientBar(width, from, to, FullBlock);
        }

        /// <summary>
        /// Draws a gradient bar at the specified position.
        /// </summary>
        /// <param name="x">The column position (0-based).</param>
        /// <param name="y">The row position (0-based).</param>
        /// <param name="width">The width of the bar in characters.</param>
        /// <param name="from">The starting color of the gradient.</param>
        /// <param name="to">The ending color of the gradient.</param>
        public static void DrawGradientBar(int x, int y, int width, MikuRgbColor from, MikuRgbColor to)
        {
            DrawGradientBar(x, y, width, from, to, FullBlock);
        }

        /// <summary>
        /// Draws a gradient bar at the current cursor position with the specified character.
        /// </summary>
        /// <param name="width">The width of the bar in characters.</param>
        /// <param name="from">The starting color of the gradient.</param>
        /// <param name="to">The ending color of the gradient.</param>
        /// <param name="character">The character to use for drawing the bar.</param>
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

        /// <summary>
        /// Draws a gradient bar at the specified position with the specified character.
        /// </summary>
        /// <param name="x">The column position (0-based).</param>
        /// <param name="y">The row position (0-based).</param>
        /// <param name="width">The width of the bar in characters.</param>
        /// <param name="from">The starting color of the gradient.</param>
        /// <param name="to">The ending color of the gradient.</param>
        /// <param name="character">The character to use for drawing the bar.</param>
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

        /// <summary>
        /// Draws a box (rectangle) with double-line borders at the specified position.
        /// </summary>
        /// <param name="x">The column position of the top-left corner (0-based).</param>
        /// <param name="y">The row position of the top-left corner (0-based).</param>
        /// <param name="width">The width of the box in characters.</param>
        /// <param name="height">The height of the box in characters.</param>
        /// <param name="color">The color of the box borders.</param>
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

        /// <summary>
        /// Hides the console cursor.
        /// </summary>
        public static void HideCursor() { System.Console.CursorVisible = false; }

        /// <summary>
        /// Shows the console cursor.
        /// </summary>
        public static void ShowCursor() { System.Console.CursorVisible = true; }

        /// <summary>
        /// Clears the console screen.
        /// </summary>
        public static void Clear() { System.Console.Clear(); }

        /// <summary>
        /// Resets all ANSI color and style attributes.
        /// </summary>
        public static void ResetColors()
        {
            lock (_lock) { System.Console.Write(MikuAnsiCodes.Reset); }
        }

        /// <summary>
        /// Resets the console foreground and background colors to their defaults.
        /// </summary>
        public static void ResetConsoleColors()
        {
            System.Console.ResetColor();
        }

        #endregion

        #region ANSI Support Check

        /// <summary>
        /// Checks if the current terminal supports ANSI escape sequences.
        /// </summary>
        /// <returns>True if ANSI is supported; otherwise, false.</returns>
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

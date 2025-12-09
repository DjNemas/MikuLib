using Miku.Core;
using System.Text;

namespace MikuLib.Console
{
    /// <summary>
    /// Provides colored console output utilities with TrueColor (24-bit RGB) and 256-color support.
    /// <para>
    /// MikuConsole is a static class that wraps <see cref="System.Console"/> with additional
    /// color capabilities including:
    /// <list type="bullet">
    ///   <item><description>TrueColor (24-bit RGB) - 16 million colors using ANSI escape sequences</description></item>
    ///   <item><description>256-Color palette - Extended terminal colors (0-255)</description></item>
    ///   <item><description>Gradient text - Smooth color transitions across text</description></item>
    ///   <item><description>Rainbow effects - Animated spectrum colors</description></item>
    ///   <item><description>Styled text - Bold, italic, underline formatting</description></item>
    ///   <item><description>Block drawing - Bars, boxes, and visual elements</description></item>
    /// </list>
    /// </para>
    /// </summary>
    /// <remarks>
    /// Like Miku's voice carrying across concert halls,
    /// let your console messages shine with color!
    /// <para>
    /// All write operations are thread-safe through internal locking.
    /// </para>
    /// <para>
    /// <b>Requirements:</b> A terminal with ANSI escape sequence support.
    /// Most modern terminals (Windows Terminal, VS Code, Linux terminals) support this.
    /// Call <see cref="Initialize"/> at application startup for best compatibility.
    /// </para>
    /// </remarks>
    /// <example>
    /// <code>
    /// // Initialize for UTF-8 support
    /// MikuConsole.Initialize();
    /// MikuConsole.HideCursor();
    /// 
    /// try
    /// {
    ///     // TrueColor output
    ///     MikuConsole.WriteLine("Hello Miku!", RgbColor.MikuCyan);
    ///     
    ///     // Gradient text
    ///     MikuConsole.WriteGradientLine("Gradient!", RgbColor.MikuCyan, RgbColor.MikuPink);
    ///     
    ///     // Rainbow text
    ///     MikuConsole.WriteRainbowLine("Rainbow colors!");
    /// }
    /// finally
    /// {
    ///     MikuConsole.ShowCursor();
    ///     MikuConsole.ResetColors();
    /// }
    /// </code>
    /// </example>
    public static class MikuConsole
    {
        /// <summary>
        /// Lock object for thread-safe console operations.
        /// </summary>
        private static readonly object _lock = new();
        
        /// <summary>
        /// Cached ANSI support detection result.
        /// </summary>
        private static bool? _supportsAnsi;

        #region Properties

        /// <summary>
        /// Gets a value indicating whether the current console supports ANSI escape sequences.
        /// <para>
        /// This property checks environment variables (TERM, COLORTERM) and the operating system
        /// to determine if the terminal likely supports ANSI colors.
        /// </para>
        /// </summary>
        /// <value>
        /// <c>true</c> if ANSI escape sequences are likely supported; otherwise, <c>false</c>.
        /// </value>
        /// <remarks>
        /// Detection is performed once and cached for subsequent calls.
        /// Returns <c>true</c> for:
        /// <list type="bullet">
        ///   <item><description>COLORTERM=truecolor or 24bit</description></item>
        ///   <item><description>TERM containing xterm, 256color, or color</description></item>
        ///   <item><description>Windows 10 or later</description></item>
        ///   <item><description>Unix/macOS platforms</description></item>
        /// </list>
        /// </remarks>
        public static bool SupportsAnsi => _supportsAnsi ??= CheckAnsiSupport();

        /// <summary>
        /// Gets the current console window width in characters.
        /// </summary>
        /// <value>The number of character columns in the console window.</value>
        /// <remarks>
        /// Use this for responsive layouts that adapt to console size.
        /// </remarks>
        /// <example>
        /// <code>
        /// int width = Math.Min(MikuConsole.WindowWidth - 4, 80);
        /// MikuConsole.DrawBar(width, RgbColor.MikuCyan);
        /// </code>
        /// </example>
        public static int WindowWidth => System.Console.WindowWidth;

        /// <summary>
        /// Gets the current console window height in rows.
        /// </summary>
        /// <value>The number of character rows in the console window.</value>
        public static int WindowHeight => System.Console.WindowHeight;

        /// <summary>
        /// Gets the current horizontal cursor position (0-based column).
        /// </summary>
        /// <value>The column index where the cursor is currently located.</value>
        public static int CursorLeft => System.Console.CursorLeft;

        /// <summary>
        /// Gets the current vertical cursor position (0-based row).
        /// </summary>
        /// <value>The row index where the cursor is currently located.</value>
        public static int CursorTop => System.Console.CursorTop;

        #endregion

        #region Initialization

        /// <summary>
        /// Initializes the console for optimal ANSI and Unicode support.
        /// <para>
        /// Sets the output encoding to UTF-8 to ensure proper display of
        /// Unicode characters including block drawing characters (?, ?, etc.)
        /// and box drawing characters (?, ?, ?, etc.).
        /// </para>
        /// </summary>
        /// <remarks>
        /// Call this method once at the start of your application before
        /// using any MikuConsole output methods.
        /// </remarks>
        /// <example>
        /// <code>
        /// public static void Main()
        /// {
        ///     MikuConsole.Initialize();
        ///     MikuConsole.WriteLine("Ready!", RgbColor.MikuCyan);
        /// }
        /// </code>
        /// </example>
        public static void Initialize()
        {
            System.Console.OutputEncoding = Encoding.UTF8;
        }

        #endregion

        #region Write Methods - Plain

        /// <summary>
        /// Writes text to the console without any color formatting.
        /// </summary>
        /// <param name="text">The text to write.</param>
        /// <remarks>
        /// Uses the current console foreground color. Thread-safe.
        /// </remarks>
        public static void Write(string text)
        {
            lock (_lock)
            {
                System.Console.Write(text);
            }
        }

        /// <summary>
        /// Writes text followed by a newline without any color formatting.
        /// </summary>
        /// <param name="text">The text to write.</param>
        /// <remarks>
        /// Uses the current console foreground color. Thread-safe.
        /// </remarks>
        public static void WriteLine(string text)
        {
            lock (_lock)
            {
                System.Console.WriteLine(text);
            }
        }

        /// <summary>
        /// Writes an empty line to the console.
        /// </summary>
        public static void WriteLine()
        {
            System.Console.WriteLine();
        }

        #endregion

        #region Write Methods - TrueColor

        /// <summary>
        /// Writes text with a TrueColor (24-bit RGB) foreground color.
        /// </summary>
        /// <param name="text">The text to write.</param>
        /// <param name="foreground">The foreground color as an <see cref="RgbColor"/>.</param>
        /// <remarks>
        /// Uses ANSI escape sequences for 24-bit color. The color is automatically
        /// reset after the text is written.
        /// </remarks>
        /// <example>
        /// <code>
        /// MikuConsole.Write("Cyan text ", RgbColor.MikuCyan);
        /// MikuConsole.Write("Pink text", RgbColor.MikuPink);
        /// </code>
        /// </example>
        public static void Write(string text, RgbColor foreground)
        {
            lock (_lock)
            {
                System.Console.Write($"{AnsiCodes.ForegroundRgb(foreground)}{text}{AnsiCodes.Reset}");
            }
        }

        /// <summary>
        /// Writes text with TrueColor foreground and background colors.
        /// </summary>
        /// <param name="text">The text to write.</param>
        /// <param name="foreground">The foreground (text) color.</param>
        /// <param name="background">The background color.</param>
        /// <example>
        /// <code>
        /// MikuConsole.Write("Highlighted!", RgbColor.White, RgbColor.MikuCyan);
        /// </code>
        /// </example>
        public static void Write(string text, RgbColor foreground, RgbColor background)
        {
            lock (_lock)
            {
                System.Console.Write($"{AnsiCodes.ForegroundRgb(foreground)}{AnsiCodes.BackgroundRgb(background)}{text}{AnsiCodes.Reset}");
            }
        }

        /// <summary>
        /// Writes text with a TrueColor foreground followed by a newline.
        /// </summary>
        /// <param name="text">The text to write.</param>
        /// <param name="foreground">The foreground color.</param>
        public static void WriteLine(string text, RgbColor foreground)
        {
            Write(text, foreground);
            System.Console.WriteLine();
        }

        /// <summary>
        /// Writes text with TrueColor foreground and background followed by a newline.
        /// </summary>
        /// <param name="text">The text to write.</param>
        /// <param name="foreground">The foreground color.</param>
        /// <param name="background">The background color.</param>
        public static void WriteLine(string text, RgbColor foreground, RgbColor background)
        {
            Write(text, foreground, background);
            System.Console.WriteLine();
        }

        #endregion

        #region Write Methods - 256 Color

        /// <summary>
        /// Writes text using the 256-color palette for the foreground.
        /// </summary>
        /// <param name="text">The text to write.</param>
        /// <param name="colorCode">
        /// The color index (0-255) from the 256-color palette:
        /// <list type="bullet">
        ///   <item><description>0-7: Standard colors</description></item>
        ///   <item><description>8-15: High-intensity colors</description></item>
        ///   <item><description>16-231: 6󬝲 RGB color cube</description></item>
        ///   <item><description>232-255: Grayscale (24 shades)</description></item>
        /// </list>
        /// </param>
        /// <example>
        /// <code>
        /// MikuConsole.Write256("Cyan-ish", 44);
        /// MikuConsole.Write256("Orange", 208);
        /// </code>
        /// </example>
        public static void Write256(string text, byte colorCode)
        {
            lock (_lock)
            {
                System.Console.Write($"{AnsiCodes.Foreground256(colorCode)}{text}{AnsiCodes.Reset}");
            }
        }

        /// <summary>
        /// Writes text using the 256-color palette for foreground and background.
        /// </summary>
        /// <param name="text">The text to write.</param>
        /// <param name="foreground">The foreground color index (0-255).</param>
        /// <param name="background">The background color index (0-255).</param>
        public static void Write256(string text, byte foreground, byte background)
        {
            lock (_lock)
            {
                System.Console.Write($"{AnsiCodes.Foreground256(foreground)}{AnsiCodes.Background256(background)}{text}{AnsiCodes.Reset}");
            }
        }

        /// <summary>
        /// Writes text using the 256-color palette followed by a newline.
        /// </summary>
        /// <param name="text">The text to write.</param>
        /// <param name="colorCode">The color index (0-255).</param>
        public static void WriteLine256(string text, byte colorCode)
        {
            Write256(text, colorCode);
            System.Console.WriteLine();
        }

        #endregion

        #region Styled Write Methods

        /// <summary>
        /// Writes bold text with the specified color.
        /// </summary>
        /// <param name="text">The text to write in bold.</param>
        /// <param name="foreground">The foreground color.</param>
        /// <remarks>
        /// Bold formatting support depends on the terminal. Some terminals
        /// may display this as bright/intense color instead of bold.
        /// </remarks>
        /// <example>
        /// <code>
        /// MikuConsole.WriteBold("Important!", RgbColor.MikuPink);
        /// </code>
        /// </example>
        public static void WriteBold(string text, RgbColor foreground)
        {
            lock (_lock)
            {
                System.Console.Write($"{AnsiCodes.Bold}{AnsiCodes.ForegroundRgb(foreground)}{text}{AnsiCodes.Reset}");
            }
        }

        /// <summary>
        /// Writes underlined text with the specified color.
        /// </summary>
        /// <param name="text">The text to underline.</param>
        /// <param name="foreground">The foreground color.</param>
        /// <remarks>
        /// Underline support varies by terminal. Windows Command Prompt
        /// may not display underlines correctly.
        /// </remarks>
        public static void WriteUnderline(string text, RgbColor foreground)
        {
            lock (_lock)
            {
                System.Console.Write($"{AnsiCodes.Underline}{AnsiCodes.ForegroundRgb(foreground)}{text}{AnsiCodes.Reset}");
            }
        }

        /// <summary>
        /// Writes italic text with the specified color.
        /// </summary>
        /// <param name="text">The text to italicize.</param>
        /// <param name="foreground">The foreground color.</param>
        /// <remarks>
        /// Italic support is limited to certain terminals (e.g., Windows Terminal, iTerm2).
        /// Many terminals will ignore this formatting.
        /// </remarks>
        public static void WriteItalic(string text, RgbColor foreground)
        {
            lock (_lock)
            {
                System.Console.Write($"{AnsiCodes.Italic}{AnsiCodes.ForegroundRgb(foreground)}{text}{AnsiCodes.Reset}");
            }
        }

        /// <summary>
        /// Writes text with custom ANSI styles and color.
        /// </summary>
        /// <param name="text">The text to write.</param>
        /// <param name="foreground">The foreground color.</param>
        /// <param name="styles">
        /// One or more ANSI style codes from <see cref="AnsiCodes"/>
        /// (e.g., <see cref="AnsiCodes.Bold"/>, <see cref="AnsiCodes.Underline"/>).
        /// </param>
        /// <example>
        /// <code>
        /// // Bold and underlined cyan text
        /// MikuConsole.WriteStyled(
        ///     "Styled text",
        ///     RgbColor.MikuCyan,
        ///     AnsiCodes.Bold,
        ///     AnsiCodes.Underline
        /// );
        /// </code>
        /// </example>
        public static void WriteStyled(string text, RgbColor foreground, params string[] styles)
        {
            lock (_lock)
            {
                var styleString = string.Concat(styles);
                System.Console.Write($"{styleString}{AnsiCodes.ForegroundRgb(foreground)}{text}{AnsiCodes.Reset}");
            }
        }

        #endregion

        #region Gradient Methods

        /// <summary>
        /// Writes text with a horizontal color gradient from left to right.
        /// <para>
        /// Each character is colored with a smoothly interpolated color
        /// between <paramref name="from"/> and <paramref name="to"/>.
        /// </para>
        /// </summary>
        /// <param name="text">The text to write with gradient coloring.</param>
        /// <param name="from">The starting color (first character).</param>
        /// <param name="to">The ending color (last character).</param>
        /// <example>
        /// <code>
        /// MikuConsole.WriteGradient("Gradient text!", RgbColor.MikuCyan, RgbColor.MikuPink);
        /// </code>
        /// </example>
        public static void WriteGradient(string text, RgbColor from, RgbColor to)
        {
            lock (_lock)
            {
                for (int i = 0; i < text.Length; i++)
                {
                    // Calculate interpolation factor (0.0 to 1.0) based on position
                    var color = RgbColor.Lerp(from, to, i / (double)(text.Length - 1));
                    System.Console.Write($"{AnsiCodes.ForegroundRgb(color)}{text[i]}");
                }
                System.Console.Write(AnsiCodes.Reset);
            }
        }

        /// <summary>
        /// Writes text with a horizontal color gradient followed by a newline.
        /// </summary>
        /// <param name="text">The text to write with gradient coloring.</param>
        /// <param name="from">The starting color.</param>
        /// <param name="to">The ending color.</param>
        public static void WriteGradientLine(string text, RgbColor from, RgbColor to)
        {
            WriteGradient(text, from, to);
            System.Console.WriteLine();
        }

        /// <summary>
        /// Writes text with rainbow colors cycling through the spectrum.
        /// <para>
        /// Uses <see cref="ColorHelper.GetRainbow(double)"/> to generate
        /// colors based on character position.
        /// </para>
        /// </summary>
        /// <param name="text">The text to write with rainbow coloring.</param>
        /// <param name="phaseOffset">
        /// Optional offset to shift the starting color in the rainbow spectrum.
        /// Useful for animating rainbow text. Default is 0.
        /// </param>
        /// <example>
        /// <code>
        /// MikuConsole.WriteRainbow("Rainbow!");
        /// 
        /// // Animated rainbow
        /// for (int i = 0; i &lt; 100; i++)
        /// {
        ///     MikuConsole.SetCursorPosition(0, 0);
        ///     MikuConsole.WriteRainbow("Animated!", phaseOffset: i * 0.2);
        ///     await Task.Delay(50);
        /// }
        /// </code>
        /// </example>
        public static void WriteRainbow(string text, double phaseOffset = 0)
        {
            lock (_lock)
            {
                for (int i = 0; i < text.Length; i++)
                {
                    // Phase offset + character position creates the rainbow effect
                    var color = ColorHelper.GetRainbow(phaseOffset + i * 0.3);
                    System.Console.Write($"{AnsiCodes.ForegroundRgb(color)}{text[i]}");
                }
                System.Console.Write(AnsiCodes.Reset);
            }
        }

        /// <summary>
        /// Writes text with rainbow colors followed by a newline.
        /// </summary>
        /// <param name="text">The text to write with rainbow coloring.</param>
        /// <param name="phaseOffset">Optional color phase offset. Default is 0.</param>
        public static void WriteRainbowLine(string text, double phaseOffset = 0)
        {
            WriteRainbow(text, phaseOffset);
            System.Console.WriteLine();
        }

        /// <summary>
        /// Writes text with Miku-themed rainbow colors (cyan and pink dominant).
        /// <para>
        /// Uses <see cref="ColorHelper.GetMikuRainbow(double)"/> which cycles
        /// through Miku's signature colors instead of the standard rainbow.
        /// </para>
        /// </summary>
        /// <param name="text">The text to write with Miku rainbow coloring.</param>
        /// <param name="phaseOffset">Optional color phase offset. Default is 0.</param>
        /// <example>
        /// <code>
        /// MikuConsole.WriteMikuRainbow("Miku colors!");
        /// </code>
        /// </example>
        public static void WriteMikuRainbow(string text, double phaseOffset = 0)
        {
            lock (_lock)
            {
                for (int i = 0; i < text.Length; i++)
                {
                    var color = ColorHelper.GetMikuRainbow(phaseOffset + i * 0.3);
                    System.Console.Write($"{AnsiCodes.ForegroundRgb(color)}{text[i]}");
                }
                System.Console.Write(AnsiCodes.Reset);
            }
        }

        /// <summary>
        /// Writes text with Miku-themed rainbow colors followed by a newline.
        /// </summary>
        /// <param name="text">The text to write with Miku rainbow coloring.</param>
        /// <param name="phaseOffset">Optional color phase offset. Default is 0.</param>
        public static void WriteMikuRainbowLine(string text, double phaseOffset = 0)
        {
            WriteMikuRainbow(text, phaseOffset);
            System.Console.WriteLine();
        }

        #endregion

        #region Positioning Methods

        /// <summary>
        /// Sets the cursor position to the specified coordinates.
        /// </summary>
        /// <param name="x">The horizontal position (0-based column from left edge).</param>
        /// <param name="y">The vertical position (0-based row from top edge).</param>
        /// <remarks>
        /// Coordinates are 0-based. Position (0, 0) is the top-left corner.
        /// </remarks>
        /// <example>
        /// <code>
        /// MikuConsole.SetCursorPosition(10, 5);
        /// MikuConsole.Write("At position (10, 5)", RgbColor.MikuCyan);
        /// </code>
        /// </example>
        public static void SetCursorPosition(int x, int y)
        {
            lock (_lock)
            {
                System.Console.SetCursorPosition(x, y);
            }
        }

        /// <summary>
        /// Gets the current cursor position as a tuple of (Left, Top).
        /// </summary>
        /// <returns>
        /// A tuple containing the cursor's column position (Left) and row position (Top),
        /// both 0-based.
        /// </returns>
        /// <remarks>
        /// This is equivalent to calling <see cref="System.Console.GetCursorPosition"/>.
        /// Use <see cref="CursorLeft"/> and <see cref="CursorTop"/> for individual values.
        /// </remarks>
        /// <example>
        /// <code>
        /// var (left, top) = MikuConsole.GetCursorPosition();
        /// MikuConsole.WriteLine($"Cursor at column {left}, row {top}");
        /// 
        /// // Draw a bar at the current row
        /// MikuConsole.DrawBar(2, MikuConsole.GetCursorPosition().Top, 40, RgbColor.MikuCyan);
        /// </code>
        /// </example>
        public static (int Left, int Top) GetCursorPosition()
        {
            return System.Console.GetCursorPosition();
        }

        /// <summary>
        /// Writes colored text at a specific position without changing the cursor.
        /// </summary>
        /// <param name="x">The horizontal position (0-based column).</param>
        /// <param name="y">The vertical position (0-based row).</param>
        /// <param name="text">The text to write.</param>
        /// <param name="foreground">The foreground color.</param>
        /// <remarks>
        /// The cursor remains at the end of the written text after this call.
        /// </remarks>
        /// <example>
        /// <code>
        /// MikuConsole.WriteAt(0, 0, "Top-left", RgbColor.MikuCyan);
        /// MikuConsole.WriteAt(50, 10, "Middle", RgbColor.MikuPink);
        /// </code>
        /// </example>
        public static void WriteAt(int x, int y, string text, RgbColor foreground)
        {
            lock (_lock)
            {
                System.Console.SetCursorPosition(x, y);
                Write(text, foreground);
            }
        }

        /// <summary>
        /// Writes text at a specific position without color formatting.
        /// </summary>
        /// <param name="x">The horizontal position (0-based column).</param>
        /// <param name="y">The vertical position (0-based row).</param>
        /// <param name="text">The text to write.</param>
        public static void WriteAt(int x, int y, string text)
        {
            lock (_lock)
            {
                System.Console.SetCursorPosition(x, y);
                System.Console.Write(text);
            }
        }

        /// <summary>
        /// Writes text horizontally centered on the current line.
        /// </summary>
        /// <param name="text">The text to center.</param>
        /// <param name="foreground">The foreground color.</param>
        /// <remarks>
        /// Calculates center position based on <see cref="WindowWidth"/>.
        /// The cursor moves to the centered position before writing.
        /// </remarks>
        /// <example>
        /// <code>
        /// MikuConsole.WriteCentered("Centered Title", RgbColor.MikuCyan);
        /// </code>
        /// </example>
        public static void WriteCentered(string text, RgbColor foreground)
        {
            int padding = (System.Console.WindowWidth - text.Length) / 2;
            lock (_lock)
            {
                System.Console.SetCursorPosition(Math.Max(0, padding), System.Console.CursorTop);
                Write(text, foreground);
            }
        }

        /// <summary>
        /// Writes centered text followed by a newline.
        /// </summary>
        /// <param name="text">The text to center.</param>
        /// <param name="foreground">The foreground color.</param>
        public static void WriteCenteredLine(string text, RgbColor foreground)
        {
            WriteCentered(text, foreground);
            System.Console.WriteLine();
        }

        #endregion

        #region Block Drawing

        /// <summary>
        /// The full block character (?) used for drawing solid bars.
        /// Unicode: U+2588
        /// </summary>
        public const char FullBlock = '\u2588';

        /// <summary>
        /// The medium shade character (?) for semi-transparent effects.
        /// Unicode: U+2592
        /// </summary>
        public const char MediumShade = '\u2592';

        /// <summary>
        /// The light shade character (?) for light/empty sections.
        /// Unicode: U+2591
        /// </summary>
        public const char LightShade = '\u2591';

        /// <summary>
        /// The dark shade character (?) for darker sections.
        /// Unicode: U+2593
        /// </summary>
        public const char DarkShade = '\u2593';

        /// <summary>Box drawing: top-left corner (?) Unicode: U+2554</summary>
        private const char BoxTopLeft = '\u2554';
        
        /// <summary>Box drawing: top-right corner (?) Unicode: U+2557</summary>
        private const char BoxTopRight = '\u2557';
        
        /// <summary>Box drawing: bottom-left corner (?) Unicode: U+255A</summary>
        private const char BoxBottomLeft = '\u255A';
        
        /// <summary>Box drawing: bottom-right corner (?) Unicode: U+255D</summary>
        private const char BoxBottomRight = '\u255D';
        
        /// <summary>Box drawing: horizontal line (?) Unicode: U+2550</summary>
        private const char BoxHorizontal = '\u2550';
        
        /// <summary>Box drawing: vertical line (?) Unicode: U+2551</summary>
        private const char BoxVertical = '\u2551';

        /// <summary>
        /// Draws a horizontal bar at the current cursor position using the full block character (?).
        /// </summary>
        /// <param name="width">The width of the bar in characters.</param>
        /// <param name="color">The color of the bar.</param>
        /// <example>
        /// <code>
        /// MikuConsole.DrawBar(40, RgbColor.MikuCyan);
        /// </code>
        /// </example>
        public static void DrawBar(int width, RgbColor color)
        {
            Write(new string(FullBlock, width), color);
        }

        /// <summary>
        /// Draws a horizontal bar at a specific position using the full block character (?).
        /// </summary>
        /// <param name="x">The horizontal position (0-based column).</param>
        /// <param name="y">The vertical position (0-based row).</param>
        /// <param name="width">The width of the bar in characters.</param>
        /// <param name="color">The color of the bar.</param>
        /// <example>
        /// <code>
        /// MikuConsole.DrawBar(10, 5, 40, RgbColor.MikuCyan);
        /// </code>
        /// </example>
        public static void DrawBar(int x, int y, int width, RgbColor color)
        {
            lock (_lock)
            {
                System.Console.SetCursorPosition(x, y);
                Write(new string(FullBlock, width), color);
            }
        }

        /// <summary>
        /// Draws a horizontal bar at the current cursor position using a custom character.
        /// </summary>
        /// <param name="width">The width of the bar in characters.</param>
        /// <param name="color">The color of the bar.</param>
        /// <param name="character">The character to use for the bar (e.g., '?', '?').</param>
        /// <example>
        /// <code>
        /// MikuConsole.DrawBar(40, RgbColor.Gray, MikuConsole.LightShade);
        /// </code>
        /// </example>
        public static void DrawBar(int width, RgbColor color, char character)
        {
            Write(new string(character, width), color);
        }

        /// <summary>
        /// Draws a horizontal bar at a specific position using a custom character.
        /// </summary>
        /// <param name="x">The horizontal position (0-based column).</param>
        /// <param name="y">The vertical position (0-based row).</param>
        /// <param name="width">The width of the bar in characters.</param>
        /// <param name="color">The color of the bar.</param>
        /// <param name="character">The character to use for the bar (e.g., '?', '?').</param>
        /// <example>
        /// <code>
        /// MikuConsole.DrawBar(10, 5, 40, RgbColor.Gray, MikuConsole.LightShade);
        /// </code>
        /// </example>
        public static void DrawBar(int x, int y, int width, RgbColor color, char character)
        {
            lock (_lock)
            {
                System.Console.SetCursorPosition(x, y);
                Write(new string(character, width), color);
            }
        }

        /// <summary>
        /// Draws a horizontal gradient bar at the current cursor position using the full block character.
        /// </summary>
        /// <param name="width">The width of the bar in characters.</param>
        /// <param name="from">The starting color (left edge).</param>
        /// <param name="to">The ending color (right edge).</param>
        /// <example>
        /// <code>
        /// MikuConsole.DrawGradientBar(50, RgbColor.MikuCyan, RgbColor.MikuPink);
        /// </code>
        /// </example>
        public static void DrawGradientBar(int width, RgbColor from, RgbColor to)
        {
            DrawGradientBar(width, from, to, FullBlock);
        }

        /// <summary>
        /// Draws a horizontal gradient bar at a specific position using the full block character.
        /// </summary>
        /// <param name="x">The horizontal position (0-based column).</param>
        /// <param name="y">The vertical position (0-based row).</param>
        /// <param name="width">The width of the bar in characters.</param>
        /// <param name="from">The starting color (left edge).</param>
        /// <param name="to">The ending color (right edge).</param>
        /// <example>
        /// <code>
        /// MikuConsole.DrawGradientBar(10, 5, 50, RgbColor.MikuCyan, RgbColor.MikuPink);
        /// </code>
        /// </example>
        public static void DrawGradientBar(int x, int y, int width, RgbColor from, RgbColor to)
        {
            DrawGradientBar(x, y, width, from, to, FullBlock);
        }

        /// <summary>
        /// Draws a horizontal gradient bar at the current cursor position using a custom character.
        /// </summary>
        /// <param name="width">The width of the bar in characters.</param>
        /// <param name="from">The starting color (left edge).</param>
        /// <param name="to">The ending color (right edge).</param>
        /// <param name="character">The character to use for the bar.</param>
        public static void DrawGradientBar(int width, RgbColor from, RgbColor to, char character)
        {
            lock (_lock)
            {
                for (int i = 0; i < width; i++)
                {
                    // Interpolate color based on position
                    var color = RgbColor.Lerp(from, to, i / (double)(width - 1));
                    System.Console.Write($"{AnsiCodes.ForegroundRgb(color)}{character}");
                }
                System.Console.Write(AnsiCodes.Reset);
            }
        }

        /// <summary>
        /// Draws a horizontal gradient bar at a specific position using a custom character.
        /// </summary>
        /// <param name="x">The horizontal position (0-based column).</param>
        /// <param name="y">The vertical position (0-based row).</param>
        /// <param name="width">The width of the bar in characters.</param>
        /// <param name="from">The starting color (left edge).</param>
        /// <param name="to">The ending color (right edge).</param>
        /// <param name="character">The character to use for the bar.</param>
        /// <example>
        /// <code>
        /// MikuConsole.DrawGradientBar(10, 5, 50, RgbColor.MikuCyan, RgbColor.MikuPink, '?');
        /// </code>
        /// </example>
        public static void DrawGradientBar(int x, int y, int width, RgbColor from, RgbColor to, char character)
        {
            lock (_lock)
            {
                System.Console.SetCursorPosition(x, y);
                for (int i = 0; i < width; i++)
                {
                    // Interpolate color based on position
                    var color = RgbColor.Lerp(from, to, i / (double)(width - 1));
                    System.Console.Write($"{AnsiCodes.ForegroundRgb(color)}{character}");
                }
                System.Console.Write(AnsiCodes.Reset);
            }
        }

        /// <summary>
        /// Draws a rectangular box with double-line borders at the specified position.
        /// <para>
        /// Uses Unicode box-drawing characters: ? ? ? ? ? ?
        /// </para>
        /// </summary>
        /// <param name="x">The horizontal position of the top-left corner.</param>
        /// <param name="y">The vertical position of the top-left corner.</param>
        /// <param name="width">The total width of the box (including borders).</param>
        /// <param name="height">The total height of the box (including borders).</param>
        /// <param name="color">The color for the box borders.</param>
        /// <remarks>
        /// The interior of the box is not cleared or filled.
        /// Minimum width and height should be 2 to display corners.
        /// </remarks>
        /// <example>
        /// <code>
        /// MikuConsole.DrawBox(5, 2, 40, 10, RgbColor.MikuCyan);
        /// MikuConsole.WriteAt(7, 4, "Inside the box", RgbColor.MikuPink);
        /// </code>
        /// </example>
        public static void DrawBox(int x, int y, int width, int height, RgbColor color)
        {
            lock (_lock)
            {
                // Top border: ??????????????
                System.Console.SetCursorPosition(x, y);
                Write($"{BoxTopLeft}{new string(BoxHorizontal, width - 2)}{BoxTopRight}", color);

                // Side borders: ?            ?
                for (int i = 1; i < height - 1; i++)
                {
                    System.Console.SetCursorPosition(x, y + i);
                    Write(BoxVertical.ToString(), color);
                    System.Console.SetCursorPosition(x + width - 1, y + i);
                    Write(BoxVertical.ToString(), color);
                }

                // Bottom border: ??????????????
                System.Console.SetCursorPosition(x, y + height - 1);
                Write($"{BoxBottomLeft}{new string(BoxHorizontal, width - 2)}{BoxBottomRight}", color);
            }
        }

        #endregion

        #region Cursor and Screen Control

        /// <summary>
        /// Hides the console cursor.
        /// <para>
        /// Useful for animations to prevent cursor flickering.
        /// Always call <see cref="ShowCursor"/> when done.
        /// </para>
        /// </summary>
        /// <example>
        /// <code>
        /// MikuConsole.HideCursor();
        /// try
        /// {
        ///     // Animation code here
        /// }
        /// finally
        /// {
        ///     MikuConsole.ShowCursor();
        /// }
        /// </code>
        /// </example>
        public static void HideCursor()
        {
            System.Console.CursorVisible = false;
        }

        /// <summary>
        /// Shows the console cursor.
        /// </summary>
        /// <remarks>
        /// Call this to restore cursor visibility after calling <see cref="HideCursor"/>.
        /// </remarks>
        public static void ShowCursor()
        {
            System.Console.CursorVisible = true;
        }

        /// <summary>
        /// Clears the entire console screen and moves cursor to (0, 0).
        /// </summary>
        public static void Clear()
        {
            System.Console.Clear();
        }

        /// <summary>
        /// Resets all ANSI styles and colors by writing the reset escape sequence.
        /// <para>
        /// This clears any active formatting (bold, underline, colors) set by
        /// ANSI escape sequences.
        /// </para>
        /// </summary>
        public static void ResetColors()
        {
            lock (_lock)
            {
                System.Console.Write(AnsiCodes.Reset);
            }
        }

        /// <summary>
        /// Resets console colors using the standard <see cref="System.Console.ResetColor"/> method.
        /// <para>
        /// Use this after using <see cref="System.Console.ForegroundColor"/> or
        /// <see cref="System.Console.BackgroundColor"/> properties.
        /// </para>
        /// </summary>
        public static void ResetConsoleColors()
        {
            System.Console.ResetColor();
        }

        #endregion

        #region ANSI Support Check

        /// <summary>
        /// Checks if the current terminal environment supports ANSI escape sequences.
        /// </summary>
        /// <returns><c>true</c> if ANSI is likely supported; otherwise, <c>false</c>.</returns>
        private static bool CheckAnsiSupport()
        {
            // Check COLORTERM for TrueColor support
            var colorTerm = Environment.GetEnvironmentVariable("COLORTERM");
            if (colorTerm == "truecolor" || colorTerm == "24bit")
                return true;

            // Check TERM for color terminal indicators
            var term = Environment.GetEnvironmentVariable("TERM");
            if (!string.IsNullOrEmpty(term) &&
                (term.Contains("xterm") || term.Contains("256color") || term.Contains("color")))
                return true;

            // Windows 10+ has native ANSI support
            if (Environment.OSVersion.Platform == PlatformID.Win32NT &&
                Environment.OSVersion.Version.Major >= 10)
                return true;

            // Unix/macOS generally support ANSI
            if (Environment.OSVersion.Platform == PlatformID.Unix ||
                Environment.OSVersion.Platform == PlatformID.MacOSX)
                return true;

            return false;
        }

        #endregion
    }
}

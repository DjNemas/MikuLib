namespace Miku.Core
{
    /// <summary>
    /// ANSI escape code constants for console styling.
    /// </summary>
    /// <remarks>
    /// Like the visual effects at Miku's concerts,
    /// these codes add style and flair to console output!
    /// </remarks>
    public static class MikuAnsiCodes
    {
        /// <summary>Escape character for ANSI sequences.</summary>
        public const string Escape = "\x1b[";

        /// <summary>Reset all styles and colors.</summary>
        public const string Reset = "\x1b[0m";

        #region Text Styles

        /// <summary>Bold text.</summary>
        public const string Bold = "\x1b[1m";

        /// <summary>Dim/faint text.</summary>
        public const string Dim = "\x1b[2m";

        /// <summary>Italic text.</summary>
        public const string Italic = "\x1b[3m";

        /// <summary>Underlined text.</summary>
        public const string Underline = "\x1b[4m";

        /// <summary>Blinking text (not supported in all terminals).</summary>
        public const string Blink = "\x1b[5m";

        /// <summary>Rapid blink (not supported in all terminals).</summary>
        public const string RapidBlink = "\x1b[6m";

        /// <summary>Reverse/invert foreground and background.</summary>
        public const string Reverse = "\x1b[7m";

        /// <summary>Hidden/invisible text.</summary>
        public const string Hidden = "\x1b[8m";

        /// <summary>Strikethrough text.</summary>
        public const string Strikethrough = "\x1b[9m";

        #endregion

        #region Reset Specific Styles

        /// <summary>Reset bold/dim.</summary>
        public const string ResetBold = "\x1b[22m";

        /// <summary>Reset italic.</summary>
        public const string ResetItalic = "\x1b[23m";

        /// <summary>Reset underline.</summary>
        public const string ResetUnderline = "\x1b[24m";

        /// <summary>Reset blink.</summary>
        public const string ResetBlink = "\x1b[25m";

        /// <summary>Reset reverse.</summary>
        public const string ResetReverse = "\x1b[27m";

        /// <summary>Reset hidden.</summary>
        public const string ResetHidden = "\x1b[28m";

        /// <summary>Reset strikethrough.</summary>
        public const string ResetStrikethrough = "\x1b[29m";

        #endregion

        #region Cursor Control

        /// <summary>Hide cursor.</summary>
        public const string HideCursor = "\x1b[?25l";

        /// <summary>Show cursor.</summary>
        public const string ShowCursor = "\x1b[?25h";

        /// <summary>Save cursor position.</summary>
        public const string SaveCursor = "\x1b[s";

        /// <summary>Restore cursor position.</summary>
        public const string RestoreCursor = "\x1b[u";

        #endregion

        #region Screen Control

        /// <summary>Clear entire screen.</summary>
        public const string ClearScreen = "\x1b[2J";

        /// <summary>Clear from cursor to end of screen.</summary>
        public const string ClearToEnd = "\x1b[0J";

        /// <summary>Clear from cursor to beginning of screen.</summary>
        public const string ClearToBeginning = "\x1b[1J";

        /// <summary>Clear entire line.</summary>
        public const string ClearLine = "\x1b[2K";

        /// <summary>Clear from cursor to end of line.</summary>
        public const string ClearLineToEnd = "\x1b[0K";

        /// <summary>Clear from cursor to beginning of line.</summary>
        public const string ClearLineToBeginning = "\x1b[1K";

        #endregion

        #region Color Helpers

        /// <summary>
        /// Gets the ANSI escape sequence for a TrueColor foreground.
        /// </summary>
        public static string ForegroundRgb(byte r, byte g, byte b) => $"\x1b[38;2;{r};{g};{b}m";

        /// <summary>
        /// Gets the ANSI escape sequence for a TrueColor foreground.
        /// </summary>
        public static string ForegroundRgb(MikuRgbColor color) => ForegroundRgb(color.R, color.G, color.B);

        /// <summary>
        /// Gets the ANSI escape sequence for a TrueColor background.
        /// </summary>
        public static string BackgroundRgb(byte r, byte g, byte b) => $"\x1b[48;2;{r};{g};{b}m";

        /// <summary>
        /// Gets the ANSI escape sequence for a TrueColor background.
        /// </summary>
        public static string BackgroundRgb(MikuRgbColor color) => BackgroundRgb(color.R, color.G, color.B);

        /// <summary>
        /// Gets the ANSI escape sequence for a 256-color foreground.
        /// </summary>
        public static string Foreground256(byte colorCode) => $"\x1b[38;5;{colorCode}m";

        /// <summary>
        /// Gets the ANSI escape sequence for a 256-color background.
        /// </summary>
        public static string Background256(byte colorCode) => $"\x1b[48;5;{colorCode}m";

        /// <summary>
        /// Moves cursor to specified position.
        /// </summary>
        public static string MoveTo(int row, int column) => $"\x1b[{row};{column}H";

        /// <summary>
        /// Moves cursor up by specified rows.
        /// </summary>
        public static string MoveUp(int rows = 1) => $"\x1b[{rows}A";

        /// <summary>
        /// Moves cursor down by specified rows.
        /// </summary>
        public static string MoveDown(int rows = 1) => $"\x1b[{rows}B";

        /// <summary>
        /// Moves cursor right by specified columns.
        /// </summary>
        public static string MoveRight(int columns = 1) => $"\x1b[{columns}C";

        /// <summary>
        /// Moves cursor left by specified columns.
        /// </summary>
        public static string MoveLeft(int columns = 1) => $"\x1b[{columns}D";

        #endregion
    }
}

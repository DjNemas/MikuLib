using Miku.Logger.Configuration.Enums;

namespace Miku.Logger.Configuration.Models
{
    /// <summary>
    /// Console color configuration for different log levels.
    /// </summary>
    /// <remarks>
    /// Like Miku's iconic cyan (#00CED1), each log level can have its own signature color.
    /// Supports three color modes:
    /// - Console: Standard 16 console colors
    /// - Extended256: 256-color palette with ANSI escape codes
    /// - TrueColor: Full 24-bit RGB colors (16 million colors!)
    /// </remarks>
    public class MikuConsoleColorOptions
    {
        /// <summary>Gets or sets whether to use colored output.</summary>
        public bool Enabled { get; set; } = true;

        /// <summary>Gets or sets the color space to use.</summary>
        public MikuColorSpace ColorSpace { get; set; } = MikuColorSpace.Console;

        #region Standard Console Colors (16 colors)

        /// <summary>Gets or sets the color for Trace level logs.</summary>
        public ConsoleColor TraceColor { get; set; } = ConsoleColor.Gray;

        /// <summary>Gets or sets the color for Debug level logs.</summary>
        public ConsoleColor DebugColor { get; set; } = ConsoleColor.Yellow;

        /// <summary>Gets or sets the color for Information level logs.</summary>
        public ConsoleColor InformationColor { get; set; } = ConsoleColor.Cyan;

        /// <summary>Gets or sets the color for Warning level logs.</summary>
        public ConsoleColor WarningColor { get; set; } = ConsoleColor.Magenta;

        /// <summary>Gets or sets the color for Error level logs.</summary>
        public ConsoleColor ErrorColor { get; set; } = ConsoleColor.Red;

        /// <summary>Gets or sets the color for Critical level logs.</summary>
        public ConsoleColor CriticalColor { get; set; } = ConsoleColor.DarkRed;

        #endregion

        #region Extended Color Options

        /// <summary>
        /// Gets or sets the Extended256 color options.
        /// Used when ColorSpace is set to Extended256.
        /// </summary>
        public MikuExtended256ColorOptions Extended256Colors { get; set; } = new();

        /// <summary>
        /// Gets or sets the TrueColor (24-bit RGB) options.
        /// Used when ColorSpace is set to TrueColor.
        /// </summary>
        public MikuTrueColorOptions TrueColors { get; set; } = new();

        #endregion
    }
}

using Miku.Core;

namespace Miku.Logger.Configuration.Models
{
    /// <summary>
    /// Color configuration for Extended256 color palette (8-bit colors).
    /// </summary>
    /// <remarks>
    /// The 256-color palette includes:
    /// - 0-7: Standard colors
    /// - 8-15: High-intensity colors
    /// - 16-231: 216 colors (6x6x6 color cube)
    /// - 232-255: 24 grayscale colors
    /// </remarks>
    public class MikuExtended256ColorOptions
    {
        /// <summary>Gets or sets the color code for Trace level logs (0-255). Default: 245 (light gray).</summary>
        public byte TraceColor { get; set; } = 245;

        /// <summary>Gets or sets the color code for Debug level logs (0-255). Default: 226 (yellow).</summary>
        public byte DebugColor { get; set; } = 226;

        /// <summary>Gets or sets the color code for Information level logs (0-255). Default: 44 (cyan/turquoise - Miku!).</summary>
        public byte InformationColor { get; set; } = 44;

        /// <summary>Gets or sets the color code for Warning level logs (0-255). Default: 208 (orange).</summary>
        public byte WarningColor { get; set; } = 208;

        /// <summary>Gets or sets the color code for Error level logs (0-255). Default: 196 (red).</summary>
        public byte ErrorColor { get; set; } = 196;

        /// <summary>Gets or sets the color code for Critical level logs (0-255). Default: 160 (dark red).</summary>
        public byte CriticalColor { get; set; } = 160;
    }
}

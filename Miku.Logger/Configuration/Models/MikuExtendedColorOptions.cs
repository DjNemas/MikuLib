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

    /// <summary>
    /// Color configuration for TrueColor (24-bit RGB) console output.
    /// </summary>
    /// <remarks>
    /// Like painting with all 16 million colors of the rainbow!
    /// Miku's cyan (#00CED1) is the default for Information level.
    /// </remarks>
    public class MikuTrueColorOptions
    {
        /// <summary>Gets or sets the RGB color for Trace level logs. Default: Gray.</summary>
        public MikuRgbColor TraceColor { get; set; } = MikuRgbColor.Gray;

        /// <summary>Gets or sets the RGB color for Debug level logs. Default: Yellow.</summary>
        public MikuRgbColor DebugColor { get; set; } = MikuRgbColor.Yellow;

        /// <summary>Gets or sets the RGB color for Information level logs. Default: Miku Cyan (#00CED1).</summary>
        public MikuRgbColor InformationColor { get; set; } = MikuRgbColor.MikuCyan;

        /// <summary>Gets or sets the RGB color for Warning level logs. Default: Orange.</summary>
        public MikuRgbColor WarningColor { get; set; } = MikuRgbColor.Orange;

        /// <summary>Gets or sets the RGB color for Error level logs. Default: Red.</summary>
        public MikuRgbColor ErrorColor { get; set; } = MikuRgbColor.Red;

        /// <summary>Gets or sets the RGB color for Critical level logs. Default: Dark Red.</summary>
        public MikuRgbColor CriticalColor { get; set; } = MikuRgbColor.DarkRed;
    }
}

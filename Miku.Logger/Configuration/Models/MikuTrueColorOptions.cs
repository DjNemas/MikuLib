using Miku.Core;

namespace Miku.Logger.Configuration.Models
{
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

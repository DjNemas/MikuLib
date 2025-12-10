namespace Miku.Logger.Configuration.Models
{
    /// <summary>
    /// Options for controlling which elements are shown in log messages.
    /// </summary>
    /// <remarks>
    /// Like adjusting the mix in a song, these options control
    /// which elements are visible in the final log output.
    /// </remarks>
    public class MikuLogFormatOptions
    {
        /// <summary>
        /// Gets or sets whether to show the date in log messages. Default: true.
        /// </summary>
        public bool ShowDate { get; set; } = true;

        /// <summary>
        /// Gets or sets whether to show the time in log messages. Default: true.
        /// </summary>
        public bool ShowTime { get; set; } = true;

        /// <summary>
        /// Gets or sets whether to show the log level in log messages. Default: true.
        /// </summary>
        public bool ShowLogLevel { get; set; } = true;

        /// <summary>
        /// Gets or sets whether to show the logger/category name in log messages. Default: true.
        /// </summary>
        public bool ShowLoggerName { get; set; } = true;
    }
}

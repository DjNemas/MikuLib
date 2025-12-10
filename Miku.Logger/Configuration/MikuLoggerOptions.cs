using Miku.Logger.Configuration.Enums;
using Miku.Logger.Configuration.Models;

namespace Miku.Logger.Configuration
{
    /// <summary>
    /// Configuration options for the MikuLogger.
    /// </summary>
    /// <remarks>
    /// Like tuning Miku's voice parameters for the perfect performance,
    /// these options fine-tune the logger's behavior.
    /// </remarks>
    public class MikuLoggerOptions
    {
        /// <summary>
        /// Gets or sets the log output targets.
        /// </summary>
        public MikuLogOutput Output { get; set; } = MikuLogOutput.ConsoleAndFile;

        /// <summary>
        /// Gets or sets the minimum log level to be written.
        /// </summary>
        public MikuLogLevel MinimumLogLevel { get; set; } = MikuLogLevel.Information;

        /// <summary>
        /// Gets or sets the console color options.
        /// </summary>
        public MikuConsoleColorOptions ConsoleColors { get; set; } = new();

        /// <summary>
        /// Gets or sets the file logging options.
        /// </summary>
        public MikuFileLoggerOptions FileOptions { get; set; } = new();

        /// <summary>
        /// Gets or sets the Server-Sent Events (SSE) logging options.
        /// </summary>
        public MikuSseLoggerOptions SseOptions { get; set; } = new();

        /// <summary>
        /// Gets or sets the log message formatting options.
        /// </summary>
        public MikuLogFormatOptions FormatOptions { get; set; } = new();

        /// <summary>
        /// Gets or sets the date format for log messages.
        /// Only used when FormatOptions.ShowDate or FormatOptions.ShowTime is true.
        /// </summary>
        public string DateFormat { get; set; } = "yyyy-MM-dd HH:mm:ss.fff";

        /// <summary>
        /// Gets or sets whether to use UTC time for log timestamps.
        /// </summary>
        public bool UseUtcTime { get; set; } = false;
    }
}

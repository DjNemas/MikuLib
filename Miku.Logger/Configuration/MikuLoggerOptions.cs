namespace Miku.Logger.Configuration
{
    /// <summary>
    /// Configuration options for the MikuLogger.
    /// </summary>
    public class MikuLoggerOptions
    {
        /// <summary>
        /// Gets or sets the log output targets.
        /// </summary>
        public LogOutput Output { get; set; } = LogOutput.ConsoleAndFile;

        /// <summary>
        /// Gets or sets the minimum log level to be written.
        /// </summary>
        public LogLevel MinimumLogLevel { get; set; } = LogLevel.Information;

        /// <summary>
        /// Gets or sets the console color options.
        /// </summary>
        public ConsoleColorOptions ConsoleColors { get; set; } = new();

        /// <summary>
        /// Gets or sets the file logging options.
        /// </summary>
        public FileLoggerOptions FileOptions { get; set; } = new();

        /// <summary>
        /// Gets or sets the log message formatting options.
        /// </summary>
        public LogFormatOptions FormatOptions { get; set; } = new();

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

    /// <summary>
    /// Options for controlling which elements are shown in log messages.
    /// </summary>
    public class LogFormatOptions
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

    /// <summary>
    /// Specifies where logs should be written.
    /// </summary>
    [Flags]
    public enum LogOutput
    {
        /// <summary>No output</summary>
        None = 0,
        /// <summary>Output to console only</summary>
        Console = 1,
        /// <summary>Output to file only</summary>
        File = 2,
        /// <summary>Output to both console and file</summary>
        ConsoleAndFile = Console | File
    }

    /// <summary>
    /// Log severity levels.
    /// </summary>
    public enum LogLevel
    {
        /// <summary>Logs that contain the most detailed messages.</summary>
        Trace = 0,
        /// <summary>Logs used for interactive investigation during development.</summary>
        Debug = 1,
        /// <summary>Logs that track the general flow of the application.</summary>
        Information = 2,
        /// <summary>Logs that highlight abnormal or unexpected events.</summary>
        Warning = 3,
        /// <summary>Logs that highlight when the current flow of execution is stopped due to a failure.</summary>
        Error = 4,
        /// <summary>Logs that describe an unrecoverable application or system crash.</summary>
        Critical = 5,
        /// <summary>Not used for writing log messages. Specifies that no messages should be written.</summary>
        None = 6
    }

    /// <summary>
    /// Console color configuration for different log levels.
    /// </summary>
    public class ConsoleColorOptions
    {
        /// <summary>Gets or sets whether to use colored output.</summary>
        public bool Enabled { get; set; } = true;

        /// <summary>Gets or sets the color space to use.</summary>
        public ColorSpace ColorSpace { get; set; } = ColorSpace.Console;

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
    }

    /// <summary>
    /// Color space options for console output.
    /// </summary>
    public enum ColorSpace
    {
        /// <summary>Standard 16 console colors</summary>
        Console,
        /// <summary>256 color palette (future support)</summary>
        Extended256,
        /// <summary>24-bit RGB colors (future support)</summary>
        TrueColor
    }

    /// <summary>
    /// File logging configuration options.
    /// </summary>
    public class FileLoggerOptions
    {
        /// <summary>Gets or sets the base directory for log files.</summary>
        public string LogDirectory { get; set; } = "./logs";

        /// <summary>Gets or sets the log file name pattern.</summary>
        public string FileNamePattern { get; set; } = "log.txt";

        /// <summary>Gets or sets whether to organize logs in date-based folders.</summary>
        public bool UseDateFolders { get; set; } = false;

        /// <summary>Gets or sets the date folder format (e.g., "yyyy-MM-dd").</summary>
        public string DateFolderFormat { get; set; } = "yyyy-MM-dd";

        /// <summary>Gets or sets the maximum file size in bytes before rotation. 0 = no rotation.</summary>
        public long MaxFileSizeBytes { get; set; } = 10 * 1024 * 1024; // 10 MB default

        /// <summary>Gets or sets the maximum number of log files to keep. 0 = unlimited.</summary>
        public int MaxFileCount { get; set; } = 10;

        /// <summary>Gets or sets whether to append to existing files or create new ones.</summary>
        public bool AppendToExisting { get; set; } = true;
    }
}

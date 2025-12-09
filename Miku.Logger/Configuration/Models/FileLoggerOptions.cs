namespace Miku.Logger.Configuration.Models
{
    /// <summary>
    /// File logging configuration options.
    /// </summary>
    /// <remarks>
    /// Like organizing concert recordings by date and venue,
    /// these options control how log files are organized and managed.
    /// </remarks>
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

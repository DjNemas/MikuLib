using Miku.Logger.Configuration.Enums;
using System.Text.Json.Serialization;

namespace Miku.Logger.Sse
{
    /// <summary>
    /// Represents a log entry for SSE streaming.
    /// </summary>
    /// <remarks>
    /// Like the data packets in Miku's digital voice,
    /// this class carries log information to connected clients.
    /// </remarks>
    public class MikuSseLogEntry
    {
        /// <summary>
        /// Gets or sets the unique identifier for this log entry.
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; } = Guid.NewGuid().ToString("N");

        /// <summary>
        /// Gets or sets the timestamp of the log entry.
        /// </summary>
        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the log level.
        /// </summary>
        [JsonPropertyName("level")]
        public string Level { get; set; } = "Information";

        /// <summary>
        /// Gets or sets the log level as numeric value.
        /// </summary>
        [JsonPropertyName("levelValue")]
        public int LevelValue { get; set; }

        /// <summary>
        /// Gets or sets the logger/category name.
        /// </summary>
        [JsonPropertyName("category")]
        public string Category { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the log message.
        /// </summary>
        [JsonPropertyName("message")]
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the exception details if any.
        /// </summary>
        [JsonPropertyName("exception")]
        public string? Exception { get; set; }

        /// <summary>
        /// Creates an MikuSseLogEntry from log parameters.
        /// </summary>
        internal static MikuSseLogEntry Create(
            MikuLogLevel logLevel,
            string category,
            string message,
            Exception? exception = null,
            bool useUtcTime = true)
        {
            return new MikuSseLogEntry
            {
                Timestamp = useUtcTime ? DateTime.UtcNow : DateTime.Now,
                Level = GetLogLevelString(logLevel),
                LevelValue = (int)logLevel,
                Category = category,
                Message = message,
                Exception = exception?.ToString()
            };
        }

        private static string GetLogLevelString(MikuLogLevel logLevel)
        {
            return logLevel switch
            {
                MikuLogLevel.Trace => "Trace",
                MikuLogLevel.Debug => "Debug",
                MikuLogLevel.Information => "Information",
                MikuLogLevel.Warning => "Warning",
                MikuLogLevel.Error => "Error",
                MikuLogLevel.Critical => "Critical",
                MikuLogLevel.None => "None",
                _ => "Unknown"
            };
        }
    }
}

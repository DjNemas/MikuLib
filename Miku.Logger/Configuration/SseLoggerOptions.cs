using Miku.Logger.Configuration.Enums;

namespace Miku.Logger.Configuration
{
    /// <summary>
    /// Configuration options for Server-Sent Events (SSE) log streaming.
    /// </summary>
    /// <remarks>
    /// Like Miku's voice streaming to fans worldwide, 
    /// these options configure how logs are broadcast to connected clients.
    /// </remarks>
    public class SseLoggerOptions
    {
        /// <summary>
        /// Gets or sets the endpoint path for SSE log streaming.
        /// Default: "/miku/logs/stream"
        /// </summary>
        /// <example>
        /// <code>
        /// options.EndpointPath = "/api/logs/live";
        /// // Clients connect to: https://yourapp.com/api/logs/live
        /// </code>
        /// </example>
        public string EndpointPath { get; set; } = "/miku/logs/stream";

        /// <summary>
        /// Gets or sets the SSE event type name.
        /// Default: "miku-log"
        /// </summary>
        public string EventType { get; set; } = "miku-log";

        /// <summary>
        /// Gets or sets the maximum number of concurrent SSE clients.
        /// 0 = unlimited. Default: 100
        /// </summary>
        public int MaxClients { get; set; } = 100;

        /// <summary>
        /// Gets or sets the reconnection interval hint for clients in milliseconds.
        /// Default: 3000 (3 seconds)
        /// </summary>
        public int ReconnectionIntervalMs { get; set; } = 3000;

        /// <summary>
        /// Gets or sets whether to include the log level in the SSE event type.
        /// When true, events are sent as "miku-log-info", "miku-log-error", etc.
        /// Default: false
        /// </summary>
        public bool IncludeLogLevelInEventType { get; set; } = false;

        /// <summary>
        /// Gets or sets whether to require authorization for the SSE endpoint.
        /// Default: false
        /// </summary>
        public bool RequireAuthorization { get; set; } = false;

        /// <summary>
        /// Gets or sets the authorization policy name for the SSE endpoint.
        /// Only used when RequireAuthorization is true.
        /// </summary>
        public string? AuthorizationPolicy { get; set; }

        /// <summary>
        /// Gets or sets the minimum log level for SSE streaming.
        /// Allows filtering logs sent to SSE clients independently of other outputs.
        /// Default: null (uses the main MinimumLogLevel from MikuLoggerOptions)
        /// </summary>
        public LogLevel? MinimumLogLevel { get; set; }
    }
}

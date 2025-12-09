namespace Miku.Logger.Configuration.Enums
{
    /// <summary>
    /// Specifies where logs should be written.
    /// </summary>
    /// <remarks>
    /// Like Miku's voice reaching fans through different channels,
    /// logs can be output to multiple destinations simultaneously.
    /// </remarks>
    [Flags]
    public enum LogOutput
    {
        /// <summary>No output</summary>
        None = 0,

        /// <summary>Output to console only</summary>
        Console = 1,

        /// <summary>Output to file only</summary>
        File = 2,

        /// <summary>Output to Server-Sent Events stream</summary>
        ServerSentEvents = 4,

        /// <summary>Output to both console and file</summary>
        ConsoleAndFile = Console | File,

        /// <summary>Output to console, file, and SSE</summary>
        All = Console | File | ServerSentEvents
    }
}

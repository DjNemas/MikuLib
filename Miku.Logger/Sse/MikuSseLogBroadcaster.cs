using Miku.Logger.Configuration;
using Miku.Logger.Configuration.Enums;
using System.Threading.Channels;

namespace Miku.Logger.Sse
{
    /// <summary>
    /// Singleton service that manages SSE log broadcasting to connected clients.
    /// </summary>
    /// <remarks>
    /// Like Miku's concert broadcast system reaching fans everywhere,
    /// this service streams logs to all connected SSE clients in real-time.
    /// Born from the CV01 spirit of connection.
    /// </remarks>
    public sealed class MikuSseLogBroadcaster : IDisposable
    {
        // Mi-Ku number - the magic of 39
        private const int MikuChannelCapacity = 39;

        private static readonly Lazy<MikuSseLogBroadcaster> _instance =
            new(() => new MikuSseLogBroadcaster());

        /// <summary>
        /// Gets the singleton instance of the MikuSseLogBroadcaster.
        /// </summary>
        public static MikuSseLogBroadcaster Instance => _instance.Value;

        private readonly List<Channel<MikuSseLogEntry>> _clientChannels = new();
        private readonly SemaphoreSlim _channelLock = new(1, 1);
        private readonly MikuSseLoggerOptions _options = new();
        private bool _disposed;

        private MikuSseLogBroadcaster() { }

        /// <summary>
        /// Configures the broadcaster with the specified options.
        /// </summary>
        /// <param name="options">The SSE logger options.</param>
        public void Configure(MikuSseLoggerOptions options)
        {
            if (options != null)
            {
                _options.EndpointPath = options.EndpointPath;
                _options.EventType = options.EventType;
                _options.MaxClients = options.MaxClients;
                _options.ReconnectionIntervalMs = options.ReconnectionIntervalMs;
                _options.IncludeLogLevelInEventType = options.IncludeLogLevelInEventType;
                _options.MinimumLogLevel = options.MinimumLogLevel;
            }
        }

        /// <summary>
        /// Gets the current SSE options.
        /// </summary>
        public MikuSseLoggerOptions Options => _options;

        /// <summary>
        /// Gets the number of currently connected clients.
        /// </summary>
        public int ClientCount
        {
            get
            {
                _channelLock.Wait();
                try
                {
                    return _clientChannels.Count;
                }
                finally
                {
                    _channelLock.Release();
                }
            }
        }

        /// <summary>
        /// Broadcasts a log entry to all connected SSE clients.
        /// </summary>
        /// <param name="logLevel">The log level.</param>
        /// <param name="category">The logger category name.</param>
        /// <param name="message">The log message.</param>
        /// <param name="exception">Optional exception.</param>
        /// <param name="useUtcTime">Whether to use UTC time.</param>
        public void Broadcast(
            MikuLogLevel logLevel,
            string category,
            string message,
            Exception? exception = null,
            bool useUtcTime = true)
        {
            // Check minimum log level filter
            if (_options.MinimumLogLevel.HasValue && logLevel < _options.MinimumLogLevel.Value)
                return;

            var entry = MikuSseLogEntry.Create(logLevel, category, message, exception, useUtcTime);
            BroadcastEntry(entry);
        }

        /// <summary>
        /// Broadcasts a log entry to all connected SSE clients.
        /// </summary>
        /// <param name="entry">The log entry to broadcast.</param>
        public void BroadcastEntry(MikuSseLogEntry entry)
        {
            if (_disposed) return;

            _channelLock.Wait();
            try
            {
                // Remove completed channels and broadcast to active ones
                var channelsToRemove = new List<Channel<MikuSseLogEntry>>();

                foreach (var channel in _clientChannels)
                {
                    if (!channel.Writer.TryWrite(entry))
                    {
                        // Channel is full or completed - mark for removal
                        if (channel.Reader.Completion.IsCompleted)
                        {
                            channelsToRemove.Add(channel);
                        }
                    }
                }

                foreach (var channel in channelsToRemove)
                {
                    _clientChannels.Remove(channel);
                }
            }
            finally
            {
                _channelLock.Release();
            }
        }

        /// <summary>
        /// Subscribes a new client and returns an async enumerable of log entries.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token for the subscription.</param>
        /// <returns>An async enumerable of log entries.</returns>
        public async IAsyncEnumerable<MikuSseLogEntry> SubscribeAsync(
            [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            // Check max clients limit
            if (_options.MaxClients > 0 && ClientCount >= _options.MaxClients)
            {
                yield break;
            }

            var channel = Channel.CreateBounded<MikuSseLogEntry>(new BoundedChannelOptions(MikuChannelCapacity)
            {
                FullMode = BoundedChannelFullMode.DropOldest,
                SingleReader = true,
                SingleWriter = false
            });

            await _channelLock.WaitAsync(cancellationToken);
            try
            {
                _clientChannels.Add(channel);
            }
            finally
            {
                _channelLock.Release();
            }

            try
            {
                await foreach (var entry in channel.Reader.ReadAllAsync(cancellationToken))
                {
                    yield return entry;
                }
            }
            finally
            {
                // Cleanup on disconnect
                await _channelLock.WaitAsync(CancellationToken.None);
                try
                {
                    _clientChannels.Remove(channel);
                    channel.Writer.Complete();
                }
                finally
                {
                    _channelLock.Release();
                }
            }
        }

        /// <summary>
        /// Disposes the broadcaster and disconnects all clients.
        /// </summary>
        public void Dispose()
        {
            if (_disposed) return;

            _disposed = true;

            _channelLock.Wait();
            try
            {
                foreach (var channel in _clientChannels)
                {
                    channel.Writer.Complete();
                }
                _clientChannels.Clear();
            }
            finally
            {
                _channelLock.Release();
                _channelLock.Dispose();
            }
        }
    }
}

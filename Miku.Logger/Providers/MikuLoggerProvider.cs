using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Miku.Logger.Configuration;
using System.Collections.Concurrent;

namespace Miku.Logger.Providers
{
    /// <summary>
    /// Provider for creating MikuLogger instances in ASP.NET Core applications.
    /// </summary>
    [ProviderAlias("MikuLogger")]
    public class MikuLoggerProvider : ILoggerProvider
    {
        private readonly MikuLoggerOptions _options;
        private readonly ConcurrentDictionary<string, MikuLogger> _loggers = new();

        /// <summary>
        /// Initializes a new instance of the MikuLoggerProvider class.
        /// </summary>
        /// <param name="options">The logger options.</param>
        public MikuLoggerProvider(IOptions<MikuLoggerOptions> options)
        {
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        }

        /// <summary>
        /// Initializes a new instance of the MikuLoggerProvider class.
        /// </summary>
        /// <param name="options">The logger options.</param>
        public MikuLoggerProvider(MikuLoggerOptions options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        /// <inheritdoc/>
        public ILogger CreateLogger(string categoryName)
        {
            return _loggers.GetOrAdd(categoryName, name => new MikuLogger(name, _options));
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            foreach (var logger in _loggers.Values)
            {
                logger.Dispose();
            }
            _loggers.Clear();
        }
    }
}
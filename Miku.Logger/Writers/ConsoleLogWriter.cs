using Miku.Logger.Configuration;

namespace Miku.Logger.Writers
{
    /// <summary>
    /// Thread-safe console writer with color support.
    /// </summary>
    internal class ConsoleLogWriter
    {
        private readonly ConsoleColorOptions _colorOptions;
        private readonly SemaphoreSlim _writeSemaphore = new(1, 1);

        public ConsoleLogWriter(ConsoleColorOptions colorOptions)
        {
            _colorOptions = colorOptions ?? throw new ArgumentNullException(nameof(colorOptions));
        }

        /// <summary>
        /// Writes a log message to console asynchronously.
        /// </summary>
        public async Task WriteAsync(string message, LogLevel logLevel, CancellationToken cancellationToken = default)
        {
            await _writeSemaphore.WaitAsync(cancellationToken);
            try
            {
                WriteToConsole(message, logLevel);
            }
            finally
            {
                _writeSemaphore.Release();
            }
        }

        /// <summary>
        /// Writes a log message to console synchronously.
        /// </summary>
        public void Write(string message, LogLevel logLevel)
        {
            _writeSemaphore.Wait();
            try
            {
                WriteToConsole(message, logLevel);
            }
            finally
            {
                _writeSemaphore.Release();
            }
        }

        private void WriteToConsole(string message, LogLevel logLevel)
        {
            if (_colorOptions.Enabled && _colorOptions.ColorSpace == ColorSpace.Console)
            {
                var color = GetColorForLogLevel(logLevel);
                Console.ForegroundColor = color;
                Console.WriteLine(message);
                Console.ResetColor();
            }
            else
            {
                Console.WriteLine(message);
            }
        }

        private ConsoleColor GetColorForLogLevel(LogLevel logLevel)
        {
            return logLevel switch
            {
                LogLevel.Trace => _colorOptions.TraceColor,
                LogLevel.Debug => _colorOptions.DebugColor,
                LogLevel.Information => _colorOptions.InformationColor,
                LogLevel.Warning => _colorOptions.WarningColor,
                LogLevel.Error => _colorOptions.ErrorColor,
                LogLevel.Critical => _colorOptions.CriticalColor,
                _ => Console.ForegroundColor
            };
        }
    }
}

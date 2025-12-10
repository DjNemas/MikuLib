using Miku.Logger.Configuration.Models;
using System.Collections.Concurrent;

namespace Miku.Logger.Writers
{
    /// <summary>
    /// Thread-safe file writer with log rotation support.
    /// High-performance implementation using singleton shared file streams.
    /// </summary>
    /// <remarks>
    /// Like writing Miku's song lyrics to digital storage,
    /// this writer ensures every log message is safely preserved.
    /// Features:
    /// - Async queue-based writing for high performance
    /// - Automatic log rotation based on file size
    /// - Date-based folder organization
    /// - Singleton file stream management for thread safety
    /// </remarks>
    internal class MikuFileLogWriter : IDisposable
    {
        private readonly MikuFileLoggerOptions _options;
        private readonly ConcurrentQueue<string> _writeQueue = new();
        private readonly CancellationTokenSource _cancellationTokenSource = new();
        private readonly Task _writerTask;
        private bool _disposed;
        private MikuSharedFileStream? _currentStream;

        /// <summary>
        /// Initializes a new instance of the <see cref="MikuFileLogWriter"/> class.
        /// </summary>
        /// <param name="options">The file logger options.</param>
        /// <exception cref="ArgumentNullException">Thrown when options is null.</exception>
        public MikuFileLogWriter(MikuFileLoggerOptions options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _writerTask = Task.Run(ProcessQueueAsync);
        }

        /// <summary>
        /// Writes a log message asynchronously by adding it to the write queue.
        /// </summary>
        /// <param name="message">The formatted log message to write.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A task that completes when the message has been queued.</returns>
        public async Task WriteAsync(string message, CancellationToken cancellationToken = default)
        {
            if (_disposed) return;

            _writeQueue.Enqueue(message);
            await Task.CompletedTask;
        }

        /// <summary>
        /// Writes a log message synchronously by adding it to the write queue.
        /// </summary>
        /// <param name="message">The formatted log message to write.</param>
        public void Write(string message)
        {
            if (_disposed) return;
            _writeQueue.Enqueue(message);
        }

        /// <summary>
        /// Background task that processes the write queue and writes messages to file.
        /// </summary>
        private async Task ProcessQueueAsync()
        {
            const int batchSize = 100;
            var batch = new List<string>(batchSize);

            while (!_cancellationTokenSource.Token.IsCancellationRequested)
            {
                try
                {
                    int collected = 0;
                    while (collected < batchSize && _writeQueue.TryDequeue(out var message))
                    {
                        batch.Add(message);
                        collected++;
                    }

                    if (batch.Count > 0)
                    {
                        await WriteBatchToFileAsync(batch);
                        batch.Clear();
                    }
                    else
                    {
                        await Task.Delay(10, _cancellationTokenSource.Token);
                    }
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error in log writer: {ex.Message}");
                    await Task.Delay(100);
                }
            }
        }

        /// <summary>
        /// Writes a batch of messages to the log file.
        /// </summary>
        /// <param name="messages">The messages to write.</param>
        private async Task WriteBatchToFileAsync(List<string> messages)
        {
            var filePath = GetCurrentLogFilePath();
            EnsureDirectoryExists(Path.GetDirectoryName(filePath)!);

            if (ShouldRotate(filePath))
            {
                RotateLogFile(filePath);
            }

            var stream = MikuSharedFileStreamManager.Instance.GetOrCreateStream(filePath);
            _currentStream = stream;

            foreach (var message in messages)
            {
                await stream.WriteAsync(message);
            }
        }

        /// <summary>
        /// Gets the current log file path based on options and date folders.
        /// </summary>
        /// <returns>The full path to the current log file.</returns>
        private string GetCurrentLogFilePath()
        {
            var baseDir = _options.LogDirectory;

            if (_options.UseDateFolders)
            {
                var dateFolder = DateTime.Now.ToString(_options.DateFolderFormat);
                baseDir = Path.Combine(baseDir, dateFolder);
            }

            return Path.Combine(baseDir, _options.FileNamePattern);
        }

        /// <summary>
        /// Determines if the log file should be rotated based on size.
        /// </summary>
        /// <param name="filePath">The path to the log file.</param>
        /// <returns>True if the file should be rotated; otherwise, false.</returns>
        private bool ShouldRotate(string filePath)
        {
            if (_options.MaxFileSizeBytes <= 0) return false;
            if (!File.Exists(filePath)) return false;

            var fileInfo = new FileInfo(filePath);
            return fileInfo.Length >= _options.MaxFileSizeBytes;
        }

        /// <summary>
        /// Rotates the log file by renaming it with a timestamp.
        /// </summary>
        /// <param name="filePath">The path to the log file to rotate.</param>
        private void RotateLogFile(string filePath)
        {
            if (!File.Exists(filePath)) return;

            try
            {
                MikuSharedFileStreamManager.Instance.RemoveStream(filePath);

                var directory = Path.GetDirectoryName(filePath)!;
                var fileName = Path.GetFileNameWithoutExtension(filePath);
                var extension = Path.GetExtension(filePath);
                var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                var rotatedFileName = $"{fileName}_{timestamp}{extension}";
                var rotatedFilePath = Path.Combine(directory, rotatedFileName);

                File.Move(filePath, rotatedFilePath);

                CleanupOldLogFiles(directory, fileName, extension);
            }
            catch
            {
                // Ignore rotation errors
            }
        }

        /// <summary>
        /// Cleans up old rotated log files based on MaxFileCount setting.
        /// </summary>
        /// <param name="directory">The directory containing log files.</param>
        /// <param name="fileName">The base file name.</param>
        /// <param name="extension">The file extension.</param>
        private void CleanupOldLogFiles(string directory, string fileName, string extension)
        {
            if (_options.MaxFileCount <= 0) return;

            try
            {
                var pattern = $"{fileName}_*{extension}";
                var files = Directory.GetFiles(directory, pattern)
                                    .Select(f => new FileInfo(f))
                                    .OrderByDescending(f => f.CreationTime)
                                    .Skip(_options.MaxFileCount)
                                    .ToList();

                foreach (var file in files)
                {
                    try
                    {
                        file.Delete();
                    }
                    catch
                    {
                        // Ignore deletion errors
                    }
                }
            }
            catch
            {
                // Ignore cleanup errors
            }
        }

        /// <summary>
        /// Ensures the specified directory exists, creating it if necessary.
        /// </summary>
        /// <param name="directory">The directory path to ensure exists.</param>
        private static void EnsureDirectoryExists(string directory)
        {
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }

        /// <summary>
        /// Disposes the file writer and ensures all queued messages are written.
        /// </summary>
        public void Dispose()
        {
            if (_disposed) return;

            _disposed = true;
            _cancellationTokenSource.Cancel();

            try
            {
                if (!_writerTask.Wait(TimeSpan.FromSeconds(5)))
                {
                    // Writer task did not complete in time
                }
            }
            catch
            {
                // Ignore cancellation exceptions
            }

            // Flush remaining messages
            while (_writeQueue.TryDequeue(out var message))
            {
                try
                {
                    var filePath = GetCurrentLogFilePath();
                    EnsureDirectoryExists(Path.GetDirectoryName(filePath)!);

                    var stream = MikuSharedFileStreamManager.Instance.GetOrCreateStream(filePath);
                    stream.Write(message);
                }
                catch
                {
                    Console.WriteLine($"[MikuLogger] Failed to write message: {message}");
                }
            }

            _cancellationTokenSource.Dispose();
        }
    }
}

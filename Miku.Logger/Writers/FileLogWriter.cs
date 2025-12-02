using Miku.Logger.Configuration;
using System.Collections.Concurrent;

namespace Miku.Logger.Writers
{
    /// <summary>
    /// Thread-safe file writer with log rotation support.
    /// High-performance implementation with single-writer-multiple-reader pattern.
    /// </summary>
    internal class FileLogWriter : IDisposable
    {
        private readonly FileLoggerOptions _options;
        private readonly ConcurrentQueue<string> _writeQueue = new();
        private readonly CancellationTokenSource _cancellationTokenSource = new();
        private readonly Task _writerTask;
        private bool _disposed;

        public FileLogWriter(FileLoggerOptions options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _writerTask = Task.Run(ProcessQueueAsync);
        }

        /// <summary>
        /// Writes a log message asynchronously.
        /// </summary>
        public async Task WriteAsync(string message, CancellationToken cancellationToken = default)
        {
            if (_disposed) return;

            _writeQueue.Enqueue(message);
            await Task.CompletedTask;
        }

        /// <summary>
        /// Writes a log message synchronously.
        /// </summary>
        public void Write(string message)
        {
            if (_disposed) return;
            _writeQueue.Enqueue(message);
        }

        private async Task ProcessQueueAsync()
        {
            // Batch size for better performance
            const int batchSize = 100;
            var batch = new List<string>(batchSize);

            while (!_cancellationTokenSource.Token.IsCancellationRequested)
            {
                try
                {
                    // Collect messages in batch
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
                        // No messages - short sleep
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
                    await Task.Delay(100); // Back off on error
                }
            }
        }

        private async Task WriteBatchToFileAsync(List<string> messages)
        {
            var filePath = GetCurrentLogFilePath();
            EnsureDirectoryExists(Path.GetDirectoryName(filePath)!);

            if (ShouldRotate(filePath))
            {
                RotateLogFile(filePath);
            }

            // Use FileStream with FileShare.Read for high performance
            // Multiple FileLogWriter instances can write to same file safely
            using var fileStream = new FileStream(
                filePath,
                FileMode.Append,
                FileAccess.Write,
                FileShare.Read,
                bufferSize: 8192,
                useAsync: true);

            using var writer = new StreamWriter(fileStream, System.Text.Encoding.UTF8, bufferSize: 8192);
            
            foreach (var message in messages)
            {
                await writer.WriteLineAsync(message);
            }
            
            await writer.FlushAsync();
        }

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

        private bool ShouldRotate(string filePath)
        {
            if (_options.MaxFileSizeBytes <= 0) return false;
            if (!File.Exists(filePath)) return false;

            var fileInfo = new FileInfo(filePath);
            return fileInfo.Length >= _options.MaxFileSizeBytes;
        }

        private void RotateLogFile(string filePath)
        {
            if (!File.Exists(filePath)) return;

            try
            {
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
                // If rotation fails, continue logging to current file
            }
        }

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

        private static void EnsureDirectoryExists(string directory)
        {
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }

        public void Dispose()
        {
            if (_disposed) return;

            _disposed = true;
            _cancellationTokenSource.Cancel();

            try
            {
                // Give background task time to finish current batch
                if (!_writerTask.Wait(TimeSpan.FromSeconds(5)))
                {
                    // Timeout - force flush remaining messages
                }
            }
            catch
            {
                // Ignore wait errors
            }

            // Flush all remaining messages in batches for performance
            var remainingMessages = new List<string>();
            while (_writeQueue.TryDequeue(out var message))
            {
                remainingMessages.Add(message);
            }

            if (remainingMessages.Count > 0)
            {
                try
                {
                    var filePath = GetCurrentLogFilePath();
                    EnsureDirectoryExists(Path.GetDirectoryName(filePath)!);

                    // Synchronous batch write for remaining messages
                    using var fileStream = new FileStream(
                        filePath,
                        FileMode.Append,
                        FileAccess.Write,
                        FileShare.Read,
                        bufferSize: 8192,
                        useAsync: false);

                    using var writer = new StreamWriter(fileStream, System.Text.Encoding.UTF8, bufferSize: 8192);
                    
                    foreach (var msg in remainingMessages)
                    {
                        writer.WriteLine(msg);
                    }
                    
                    writer.Flush();
                }
                catch
                {
                    // Log to console as last resort
                    Console.WriteLine($"[MikuLogger] Failed to write {remainingMessages.Count} remaining messages");
                }
            }

            _cancellationTokenSource.Dispose();
        }
    }
}

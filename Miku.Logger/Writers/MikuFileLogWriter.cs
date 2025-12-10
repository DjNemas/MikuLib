using Miku.Logger.Configuration.Models;
using System.Collections.Concurrent;

namespace Miku.Logger.Writers
{
    /// <summary>
    /// Thread-safe file writer with log rotation support.
    /// High-performance implementation using singleton shared file streams.
    /// </summary>
    internal class MikuFileLogWriter : IDisposable
    {
        private readonly MikuFileLoggerOptions _options;
        private readonly ConcurrentQueue<string> _writeQueue = new();
        private readonly CancellationTokenSource _cancellationTokenSource = new();
        private readonly Task _writerTask;
        private bool _disposed;
        private MikuSharedFileStream? _currentStream;

        public MikuFileLogWriter(MikuFileLoggerOptions options)
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
                    }
                }
            }
            catch
            {
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
                if (!_writerTask.Wait(TimeSpan.FromSeconds(5)))
                {
                }
            }
            catch
            {
            }

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

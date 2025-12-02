using Miku.Logger.Configuration;
using System.Collections.Concurrent;

namespace Miku.Logger.Writers
{
    /// <summary>
    /// Thread-safe file writer with log rotation support.
    /// </summary>
    internal class FileLogWriter : IDisposable
    {
        private readonly FileLoggerOptions _options;
        private readonly SemaphoreSlim _writeSemaphore = new(1, 1);
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
            while (!_cancellationTokenSource.Token.IsCancellationRequested)
            {
                try
                {
                    if (_writeQueue.TryDequeue(out var message))
                    {
                        await WriteToFileAsync(message);
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
                }
            }
        }

        private async Task WriteToFileAsync(string message)
        {
            await _writeSemaphore.WaitAsync();
            try
            {
                var filePath = GetCurrentLogFilePath();
                EnsureDirectoryExists(Path.GetDirectoryName(filePath)!);

                if (ShouldRotate(filePath))
                {
                    RotateLogFile(filePath);
                }

                // Use named mutex for cross-process synchronization
                var mutexName = $"Global\\MikuLogger_{GetMutexNameFromPath(filePath)}";
                using var mutex = new Mutex(false, mutexName);
                
                bool mutexAcquired = false;
                try
                {
                    // Handle abandoned mutex gracefully
                    mutexAcquired = mutex.WaitOne(TimeSpan.FromSeconds(30));
                    if (!mutexAcquired)
                    {
                        Console.WriteLine("[MikuLogger] Warning: Could not acquire mutex within timeout");
                        return;
                    }
                    
                    // Use FileStream with FileShare.Read to allow concurrent read access
                    using var fileStream = new FileStream(
                        filePath,
                        FileMode.Append,
                        FileAccess.Write,
                        FileShare.Read,
                        bufferSize: 4096,
                        useAsync: true);
                    
                    using var writer = new StreamWriter(fileStream, System.Text.Encoding.UTF8);
                    await writer.WriteLineAsync(message);
                    await writer.FlushAsync();
                }
                catch (AbandonedMutexException)
                {
                    // Mutex was abandoned - we now own it, continue writing
                    mutexAcquired = true;
                    
                    using var fileStream = new FileStream(
                        filePath,
                        FileMode.Append,
                        FileAccess.Write,
                        FileShare.Read,
                        bufferSize: 4096,
                        useAsync: true);
                    
                    using var writer = new StreamWriter(fileStream, System.Text.Encoding.UTF8);
                    await writer.WriteLineAsync(message);
                    await writer.FlushAsync();
                }
                finally
                {
                    if (mutexAcquired)
                    {
                        try
                        {
                            mutex.ReleaseMutex();
                        }
                        catch (ApplicationException)
                        {
                            // Mutex was already released or not owned
                        }
                    }
                }
            }
            finally
            {
                _writeSemaphore.Release();
            }
        }

        private static string GetMutexNameFromPath(string filePath)
        {
            // Create a stable mutex name from file path
            // Use SHA256 to avoid issues with path length and special characters
            using var sha = System.Security.Cryptography.SHA256.Create();
            var hash = sha.ComputeHash(System.Text.Encoding.UTF8.GetBytes(filePath.ToLowerInvariant()));
            return Convert.ToHexString(hash);
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

            var directory = Path.GetDirectoryName(filePath)!;
            var fileName = Path.GetFileNameWithoutExtension(filePath);
            var extension = Path.GetExtension(filePath);
            var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            var rotatedFileName = $"{fileName}_{timestamp}{extension}";
            var rotatedFilePath = Path.Combine(directory, rotatedFileName);

            File.Move(filePath, rotatedFilePath);

            CleanupOldLogFiles(directory, fileName, extension);
        }

        private void CleanupOldLogFiles(string directory, string fileName, string extension)
        {
            if (_options.MaxFileCount <= 0) return;

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
                // Wait longer for background task to finish processing
                _writerTask.Wait(TimeSpan.FromSeconds(30));
            }
            catch
            {
                // Timeout or cancellation - continue to flush remaining messages
            }

            // Write ALL remaining messages synchronously - no data loss allowed!
            int remainingMessages = _writeQueue.Count;
            if (remainingMessages > 0)
            {
                Console.WriteLine($"[MikuLogger] Flushing {remainingMessages} remaining messages...");
            }

            while (_writeQueue.TryDequeue(out var message))
            {
                try
                {
                    var filePath = GetCurrentLogFilePath();
                    EnsureDirectoryExists(Path.GetDirectoryName(filePath)!);
                    
                    // Use named mutex for cross-process synchronization
                    var mutexName = $"Global\\MikuLogger_{GetMutexNameFromPath(filePath)}";
                    using var mutex = new Mutex(false, mutexName);
                    
                    try
                    {
                        mutex.WaitOne();
                        
                        // Synchronous write in Dispose
                        using var fileStream = new FileStream(
                            filePath,
                            FileMode.Append,
                            FileAccess.Write,
                            FileShare.Read,
                            bufferSize: 4096,
                            useAsync: false);
                        
                        using var writer = new StreamWriter(fileStream, System.Text.Encoding.UTF8);
                        writer.WriteLine(message);
                        writer.Flush();
                    }
                    finally
                    {
                        mutex.ReleaseMutex();
                    }
                }
                catch (Exception ex)
                {
                    // Log to console but don't lose the message
                    Console.WriteLine($"[MikuLogger] Error writing remaining message during dispose: {ex.Message}");
                }
            }

            _writeSemaphore.Dispose();
            _cancellationTokenSource.Dispose();
        }
    }
}

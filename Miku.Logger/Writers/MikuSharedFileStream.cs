namespace Miku.Logger.Writers
{
    /// <summary>
    /// Thread-safe wrapper around FileStream with reference counting.
    /// </summary>
    /// <remarks>
    /// Like the digital voice carrier for Miku's songs,
    /// this stream safely transports your log messages to storage!
    /// </remarks>
    internal sealed class MikuSharedFileStream : IDisposable
    {
        private readonly string _filePath;
        private readonly SemaphoreSlim _writeLock = new(1, 1);
        private FileStream? _fileStream;
        private int _refCount;
        private bool _disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="MikuSharedFileStream"/> class.
        /// </summary>
        /// <param name="filePath">The path to the log file.</param>
        public MikuSharedFileStream(string filePath)
        {
            _filePath = filePath;
        }

        /// <summary>
        /// Asynchronously writes a message to the file.
        /// </summary>
        /// <param name="message">The message to write.</param>
        public async Task WriteAsync(string message)
        {
            await _writeLock.WaitAsync();
            try
            {
                if (_disposed) return;

                _fileStream ??= new FileStream(
                    _filePath,
                    FileMode.Append,
                    FileAccess.Write,
                    FileShare.ReadWrite,
                    bufferSize: 8192,
                    useAsync: true);

                var bytes = System.Text.Encoding.UTF8.GetBytes(message + Environment.NewLine);
                await _fileStream.WriteAsync(bytes);
                await _fileStream.FlushAsync();
            }
            finally
            {
                _writeLock.Release();
            }
        }

        /// <summary>
        /// Synchronously writes a message to the file.
        /// </summary>
        /// <param name="message">The message to write.</param>
        public void Write(string message)
        {
            _writeLock.Wait();
            try
            {
                if (_disposed) return;

                _fileStream ??= new FileStream(
                    _filePath,
                    FileMode.Append,
                    FileAccess.Write,
                    FileShare.ReadWrite,
                    bufferSize: 8192,
                    useAsync: false);

                var bytes = System.Text.Encoding.UTF8.GetBytes(message + Environment.NewLine);
                _fileStream.Write(bytes);
                _fileStream.Flush();
            }
            finally
            {
                _writeLock.Release();
            }
        }

        /// <summary>
        /// Increments the reference count for this stream.
        /// </summary>
        public void IncrementRef()
        {
            Interlocked.Increment(ref _refCount);
        }

        /// <summary>
        /// Decrements the reference count and disposes if no more references exist.
        /// </summary>
        public void DecrementRef()
        {
            if (Interlocked.Decrement(ref _refCount) <= 0)
            {
                Dispose();
            }
        }

        /// <summary>
        /// Disposes the file stream and releases all resources.
        /// </summary>
        public void Dispose()
        {
            if (_disposed) return;

            _writeLock.Wait();
            try
            {
                _disposed = true;
                _fileStream?.Dispose();
                _fileStream = null;
            }
            finally
            {
                _writeLock.Release();
                _writeLock.Dispose();
            }
        }
    }
}

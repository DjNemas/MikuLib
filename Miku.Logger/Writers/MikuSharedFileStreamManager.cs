using System.Collections.Concurrent;

namespace Miku.Logger.Writers
{
    /// <summary>
    /// Thread-safe singleton manager for shared file streams.
    /// Ensures only one FileStream per file path across all MikuFileLogWriter instances.
    /// </summary>
    internal sealed class MikuSharedFileStreamManager
    {
        private static readonly Lazy<MikuSharedFileStreamManager> _instance =
            new(() => new MikuSharedFileStreamManager());

        public static MikuSharedFileStreamManager Instance => _instance.Value;

        private readonly ConcurrentDictionary<string, MikuSharedFileStream> _streams = new();

        private MikuSharedFileStreamManager() { }

        public MikuSharedFileStream GetOrCreateStream(string filePath)
        {
            return _streams.GetOrAdd(filePath, path => new MikuSharedFileStream(path));
        }

        public void RemoveStream(string filePath)
        {
            if (_streams.TryRemove(filePath, out var stream))
            {
                stream.Dispose();
            }
        }

        public void DisposeAll()
        {
            foreach (var stream in _streams.Values)
            {
                stream.Dispose();
            }
            _streams.Clear();
        }
    }

    /// <summary>
    /// Thread-safe wrapper around FileStream with reference counting.
    /// </summary>
    internal sealed class MikuSharedFileStream : IDisposable
    {
        private readonly string _filePath;
        private readonly SemaphoreSlim _writeLock = new(1, 1);
        private FileStream? _fileStream;
        private int _refCount;
        private bool _disposed;

        public MikuSharedFileStream(string filePath)
        {
            _filePath = filePath;
        }

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

        public void IncrementRef()
        {
            Interlocked.Increment(ref _refCount);
        }

        public void DecrementRef()
        {
            if (Interlocked.Decrement(ref _refCount) <= 0)
            {
                Dispose();
            }
        }

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

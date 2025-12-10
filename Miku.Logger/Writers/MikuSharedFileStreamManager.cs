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
}

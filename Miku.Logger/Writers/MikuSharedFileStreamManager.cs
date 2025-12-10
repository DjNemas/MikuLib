using System.Collections.Concurrent;

namespace Miku.Logger.Writers
{
    /// <summary>
    /// Thread-safe singleton manager for shared file streams.
    /// Ensures only one FileStream per file path across all MikuFileLogWriter instances.
    /// </summary>
    /// <remarks>
    /// Like the sound engineer managing Miku's audio channels,
    /// this manager coordinates all file stream access!
    /// </remarks>
    internal sealed class MikuSharedFileStreamManager
    {
        private static readonly Lazy<MikuSharedFileStreamManager> _instance =
            new(() => new MikuSharedFileStreamManager());

        /// <summary>
        /// Gets the singleton instance of the <see cref="MikuSharedFileStreamManager"/>.
        /// </summary>
        public static MikuSharedFileStreamManager Instance => _instance.Value;

        private readonly ConcurrentDictionary<string, MikuSharedFileStream> _streams = new();

        private MikuSharedFileStreamManager() { }

        /// <summary>
        /// Gets an existing shared file stream or creates a new one for the specified file path.
        /// </summary>
        /// <param name="filePath">The path to the log file.</param>
        /// <returns>A shared file stream for the specified path.</returns>
        public MikuSharedFileStream GetOrCreateStream(string filePath)
        {
            return _streams.GetOrAdd(filePath, path => new MikuSharedFileStream(path));
        }

        /// <summary>
        /// Removes and disposes the shared file stream for the specified file path.
        /// </summary>
        /// <param name="filePath">The path to the log file.</param>
        public void RemoveStream(string filePath)
        {
            if (_streams.TryRemove(filePath, out var stream))
            {
                stream.Dispose();
            }
        }

        /// <summary>
        /// Disposes all managed file streams and clears the collection.
        /// </summary>
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

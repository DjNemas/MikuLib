using Miku.Logger.Configuration;
using Miku.Logger.Writers;
using System.Reflection;

namespace Miku.Logger.Tests;

public class FileLogWriterTests : IDisposable
{
    private readonly string _testDirectory;
    private readonly List<FileLogWriter> _writers = new();

    public FileLogWriterTests()
    {
        _testDirectory = Path.Combine(Path.GetTempPath(), $"MikuLoggerTests_{Guid.NewGuid()}");
        Directory.CreateDirectory(_testDirectory);
    }

    public void Dispose()
    {
        foreach (var writer in _writers)
        {
            writer.Dispose();
        }
        _writers.Clear();

        if (Directory.Exists(_testDirectory))
        {
            try
            {
                Directory.Delete(_testDirectory, true);
            }
            catch
            {
                // Ignore cleanup errors
            }
        }
    }

    private FileLogWriter CreateWriter(string? fileName = null)
    {
        var options = new FileLoggerOptions
        {
            LogDirectory = _testDirectory,
            FileNamePattern = fileName ?? "test.log",
            MaxFileSizeBytes = 1024 * 1024, // 1 MB
            UseDateFolders = false
        };

        var writer = new FileLogWriter(options);
        _writers.Add(writer);
        return writer;
    }

    [Fact]
    public async Task SingleWriter_WritesSingleMessage()
    {
        // Arrange
        var writer = CreateWriter();
        var message = "Test message";

        // Act
        await writer.WriteAsync(message);
        await Task.Delay(100); // Wait for background processing

        // Assert
        var logFile = Path.Combine(_testDirectory, "test.log");
        Assert.True(File.Exists(logFile));
        var content = await File.ReadAllTextAsync(logFile);
        Assert.Contains(message, content);
    }

    [Fact]
    public async Task SingleWriter_WritesMultipleMessages()
    {
        // Arrange
        var writer = CreateWriter();
        var messages = Enumerable.Range(1, 100).Select(i => $"Message {i}").ToList();

        // Act
        foreach (var message in messages)
        {
            await writer.WriteAsync(message);
        }
        writer.Dispose(); // Ensure all messages are flushed

        // Assert
        var logFile = Path.Combine(_testDirectory, "test.log");
        var lines = await File.ReadAllLinesAsync(logFile);
        
        Assert.Equal(messages.Count, lines.Length);
        for (int i = 0; i < messages.Count; i++)
        {
            Assert.Equal(messages[i], lines[i]);
        }
    }

    [Fact]
    public async Task MultipleWriters_SameFile_AllMessagesWritten()
    {
        // Arrange
        const int writerCount = 5;
        const int messagesPerWriter = 100;
        var writers = new List<FileLogWriter>();
        
        for (int i = 0; i < writerCount; i++)
        {
            writers.Add(CreateWriter("shared.log"));
        }

        // Act
        var tasks = new List<Task>();
        for (int w = 0; w < writerCount; w++)
        {
            var writerId = w;
            var task = Task.Run(async () =>
            {
                for (int m = 0; m < messagesPerWriter; m++)
                {
                    await writers[writerId].WriteAsync($"Writer{writerId}_Message{m}");
                }
            });
            tasks.Add(task);
        }

        await Task.WhenAll(tasks);
        
        // Dispose all writers to ensure all messages are flushed
        foreach (var writer in writers)
        {
            writer.Dispose();
        }

        // Assert
        var logFile = Path.Combine(_testDirectory, "shared.log");
        Assert.True(File.Exists(logFile));
        
        var lines = await File.ReadAllLinesAsync(logFile);
        Assert.Equal(writerCount * messagesPerWriter, lines.Length);

        // Verify all messages from all writers are present
        for (int w = 0; w < writerCount; w++)
        {
            for (int m = 0; m < messagesPerWriter; m++)
            {
                var expectedMessage = $"Writer{w}_Message{m}";
                Assert.Contains(lines, line => line == expectedMessage);
            }
        }
    }

    [Fact]
    public async Task MultipleWriters_ConcurrentWrite_NoIOException()
    {
        // Arrange
        const int writerCount = 10;
        const int messagesPerWriter = 50;
        var writers = new List<FileLogWriter>();
        var exceptions = new List<Exception>();
        
        for (int i = 0; i < writerCount; i++)
        {
            writers.Add(CreateWriter("concurrent.log"));
        }

        // Act
        var tasks = new List<Task>();
        for (int w = 0; w < writerCount; w++)
        {
            var writerId = w;
            var task = Task.Run(async () =>
            {
                try
                {
                    for (int m = 0; m < messagesPerWriter; m++)
                    {
                        await writers[writerId].WriteAsync($"Writer{writerId}_Msg{m}");
                        // Simulate realistic timing
                        await Task.Delay(Random.Shared.Next(1, 5));
                    }
                }
                catch (Exception ex)
                {
                    lock (exceptions)
                    {
                        exceptions.Add(ex);
                    }
                }
            });
            tasks.Add(task);
        }

        await Task.WhenAll(tasks);
        
        // Dispose all writers to ensure all messages are flushed
        foreach (var writer in writers)
        {
            writer.Dispose();
        }

        // Assert
        Assert.Empty(exceptions); // No IOException should occur
        
        var logFile = Path.Combine(_testDirectory, "concurrent.log");
        var lines = await File.ReadAllLinesAsync(logFile);
        Assert.Equal(writerCount * messagesPerWriter, lines.Length);
    }

    [Fact]
    public async Task Dispose_WritesRemainingMessages()
    {
        // Arrange
        var writer = CreateWriter("dispose-test.log");
        var messages = Enumerable.Range(1, 50).Select(i => $"Message {i}").ToList();

        // Act - Write messages and immediately dispose
        foreach (var message in messages)
        {
            writer.Write(message);
        }
        writer.Dispose(); // Should flush all remaining messages

        // Assert
        var logFile = Path.Combine(_testDirectory, "dispose-test.log");
        var lines = await File.ReadAllLinesAsync(logFile);
        Assert.Equal(messages.Count, lines.Length);
    }

    [Fact]
    public async Task MultipleWriters_SimultaneousDispose_AllMessagesWritten()
    {
        // Arrange
        const int writerCount = 5;
        const int messagesPerWriter = 30;
        var localWriters = new List<FileLogWriter>();
        
        for (int i = 0; i < writerCount; i++)
        {
            var options = new FileLoggerOptions
            {
                LogDirectory = _testDirectory,
                FileNamePattern = "dispose-concurrent.log",
                MaxFileSizeBytes = 1024 * 1024,
                UseDateFolders = false
            };
            localWriters.Add(new FileLogWriter(options));
        }

        // Act - Write and dispose simultaneously
        var tasks = new List<Task>();
        for (int w = 0; w < writerCount; w++)
        {
            var writerId = w;
            var task = Task.Run(() =>
            {
                for (int m = 0; m < messagesPerWriter; m++)
                {
                    localWriters[writerId].Write($"Writer{writerId}_Msg{m}");
                }
                localWriters[writerId].Dispose();
            });
            tasks.Add(task);
        }

        await Task.WhenAll(tasks);
        
        // Additional wait to ensure file operations are complete
        await Task.Delay(100);

        // Assert
        var logFile = Path.Combine(_testDirectory, "dispose-concurrent.log");
        var lines = await File.ReadAllLinesAsync(logFile);
        Assert.Equal(writerCount * messagesPerWriter, lines.Length);
    }

    [Fact]
    public async Task HighLoad_ManyWritersAndMessages_NoDataLoss()
    {
        // Arrange
        const int writerCount = 20;
        const int messagesPerWriter = 200;
        var writers = new List<FileLogWriter>();
        
        for (int i = 0; i < writerCount; i++)
        {
            writers.Add(CreateWriter("high-load.log"));
        }

        // Act
        var tasks = new List<Task>();
        for (int w = 0; w < writerCount; w++)
        {
            var writerId = w;
            var task = Task.Run(async () =>
            {
                for (int m = 0; m < messagesPerWriter; m++)
                {
                    await writers[writerId].WriteAsync($"W{writerId:D2}_M{m:D4}");
                }
            });
            tasks.Add(task);
        }

        await Task.WhenAll(tasks);
        
        // Dispose all writers to ensure all messages are flushed
        foreach (var writer in writers)
        {
            writer.Dispose();
        }

        // Assert
        var logFile = Path.Combine(_testDirectory, "high-load.log");
        var lines = await File.ReadAllLinesAsync(logFile);
        
        // All messages should be written
        Assert.Equal(writerCount * messagesPerWriter, lines.Length);
        
        // Verify message integrity
        var expectedMessages = new HashSet<string>();
        for (int w = 0; w < writerCount; w++)
        {
            for (int m = 0; m < messagesPerWriter; m++)
            {
                expectedMessages.Add($"W{w:D2}_M{m:D4}");
            }
        }
        
        var actualMessages = new HashSet<string>(lines);
        Assert.Equal(expectedMessages, actualMessages);
    }

    [Fact]
    public async Task MultipleWriters_WithDateFolders_NoConflict()
    {
        // Arrange
        var options = new FileLoggerOptions
        {
            LogDirectory = _testDirectory,
            FileNamePattern = "dated.log",
            UseDateFolders = true,
            DateFolderFormat = "yyyy-MM-dd"
        };

        var writers = new List<FileLogWriter>
        {
            new FileLogWriter(options),
            new FileLogWriter(options),
            new FileLogWriter(options)
        };
        _writers.AddRange(writers);

        // Act
        var tasks = writers.Select((w, i) => Task.Run(async () =>
        {
            for (int m = 0; m < 50; m++)
            {
                await w.WriteAsync($"Writer{i}_Message{m}");
            }
        })).ToArray();

        await Task.WhenAll(tasks);
        
        // Dispose all writers to ensure all messages are flushed
        foreach (var writer in writers)
        {
            writer.Dispose();
        }

        // Assert
        var dateFolder = DateTime.Now.ToString("yyyy-MM-dd");
        var logFile = Path.Combine(_testDirectory, dateFolder, "dated.log");
        Assert.True(File.Exists(logFile));
        
        var lines = await File.ReadAllLinesAsync(logFile);
        Assert.Equal(150, lines.Length); // 3 writers * 50 messages
    }

    [Fact]
    public async Task StressTest_RapidWritesFromMultipleThreads()
    {
        // Arrange
        const int threadCount = 50;
        const int messagesPerThread = 100;
        var writer = CreateWriter("stress-test.log");

        // Act
        var tasks = Enumerable.Range(0, threadCount).Select(t => Task.Run(async () =>
        {
            for (int m = 0; m < messagesPerThread; m++)
            {
                await writer.WriteAsync($"T{t:D3}_M{m:D3}");
            }
        })).ToArray();

        await Task.WhenAll(tasks);
        writer.Dispose();

        // Assert
        var logFile = Path.Combine(_testDirectory, "stress-test.log");
        var lines = await File.ReadAllLinesAsync(logFile);
        Assert.Equal(threadCount * messagesPerThread, lines.Length);
        
        // Verify no duplicates
        var uniqueLines = new HashSet<string>(lines);
        Assert.Equal(lines.Length, uniqueLines.Count);
    }
}

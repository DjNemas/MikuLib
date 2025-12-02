using Miku.Logger.Configuration;
using Miku.Logger.Writers;

namespace Miku.Logger.Tests;

public class MultiProcessFileLogWriterTests : IDisposable
{
    private readonly string _testDirectory;

    public MultiProcessFileLogWriterTests()
    {
        _testDirectory = Path.Combine(Path.GetTempPath(), $"MikuLoggerMultiProcess_{Guid.NewGuid()}");
        Directory.CreateDirectory(_testDirectory);
    }

    public void Dispose()
    {
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

    [Fact]
    public async Task MultipleProcesses_SameFile_NoIOException()
    {
        // This test simulates the real Gateway scenario:
        // Multiple independent FileLogWriter instances (like different processes)
        // writing to the same file simultaneously

        const int processCount = 5;
        const int messagesPerProcess = 100;
        var logFile = Path.Combine(_testDirectory, "multi-process.log");
        var exceptions = new List<Exception>();

        // Simulate multiple processes by creating completely independent writers
        var tasks = Enumerable.Range(0, processCount).Select(processId => Task.Run(async () =>
        {
            try
            {
                // Each "process" creates its own FileLogWriter instance
                var options = new FileLoggerOptions
                {
                    LogDirectory = _testDirectory,
                    FileNamePattern = "multi-process.log",
                    MaxFileSizeBytes = 1024 * 1024,
                    UseDateFolders = false
                };

                using var writer = new FileLogWriter(options);

                // Simulate rapid concurrent writes
                for (int m = 0; m < messagesPerProcess; m++)
                {
                    await writer.WriteAsync($"Process{processId}_Message{m}");

                    // Add small random delay to increase chance of collision
                    if (m % 10 == 0)
                    {
                        await Task.Delay(Random.Shared.Next(1, 5));
                    }
                }

                // Explicit dispose to flush
                writer.Dispose();
            }
            catch (Exception ex)
            {
                lock (exceptions)
                {
                    exceptions.Add(ex);
                }
            }
        })).ToArray();

        await Task.WhenAll(tasks);

        // Wait a bit for streams to flush
        await Task.Delay(500);

        // Cleanup shared streams
        SharedFileStreamManager.Instance.DisposeAll();

        // Assert
        Assert.Empty(exceptions); // No IOException should occur

        if (File.Exists(logFile))
        {
            var lines = await File.ReadAllLinesAsync(logFile);

            // All messages should be present - zero data loss
            Assert.Equal(processCount * messagesPerProcess, lines.Length);
        }
    }

    [Fact]
    public async Task SimulateGatewayScenario_ThreeServices_SameLogFile()
    {
        // Simulates Gateway, Homepage, RestAPI all logging to same file
        const int messagesPerService = 200;
        var logFile = Path.Combine(_testDirectory, "gateway-scenario.log");
        var exceptions = new List<Exception>();

        var serviceNames = new[] { "Gateway", "Homepage", "RestAPI" };

        var tasks = serviceNames.Select(serviceName => Task.Run(async () =>
        {
            try
            {
                var options = new FileLoggerOptions
                {
                    LogDirectory = _testDirectory,
                    FileNamePattern = "gateway-scenario.log",
                    MaxFileSizeBytes = 10 * 1024 * 1024,
                    UseDateFolders = false
                };

                using var writer = new FileLogWriter(options);

                for (int i = 0; i < messagesPerService; i++)
                {
                    await writer.WriteAsync($"[{serviceName}] Log message {i}");

                    // Simulate realistic workload
                    if (i % 20 == 0)
                    {
                        await Task.Delay(Random.Shared.Next(5, 15));
                    }
                }

                writer.Dispose();
            }
            catch (Exception ex)
            {
                lock (exceptions)
                {
                    exceptions.Add(ex);
                }
            }
        })).ToArray();

        await Task.WhenAll(tasks);

        // Wait for all streams to flush
        await Task.Delay(500);

        // Cleanup shared streams
        SharedFileStreamManager.Instance.DisposeAll();

        // Assert
        Assert.Empty(exceptions); // No IOException

        if (File.Exists(logFile))
        {
            var lines = await File.ReadAllLinesAsync(logFile);

            // All 600 messages should be present - zero data loss
            Assert.Equal(serviceNames.Length * messagesPerService, lines.Length);

            // Verify each service's messages are present
            foreach (var serviceName in serviceNames)
            {
                var serviceMessages = lines.Count(l => l.Contains($"[{serviceName}]"));
                Assert.Equal(messagesPerService, serviceMessages);
            }
        }
    }
}

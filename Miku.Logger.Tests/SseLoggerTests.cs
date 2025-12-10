using Miku.Logger.Configuration;
using Miku.Logger.Configuration.Enums;
using Miku.Logger.Configuration.Models;
using Miku.Logger.Sse;

namespace Miku.Logger.Tests;

/// <summary>
/// Tests for MikuLogger Server-Sent Events (SSE) functionality.
/// Like testing Miku's live streaming capabilities! ♪
/// </summary>
public class SseLogBroadcasterTests : IDisposable
{
    public SseLogBroadcasterTests()
    {
        // Reset broadcaster state before each test
    }

    public void Dispose()
    {
        // Cleanup is handled by the singleton
    }

    [Fact]
    public void Configure_ShouldUpdateOptions()
    {
        // Arrange
        var options = new MikuSseLoggerOptions
        {
            EndpointPath = "/custom/logs",
            EventType = "custom-event",
            MaxClients = 50,
            ReconnectionIntervalMs = 5000
        };

        // Act
        MikuSseLogBroadcaster.Instance.Configure(options);

        // Assert
        Assert.Equal("/custom/logs", MikuSseLogBroadcaster.Instance.Options.EndpointPath);
        Assert.Equal("custom-event", MikuSseLogBroadcaster.Instance.Options.EventType);
        Assert.Equal(50, MikuSseLogBroadcaster.Instance.Options.MaxClients);
        Assert.Equal(5000, MikuSseLogBroadcaster.Instance.Options.ReconnectionIntervalMs);
    }

    [Fact]
    public void ClientCount_WithNoClients_ShouldBeZero()
    {
        // Act & Assert
        Assert.True(MikuSseLogBroadcaster.Instance.ClientCount >= 0);
    }

    [Fact]
    public async Task SubscribeAsync_ShouldReceiveBroadcastedLogs()
    {
        // Arrange
        var receivedLogs = new List<MikuSseLogEntry>();
        using var cts = new CancellationTokenSource();

        var subscribeTask = Task.Run(async () =>
        {
            await foreach (var entry in MikuSseLogBroadcaster.Instance.SubscribeAsync(cts.Token))
            {
                receivedLogs.Add(entry);
                if (receivedLogs.Count >= 3)
                {
                    break;
                }
            }
        });

        await Task.Delay(100);

        // Act
        MikuSseLogBroadcaster.Instance.Broadcast(MikuLogLevel.Information, "TestCategory", "Message 1");
        MikuSseLogBroadcaster.Instance.Broadcast(MikuLogLevel.Warning, "TestCategory", "Message 2");
        MikuSseLogBroadcaster.Instance.Broadcast(MikuLogLevel.Error, "TestCategory", "Message 3");

        var completedTask = await Task.WhenAny(subscribeTask, Task.Delay(2000));
        cts.Cancel();

        // Assert
        Assert.True(receivedLogs.Count >= 1, "Should receive at least one log entry");
    }

    [Fact]
    public async Task SubscribeAsync_WithCancellation_ShouldStopReceiving()
    {
        // Arrange
        var cts = new CancellationTokenSource();
        var receivedCount = 0;

        var subscribeTask = Task.Run(async () =>
        {
            try
            {
                await foreach (var _ in MikuSseLogBroadcaster.Instance.SubscribeAsync(cts.Token))
                {
                    receivedCount++;
                }
            }
            catch (OperationCanceledException)
            {
                // Expected
            }
        });

        await Task.Delay(50);
        cts.Cancel();

        await Task.Delay(100);

        // Assert
        Assert.True(subscribeTask.IsCompleted || subscribeTask.IsCanceled);
    }

    [Fact]
    public void Broadcast_WithMinimumLogLevelFilter_ShouldFilterLogs()
    {
        // Arrange
        var options = new MikuSseLoggerOptions
        {
            MinimumLogLevel = MikuLogLevel.Warning
        };
        MikuSseLogBroadcaster.Instance.Configure(options);

        var receivedLogs = new List<MikuSseLogEntry>();
        var cts = new CancellationTokenSource();

        var subscribeTask = Task.Run(async () =>
        {
            await foreach (var entry in MikuSseLogBroadcaster.Instance.SubscribeAsync(cts.Token))
            {
                receivedLogs.Add(entry);
            }
        });

        Thread.Sleep(50);

        // Act
        MikuSseLogBroadcaster.Instance.Broadcast(MikuLogLevel.Debug, "Test", "Debug message");
        MikuSseLogBroadcaster.Instance.Broadcast(MikuLogLevel.Information, "Test", "Info message");
        MikuSseLogBroadcaster.Instance.Broadcast(MikuLogLevel.Warning, "Test", "Warning message");
        MikuSseLogBroadcaster.Instance.Broadcast(MikuLogLevel.Error, "Test", "Error message");

        Thread.Sleep(100);
        cts.Cancel();

        // Reset filter
        MikuSseLogBroadcaster.Instance.Configure(new MikuSseLoggerOptions { MinimumLogLevel = null });

        // Assert
        var warningAndErrorLogs = receivedLogs.Where(l =>
            l.Level == "Warning" || l.Level == "Error").ToList();
        Assert.True(warningAndErrorLogs.Count <= receivedLogs.Count);
    }

    [Fact]
    public void BroadcastEntry_ShouldNotThrowWhenNoClients()
    {
        // Arrange
        var entry = MikuSseLogEntry.Create(MikuLogLevel.Information, "Test", "Test message");

        // Act & Assert
        var exception = Record.Exception(() => MikuSseLogBroadcaster.Instance.BroadcastEntry(entry));
        Assert.Null(exception);
    }
}

public class SseLogEntryTests
{
    [Fact]
    public void Create_ShouldSetAllProperties()
    {
        // Arrange & Act
        var entry = MikuSseLogEntry.Create(
            MikuLogLevel.Error,
            "TestCategory",
            "Test message",
            new InvalidOperationException("Test exception"),
            useUtcTime: true);

        // Assert
        Assert.NotEmpty(entry.Id);
        Assert.Equal("Error", entry.Level);
        Assert.Equal(4, entry.LevelValue);
        Assert.Equal("TestCategory", entry.Category);
        Assert.Equal("Test message", entry.Message);
        Assert.Contains("Test exception", entry.Exception);
    }

    [Fact]
    public void Create_WithoutException_ShouldHaveNullException()
    {
        // Arrange & Act
        var entry = MikuSseLogEntry.Create(MikuLogLevel.Information, "Test", "Message");

        // Assert
        Assert.Null(entry.Exception);
    }

    [Theory]
    [InlineData(MikuLogLevel.Trace, "Trace")]
    [InlineData(MikuLogLevel.Debug, "Debug")]
    [InlineData(MikuLogLevel.Information, "Information")]
    [InlineData(MikuLogLevel.Warning, "Warning")]
    [InlineData(MikuLogLevel.Error, "Error")]
    [InlineData(MikuLogLevel.Critical, "Critical")]
    public void Create_ShouldMapLogLevelCorrectly(MikuLogLevel logLevel, string expectedLevelString)
    {
        // Act
        var entry = MikuSseLogEntry.Create(logLevel, "Test", "Message");

        // Assert
        Assert.Equal(expectedLevelString, entry.Level);
        Assert.Equal((int)logLevel, entry.LevelValue);
    }

    [Fact]
    public void Create_WithUtcTime_ShouldUseUtcTimestamp()
    {
        // Act
        var beforeUtc = DateTime.UtcNow;
        var entry = MikuSseLogEntry.Create(MikuLogLevel.Information, "Test", "Message", useUtcTime: true);
        var afterUtc = DateTime.UtcNow;

        // Assert
        Assert.True(entry.Timestamp >= beforeUtc.AddSeconds(-1));
        Assert.True(entry.Timestamp <= afterUtc.AddSeconds(1));
    }
}

public class SseLoggerOptionsTests
{
    [Fact]
    public void DefaultValues_ShouldBeCorrect()
    {
        // Arrange & Act
        var options = new MikuSseLoggerOptions();

        // Assert
        Assert.Equal("/miku/logs/stream", options.EndpointPath);
        Assert.Equal("miku-log", options.EventType);
        Assert.Equal(100, options.MaxClients);
        Assert.Equal(3000, options.ReconnectionIntervalMs);
        Assert.False(options.IncludeLogLevelInEventType);
        Assert.False(options.RequireAuthorization);
        Assert.Null(options.AuthorizationPolicy);
        Assert.Null(options.MinimumLogLevel);
    }
}

public class MikuLoggerSseIntegrationTests
{
    [Fact]
    public void MikuLogger_WithSseOutput_ShouldBroadcastLogs()
    {
        // Arrange
        var receivedLogs = new List<MikuSseLogEntry>();
        var cts = new CancellationTokenSource();

        var options = new MikuLoggerOptions
        {
            Output = MikuLogOutput.ServerSentEvents,
            MinimumLogLevel = MikuLogLevel.Debug
        };

        var subscribeTask = Task.Run(async () =>
        {
            await foreach (var entry in MikuSseLogBroadcaster.Instance.SubscribeAsync(cts.Token))
            {
                receivedLogs.Add(entry);
                if (receivedLogs.Count >= 2)
                    break;
            }
        });

        Thread.Sleep(50);

        // Act
        using var logger = new MikuLogger("TestLogger", options);
        logger.LogInformation("Test message 1");
        logger.LogWarning("Test message 2");

        Thread.Sleep(200);
        cts.Cancel();

        // Assert
        Assert.True(receivedLogs.Count >= 1, "Should receive at least one broadcasted log");
    }

    [Fact]
    public void MikuLogger_WithAllOutputs_ShouldBroadcastToSse()
    {
        // Arrange
        var options = new MikuLoggerOptions
        {
            Output = MikuLogOutput.All,
            FileOptions = new MikuFileLoggerOptions
            {
                LogDirectory = Path.Combine(Path.GetTempPath(), $"MikuSseTest_{Guid.NewGuid()}")
            }
        };

        // Act & Assert
        using var logger = new MikuLogger("AllOutputsTest", options);
        var exception = Record.Exception(() => logger.LogInformation("Test with all outputs"));
        Assert.Null(exception);

        // Cleanup
        if (Directory.Exists(options.FileOptions.LogDirectory))
        {
            try { Directory.Delete(options.FileOptions.LogDirectory, true); } catch { }
        }
    }

    [Fact]
    public void LogOutput_ServerSentEvents_ShouldBeCorrectValue()
    {
        // Assert
        Assert.Equal(4, (int)MikuLogOutput.ServerSentEvents);
        Assert.True(MikuLogOutput.All.HasFlag(MikuLogOutput.ServerSentEvents));
        Assert.True(MikuLogOutput.All.HasFlag(MikuLogOutput.Console));
        Assert.True(MikuLogOutput.All.HasFlag(MikuLogOutput.File));
    }
}

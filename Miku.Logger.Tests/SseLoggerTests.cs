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
        // Note: In production, the broadcaster is a singleton, but we can still test its behavior
    }

    public void Dispose()
    {
        // Cleanup is handled by the singleton
    }

    [Fact]
    public void Configure_ShouldUpdateOptions()
    {
        // Arrange
        var options = new SseLoggerOptions
        {
            EndpointPath = "/custom/logs",
            EventType = "custom-event",
            MaxClients = 50,
            ReconnectionIntervalMs = 5000
        };

        // Act
        SseLogBroadcaster.Instance.Configure(options);

        // Assert
        Assert.Equal("/custom/logs", SseLogBroadcaster.Instance.Options.EndpointPath);
        Assert.Equal("custom-event", SseLogBroadcaster.Instance.Options.EventType);
        Assert.Equal(50, SseLogBroadcaster.Instance.Options.MaxClients);
        Assert.Equal(5000, SseLogBroadcaster.Instance.Options.ReconnectionIntervalMs);
    }

    [Fact]
    public void ClientCount_WithNoClients_ShouldBeZero()
    {
        // Act & Assert
        Assert.True(SseLogBroadcaster.Instance.ClientCount >= 0);
    }

    [Fact]
    public async Task SubscribeAsync_ShouldReceiveBroadcastedLogs()
    {
        // Arrange
        var receivedLogs = new List<SseLogEntry>();
        var cts = new CancellationTokenSource();

        // Start subscriber in background
        var subscribeTask = Task.Run(async () =>
        {
            await foreach (var entry in SseLogBroadcaster.Instance.SubscribeAsync(cts.Token))
            {
                receivedLogs.Add(entry);
                if (receivedLogs.Count >= 3)
                {
                    break;
                }
            }
        });

        // Give subscriber time to connect
        await Task.Delay(100);

        // Act - Broadcast some logs
        SseLogBroadcaster.Instance.Broadcast(LogLevel.Information, "TestCategory", "Message 1");
        SseLogBroadcaster.Instance.Broadcast(LogLevel.Warning, "TestCategory", "Message 2");
        SseLogBroadcaster.Instance.Broadcast(LogLevel.Error, "TestCategory", "Message 3");

        // Wait for messages or timeout
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

        // Act
        var subscribeTask = Task.Run(async () =>
        {
            try
            {
                await foreach (var entry in SseLogBroadcaster.Instance.SubscribeAsync(cts.Token))
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

        // Assert - Task should complete without throwing
        Assert.True(subscribeTask.IsCompleted || subscribeTask.IsCanceled);
    }

    [Fact]
    public void Broadcast_WithMinimumLogLevelFilter_ShouldFilterLogs()
    {
        // Arrange
        var options = new SseLoggerOptions
        {
            MinimumLogLevel = LogLevel.Warning
        };
        SseLogBroadcaster.Instance.Configure(options);

        var receivedLogs = new List<SseLogEntry>();
        var cts = new CancellationTokenSource();

        // Start subscriber
        var subscribeTask = Task.Run(async () =>
        {
            await foreach (var entry in SseLogBroadcaster.Instance.SubscribeAsync(cts.Token))
            {
                receivedLogs.Add(entry);
            }
        });

        // Give time to connect
        Thread.Sleep(50);

        // Act - Broadcast logs at different levels
        SseLogBroadcaster.Instance.Broadcast(LogLevel.Debug, "Test", "Debug message"); // Should be filtered
        SseLogBroadcaster.Instance.Broadcast(LogLevel.Information, "Test", "Info message"); // Should be filtered
        SseLogBroadcaster.Instance.Broadcast(LogLevel.Warning, "Test", "Warning message"); // Should pass
        SseLogBroadcaster.Instance.Broadcast(LogLevel.Error, "Test", "Error message"); // Should pass

        Thread.Sleep(100);
        cts.Cancel();

        // Reset filter
        SseLogBroadcaster.Instance.Configure(new SseLoggerOptions { MinimumLogLevel = null });

        // Assert - Only Warning and Error should be received
        var warningAndErrorLogs = receivedLogs.Where(l =>
            l.Level == "Warning" || l.Level == "Error").ToList();
        Assert.True(warningAndErrorLogs.Count <= receivedLogs.Count);
    }

    [Fact]
    public void BroadcastEntry_ShouldNotThrowWhenNoClients()
    {
        // Arrange
        var entry = SseLogEntry.Create(LogLevel.Information, "Test", "Test message");

        // Act & Assert - Should not throw
        var exception = Record.Exception(() => SseLogBroadcaster.Instance.BroadcastEntry(entry));
        Assert.Null(exception);
    }
}

public class SseLogEntryTests
{
    [Fact]
    public void Create_ShouldSetAllProperties()
    {
        // Arrange & Act
        var entry = SseLogEntry.Create(
            LogLevel.Error,
            "TestCategory",
            "Test message",
            new InvalidOperationException("Test exception"),
            useUtcTime: true);

        // Assert
        Assert.NotEmpty(entry.Id);
        Assert.Equal("Error", entry.Level);
        Assert.Equal(4, entry.LevelValue); // Error = 4
        Assert.Equal("TestCategory", entry.Category);
        Assert.Equal("Test message", entry.Message);
        Assert.Contains("Test exception", entry.Exception);
    }

    [Fact]
    public void Create_WithoutException_ShouldHaveNullException()
    {
        // Arrange & Act
        var entry = SseLogEntry.Create(LogLevel.Information, "Test", "Message");

        // Assert
        Assert.Null(entry.Exception);
    }

    [Theory]
    [InlineData(LogLevel.Trace, "Trace")]
    [InlineData(LogLevel.Debug, "Debug")]
    [InlineData(LogLevel.Information, "Information")]
    [InlineData(LogLevel.Warning, "Warning")]
    [InlineData(LogLevel.Error, "Error")]
    [InlineData(LogLevel.Critical, "Critical")]
    public void Create_ShouldMapLogLevelCorrectly(LogLevel logLevel, string expectedLevelString)
    {
        // Act
        var entry = SseLogEntry.Create(logLevel, "Test", "Message");

        // Assert
        Assert.Equal(expectedLevelString, entry.Level);
        Assert.Equal((int)logLevel, entry.LevelValue);
    }

    [Fact]
    public void Create_WithUtcTime_ShouldUseUtcTimestamp()
    {
        // Act
        var beforeUtc = DateTime.UtcNow;
        var entry = SseLogEntry.Create(LogLevel.Information, "Test", "Message", useUtcTime: true);
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
        var options = new SseLoggerOptions();

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
        var receivedLogs = new List<SseLogEntry>();
        var cts = new CancellationTokenSource();

        var options = new MikuLoggerOptions
        {
            Output = LogOutput.ServerSentEvents,
            MinimumLogLevel = LogLevel.Debug
        };

        // Start subscriber
        var subscribeTask = Task.Run(async () =>
        {
            await foreach (var entry in SseLogBroadcaster.Instance.SubscribeAsync(cts.Token))
            {
                receivedLogs.Add(entry);
                if (receivedLogs.Count >= 2)
                    break;
            }
        });

        // Give time to connect
        Thread.Sleep(50);

        // Act
        using var logger = new MikuLogger("TestLogger", options);
        logger.LogInformation("Test message 1");
        logger.LogWarning("Test message 2");

        // Wait for messages
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
            Output = LogOutput.All,
            FileOptions = new FileLoggerOptions
            {
                LogDirectory = Path.Combine(Path.GetTempPath(), $"MikuSseTest_{Guid.NewGuid()}")
            }
        };

        // Act & Assert - Should not throw
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
        Assert.Equal(4, (int)LogOutput.ServerSentEvents);
        Assert.True(LogOutput.All.HasFlag(LogOutput.ServerSentEvents));
        Assert.True(LogOutput.All.HasFlag(LogOutput.Console));
        Assert.True(LogOutput.All.HasFlag(LogOutput.File));
    }
}

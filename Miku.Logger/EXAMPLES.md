# Miku.Logger - Usage Examples

This document provides detailed examples for using Miku.Logger in various scenarios.

## Table of Contents
1. [Basic Usage](#basic-usage)
2. [Custom Configuration](#custom-configuration)
3. [Async Logging](#async-logging)
4. [Exception Logging](#exception-logging)
5. [ASP.NET Core Integration](#aspnet-core-integration)
6. [File-Only Logging](#file-only-logging)
7. [Console-Only Logging](#console-only-logging)
8. [Log Rotation](#log-rotation)
9. [Dependency Injection](#dependency-injection)
10. [Advanced: Custom Log Format](#advanced-custom-log-format)
11. [Server-Sent Events (SSE) Logging](#server-sent-events-sse-logging)

---

## Basic Usage

```csharp
using Miku.Logger;

// Example 1: Automatic class name detection
public class UserService
{
    private readonly MikuLogger _logger = new MikuLogger();
    
    public void CreateUser(string username)
    {
        _logger.LogInformation("Creating user: {Username}", username);
        // Output: 2025-11-29 21:45:23.123 [INFO] [UserService] Creating user: johndoe
        
        _logger.LogDebug("User validation started");
        _logger.LogWarning("Username already exists");
        _logger.LogError("Database connection failed");
    }
}

// Example 2: Custom logger name
var logger = new MikuLogger("MyApp");

logger.LogTrace("This is a trace message");
logger.LogDebug("This is a debug message");
logger.LogInformation("Application started successfully");
logger.LogWarning("This is a warning");
logger.LogError("This is an error");
logger.LogCritical("This is critical!");

logger.Dispose();
```

## Custom Configuration

```csharp
using Miku.Logger;
using Miku.Logger.Configuration;
using Miku.Logger.Configuration.Enums;
using Miku.Logger.Configuration.Models;

var options = new MikuLoggerOptions
{
    Output = MikuLogOutput.ConsoleAndFile,
    MinimumLogLevel = MikuLogLevel.Debug,
    UseUtcTime = false,
    DateFormat = "yyyy-MM-dd HH:mm:ss.fff",
    
    ConsoleColors = new MikuConsoleColorOptions
    {
        Enabled = true,
        DebugColor = ConsoleColor.Yellow,
        InformationColor = ConsoleColor.Green,
        ErrorColor = ConsoleColor.Red
    },
    
    FileOptions = new MikuFileLoggerOptions
    {
        LogDirectory = "./logs",
        FileNamePattern = "app.log",
        MaxFileSizeBytes = 10 * 1024 * 1024, // 10 MB
        MaxFileCount = 5,
        UseDateFolders = true,
        DateFolderFormat = "yyyy-MM-dd"
    }
};

var logger = new MikuLogger("MyApp", options);

logger.LogDebug("Debug mode enabled");
logger.LogInformation("Configuration loaded: {Config}", "Production");
logger.LogWarning("Memory usage: {Usage}MB", 125);

logger.Dispose();
```

## Async Logging

```csharp
using Miku.Logger;

var logger = new MikuLogger("AsyncApp");

// Single async log
await logger.LogInformationAsync("Processing started");

// Multiple async logs
var tasks = new List<Task>();
for (int i = 0; i < 10; i++)
{
    int index = i;
    tasks.Add(logger.LogInformationAsync("Processing item {Index}", index));
}

await Task.WhenAll(tasks);
logger.LogInformation("All async operations completed");

logger.Dispose();
```

## Exception Logging

```csharp
using Miku.Logger;

var logger = new MikuLogger("ErrorApp");

try
{
    // Some operation that might fail
    throw new InvalidOperationException("Something went wrong!");
}
catch (Exception ex)
{
    // Log with exception details
    logger.LogError(ex, "Operation failed: {Operation}", "DataProcessing");
    
    // Async error logging
    await logger.LogErrorAsync(ex, "Async error in {Method}", "ProcessData");
}

logger.Dispose();
```

## ASP.NET Core Integration

### Program.cs

```csharp
using Miku.Logger.Extensions;
using Miku.Logger.Configuration;
using Miku.Logger.Configuration.Enums;
using Miku.Logger.Configuration.Models;

var builder = WebApplication.CreateBuilder(args);

// Add MikuLogger to ASP.NET Core logging
builder.Logging.ClearProviders(); // Optional: clear default providers
builder.Logging.AddMikuLogger(options =>
{
    options.Output = MikuLogOutput.ConsoleAndFile;
    options.MinimumLogLevel = MikuLogLevel.Information;
    options.FileOptions = new MikuFileLoggerOptions
    {
        LogDirectory = "./logs",
        MaxFileSizeBytes = 5 * 1024 * 1024,
        UseDateFolders = true
    };
});

var app = builder.Build();
app.Run();
```

### Controller Usage

```csharp
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        _logger.LogInformation("Home page accessed");
        return View();
    }

    public async Task<IActionResult> ProcessAsync()
    {
        try
        {
            _logger.LogInformation("Processing request");
            // Process logic
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Processing failed");
            return StatusCode(500);
        }
    }
}
```

## File-Only Logging

```csharp
using Miku.Logger;
using Miku.Logger.Configuration;
using Miku.Logger.Configuration.Enums;
using Miku.Logger.Configuration.Models;

var options = new MikuLoggerOptions
{
    Output = MikuLogOutput.File, // Only to file, no console
    MinimumLogLevel = MikuLogLevel.Information,
    FileOptions = new MikuFileLoggerOptions
    {
        LogDirectory = "./logs",
        FileNamePattern = "silent.log",
        MaxFileSizeBytes = 10 * 1024 * 1024
    }
};

var logger = new MikuLogger("SilentApp", options);

logger.LogInformation("This only goes to file");
logger.LogError("Errors also only in file");

logger.Dispose();
```

## Console-Only Logging

```csharp
using Miku.Logger;
using Miku.Logger.Configuration;
using Miku.Logger.Configuration.Enums;
using Miku.Logger.Configuration.Models;

var options = new MikuLoggerOptions
{
    Output = MikuLogOutput.Console, // Only to console, no file
    MinimumLogLevel = MikuLogLevel.Debug,
    ConsoleColors = new MikuConsoleColorOptions
    {
        Enabled = true,
        DebugColor = ConsoleColor.Yellow,
        InformationColor = ConsoleColor.Cyan
    }
};

var logger = new MikuLogger("ConsoleApp", options);

logger.LogDebug("Debug info on console");
logger.LogInformation("Info on console");

logger.Dispose();
```

## Log Rotation

```csharp
using Miku.Logger;
using Miku.Logger.Configuration;
using Miku.Logger.Configuration.Enums;
using Miku.Logger.Configuration.Models;

var options = new MikuLoggerOptions
{
    Output = MikuLogOutput.File,
    FileOptions = new MikuFileLoggerOptions
    {
        LogDirectory = "./logs",
        FileNamePattern = "app.log",
        
        // Rotate when file reaches 1MB
        MaxFileSizeBytes = 1 * 1024 * 1024,
        
        // Keep last 10 rotated files
        MaxFileCount = 10,
        
        // Organize in date folders
        UseDateFolders = true,
        DateFolderFormat = "yyyy-MM-dd"
    }
};

var logger = new MikuLogger("RotatingApp", options);

// Logs will be organized like:
// logs/
//   2025-11-29/
//     app.log              (current)
//     app_20251129_143022.log (rotated)
//     app_20251129_150133.log (rotated)

for (int i = 0; i < 10000; i++)
{
    logger.LogInformation("Log entry {Index}", i);
}

logger.Dispose();
```

## Dependency Injection

### Register as Singleton

```csharp
using Microsoft.Extensions.DependencyInjection;
using Miku.Logger;
using Miku.Logger.Configuration;
using Miku.Logger.Configuration.Enums;

var services = new ServiceCollection();

// Simple registration with default options
services.AddSingleton<MikuLogger>();

// Or with custom options
var loggerOptions = new MikuLoggerOptions
{
    Output = MikuLogOutput.ConsoleAndFile,
    MinimumLogLevel = MikuLogLevel.Information
};

services.AddSingleton(loggerOptions);
services.AddSingleton<MikuLogger>();

var serviceProvider = services.BuildServiceProvider();
```

### Use in Service with Automatic Class Name

```csharp
public class DataService
{
    private readonly MikuLogger _logger;

    public DataService(MikuLogger logger)
    {
        _logger = logger;
    }

    public async Task ProcessDataAsync()
    {
        await _logger.LogInformationAsync("Starting data processing");
        // Output: 2025-11-29 21:45:23.123 [INFO] [DataService] Starting data processing
        
        try
        {
            // Process data
            await Task.Delay(1000);
            await _logger.LogInformationAsync("Data processed successfully");
        }
        catch (Exception ex)
        {
            await _logger.LogErrorAsync(ex, "Data processing failed");
            throw;
        }
    }
}
```

### Use with Custom Name

```csharp
public class PaymentService
{
    private readonly MikuLogger _logger;

    public PaymentService()
    {
        // Use custom category name
        _logger = new MikuLogger("Payment");
    }

    public void ProcessPayment(decimal amount)
    {
        _logger.LogInformation("Processing payment: ${Amount}", amount);
        // Output: 2025-11-29 21:45:23.123 [INFO] [Payment] Processing payment: $99.99
    }
}
```

### ASP.NET Core with Dependency Injection

```csharp
using Miku.Logger.Configuration;
using Miku.Logger.Configuration.Enums;
using Miku.Logger.Configuration.Models;

// In Program.cs
var builder = WebApplication.CreateBuilder(args);

// Configure MikuLogger options
var loggerOptions = new MikuLoggerOptions
{
    Output = MikuLogOutput.ConsoleAndFile,
    MinimumLogLevel = MikuLogLevel.Information,
    FileOptions = new MikuFileLoggerOptions
    {
        LogDirectory = "./logs",
        MaxFileSizeBytes = 10 * 1024 * 1024,
        UseDateFolders = true
    }
};

builder.Services.AddSingleton(loggerOptions);
builder.Services.AddSingleton<MikuLogger>();

var app = builder.Build();
```

### Multiple Logger Instances with Different Configurations

```csharp
using Miku.Logger.Configuration;
using Miku.Logger.Configuration.Enums;
using Miku.Logger.Configuration.Models;

// For scenarios where you need multiple loggers with different configurations
services.AddSingleton(sp => new MikuLogger("Application", new MikuLoggerOptions
{
    Output = MikuLogOutput.ConsoleAndFile,
    MinimumLogLevel = MikuLogLevel.Information
}));

services.AddSingleton(sp => new MikuLogger("Security", new MikuLoggerOptions
{
    Output = MikuLogOutput.File,
    MinimumLogLevel = MikuLogLevel.Warning,
    FileOptions = new MikuFileLoggerOptions
    {
        LogDirectory = "./security-logs"
    }
}));
```

## Advanced: Custom Log Format

```csharp
using Miku.Logger;
using Miku.Logger.Configuration;
using Miku.Logger.Configuration.Enums;
using Miku.Logger.Configuration.Models;

// Example 1: Only show time and message (no date, level, or logger name)
var options1 = new MikuLoggerOptions
{
    Output = MikuLogOutput.Console,
    FormatOptions = new MikuLogFormatOptions
    {
        ShowDate = false,
        ShowTime = true,
        ShowLogLevel = false,
        ShowLoggerName = false
    }
};
var logger1 = new MikuLogger("SimpleLogger", options1);
logger1.LogInformation("Simple message");
// Output: 14:30:25.123 Simple message

// Example 2: Only show level and message (for minimal logging)
var options2 = new MikuLoggerOptions
{
    Output = MikuLogOutput.Console,
    FormatOptions = new MikuLogFormatOptions
    {
        ShowDate = false,
        ShowTime = false,
        ShowLogLevel = true,
        ShowLoggerName = false
    }
};
var logger2 = new MikuLogger("MinimalLogger", options2);
logger2.LogError("Error occurred");
// Output: [ERROR] Error occurred

// Example 3: Custom date/time format with full information
var options3 = new MikuLoggerOptions
{
    Output = MikuLogOutput.ConsoleAndFile,
    DateFormat = "yyyy-MM-dd HH:mm:ss.fff zzz", // Include timezone
    UseUtcTime = true, // Use UTC instead of local time
    FormatOptions = new MikuLogFormatOptions
    {
        ShowDate = true,
        ShowTime = true,
        ShowLogLevel = true,
        ShowLoggerName = true
    },
    FileOptions = new MikuFileLoggerOptions
    {
        LogDirectory = "./logs",
        FileNamePattern = "app.log"
    }
};
var logger3 = new MikuLogger("FullLogger", options3);
logger3.LogInformation("Detailed log entry");
// Output: 2025-11-29 21:30:25.123 +00:00 [INFO] [FullLogger] Detailed log entry

// Example 4: Separate date and time formats
var options4 = new MikuLoggerOptions
{
    Output = MikuLogOutput.Console,
    FormatOptions = new MikuLogFormatOptions
    {
        ShowDate = true,
        ShowTime = false,  // Only date, no time
        ShowLogLevel = true,
        ShowLoggerName = true
    }
};
var logger4 = new MikuLogger("DateOnlyLogger", options4);
logger4.LogInformation("Daily summary");
// Output: 2025-11-29 [INFO] [DateOnlyLogger] Daily summary

logger1.Dispose();
logger2.Dispose();
logger3.Dispose();
logger4.Dispose();
```

## Performance: High-Volume Logging

```csharp
using Miku.Logger;
using Miku.Logger.Configuration;
using Miku.Logger.Configuration.Enums;
using Miku.Logger.Configuration.Models;

var options = new MikuLoggerOptions
{
    Output = MikuLogOutput.File, // File only for performance
    MinimumLogLevel = MikuLogLevel.Information,
    FileOptions = new MikuFileLoggerOptions
    {
        LogDirectory = "./logs",
        MaxFileSizeBytes = 50 * 1024 * 1024, // 50 MB
        MaxFileCount = 20
    }
};

using var logger = new MikuLogger("HighVolume", options);

// Async logging for better performance
var tasks = Enumerable.Range(0, 1000)
    .Select(i => logger.LogInformationAsync("Processing {Index}", i))
    .ToArray();

await Task.WhenAll(tasks);
```

## Server-Sent Events (SSE) Logging

MikuLogger supports real-time log streaming via Server-Sent Events (SSE), allowing you to monitor logs in real-time from web clients.

### Basic SSE Setup

```csharp
using Miku.Logger.Extensions;
using Miku.Logger.Configuration;
using Miku.Logger.Configuration.Enums;

var builder = WebApplication.CreateBuilder(args);

// Add MikuLogger with SSE support
builder.Logging.AddMikuLoggerWithSse(options =>
{
    options.Output = MikuLogOutput.All; // Console, File, and SSE
    options.MinimumLogLevel = MikuLogLevel.Information;
    options.SseOptions.EndpointPath = "/miku/logs/stream";
});

var app = builder.Build();

// Map the SSE endpoint
app.MapMikuLoggerSse();

app.Run();

// Clients can now connect to: https://yourapp.com/miku/logs/stream
```

### Custom SSE Configuration

```csharp
using Miku.Logger.Extensions;
using Miku.Logger.Configuration;
using Miku.Logger.Configuration.Enums;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddMikuLoggerWithSse(options =>
{
    options.Output = MikuLogOutput.All;
    options.MinimumLogLevel = MikuLogLevel.Debug;
    
    // SSE-specific options
    options.SseOptions = new MikuSseLoggerOptions
    {
        EndpointPath = "/api/logs/live",
        EventType = "log-event",
        MaxClients = 50,                      // Limit concurrent connections
        ReconnectionIntervalMs = 5000,        // 5 seconds reconnection hint
        IncludeLogLevelInEventType = true,    // Events: "log-event-info", "log-event-error"
        MinimumLogLevel = MikuLogLevel.Warning // Only stream warnings and above
    };
});

var app = builder.Build();
app.MapMikuLoggerSse();
app.Run();
```

### SSE with Authorization

```csharp
using Miku.Logger.Extensions;
using Miku.Logger.Configuration;
using Miku.Logger.Configuration.Enums;

var builder = WebApplication.CreateBuilder(args);

// Add authentication/authorization services
builder.Services.AddAuthentication().AddJwtBearer();
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
});

builder.Logging.AddMikuLoggerWithSse(options =>
{
    options.Output = MikuLogOutput.All;
    options.SseOptions.RequireAuthorization = true;
    options.SseOptions.AuthorizationPolicy = "AdminOnly";
});

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapMikuLoggerSse();
app.Run();
```

### Adding SSE to Existing Logger Configuration

```csharp
using Miku.Logger.Extensions;
using Miku.Logger.Configuration;
using Miku.Logger.Configuration.Enums;

var builder = WebApplication.CreateBuilder(args);

// Add SSE services separately
builder.Services.AddMikuLoggerSse(options =>
{
    options.EndpointPath = "/logs/stream";
    options.MaxClients = 100;
    options.IncludeLogLevelInEventType = true;
});

// Add MikuLogger with SSE output
builder.Logging.AddMikuLogger(options =>
{
    options.Output = MikuLogOutput.ConsoleAndFile | MikuLogOutput.ServerSentEvents;
    options.MinimumLogLevel = MikuLogLevel.Information;
});

var app = builder.Build();

// Map with custom path
app.MapMikuLoggerSse("/logs/stream");

app.Run();
```

### JavaScript Client Example

```html
<!DOCTYPE html>
<html>
<head>
    <title>MikuLogger Live Logs</title>
    <style>
        #logs { font-family: monospace; background: #1e1e1e; color: #d4d4d4; padding: 10px; height: 400px; overflow-y: auto; }
        .log-info { color: #00CED1; }
        .log-warning { color: #FFD700; }
        .log-error { color: #FF6B6B; }
        .log-critical { color: #FF0000; font-weight: bold; }
    </style>
</head>
<body>
    <h1>?? MikuLogger Live Stream</h1>
    <div id="logs"></div>
    
    <script>
        const logsDiv = document.getElementById('logs');
        const eventSource = new EventSource('/miku/logs/stream');
        
        eventSource.addEventListener('miku-log', (event) => {
            const entry = JSON.parse(event.data);
            const logLine = document.createElement('div');
            logLine.className = `log-${entry.level.toLowerCase()}`;
            logLine.textContent = `${entry.timestamp} [${entry.level}] [${entry.category}] ${entry.message}`;
            logsDiv.appendChild(logLine);
            logsDiv.scrollTop = logsDiv.scrollHeight;
        });
        
        // With IncludeLogLevelInEventType = true
        eventSource.addEventListener('miku-log-error', (event) => {
            const entry = JSON.parse(event.data);
            console.error('Error log received:', entry);
        });
        
        eventSource.onerror = () => {
            console.log('Connection lost, reconnecting...');
        };
    </script>
</body>
</html>
```

### SSE Log Entry Format

When connecting to the SSE endpoint, log entries are sent as JSON with the following structure:

```json
{
    "timestamp": "2025-11-29T21:45:23.123Z",
    "level": "Information",
    "category": "MyApp.Services.UserService",
    "message": "User created successfully: johndoe",
    "exception": null
}
```

---

## Best Practices

1. **Always Dispose**: Use `using` or call `Dispose()` to flush remaining logs
2. **Use Async for High Volume**: Async methods provide better performance
3. **Configure Log Rotation**: Prevent disk space issues
4. **Appropriate Log Levels**: Use correct levels (Debug for dev, Info for production)
5. **Structured Logging**: Use placeholders for better searchability
6. **Exception Logging**: Always log exceptions with context
7. **Singleton Pattern**: Reuse logger instances in DI scenarios

## Troubleshooting

### Logs Not Appearing
- Check `MinimumLogLevel` setting
- Verify `Output` includes desired target
- Ensure `Dispose()` is called to flush

### File Permission Errors
- Check write permissions for `LogDirectory`
- Ensure directory path is valid

### Performance Issues
- Use `MikuLogOutput.File` instead of console for production
- Increase `MaxFileSizeBytes` to reduce rotation frequency
- Use async methods for better throughput

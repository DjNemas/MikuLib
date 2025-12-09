# MikuLib.Logger

A powerful, thread-safe logging library for .NET 10 with console, file, and **Server-Sent Events (SSE)** output support, featuring async operations, log rotation, and Microsoft.Extensions.Logging compatibility.

> "Tell your logs to the world!" - Hatsune Nemas

*Singing since August 31st, 2007 - Logging since 2025 - Now streaming live!*

![MikuLib Console & Logger Demo](https://djnemas.de/SX/WindowsTerminal_Utmoe5RPDL.gif)

## Features

- **ILogger Compatible** - Implements `Microsoft.Extensions.Logging.ILogger` interface
- **Thread-Safe** - Lock-free architecture with ConcurrentQueue
- **Async Support** - Asynchronous logging methods for better performance
- **High Performance** - Batch writing up to 100 messages per operation
- **Multiple Output Targets** - Console, File, SSE, or any combination
- **Server-Sent Events** - Real-time log streaming to web clients (NEW!)
- **Log Levels** - Trace, Debug, Information, Warning, Error, Critical
- **Colored Console Output** - Customizable colors for each log level
- **Log Rotation** - Automatic file rotation based on file size
- **Date-Based Folders** - Organize logs by date
- **Configurable** - Extensive configuration options
- **Zero Data Loss** - Guaranteed message delivery even under high load

> The default cyan color for Information logs is #00CED1

## Installation

```bash
dotnet add package MikuLib.Logger
```

Or add to your `.csproj`:
```xml
<PackageReference Include="MikuLib.Logger" Version="10.2.39" />
```

## Quick Start

### Basic Usage

```csharp
using Miku.Logger;
using Miku.Logger.Configuration;

// Automatic class name detection - uses calling class name
public class MyService
{
    private readonly MikuLogger _logger = new MikuLogger();
    
    public void DoWork()
    {
        _logger.LogInformation("Work started");
        // Output: 2025-11-29 21:45:23.123 [INFO] [MyService] Work started
    }
}

// Or specify a custom name
var logger = new MikuLogger("MyApp");
logger.LogInformation("Application started");

// Async logging
await logger.LogInformationAsync("Async log message");
```

### Custom Configuration

```csharp
var options = new MikuLoggerOptions
{
    Output = LogOutput.All, // Console, File, and SSE
    MinimumLogLevel = LogLevel.Debug,
    DateFormat = "yyyy-MM-dd HH:mm:ss.fff",
    UseUtcTime = false,
    
    // Console options
    ConsoleColors = new ConsoleColorOptions
    {
        Enabled = true,
        DebugColor = ConsoleColor.Yellow,
        ErrorColor = ConsoleColor.Red,
        InformationColor = ConsoleColor.Cyan
    },
    
    // File options
    FileOptions = new FileLoggerOptions
    {
        LogDirectory = "./logs",
        FileNamePattern = "app.log",
        MaxFileSizeBytes = 10 * 1024 * 1024, // 10 MB
        MaxFileCount = 5,
        UseDateFolders = true,
        DateFolderFormat = "yyyy-MM-dd"
    },
    
    // SSE options (NEW!)
    SseOptions = new SseLoggerOptions
    {
        EndpointPath = "/miku/logs/stream",
        EventType = "miku-log",
        MaxClients = 100
    }
};

var logger = new MikuLogger("MyApp", options);
```

## Server-Sent Events (SSE) - NEW! 🎤

Stream your logs in real-time to web clients using Server-Sent Events!

### ASP.NET Core Integration

```csharp
using Miku.Logger.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add MikuLogger with SSE support
builder.Logging.AddMikuLoggerWithSse(options =>
{
    options.Output = LogOutput.All;
    options.MinimumLogLevel = LogLevel.Debug;
    options.SseOptions.EndpointPath = "/api/logs/stream";
    options.SseOptions.MaxClients = 50;
});

var app = builder.Build();

// Map the SSE endpoint
app.MapMikuLoggerSse();

// Or with custom path
app.MapMikuLoggerSse("/custom/logs/stream");

app.Run();
```

### JavaScript Client Example

```javascript
const eventSource = new EventSource('/miku/logs/stream');

eventSource.addEventListener('miku-log', (event) => {
    const logEntry = JSON.parse(event.data);
    console.log(`[${logEntry.level}] ${logEntry.category}: ${logEntry.message}`);
});

eventSource.onerror = (error) => {
    console.error('SSE connection error:', error);
};
```

### SSE Log Entry Format

```json
{
    "id": "a1b2c3d4e5f6",
    "timestamp": "2025-12-02T14:30:00.123Z",
    "level": "Information",
    "levelValue": 2,
    "category": "MyController",
    "message": "Request processed successfully",
    "exception": null
}
```

### SSE Configuration Options

```csharp
SseOptions = new SseLoggerOptions
{
    // Endpoint path for SSE streaming
    EndpointPath = "/miku/logs/stream",
    
    // SSE event type name
    EventType = "miku-log",
    
    // Maximum concurrent clients (0 = unlimited)
    MaxClients = 100,
    
    // Reconnection interval hint for clients (ms)
    ReconnectionIntervalMs = 3000,
    
    // Include log level in event type (e.g., "miku-log-error")
    IncludeLogLevelInEventType = false,
    
    // Authorization settings
    RequireAuthorization = false,
    AuthorizationPolicy = null,
    
    // Minimum log level for SSE (independent of other outputs)
    MinimumLogLevel = LogLevel.Information
}
```

## ASP.NET Core Integration

### Add to Program.cs

```csharp
using Miku.Logger.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add MikuLogger to ASP.NET Core logging
builder.Logging.AddMikuLogger(options =>
{
    options.Output = LogOutput.ConsoleAndFile;
    options.MinimumLogLevel = LogLevel.Information;
    options.FileOptions.MaxFileSizeBytes = 5 * 1024 * 1024; // 5 MB
    options.FileOptions.UseDateFolders = true;
});

var app = builder.Build();
```

### Use in Controllers

```csharp
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        _logger.LogInformation("Home page visited");
        return View();
    }
}
```

## Configuration Options

### Output Targets

```csharp
LogOutput.None              // No output
LogOutput.Console           // Console only
LogOutput.File              // File only
LogOutput.ServerSentEvents  // SSE only (NEW!)
LogOutput.ConsoleAndFile    // Both console and file
LogOutput.All               // Console, File, and SSE
```

### Log Levels

```csharp
LogLevel.Trace       // Most detailed messages
LogLevel.Debug       // Development debugging
LogLevel.Information // General information
LogLevel.Warning     // Abnormal events
LogLevel.Error       // Error events
LogLevel.Critical    // Critical failures
LogLevel.None        // No logging
```

### Format Options

Control which elements are shown in log messages:

```csharp
var options = new MikuLoggerOptions
{
    FormatOptions = new LogFormatOptions
    {
        ShowDate = true,        // Show date (default: true)
        ShowTime = true,        // Show time (default: true)
        ShowLogLevel = true,    // Show log level (default: true)
        ShowLoggerName = true   // Show logger name (default: true)
    }
};
```

### File Rotation

The logger automatically rotates log files when they exceed the specified size:

```csharp
FileOptions = new FileLoggerOptions
{
    MaxFileSizeBytes = 10 * 1024 * 1024, // 10 MB
    MaxFileCount = 10,                    // Keep last 10 files
    UseDateFolders = true                 // Organize in date folders
}
```

### Console Colors

MikuLogger supports three color modes for console output:

#### Standard Console Colors (16 colors)

```csharp
ConsoleColors = new ConsoleColorOptions
{
    Enabled = true,
    ColorSpace = ColorSpace.Console, // Default
    TraceColor = ConsoleColor.Gray,
    DebugColor = ConsoleColor.Yellow,
    InformationColor = ConsoleColor.Cyan,
    WarningColor = ConsoleColor.Magenta,
    ErrorColor = ConsoleColor.Red,
    CriticalColor = ConsoleColor.DarkRed
}
```

#### Extended 256 Colors

For terminals supporting 256-color palette:

```csharp
ConsoleColors = new ConsoleColorOptions
{
    Enabled = true,
    ColorSpace = ColorSpace.Extended256,
    Extended256Colors = new Extended256ColorOptions
    {
        TraceColor = 245,      // Light gray
        DebugColor = 226,      // Yellow
        InformationColor = 44, // Cyan (Miku!)
        WarningColor = 208,    // Orange
        ErrorColor = 196,      // Red
        CriticalColor = 160    // Dark red
    }
}
```

#### TrueColor (24-bit RGB)

For modern terminals supporting 16 million colors:

```csharp
ConsoleColors = new ConsoleColorOptions
{
    Enabled = true,
    ColorSpace = ColorSpace.TrueColor,
    TrueColors = new TrueColorOptions
    {
        TraceColor = RgbColor.Gray,
        DebugColor = RgbColor.Yellow,
        InformationColor = RgbColor.MikuCyan, // #00CED1 - Miku's signature color!
        WarningColor = RgbColor.Orange,
        ErrorColor = RgbColor.Red,
        CriticalColor = RgbColor.DarkRed
    }
}

// Custom hex colors
TrueColors = new TrueColorOptions
{
    InformationColor = RgbColor.FromHex("#39C5BB"), // Miku's hair highlight
    WarningColor = new RgbColor(255, 165, 0),       // RGB values
    ErrorColor = RgbColor.FromHex("FF4444")         // Without # prefix
}
```

#### Predefined Miku Colors

```csharp
RgbColor.MikuCyan     // #00CED1 - Signature cyan
RgbColor.MikuTeal     // #39C5BB - Hair highlight
RgbColor.MikuDarkCyan // #008B8B - Dark accent
```

## Performance

- **Lock-Free Architecture**: Uses ConcurrentQueue for maximum throughput
- **Singleton File Stream Management**: Only one FileStream per file for safe multi-instance access
- **Batch Writing**: Collects up to 100 messages and writes them in one operation
- **Thread-Safe**: SemaphoreSlim-based synchronization per file
- **Async File Writing**: Background queue processing with 8KB buffer
- **Zero Data Loss**: Guaranteed message delivery even when disposing
- **Channel-Based SSE**: Efficient bounded channels for SSE broadcasting
- **Multi-Instance Safe**: Multiple FileLogWriter instances can safely write to same file

## License

MIT License - See LICENSE file for details

## Contributing

Contributions are welcome! Please open an issue or pull request on GitHub.

## Credits

Created by **Hatsune Nemas** with inspiration from:
- Hatsune Miku (初音ミク) - CV01, born August 31st, 2007
- Crypton Future Media
- The Vocaloid community worldwide

## Repository

https://github.com/DjNemas/MikuLib

---

*"The future of voice, the future of logging - now streaming live!"*

**Version**: 10.2.39 (CV01 Edition)  
**Default Color**: Cyan (#00CED1)

# MikuLib.Logger

A powerful, thread-safe logging library for .NET 10 with console and file output support, featuring async operations, log rotation, and Microsoft.Extensions.Logging compatibility.

> "Tell your logs to the world!" - Hatsune Nemas

*Singing since August 31st, 2007 - Logging since 2025*

## Features

- **ILogger Compatible** - Implements `Microsoft.Extensions.Logging.ILogger` interface
- **Thread-Safe** - All operations are thread-safe using semaphores
- **Async Support** - Asynchronous logging methods for better performance
- **Multiple Output Targets** - Console, File, or both
- **Log Levels** - Trace, Debug, Information, Warning, Error, Critical
- **Colored Console Output** - Customizable colors for each log level
- **Log Rotation** - Automatic file rotation based on file size
- **Date-Based Folders** - Organize logs by date
- **Configurable** - Extensive configuration options
- **High Performance** - Asynchronous queue-based file writing

> The default cyan color for Information logs is #00CED1

## Installation

```bash
dotnet add package MikuLib.Logger
```

Or add to your `.csproj`:
```xml
<PackageReference Include="MikuLib.Logger" Version="10.0.39" />
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
    Output = LogOutput.ConsoleAndFile,
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
    }
};

var logger = new MikuLogger("MyApp", options);
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
LogOutput.None           // No output
LogOutput.Console        // Console only
LogOutput.File           // File only
LogOutput.ConsoleAndFile // Both console and file
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

**Examples of different format combinations:**

```csharp
// All enabled (default)
// Output: 2025-11-29 21:45:23.123 [INFO] [MyApp] Application started

// Only message
FormatOptions = new LogFormatOptions { ShowDate = false, ShowTime = false, ShowLogLevel = false, ShowLoggerName = false }
// Output: Application started

// Date and message only
FormatOptions = new LogFormatOptions { ShowDate = true, ShowTime = false, ShowLogLevel = false, ShowLoggerName = false }
// Output: 2025-11-29 Application started

// Time, level and message
FormatOptions = new LogFormatOptions { ShowDate = false, ShowTime = true, ShowLogLevel = true, ShowLoggerName = false }
// Output: 21:45:23.123 [INFO] Application started
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

Example folder structure with date folders:
```
logs/
├── 2025-11-29/
│   ├── app.log
│   ├── app_20251129_143022.log
│   └── app_20251129_150133.log
└── 2025-11-30/
    └── app.log
```

### Console Colors

Customize colors for each log level:

```csharp
ConsoleColors = new ConsoleColorOptions
{
    Enabled = true,
    TraceColor = ConsoleColor.Gray,
    DebugColor = ConsoleColor.Yellow,
    InformationColor = ConsoleColor.Cyan,
    WarningColor = ConsoleColor.Magenta,
    ErrorColor = ConsoleColor.Red,
    CriticalColor = ConsoleColor.DarkRed
}
```

## Advanced Usage

### Exception Logging

```csharp
try
{
    // Some operation
}
catch (Exception ex)
{
    logger.LogError(ex, "Operation failed: {Operation}", "DataProcessing");
    await logger.LogErrorAsync(ex, "Async error logging");
}
```

### Using with Dependency Injection

```csharp
// Register in DI container
services.AddSingleton(new MikuLogger("ServiceName", options));

// Inject and use
public class MyService
{
    private readonly MikuLogger _logger;
    
    public MyService(MikuLogger logger)
    {
        _logger = logger;
    }
    
    public async Task ProcessAsync()
    {
        await _logger.LogInformationAsync("Processing started");
        // ...
    }
}
```

## Performance

- **Async File Writing**: Uses background queue for non-blocking file operations
- **Thread-Safe**: Semaphore-based synchronization
- **Minimal Overhead**: Efficient message formatting and output

## Thread Safety

All logging operations are thread-safe:
- Console writes are synchronized
- File writes use async queues with semaphore protection
- Safe for concurrent access from multiple threads

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

*"The future of voice, the future of logging!"*

**Version**: 10.0.39 (CV01 Edition)  
**Default Color**: Cyan (#00CED1)
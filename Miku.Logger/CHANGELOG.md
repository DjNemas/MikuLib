# Changelog

All notable changes to Miku.Logger will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/).

## [10.2.39] - 2025-12-10

### Breaking Changes ??
- **All enums and configuration models renamed with `Miku` prefix**
  - `LogLevel` ? `MikuLogLevel`
  - `LogOutput` ? `MikuLogOutput`
  - `ColorSpace` ? `MikuColorSpace`
  - `ConsoleColorOptions` ? `MikuConsoleColorOptions`
  - `FileLoggerOptions` ? `MikuFileLoggerOptions`
  - `LogFormatOptions` ? `MikuLogFormatOptions`
  - `SseLoggerOptions` ? `MikuSseLoggerOptions`
  - `Extended256ColorOptions` ? `MikuExtended256ColorOptions`
  - `TrueColorOptions` ? `MikuTrueColorOptions`
  - `RgbColor` ? `MikuRgbColor`
  - `AnsiCodes` ? `MikuAnsiCodes`
  - `ColorHelper` ? `MikuColorHelper`
  - `SseLogBroadcaster` ? `MikuSseLogBroadcaster`
  - `SseLogEntry` ? `MikuSseLogEntry`
  - `FileLogWriter` ? `MikuFileLogWriter`
  - `SharedFileStreamManager` ? `MikuSharedFileStreamManager`

### Migration Guide
```csharp
// Before
var options = new MikuLoggerOptions
{
    Output = LogOutput.Console,
    MinimumLogLevel = LogLevel.Information,
    ConsoleColors = new ConsoleColorOptions { ColorSpace = ColorSpace.TrueColor }
};

// After
var options = new MikuLoggerOptions
{
    Output = MikuLogOutput.Console,
    MinimumLogLevel = MikuLogLevel.Information,
    ConsoleColors = new MikuConsoleColorOptions { ColorSpace = MikuColorSpace.TrueColor }
};
```

### Added
- **Server-Sent Events (SSE) Support** - Real-time log streaming to web clients!
  - New `MikuSseLogBroadcaster` singleton for managing SSE client connections
  - New `MikuSseLogEntry` class for structured log data in JSON format
  - New `MikuSseLoggerOptions` for configuring SSE behavior
  - New `MikuLogOutput.ServerSentEvents` flag for enabling SSE output
  - New `MikuLogOutput.All` combining Console, File, and SSE
  - ASP.NET Core extensions:
    - `AddMikuLoggerWithSse()` - Adds MikuLogger with SSE to logging builder
    - `AddMikuLoggerSse()` - Adds SSE services to DI container
    - `MapMikuLoggerSse()` - Maps SSE endpoint for log streaming
  - Uses .NET 10's native `TypedResults.ServerSentEvents()` API
  - Channel-based broadcasting with bounded capacity (39 messages - the Miku number!)
  - Configurable max clients, reconnection interval, and authorization
  - Independent minimum log level filter for SSE output

- **Extended Color Support** - Full 256-color and TrueColor (24-bit RGB) support!
  - `MikuExtended256ColorOptions` for 256-color palette configuration
  - `MikuTrueColorOptions` for TrueColor RGB configuration
  - ANSI escape sequence support for modern terminals
  - Automatic fallback to standard colors when ANSI not supported

- Namespace reorganization: Enums and Models now in separate files and namespaces
  - `Miku.Logger.Configuration.Enums` - MikuLogOutput, MikuLogLevel, MikuColorSpace
  - `Miku.Logger.Configuration.Models` - MikuLogFormatOptions, MikuConsoleColorOptions, MikuFileLoggerOptions, etc.

### Changed
- **Now depends on Miku.Core** for shared types (MikuRgbColor, MikuAnsiCodes, MikuColorHelper)
- `MikuLoggerOptions` now includes `SseOptions` property
- `MikuLogger` now broadcasts to SSE clients when enabled
- `MikuConsoleColorOptions` now includes `Extended256Colors` and `TrueColors` properties
- `ConsoleLogWriter` now supports all three color spaces with automatic ANSI detection
- Updated package description and tags to include SSE and extended colors

### Documentation
- Added comprehensive SSE documentation to README
- Added Extended256 and TrueColor color configuration examples
- Added JavaScript client example for SSE consumption
- Added predefined Miku color reference

---

## [10.1.39] - 2025-12-02

### Fixed
- MikuFileLogWriter now handles multi-instance scenarios correctly without IOException
- Eliminated data loss in high-concurrency logging scenarios
- Fixed file access conflicts when multiple MikuFileLogWriter instances write to the same file

### Added
- Singleton MikuSharedFileStreamManager for centralized file stream management
- MikuSharedFileStream wrapper with SemaphoreSlim for thread-safe write operations
- Only one FileStream per file across all MikuFileLogWriter instances (true singleton pattern)
- Comprehensive multi-instance tests (11 test scenarios including 2 multi-process tests)
- Performance optimization documentation
- Production best practices guide for multi-process logging

### Changed
- Implemented singleton pattern for shared file stream management
- Changed from individual FileStream per writer to centralized MikuSharedFileStreamManager
- Improved Dispose() method to guarantee flush of all remaining messages
- MikuFileLogWriter now uses MikuSharedFileStreamManager.GetOrCreateStream() for file access

### Performance
- Zero data loss guarantee even under high load
- Thread-safe write operations with SemaphoreSlim per file
- Batch writing (up to 100 messages per batch)
- Async I/O with 8KB buffer
- Efficient resource management through singleton pattern

---

## [10.0.39] - 2025-11-29

### Initial Release

A professional, thread-safe logging library for .NET 10 with extensive features and Microsoft.Extensions.Logging compatibility.

#### Core Features
- **ILogger Compatibility**: Implements `Microsoft.Extensions.Logging.ILogger` interface for seamless ASP.NET Core integration
- **Automatic Class Name Detection**: Logger automatically uses the calling class name if no category name is provided
- **Thread-Safe Operations**: All logging operations are thread-safe using semaphores
- **Async/Await Support**: Full asynchronous logging methods for better performance
- **Flexible Output Targets**: Console, File, or both simultaneously

#### Log Levels
- Trace (most detailed)
- Debug (development debugging)
- Information (general flow)
- Warning (abnormal events)
- Error (error events)
- Critical (critical failures)
- None (no logging)

#### Console Features
- **Colored Output**: Customizable colors for each log level
- **Multiple Color Spaces**: Support for standard 16 console colors (Extended256 and TrueColor prepared for future)
- **Thread-Safe Console Writing**: Synchronized console output

#### File Features
- **Async Queue-Based Writing**: High-performance non-blocking file operations
- **Automatic Log Rotation**: Rotate files based on configurable size limits
- **Date-Based Organization**: Optional date-based folder structure
- **File Retention**: Automatic cleanup of old log files
- **Configurable Naming**: Custom file name patterns

#### Format Options
- **Flexible Formatting**: Control visibility of Date, Time, LogLevel, and LoggerName
- **Custom Date Formats**: Fully configurable timestamp formats
- **UTC Support**: Option to use UTC time instead of local time
- **Minimal Overhead**: Only format what's needed

#### ASP.NET Core Integration
- **ILoggerProvider**: Full support for dependency injection
- **Extension Methods**: Easy setup with `AddMikuLogger()`
- **Configuration Support**: Integration with ASP.NET Core configuration system

#### Performance
- Background queue processing for file writes
- Efficient message formatting
- Minimal locking overhead
- Optimized for high-volume scenarios

#### Architecture
- Clean separation of concerns
- Extensible design
- SOLID principles
- Well-documented code with XML comments

### Dependencies
- `Microsoft.Extensions.Logging.Abstractions` v10.0.0
- `Microsoft.Extensions.Options` v10.0.0

### Documentation
- Comprehensive README with quick start guide
- Detailed EXAMPLES.md with various usage scenarios
- Full XML documentation for IntelliSense support

---

## Version Numbering

Miku.Logger follows this versioning pattern:

```
MAJOR.MINOR.39
```

- **MAJOR**: Matches .NET version (10, 11, 12, etc.)
- **MINOR**: Increments for new features and updates
- **39**: Always 39 in honor of Hatsune Miku (Mi-Ku)

**Examples**: 10.0.39 ? 10.1.39 ? 10.2.39 ? 10.3.39 ? 11.0.39

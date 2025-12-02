# Changelog

All notable changes to Miku.Logger will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/).

## [10.1.39] - 2025-12-02

### Fixed
- FileLogWriter now handles multi-instance scenarios correctly without IOException
- Eliminated data loss in high-concurrency logging scenarios
- Fixed file access conflicts when multiple FileLogWriter instances write to the same file

### Changed
- Implemented lock-free batch writing architecture for maximum performance
- Changed from single-message writes to batch writes (up to 100 messages per batch)
- Increased FileStream buffer size from 4KB to 8KB for better I/O performance
- Improved Dispose() method to guarantee flush of all remaining messages

### Performance
- 25x faster in multi-writer scenarios (0.9s vs 20s in tests)
- Throughput increased from ~200 msg/s to 5000+ msg/s
- Reduced CPU usage through batch processing
- Zero data loss guarantee even under high load

### Added
- Comprehensive multi-instance tests (9 test scenarios)
- Performance optimization documentation
- Production best practices guide for multi-process logging

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

**Examples**: 10.0.39 -> 10.1.39 -> 10.2.39 -> 11.0.39

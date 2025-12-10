# MikuLib

A collection of powerful .NET 10 libraries inspired by Hatsune Miku.

> "The world is mine... to code!" - Hatsune Nemas

*Powered by CV01 energy since 2007*

## What is MikuLib?

MikuLib is a comprehensive .NET 10 library collection that brings together powerful utilities for modern application development. Named after and inspired by Hatsune Miku, the virtual singer who revolutionized music and technology.

![MikuLib Console & Logger Demo](https://djnemas.de/SX/WindowsTerminal_Utmoe5RPDL.gif)

## ?? Breaking Changes in Version 10.2.39

**All enums and configuration models have been renamed with the `Miku` prefix.**

This change was made for better consistency across the library. When updating from earlier versions, you'll need to update your type references:

### Type Renames

| Old Name | New Name |
|----------|----------|
| `LogLevel` | `MikuLogLevel` |
| `LogOutput` | `MikuLogOutput` |
| `ColorSpace` | `MikuColorSpace` |
| `ConsoleColorOptions` | `MikuConsoleColorOptions` |
| `FileLoggerOptions` | `MikuFileLoggerOptions` |
| `LogFormatOptions` | `MikuLogFormatOptions` |
| `SseLoggerOptions` | `MikuSseLoggerOptions` |
| `Extended256ColorOptions` | `MikuExtended256ColorOptions` |
| `TrueColorOptions` | `MikuTrueColorOptions` |
| `RgbColor` | `MikuRgbColor` |
| `AnsiCodes` | `MikuAnsiCodes` |
| `ColorHelper` | `MikuColorHelper` |
| `SseLogBroadcaster` | `MikuSseLogBroadcaster` |
| `SseLogEntry` | `MikuSseLogEntry` |
| `FileLogWriter` | `MikuFileLogWriter` |
| `SharedFileStreamManager` | `MikuSharedFileStreamManager` |

### Migration Example

```csharp
// Before (10.1.39 and earlier)
var options = new MikuLoggerOptions
{
    Output = LogOutput.Console,
    MinimumLogLevel = LogLevel.Information,
    ConsoleColors = new ConsoleColorOptions
    {
        ColorSpace = ColorSpace.TrueColor
    }
};

// After (10.2.39)
var options = new MikuLoggerOptions
{
    Output = MikuLogOutput.Console,
    MinimumLogLevel = MikuLogLevel.Information,
    ConsoleColors = new MikuConsoleColorOptions
    {
        ColorSpace = MikuColorSpace.TrueColor
    }
};
```

## Libraries

### MikuLib.Core
**Core types and utilities** shared across all MikuLib packages:
- **MikuRgbColor**: 24-bit RGB color structure with hex parsing and interpolation
- **MikuAnsiCodes**: ANSI escape code constants for console styling
- **MikuColorHelper**: Color manipulation utilities (gradients, rainbow, darken/lighten)

[Documentation](./Miku.Core/README.md) | [Changelog](./Miku.Core/CHANGELOG.md)

### MikuLib.Console
**Beautiful colored console output** with TrueColor and 256-color support:
- **MikuConsole**: Colored text output with gradients and rainbow effects
- **MikuConsoleAnimation**: Typewriter, fade, pulse, wave, and loading animations

[Documentation](./MikuLib.Console/README.md) | [Examples](./MikuLib.Console/EXAMPLES.md) | [Changelog](./MikuLib.Console/CHANGELOG.md)

### MikuLib.Utils
**Comprehensive utility library** featuring:
- **MikuMapper**: Object-to-object mapping with nullable primitive support
- **CommandLineHelper**: Modern command line argument parsing with auto-detection

[Documentation](./Miku.Utils/README.md) | [Examples](./Miku.Utils/EXAMPLES.md) | [Changelog](./Miku.Utils/CHANGELOG.md)

### MikuLib.Logger
**Thread-safe logging library** featuring:
- Console and file output with customizable colors
- **Server-Sent Events (SSE)** for real-time log streaming
- **TrueColor (24-bit RGB)** and **256-color** support
- ILogger compatibility for ASP.NET Core
- Async operations and automatic log rotation
- Zero data loss guarantee

[Documentation](./Miku.Logger/README.md) | [Examples](./Miku.Logger/EXAMPLES.md) | [Changelog](./Miku.Logger/CHANGELOG.md)

### MikuLib (Meta Package)
**Convenience package** that includes all Miku libraries for easy installation.

[Documentation](./Miku/README.md)

## Installation

Install individual libraries:

```bash
dotnet add package MikuLib.Core
dotnet add package MikuLib.Console
dotnet add package MikuLib.Utils
dotnet add package MikuLib.Logger
```

Or install the meta package to get everything:

```bash
dotnet add package MikuLib
```

Add to your `.csproj`:
```xml
<PackageReference Include="MikuLib" Version="10.2.39" />
<!-- Or individual packages -->
<PackageReference Include="MikuLib.Core" Version="10.0.39" />
<PackageReference Include="MikuLib.Console" Version="10.0.39" />
<PackageReference Include="MikuLib.Utils" Version="10.1.39" />
<PackageReference Include="MikuLib.Logger" Version="10.2.39" />
```

## Quick Start

### Core Color Types
```csharp
using Miku.Core;

// Create colors
var cyan = MikuRgbColor.MikuCyan;           // Predefined Miku cyan
var pink = MikuRgbColor.FromHex("#E12885"); // From hex string

// Interpolate colors
var blend = MikuRgbColor.Lerp(cyan, pink, 0.5);

// Rainbow effects
var rainbow = MikuColorHelper.GetRainbow(phase);
var mikuRainbow = MikuColorHelper.GetMikuRainbow(phase);
```

### Console Output with Colors
```csharp
using Miku.Core;
using MikuLib.Console;

// Colored output
MikuConsole.WriteLine("Hello Miku!", MikuRgbColor.MikuCyan);
MikuConsole.WriteGradientLine("Gradient text", MikuRgbColor.MikuCyan, MikuRgbColor.MikuPink);

// Animations
await MikuConsoleAnimation.TypewriterAsync("Typing...", MikuRgbColor.MikuCyan, 30);
await MikuConsoleAnimation.PulseAsync("Pulsing!", MikuRgbColor.MikuCyan, MikuRgbColor.MikuPink, 0, 0);
```

### Logging with Colors
```csharp
using Miku.Core;
using Miku.Logger;
using Miku.Logger.Configuration;
using Miku.Logger.Configuration.Enums;
using Miku.Logger.Configuration.Models;

var options = new MikuLoggerOptions
{
    Output = MikuLogOutput.Console,
    ConsoleColors = new MikuConsoleColorOptions
    {
        ColorSpace = MikuColorSpace.TrueColor,
        TrueColors = new MikuTrueColorOptions
        {
            InformationColor = MikuRgbColor.MikuCyan,
            ErrorColor = MikuRgbColor.MikuPink
        }
    }
};

using var logger = new MikuLogger("MyApp", options);
logger.LogInformation("Hello with Miku Cyan!");
```

### Object Mapping
```csharp
using Miku.Utils;

var userDto = MikuMapper.MapProperties<UserDto>(user);
```

## Features at a Glance

### Core Types (MikuLib.Core)
- 24-bit RGB color structure with hex parsing
- Color interpolation and manipulation
- ANSI escape codes for console styling
- Rainbow and gradient generators
- Predefined Miku colors (Cyan, Pink, Teal, DarkCyan)

### Console Utilities (MikuLib.Console)
- TrueColor (16 million colors) console output
- 256-color palette support
- Gradient text and progress bars
- Typewriter, fade, pulse, and wave animations
- Centered and positioned text output
- Block drawing characters and boxes

### Utility Functions (MikuLib.Utils)
- Object-to-object property mapping
- Nullable primitive support
- Command line argument parsing
- Environment detection

### Logging (MikuLib.Logger)
- Three color modes: Console (16), Extended256 (256), TrueColor (16M)
- Server-Sent Events for real-time streaming
- Multiple output targets (Console, File, SSE)
- Thread-safe async operations
- Automatic log rotation
- ILogger compatibility

## Predefined Miku Colors

| Color | Hex | Description |
|-------|-----|-------------|
| MikuCyan | #00CED1 | Signature cyan |
| MikuPink | #E12885 | Secondary pink |
| MikuTeal | #39C5BB | Hair highlight |
| MikuDarkCyan | #008B8B | Dark accent |

## Version Numbering

MikuLib follows this versioning pattern:

```
MAJOR.MINOR.39
  |     |    +--- Always 39 (Mi-Ku Easter Egg)
  |     +-------- Increments for new features/updates
  +-------------- Matches .NET version (10, 11, 12, etc.)
```

**Current Versions:**
- MikuLib.Core: 10.0.39
- MikuLib.Console: 10.0.39
- MikuLib.Utils: 10.1.39
- MikuLib.Logger: 10.2.39

## Requirements

- .NET 10.0 or higher
- C# 14.0

## Why "Miku"?

This library is named after and inspired by **Hatsune Miku**, the virtual singer:

- **Born**: August 31st, 2007
- **Created by**: Crypton Future Media
- **Series**: Character Vocal Series 01 (CV01)
- **Number**: 39 (Mi-Ku in Japanese goroawase)
- **Age**: 16 years old
- **Signature Color**: Cyan (#00CED1)

Just as Miku revolutionized music through technology, these libraries aim to make development more efficient and harmonious.

## Easter Eggs

This library contains subtle Hatsune Miku references throughout:

- Version numbers ending in 39 (Mi-Ku)
- Default colors inspired by Miku's aesthetic
- CV01 references in code and documentation
- Constants using the number 39
- Predefined Miku color palette

For those who know, you know.

## Documentation

### MikuLib.Core
- [README](./Miku.Core/README.md) - Full documentation
- [CHANGELOG](./Miku.Core/CHANGELOG.md) - Version history

### MikuLib.Console
- [README](./MikuLib.Console/README.md) - Full documentation
- [EXAMPLES](./MikuLib.Console/EXAMPLES.md) - Practical usage examples
- [CHANGELOG](./MikuLib.Console/CHANGELOG.md) - Version history

### MikuLib.Utils
- [README](./Miku.Utils/README.md) - Full documentation
- [EXAMPLES](./Miku.Utils/EXAMPLES.md) - Practical usage examples
- [CHANGELOG](./Miku.Utils/CHANGELOG.md) - Version history

### MikuLib.Logger
- [README](./Miku.Logger/README.md) - Full documentation
- [EXAMPLES](./Miku.Logger/EXAMPLES.md) - Practical usage examples
- [CHANGELOG](./Miku.Logger/CHANGELOG.md) - Version history

### MikuLib (Meta Package)
- [README](./Miku/README.md) - Package information

## Contributing

Contributions are welcome! Please:

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## Credits

**Created by**: Hatsune Nemas

**Inspired by**:
- Hatsune Miku - The virtual diva who started it all
- Crypton Future Media - For creating VOCALOID technology
- The global Vocaloid community - For endless creativity and passion

## License

MIT License - See [LICENSE](LICENSE) file for details.

## Links

- [GitHub Repository](https://github.com/DjNemas/MikuLib)
- [Issue Tracker](https://github.com/DjNemas/MikuLib/issues)
- [NuGet Gallery](https://www.nuget.org/profiles/HatsuneNemas)

## Repository Structure

```
MikuLib/
? Miku/                   # Meta package
? Miku.Core/              # Core types (MikuRgbColor, MikuAnsiCodes, MikuColorHelper)
? MikuLib.Console/        # Console utilities (MikuConsole, MikuConsoleAnimation)
? Miku.Utils/             # Utility library (MikuMapper, CommandLineHelper)
? Miku.Logger/            # Logging library with SSE and TrueColor support
? Miku.*.Tests/           # Unit tests
? MikuConsoleAndLogger.Preview/  # Color demo application
? README.md               # This file
```

---

*"Singing the code since 2007!"*

**Version**: 10.2.39 (The Mi-Ku Edition)  
**Series**: CV01 Developer Tools  
**Default Color**: Cyan (#00CED1)

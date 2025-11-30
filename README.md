# MikuLib

A collection of powerful .NET 10 libraries inspired by Hatsune Miku.

> "The world is mine... to code!" - Hatsune Nemas

*Powered by CV01 energy since 2007*

## What is MikuLib?

MikuLib is a comprehensive .NET 10 library collection that brings together powerful utilities for modern application development. Named after and inspired by Hatsune Miku, the virtual singer who revolutionized music and technology.

## Libraries

### MikuLib.Utils
**Comprehensive utility library** featuring:
- **MikuMapper**: Object-to-object mapping with nullable primitive support
- **CommandLineHelper**: Modern command line argument parsing with auto-detection

[Documentation](./Miku.Utils/README.md) | [Examples](./Miku.Utils/EXAMPLES.md) | [Changelog](./Miku.Utils/CHANGELOG.md)

### MikuLib.Logger
**Thread-safe logging library** featuring:
- Console and file output with customizable colors
- ILogger compatibility for ASP.NET Core
- Async operations and automatic log rotation
- Date-based folder organization

[Documentation](./Miku.Logger/README.md) | [Examples](./Miku.Logger/EXAMPLES.md) | [Changelog](./Miku.Logger/CHANGELOG.md)

### MikuLib (Meta Package)
**Convenience package** that includes all Miku libraries for easy installation.

[Documentation](./Miku/README.md)

## Installation

Install individual libraries:

```bash
dotnet add package MikuLib.Utils
dotnet add package MikuLib.Logger
```

Or install the meta package to get everything:

```bash
dotnet add package MikuLib
```

Add to your `.csproj`:
```xml
<PackageReference Include="MikuLib" Version="10.0.39" />
<!-- Or individual packages -->
<PackageReference Include="MikuLib.Utils" Version="10.0.39" />
<PackageReference Include="MikuLib.Logger" Version="10.0.39" />
```

## Quick Start

```csharp
using Miku.Utils;
using Miku.Logger;
using Miku.Logger.Configuration;

// Object Mapping
var userDto = MikuMapper.MapPropertys<UserDto>(user);

// Command Line Parsing
if (CommandLineHelper.IsReleaseConfiguration())
{
    // Use release settings
}

// Logging
var logger = new MikuLogger("MyApp");
logger.LogInformation("Application started");
```

## Features at a Glance

### Core Utilities (MikuLib.Utils)
- Property mapping with automatic type conversion
- Nullable primitive support (int ? int?, bool ? bool?, etc.)
- Command line argument parsing (--param, --param:value, --param=value)
- Multi-value parameter support
- Environment detection
- Auto-detection via Environment.GetCommandLineArgs()

### Logging (MikuLib.Logger)
- Multiple output targets (Console, File, Both)
- Thread-safe async operations
- Colored console output (Cyan by default - #00CED1)
- Automatic log rotation based on file size
- Date-based folder organization
- Configurable log levels and formats
- ILogger compatibility for ASP.NET Core

### Performance
- High-performance reflection-based mapping
- LINQ-optimized command line parsing
- Async queue-based file I/O
- Thread-safe operations throughout
- Minimal allocation overhead

## Version Numbering

MikuLib follows this versioning pattern:

```
MAJOR.MINOR.39
  |     |    ???? Always 39 (Mi-Ku Easter Egg)
  |     ????????? Increments for new features/updates
  ??????????????? Matches .NET version (10, 11, 12, etc.)
```

**Current Version**: 10.0.39

**Examples:**
- `10.0.39` - Initial .NET 10 release
- `10.1.39` - Feature update for .NET 10
- `10.2.39` - Another feature update
- `11.0.39` - .NET 11 release

## Requirements

- .NET 10.0 or higher
- C# 14.0

## Why "Miku"?

This library is named after and inspired by **Hatsune Miku** (????), the virtual singer:

- **Born**: August 31st, 2007
- **Created by**: Crypton Future Media
- **Series**: Character Vocal Series 01 (CV01)
- **Number**: 39 (Mi-Ku in Japanese goroawase)
- **Age**: 16 years old
- **Signature Color**: Cyan (#00CED1)

Just as Miku revolutionized music through technology, these libraries aim to make development more efficient and harmonious.

## Easter Eggs

This library contains subtle Hatsune Miku references throughout:

- Version 10.0.39 (10 for .NET 10, 39 for Mi-Ku)
- Default colors inspired by Miku's aesthetic
- CV01 references in code and documentation
- Comments with song references
- Constants using the number 39

For those who know, you know.

## Philosophy

> "The future of voice, the future of code!"

MikuLib aims to:
- Be accessible to developers worldwide
- Work in harmony with existing .NET ecosystems
- Stay modern and future-proof
- Bring efficiency and joy to coding

## Documentation

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
- Hatsune Miku (????) - The virtual diva who started it all
- Crypton Future Media - For creating VOCALOID technology
- The global Vocaloid community - For endless creativity and passion

**Songs that inspired development**:
- "World is Mine" - For confidence to build
- "Tell Your World" - For sharing our code
- "Senbonzakura" - For epic scale

## License

MIT License - See [LICENSE](LICENSE) file for details.

## Links

- [GitHub Repository](https://github.com/DjNemas/MikuLib)
- [Issue Tracker](https://github.com/DjNemas/MikuLib/issues)
- [Discussions](https://github.com/DjNemas/MikuLib/discussions)
- [NuGet Gallery](https://www.nuget.org/profiles/HatsuneNemas)

## Repository Structure

```
MikuLib/
??? Miku/                   # Meta package
??? Miku.Utils/             # Utility library (MikuMapper, CommandLineHelper)
??? Miku.Logger/            # Logging library
??? MikuLib.sln             # Solution file
??? README.md               # This file
```

---

*"Singing the code since 2007!"*

**Version**: 10.0.39 (The Mi-Ku Edition)  
**Series**: CV01 Developer Tools  
**Default Color**: Cyan (#00CED1)

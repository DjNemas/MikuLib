# MikuLib

> "The world is mine... and so are these utilities!" - Hatsune Nemas

*Singing since August 31st, 2007 - Coding since 2025*

A collection of powerful .NET 10 libraries inspired by Hatsune Miku, the virtual singer who revolutionized music and technology.

[![.NET Version](https://img.shields.io/badge/.NET-10.0-512BD4)](https://dotnet.microsoft.com/)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)
[![NuGet](https://img.shields.io/badge/nuget-10.1.39-blue)](https://www.nuget.org/profiles/HatsuneNemas)

## What is MikuLib?

MikuLib is a comprehensive .NET 10 library collection that brings together powerful utilities for modern application development. Just as Hatsune Miku revolutionized music through technology, these libraries aim to make development more efficient and harmonious.

## Libraries

### MikuLib.Logger [![NuGet](https://img.shields.io/badge/nuget-10.1.39-blue)](https://www.nuget.org/packages/MikuLib.Logger)

High-performance, thread-safe logging library with zero data loss guarantee.

**Key Features:**
- **Singleton File Stream Management** - Safe multi-instance logging
- **Zero Data Loss** - Guaranteed message delivery
- **High Performance** - 5000+ msg/s throughput
- **Lock-Free Architecture** - ConcurrentQueue with batch writing
- **ILogger Compatible** - Seamless ASP.NET Core integration
- **Colored Console Output** - Customizable per log level
- **Automatic Log Rotation** - Size-based with file retention

**Quick Start:**
```csharp
using Miku.Logger;

var logger = new MikuLogger("MyApp");
logger.LogInformation("Application started");
```

[Full Documentation](Miku.Logger/README.md) | [Examples](Miku.Logger/EXAMPLES.md) | [Changelog](Miku.Logger/CHANGELOG.md)

---

### MikuLib.Utils [![NuGet](https://img.shields.io/badge/nuget-10.0.39-blue)](https://www.nuget.org/packages/MikuLib.Utils)

Essential utilities for .NET applications.

**Key Features:**
- **MikuMapper** - High-performance object-to-object mapping
- **CommandLineHelper** - Modern command line argument parsing
- **Nullable Support** - Seamless int ? int?, bool ? bool? conversion

**Quick Start:**
```csharp
using Miku.Utils;

// Object mapping
var userDto = MikuMapper.MapPropertys<UserDto>(user);

// Command line parsing
if (CommandLineHelper.IsReleaseConfiguration())
{
    // Use release settings
}
```

[Full Documentation](Miku.Utils/README.md) | [Examples](Miku.Utils/EXAMPLES.md) | [Changelog](Miku.Utils/CHANGELOG.md)

---

## Installation

### Install all libraries (recommended)

```bash
dotnet add package MikuLib --version 10.1.39
```

### Install individual libraries

```bash
dotnet add package MikuLib.Logger --version 10.1.39
dotnet add package MikuLib.Utils --version 10.0.39
```

## Performance Highlights

- **25x faster** multi-writer scenarios
- **Zero data loss** under high load
- **Lock-free** architecture
- **Batch writing** (100 messages per operation)
- **Thread-safe** operations throughout

See [Performance Optimization](PERFORMANCE_OPTIMIZATION.md) for detailed benchmarks.

## Version Numbering

MikuLib follows a unique versioning pattern:

```
MAJOR.MINOR.39
  |     |    ???? Always 39 (Mi-Ku in Japanese)
  |     ????????? Feature updates
  ??????????????? Matches .NET version
```

**Current Version:** 10.1.39

## Why "39"?

In Japanese wordplay (goroawase), 3 can be read as "mi" and 9 as "ku", forming "Miku" (??). The number 39 is deeply associated with Hatsune Miku and represents her identity in the Vocaloid community.

## Why "Miku"?

This library is inspired by **Hatsune Miku** (????):

- **Born:** August 31st, 2007
- **Created by:** Crypton Future Media
- **Character Vocal Series:** CV01
- **Signature Color:** Cyan (#00CED1)

Just as Miku gave voice to creativity, MikuLib aims to give voice to your code.

## Requirements

- .NET 10.0 or higher
- C# 14.0

## Documentation

- [MikuLib.Logger Documentation](Miku.Logger/README.md)
- [MikuLib.Utils Documentation](Miku.Utils/README.md)
- [Performance Optimization](PERFORMANCE_OPTIMIZATION.md)
- [Test Suite](TEST_SUITE_README.md)

## Contributing

Contributions are welcome! Please:

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## License

MIT License - See [LICENSE](LICENSE) file for details.

## Credits

**Created by:** Hatsune Nemas

**Inspired by:**
- Hatsune Miku (????) - CV01
- Crypton Future Media
- The global Vocaloid community

**Songs that inspired development:**
- "World is Mine" - For confidence to build
- "Tell Your World" - For sharing our code
- "Senbonzakura" - For epic scale

## Links

- [GitHub Repository](https://github.com/DjNemas/MikuLib)
- [Issue Tracker](https://github.com/DjNemas/MikuLib/issues)
- [Discussions](https://github.com/DjNemas/MikuLib/discussions)
- [NuGet Profile](https://www.nuget.org/profiles/HatsuneNemas)

---

*"The future of voice, the future of code!"*

**Version:** 10.1.39 (CV01 Edition) | **Default Color:** Cyan (#00CED1)

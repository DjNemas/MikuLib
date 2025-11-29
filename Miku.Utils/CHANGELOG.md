# Changelog

All notable changes to Miku.Utils will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/).

## [10.0.39] - 2025-11-29

### Initial Release

A comprehensive utility library for .NET 10 with essential tools for modern application development.

#### MikuMapper Features
- **Property Mapping**: Automatic property mapping between objects using reflection
- **Nullable Primitive Support**: Seamless conversion between nullable and non-nullable primitives
  - Supports: `int ? int?`, `bool ? bool?`, `decimal ? decimal?`, etc.
- **Property Exclusion**: Exclude specific properties from mapping
- **Collection Mapping**: Map entire collections with a single call
- **Update Existing Objects**: Map properties to existing instances
- **Null Handling**: Configurable null value handling (ignore or map)
- **Type Compatibility**: Automatic detection of compatible types

#### CommandLineHelper Features
- **Modern Parsing**: LINQ-based efficient argument parsing
- **Auto-Detection**: Automatic use of `Environment.GetCommandLineArgs()` when no args provided
- **Multiple Formats**: Support for `--param value`, `--param:value`, `--param=value`, `/param value`
- **Multi-Value Support**: Handle parameters that appear multiple times
  - `ParseArguments()` - Single value (overwrites duplicates)
  - `ParseArgumentsWithMultipleValues()` - Collects all values
  - `Parse()` - Modern API with `ParsedArguments` class
- **Configuration Detection**: Built-in `IsReleaseConfiguration()` and `IsDebugConfiguration()`
- **Environment Integration**: Check command line args AND environment variables
- **Case-Insensitive**: Uses `OrdinalIgnoreCase` by default for better compatibility
- **Optional Parameters**: All methods have optional `args` parameter

#### Architecture
- Clean, maintainable code following SOLID principles
- Comprehensive XML documentation for IntelliSense
- Thread-safe operations
- High performance with minimal overhead
- Fully nullable-aware (.NET 10)

### Dependencies
None - Pure .NET 10 implementation

### Documentation
- Comprehensive README with quick start guide
- EXAMPLES.md with practical usage scenarios
- Full XML documentation for all public APIs

---

## Version Numbering

Miku.Utils follows this versioning pattern:

```
MAJOR.MINOR.39
```

- **MAJOR**: Matches .NET version (10, 11, 12, etc.)
- **MINOR**: Increments for new features and updates
- **39**: Always 39 in honor of Hatsune Miku (Mi-Ku)

**Examples**: 10.0.39 ? 10.1.39 ? 10.2.39 ? 11.0.39

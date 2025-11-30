# MikuLib Test Suite

## ?? Overview

This test suite provides comprehensive tests for all MikuLib projects. All tests use xUnit as the test framework and are written for .NET 10.0.

## ?? Test Statistics

**Total: 100 Tests**
- ? Passed: 100
- ? Failed: 0
- ?? Skipped: 0

## ?? Test Projects

### 1. Miku.Utils.Tests (40 Tests)

Tests for the utility library:

#### MikuMapperTests (18 Tests)
- Property mapping between objects
- Nullable/Non-Nullable conversions
- Collection mapping
- Property exclusion
- Null handling
- ArgumentNullException tests

#### CommandLineHelperTests (22 Tests)
- Parameter detection
- Value extraction
- Release/Debug configuration
- Multiple values
- Various parameter formats (--param value, --param:value, --param=value)
- ParsedArguments functionality

### 2. Miku.Logger.Tests (52 Tests)

Tests for the logging framework:

#### MikuLoggerTests (25 Tests)
- Logger initialization
- All log levels (Trace, Debug, Info, Warning, Error, Critical)
- Synchronous and asynchronous logging methods
- Exception logging
- File output
- Format options
- ILogger interface compatibility
- Dispose pattern

#### MikuLoggerOptionsTests (20 Tests)
- Default values for all option classes
- Custom values
- LogOutput flags
- LogLevel ordering
- Color configurations
- File options

#### MikuLoggerProviderTests (9 Tests)
- Provider creation
- Logger factory integration
- Singleton pattern for same categories
- Dispose handling
- Null argument validation

#### MikuLoggerExtensionsTests (8 Tests)
- AddMikuLogger extension methods
- Integration with ILoggingBuilder
- Service collection integration
- Configuration via Action<MikuLoggerOptions>
- Multiple logger categories

### 3. Miku.Tests (8 Tests)

Tests for the main package:

#### MikuLibraryTests
- Reference validation
- Package integration
- MikuMapper access via main library
- CommandLineHelper access via main library
- MikuLogger access via main library
- Miku constants (Number 39, Age 16, Birth date)

## ?? Running Tests

### All Tests
```bash
dotnet test
```

### Test Individual Project
```bash
dotnet test Miku.Utils.Tests
dotnet test Miku.Logger.Tests
dotnet test Miku.Tests
```

### With Code Coverage
```bash
dotnet test --collect:"XPlat Code Coverage"
```

### Specific Tests Only
```bash
dotnet test --filter "FullyQualifiedName~MikuMapper"
```

## ?? Test Categories

### Unit Tests
- Isolated tests of individual components
- Mocking of dependencies (where necessary)
- Fast execution

### Integration Tests
- Logger with Dependency Injection
- Extension methods with ServiceCollection
- File I/O tests

### Edge Cases
- Null values
- Empty collections
- Exception handling
- Boundary conditions

## ?? Test Coverage

The test suite covers the following areas:

### Miku.Utils
- ? MikuMapper: All public methods
- ? CommandLineHelper: All public methods
- ? ParsedArguments: All public methods

### Miku.Logger
- ? MikuLogger: All log levels and methods
- ? MikuLoggerOptions: All configurations
- ? MikuLoggerProvider: Factory pattern
- ? MikuLoggerExtensions: DI integration
- ? ConsoleLogWriter: Indirectly through MikuLogger
- ? FileLogWriter: Indirectly through MikuLogger

### Miku (Main Package)
- ? Package references
- ? Public API access
- ? Integration between sub-projects

## ?? Maintenance

### Adding New Tests
1. Create a new test class in the corresponding project
2. Use the `[Fact]` or `[Theory]` attributes
3. Follow the AAA pattern (Arrange, Act, Assert)
4. Use descriptive test names

### Best Practices
- One test per scenario
- Independent tests (no dependencies between tests)
- Cleanup in `Dispose()` or with `IDisposable`
- Meaningful assertions
- Documentation of complex test scenarios

## ?? CV01 Edition Notes

All tests were created with the same precision and attention to detail as Hatsune Miku sings her songs. Version 10.0.39 - The world is mine... to test! ????

---
*Born on August 31st, 2007 - Testing since 2025*

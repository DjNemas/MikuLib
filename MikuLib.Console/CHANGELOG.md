# Changelog

All notable changes to MikuLib.Console will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/).

## [10.0.39] - 2025-12-06

### Initial Release

Beautiful colored console output with TrueColor and 256-color support.

#### MikuConsole - Colored Output

**Write Methods**
- `Write(string)` / `WriteLine(string)` - Plain text output
- `Write(string, RgbColor)` / `WriteLine(string, RgbColor)` - TrueColor output
- `Write(string, RgbColor, RgbColor)` - With foreground and background
- `Write256(string, byte)` / `WriteLine256(string, byte)` - 256-color output

**Styled Text**
- `WriteBold(string, RgbColor)` - Bold text
- `WriteUnderline(string, RgbColor)` - Underlined text
- `WriteItalic(string, RgbColor)` - Italic text
- `WriteStyled(string, RgbColor, params string[])` - Custom ANSI styles

**Gradient and Rainbow**
- `WriteGradient(string, RgbColor, RgbColor)` - Horizontal gradient
- `WriteGradientLine(string, RgbColor, RgbColor)` - Gradient with newline
- `WriteRainbow(string, double)` / `WriteRainbowLine(string, double)` - Rainbow colors
- `WriteMikuRainbow(string, double)` / `WriteMikuRainbowLine(string, double)` - Miku-themed rainbow

**Positioning**
- `SetCursorPosition(int, int)` - Set cursor position
- `CursorLeft` / `CursorTop` - Get cursor position
- `WriteAt(int, int, string)` - Write at position (plain)
- `WriteAt(int, int, string, RgbColor)` - Write at position (colored)
- `WriteCentered(string, RgbColor)` / `WriteCenteredLine(string, RgbColor)` - Centered text

**Drawing**
- `DrawBar(int, RgbColor, char)` - Solid color bar
- `DrawGradientBar(int, RgbColor, RgbColor, char)` - Gradient bar
- `DrawBox(int, int, int, int, RgbColor)` - Box with Unicode borders

**Console Control**
- `Initialize()` - Setup UTF-8 encoding
- `Clear()` - Clear screen
- `HideCursor()` / `ShowCursor()` - Cursor visibility
- `ResetColors()` - Reset ANSI colors
- `ResetConsoleColors()` - Reset console colors
- `WindowWidth` / `WindowHeight` - Console dimensions
- `SupportsAnsi` - ANSI support detection

#### MikuConsoleAnimation - Animation Effects

**Typewriter Effects**
- `TypewriterAsync()` - Character-by-character typing
- `TypewriterLineAsync()` - Typewriter with newline
- `TypewriterGradientAsync()` - Typewriter with gradient colors

**Fade Effects**
- `FadeInAsync()` - Fade in from black
- `FadeOutAsync()` - Fade out to black

**Pulse Effects**
- `PulseAsync()` - Pulse between two colors
- `BreathingAsync()` - Smooth breathing effect

**Wave Effects**
- `ColorWaveAsync()` - Color wave across text
- `RainbowWaveAsync()` - Rainbow wave animation

**Loading Indicators**
- `SpinnerAsync()` - Animated spinner
- `ProgressBarAsync()` - Animated progress bar

**Screen Effects**
- `RevealLinesAsync()` - Line-by-line reveal
- `RevealLinesAlternatingAsync()` - Alternating color reveal

#### Features
- Thread-safe console operations with locking
- Automatic ANSI support detection
- CancellationToken support for all animations
- Configurable timing for all effects
- Uses Miku.Core for color types (RgbColor, AnsiCodes, ColorHelper)

---

## Version Numbering

MikuLib.Console follows this versioning pattern:

```
MAJOR.MINOR.39
```

- **MAJOR**: Matches .NET version (10, 11, 12, etc.)
- **MINOR**: Increments for new features and updates
- **39**: Always 39 in honor of Hatsune Miku (Mi-Ku)

**Examples**: 10.0.39 -> 10.1.39 -> 11.0.39

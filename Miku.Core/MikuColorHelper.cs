namespace Miku.Core
{
    /// <summary>
    /// Provides color manipulation utilities.
    /// </summary>
    /// <remarks>
    /// Like the light shows at Miku's concerts,
    /// create stunning color effects!
    /// </remarks>
    public static class MikuColorHelper
    {
        /// <summary>
        /// Interpolates between two colors.
        /// </summary>
        public static MikuRgbColor Lerp(MikuRgbColor from, MikuRgbColor to, double t)
            => MikuRgbColor.Lerp(from, to, t);

        /// <summary>
        /// Gets a rainbow color based on phase.
        /// </summary>
        /// <param name="phase">Phase in radians.</param>
        public static MikuRgbColor GetRainbow(double phase)
        {
            double r = Math.Sin(phase) * 127 + 128;
            double g = Math.Sin(phase + 2.094) * 127 + 128; // 2pi/3
            double b = Math.Sin(phase + 4.189) * 127 + 128; // 4pi/3
            return new MikuRgbColor((byte)r, (byte)g, (byte)b);
        }

        /// <summary>
        /// Gets a Miku-themed rainbow color (biased towards cyan/pink).
        /// </summary>
        /// <param name="phase">Phase in radians.</param>
        /// <param name="mikuBlend">Blend factor towards Miku colors (0.0 - 1.0). Default: 0.3</param>
        public static MikuRgbColor GetMikuRainbow(double phase, double mikuBlend = 0.3)
        {
            var rainbow = GetRainbow(phase);
            var mikuColor = phase % 2 < 1 ? MikuRgbColor.MikuCyan : MikuRgbColor.MikuPink;
            return Lerp(rainbow, mikuColor, mikuBlend);
        }

        /// <summary>
        /// Creates a gradient array of colors.
        /// </summary>
        public static MikuRgbColor[] CreateGradient(MikuRgbColor from, MikuRgbColor to, int steps)
        {
            if (steps < 2)
                throw new ArgumentException("Steps must be at least 2.", nameof(steps));

            var gradient = new MikuRgbColor[steps];
            for (int i = 0; i < steps; i++)
            {
                gradient[i] = Lerp(from, to, i / (double)(steps - 1));
            }
            return gradient;
        }

        /// <summary>
        /// Creates a multi-stop gradient array.
        /// </summary>
        public static MikuRgbColor[] CreateMultiGradient(MikuRgbColor[] colors, int stepsPerSegment)
        {
            if (colors.Length < 2)
                throw new ArgumentException("At least 2 colors required.", nameof(colors));

            var result = new List<MikuRgbColor>();

            for (int i = 0; i < colors.Length - 1; i++)
            {
                var segment = CreateGradient(colors[i], colors[i + 1], stepsPerSegment);
                result.AddRange(i == 0 ? segment : segment.Skip(1));
            }

            return [.. result];
        }

        /// <summary>
        /// Darkens a color by a factor.
        /// </summary>
        /// <param name="color">The color to darken.</param>
        /// <param name="factor">Factor (0.0 = black, 1.0 = original).</param>
        public static MikuRgbColor Darken(MikuRgbColor color, double factor)
        {
            factor = Math.Clamp(factor, 0, 1);
            return new MikuRgbColor(
                (byte)(color.R * factor),
                (byte)(color.G * factor),
                (byte)(color.B * factor)
            );
        }

        /// <summary>
        /// Lightens a color by a factor.
        /// </summary>
        /// <param name="color">The color to lighten.</param>
        /// <param name="factor">Factor (0.0 = original, 1.0 = white).</param>
        public static MikuRgbColor Lighten(MikuRgbColor color, double factor)
        {
            factor = Math.Clamp(factor, 0, 1);
            return new MikuRgbColor(
                (byte)(color.R + (255 - color.R) * factor),
                (byte)(color.G + (255 - color.G) * factor),
                (byte)(color.B + (255 - color.B) * factor)
            );
        }

        /// <summary>
        /// Gets the complementary color.
        /// </summary>
        public static MikuRgbColor GetComplementary(MikuRgbColor color)
            => new((byte)(255 - color.R), (byte)(255 - color.G), (byte)(255 - color.B));

        /// <summary>
        /// Calculates perceived brightness (0-255).
        /// </summary>
        public static int GetBrightness(MikuRgbColor color)
            => (int)(0.299 * color.R + 0.587 * color.G + 0.114 * color.B);

        /// <summary>
        /// Determines if a color is considered "dark" (brightness less than 128).
        /// </summary>
        public static bool IsDark(MikuRgbColor color)
            => GetBrightness(color) < 128;
    }
}

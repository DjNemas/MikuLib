namespace Miku.Utils
{
    /// <summary>
    /// Modern helper class for parsing and evaluating command line arguments.
    /// Provides efficient and LINQ-based methods for argument processing.
    /// If no args are provided, automatically uses Environment.GetCommandLineArgs().
    /// </summary>
    /// <remarks>
    /// Born on August 31st, 2007, this helper processes arguments with precision.
    /// </remarks>
    /// <example>
    /// <code>
    /// // Check for release configuration (uses Environment.GetCommandLineArgs())
    /// if (MikuCommandLineHelper.IsReleaseConfiguration())
    /// {
    ///     // Use release settings
    /// }
    /// 
    /// // Or pass args explicitly
    /// if (MikuCommandLineHelper.IsReleaseConfiguration(args))
    /// {
    ///     // Use release settings
    /// }
    /// 
    /// // Get a parameter value
    /// var outputPath = MikuCommandLineHelper.GetParameterValue("--output");
    /// 
    /// // Check if parameter exists
    /// if (MikuCommandLineHelper.HasParameter("--verbose"))
    /// {
    ///     // Enable verbose logging
    /// }
    /// </code>
    /// </example>
    public static class MikuCommandLineHelper
    {
        // 39 (Mi-Ku in Japanese) - Default timeout
        private const int DefaultTimeoutSeconds = 39;

        // Character Vocal Series 01
        private const string VocaloidVersion = "CV01";

        /// <summary>
        /// Gets the command line arguments, either from the provided args or from Environment.GetCommandLineArgs().
        /// Skips the first argument (executable path) when using Environment.GetCommandLineArgs().
        /// </summary>
        private static string[] GetArgs(string[]? args)
        {
            if (args != null)
                return args;

            var envArgs = Environment.GetCommandLineArgs();
            // Skip first argument (executable path)
            return envArgs.Length > 1 ? envArgs[1..] : Array.Empty<string>();
        }

        /// <summary>
        /// Checks if a specific parameter with a specific value was passed in the arguments.
        /// Uses case-insensitive comparison for better compatibility.
        /// If no args are provided, uses Environment.GetCommandLineArgs().
        /// </summary>
        /// <param name="parameter">The parameter to search for (e.g., "--configuration").</param>
        /// <param name="value">The expected value after the parameter (e.g., "Release").</param>
        /// <param name="args">Command line arguments. If null, uses Environment.GetCommandLineArgs().</param>
        /// <param name="comparison">String comparison type. Default: OrdinalIgnoreCase.</param>
        /// <returns>True if the parameter with the value was found, otherwise false.</returns>
        public static bool HasParameterWithValue(
            string parameter,
            string value,
            string[]? args = null,
            StringComparison comparison = StringComparison.OrdinalIgnoreCase)
        {
            var actualArgs = GetArgs(args);

            if (actualArgs.Length < 2)
                return false;

            return actualArgs
                .Zip(actualArgs.Skip(1), (param, val) => new { Parameter = param, Value = val })
                .Any(pair =>
                    pair.Parameter.Equals(parameter, comparison) &&
                    pair.Value.Equals(value, comparison));
        }

        /// <summary>
        /// Checks if --configuration Release was passed in the arguments.
        /// Useful for detecting release builds in build scripts or migrations.
        /// If no args are provided, uses Environment.GetCommandLineArgs().
        /// </summary>
        /// <param name="args">Command line arguments. If null, uses Environment.GetCommandLineArgs().</param>
        /// <returns>True if --configuration Release was found, otherwise false.</returns>
        public static bool IsReleaseConfiguration(string[]? args = null)
        {
            return HasParameterWithValue("--configuration", "Release", args);
        }

        /// <summary>
        /// Checks if --configuration Debug was passed in the arguments.
        /// If no args are provided, uses Environment.GetCommandLineArgs().
        /// </summary>
        /// <param name="args">Command line arguments. If null, uses Environment.GetCommandLineArgs().</param>
        /// <returns>True if --configuration Debug was found, otherwise false.</returns>
        public static bool IsDebugConfiguration(string[]? args = null)
        {
            return HasParameterWithValue("--configuration", "Debug", args);
        }

        /// <summary>
        /// Checks if a specific parameter exists in the arguments.
        /// Uses case-insensitive comparison by default.
        /// If no args are provided, uses Environment.GetCommandLineArgs().
        /// </summary>
        /// <param name="parameter">The parameter to search for (e.g., "--verbose").</param>
        /// <param name="args">Command line arguments. If null, uses Environment.GetCommandLineArgs().</param>
        /// <param name="comparison">String comparison type. Default: OrdinalIgnoreCase.</param>
        /// <returns>True if the parameter was found, otherwise false.</returns>
        public static bool HasParameter(
            string parameter,
            string[]? args = null,
            StringComparison comparison = StringComparison.OrdinalIgnoreCase)
        {
            var actualArgs = GetArgs(args);

            if (actualArgs.Length == 0)
                return false;

            return actualArgs.Any(arg => arg.Equals(parameter, comparison));
        }

        /// <summary>
        /// Returns the value of a parameter if present.
        /// Supports multiple parameter formats: --param value, -p value, /param:value, /param=value
        /// If no args are provided, uses Environment.GetCommandLineArgs().
        /// </summary>
        /// <param name="parameter">The parameter to search for (e.g., "--output").</param>
        /// <param name="args">Command line arguments. If null, uses Environment.GetCommandLineArgs().</param>
        /// <param name="comparison">String comparison type. Default: OrdinalIgnoreCase.</param>
        /// <returns>The value after the parameter or null if not found.</returns>
        public static string? GetParameterValue(
            string parameter,
            string[]? args = null,
            StringComparison comparison = StringComparison.OrdinalIgnoreCase)
        {
            var actualArgs = GetArgs(args);

            if (actualArgs.Length < 2)
                return null;

            for (int i = 0; i < actualArgs.Length - 1; i++)
            {
                if (actualArgs[i].Equals(parameter, comparison))
                {
                    return actualArgs[i + 1];
                }

                // Support format: --param:value or /param=value
                if (actualArgs[i].StartsWith(parameter + ":", comparison) ||
                    actualArgs[i].StartsWith(parameter + "=", comparison))
                {
                    var separatorIndex = actualArgs[i].IndexOfAny([':', '=']);
                    if (separatorIndex > 0 && separatorIndex < actualArgs[i].Length - 1)
                    {
                        return actualArgs[i][(separatorIndex + 1)..];
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Gets all values for a parameter that can appear multiple times.
        /// If no args are provided, uses Environment.GetCommandLineArgs().
        /// </summary>
        /// <param name="parameter">The parameter to search for.</param>
        /// <param name="args">Command line arguments. If null, uses Environment.GetCommandLineArgs().</param>
        /// <param name="comparison">String comparison type. Default: OrdinalIgnoreCase.</param>
        /// <returns>Array of all values found for the parameter.</returns>
        public static string[] GetParameterValues(
            string parameter,
            string[]? args = null,
            StringComparison comparison = StringComparison.OrdinalIgnoreCase)
        {
            var actualArgs = GetArgs(args);

            if (actualArgs.Length < 2)
                return Array.Empty<string>();

            var values = new List<string>();

            for (int i = 0; i < actualArgs.Length - 1; i++)
            {
                if (actualArgs[i].Equals(parameter, comparison))
                {
                    values.Add(actualArgs[i + 1]);
                }
            }

            return values.ToArray();
        }

        /// <summary>
        /// Parses all command line arguments into a dictionary.
        /// If a parameter appears multiple times, only the last value is kept.
        /// For multiple values, use <see cref="ParseArgumentsWithMultipleValues"/>.
        /// If no args are provided, uses Environment.GetCommandLineArgs().
        /// </summary>
        /// <param name="args">Command line arguments. If null, uses Environment.GetCommandLineArgs().</param>
        /// <returns>Dictionary with parameter names as keys and values as values.</returns>
        public static Dictionary<string, string> ParseArguments(string[]? args = null)
        {
            var actualArgs = GetArgs(args);
            var result = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            if (actualArgs.Length == 0)
                return result;

            for (int i = 0; i < actualArgs.Length; i++)
            {
                var arg = actualArgs[i];

                // Handle --param:value or --param=value format
                var separatorIndex = arg.IndexOfAny([':', '=']);
                if (separatorIndex > 0)
                {
                    var key = arg[..separatorIndex];
                    var value = arg[(separatorIndex + 1)..];
                    result[key] = value;
                    continue;
                }

                // Handle --param value format
                if (arg.StartsWith('-') || arg.StartsWith('/'))
                {
                    if (i + 1 < actualArgs.Length && !actualArgs[i + 1].StartsWith('-') && !actualArgs[i + 1].StartsWith('/'))
                    {
                        result[arg] = actualArgs[i + 1];
                        i++;
                    }
                    else
                    {
                        result[arg] = string.Empty;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Parses all command line arguments into a dictionary that supports multiple values per parameter.
        /// Parameters that appear multiple times will have all their values collected in a list.
        /// If no args are provided, uses Environment.GetCommandLineArgs().
        /// </summary>
        /// <param name="args">Command line arguments. If null, uses Environment.GetCommandLineArgs().</param>
        /// <returns>Dictionary with parameter names as keys and lists of values.</returns>
        public static Dictionary<string, List<string>> ParseArgumentsWithMultipleValues(string[]? args = null)
        {
            var actualArgs = GetArgs(args);
            var result = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);

            if (actualArgs.Length == 0)
                return result;

            for (int i = 0; i < actualArgs.Length; i++)
            {
                var arg = actualArgs[i];

                // Handle --param:value or --param=value format
                var separatorIndex = arg.IndexOfAny([':', '=']);
                if (separatorIndex > 0)
                {
                    var key = arg[..separatorIndex];
                    var value = arg[(separatorIndex + 1)..];

                    if (!result.ContainsKey(key))
                        result[key] = new List<string>();

                    result[key].Add(value);
                    continue;
                }

                // Handle --param value format
                if (arg.StartsWith('-') || arg.StartsWith('/'))
                {
                    if (!result.ContainsKey(arg))
                        result[arg] = new List<string>();

                    if (i + 1 < actualArgs.Length && !actualArgs[i + 1].StartsWith('-') && !actualArgs[i + 1].StartsWith('/'))
                    {
                        result[arg].Add(actualArgs[i + 1]);
                        i++;
                    }
                    else
                    {
                        result[arg].Add(string.Empty);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Parses command line arguments and returns an object with both single and multiple value support.
        /// Provides convenient access to both scenarios.
        /// If no args are provided, uses Environment.GetCommandLineArgs().
        /// </summary>
        /// <param name="args">Command line arguments. If null, uses Environment.GetCommandLineArgs().</param>
        /// <returns>MikuParsedArguments object with helper methods.</returns>
        public static MikuParsedArguments Parse(string[]? args = null)
        {
            return new MikuParsedArguments(GetArgs(args));
        }
    }

    /// <summary>
    /// Represents parsed command line arguments with convenient access methods.
    /// Supports both single and multiple values per parameter.
    /// </summary>
    public class MikuParsedArguments
    {
        private readonly Dictionary<string, List<string>> _arguments;

        internal MikuParsedArguments(string[] args)
        {
            _arguments = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);
            ParseArgs(args);
        }

        private void ParseArgs(string[] args)
        {
            if (args.Length == 0)
                return;

            for (int i = 0; i < args.Length; i++)
            {
                var arg = args[i];

                // Handle --param:value or --param=value format
                var separatorIndex = arg.IndexOfAny([':', '=']);
                if (separatorIndex > 0)
                {
                    var key = arg[..separatorIndex];
                    var value = arg[(separatorIndex + 1)..];

                    if (!_arguments.ContainsKey(key))
                        _arguments[key] = new List<string>();

                    _arguments[key].Add(value);
                    continue;
                }

                // Handle --param value format
                if (arg.StartsWith('-') || arg.StartsWith('/'))
                {
                    if (!_arguments.ContainsKey(arg))
                        _arguments[arg] = new List<string>();

                    if (i + 1 < args.Length && !args[i + 1].StartsWith('-') && !args[i + 1].StartsWith('/'))
                    {
                        _arguments[arg].Add(args[i + 1]);
                        i++;
                    }
                    else
                    {
                        _arguments[arg].Add(string.Empty);
                    }
                }
            }
        }

        /// <summary>
        /// Gets the last value for a parameter. Useful when only one value is expected.
        /// </summary>
        public string? GetValue(string parameter)
        {
            return _arguments.TryGetValue(parameter, out var values) && values.Count > 0
                ? values[^1]
                : null;
        }

        /// <summary>
        /// Gets all values for a parameter. Useful for parameters that can appear multiple times.
        /// </summary>
        public string[] GetValues(string parameter)
        {
            return _arguments.TryGetValue(parameter, out var values)
                ? values.ToArray()
                : Array.Empty<string>();
        }

        /// <summary>
        /// Gets the last value for a parameter or a default value if not found.
        /// </summary>
        public string GetValueOrDefault(string parameter, string defaultValue)
        {
            return GetValue(parameter) ?? defaultValue;
        }

        /// <summary>
        /// Checks if a parameter exists in the arguments.
        /// </summary>
        public bool HasParameter(string parameter)
        {
            return _arguments.ContainsKey(parameter);
        }

        /// <summary>
        /// Gets the count of values for a parameter.
        /// </summary>
        public int GetValueCount(string parameter)
        {
            return _arguments.TryGetValue(parameter, out var values) ? values.Count : 0;
        }

        /// <summary>
        /// Gets all parameter names.
        /// </summary>
        public string[] GetParameterNames()
        {
            return _arguments.Keys.ToArray();
        }

        /// <summary>
        /// Tries to get the value for a parameter.
        /// </summary>
        public bool TryGetValue(string parameter, out string? value)
        {
            value = GetValue(parameter);
            return value != null;
        }
    }
}

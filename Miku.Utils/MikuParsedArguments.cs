namespace Miku.Utils
{
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

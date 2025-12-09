using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Miku.Logger.Configuration;
using Miku.Logger.Configuration.Enums;
using Miku.Logger.Providers;

namespace Miku.Logger.Extensions
{
    /// <summary>
    /// Extension methods for adding MikuLogger to ILoggingBuilder.
    /// </summary>
    public static class MikuLoggerExtensions
    {
        /// <summary>
        /// Adds MikuLogger to the logging builder with default options.
        /// </summary>
        /// <param name="builder">The logging builder.</param>
        /// <returns>The logging builder for chaining.</returns>
        public static ILoggingBuilder AddMikuLogger(this ILoggingBuilder builder)
        {
            return AddMikuLogger(builder, new MikuLoggerOptions());
        }

        /// <summary>
        /// Adds MikuLogger to the logging builder with custom options.
        /// </summary>
        /// <param name="builder">The logging builder.</param>
        /// <param name="options">The logger options.</param>
        /// <returns>The logging builder for chaining.</returns>
        public static ILoggingBuilder AddMikuLogger(this ILoggingBuilder builder, MikuLoggerOptions options)
        {
            builder.Services.AddSingleton<ILoggerProvider>(new MikuLoggerProvider(options));
            return builder;
        }

        /// <summary>
        /// Adds MikuLogger to the logging builder with options configuration.
        /// </summary>
        /// <param name="builder">The logging builder.</param>
        /// <param name="configure">Action to configure the logger options.</param>
        /// <returns>The logging builder for chaining.</returns>
        /// <example>
        /// <code>
        /// builder.Logging.AddMikuLogger(options =>
        /// {
        ///     options.Output = MikuLogOutput.ConsoleAndFile;
        ///     options.MinimumLogLevel = MikuLogLevel.Debug;
        ///     options.FileOptions.MaxFileSizeBytes = 5 * 1024 * 1024; // 5 MB
        ///     options.FileOptions.UseDateFolders = true;
        /// });
        /// </code>
        /// </example>
        public static ILoggingBuilder AddMikuLogger(this ILoggingBuilder builder, Action<MikuLoggerOptions> configure)
        {
            var options = new MikuLoggerOptions();
            configure(options);
            return AddMikuLogger(builder, options);
        }
    }
}

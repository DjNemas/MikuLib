using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Miku.Logger.Configuration;
using Miku.Logger.Configuration.Enums;
using Miku.Logger.Providers;
using Miku.Logger.Sse;
using System.Net.ServerSentEvents;

namespace Miku.Logger.Extensions
{
    /// <summary>
    /// Extension methods for adding MikuLogger SSE streaming to ASP.NET Core applications.
    /// </summary>
    /// <remarks>
    /// Like Miku's live concert streams reaching fans worldwide,
    /// these extensions enable real-time log streaming via Server-Sent Events.
    /// </remarks>
    public static class MikuLoggerSseExtensions
    {
        /// <summary>
        /// Adds MikuLogger with SSE support to the logging builder.
        /// </summary>
        /// <param name="builder">The logging builder.</param>
        /// <param name="configure">Action to configure the logger options.</param>
        /// <returns>The logging builder for chaining.</returns>
        /// <example>
        /// <code>
        /// builder.Logging.AddMikuLoggerWithSse(options =>
        /// {
        ///     options.Output = MikuLogOutput.All;
        ///     options.SseOptions.EndpointPath = "/api/logs/stream";
        /// });
        /// </code>
        /// </example>
        public static ILoggingBuilder AddMikuLoggerWithSse(
            this ILoggingBuilder builder,
            Action<MikuLoggerOptions> configure)
        {
            var options = new MikuLoggerOptions();
            configure(options);

            // Ensure SSE is enabled
            if (!options.Output.HasFlag(MikuLogOutput.ServerSentEvents))
            {
                options.Output |= MikuLogOutput.ServerSentEvents;
            }

            // Configure the SSE broadcaster
            SseLogBroadcaster.Instance.Configure(options.SseOptions);

            // Register the logger provider
            builder.Services.AddSingleton<ILoggerProvider>(new MikuLoggerProvider(options));

            // Register the broadcaster as a service for DI access
            builder.Services.AddSingleton(SseLogBroadcaster.Instance);

            return builder;
        }

        /// <summary>
        /// Adds MikuLogger SSE services to the service collection.
        /// Use this when you want to configure SSE separately from the main logger.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="configure">Optional action to configure SSE options.</param>
        /// <returns>The service collection for chaining.</returns>
        public static IServiceCollection AddMikuLoggerSse(
            this IServiceCollection services,
            Action<MikuSseLoggerOptions>? configure = null)
        {
            var options = new MikuSseLoggerOptions();
            configure?.Invoke(options);

            SseLogBroadcaster.Instance.Configure(options);
            services.AddSingleton(SseLogBroadcaster.Instance);

            return services;
        }

        /// <summary>
        /// Maps the MikuLogger SSE endpoint for log streaming.
        /// </summary>
        /// <param name="endpoints">The endpoint route builder.</param>
        /// <param name="pattern">Optional custom endpoint pattern. If null, uses the configured path.</param>
        /// <returns>The route handler builder for further configuration.</returns>
        /// <example>
        /// <code>
        /// var app = builder.Build();
        /// 
        /// // Use default path from options
        /// app.MapMikuLoggerSse();
        /// 
        /// // Or specify custom path
        /// app.MapMikuLoggerSse("/custom/logs/stream");
        /// </code>
        /// </example>
        public static RouteHandlerBuilder MapMikuLoggerSse(
            this IEndpointRouteBuilder endpoints,
            string? pattern = null)
        {
            var broadcaster = SseLogBroadcaster.Instance;
            var endpointPath = pattern ?? broadcaster.Options.EndpointPath;
            var options = broadcaster.Options;

            var routeBuilder = endpoints.MapGet(endpointPath, (CancellationToken cancellationToken) =>
            {
                return TypedResults.ServerSentEvents(
                    GetSseItems(broadcaster, cancellationToken),
                    eventType: options.EventType);
            });

            // Apply authorization if configured
            if (options.RequireAuthorization)
            {
                if (!string.IsNullOrEmpty(options.AuthorizationPolicy))
                {
                    routeBuilder.RequireAuthorization(options.AuthorizationPolicy);
                }
                else
                {
                    routeBuilder.RequireAuthorization();
                }
            }

            return routeBuilder;
        }

        /// <summary>
        /// Maps the MikuLogger SSE endpoint with custom configuration.
        /// </summary>
        /// <param name="endpoints">The endpoint route builder.</param>
        /// <param name="configure">Action to configure SSE options.</param>
        /// <returns>The route handler builder for further configuration.</returns>
        public static RouteHandlerBuilder MapMikuLoggerSse(
            this IEndpointRouteBuilder endpoints,
            Action<MikuSseLoggerOptions> configure)
        {
            var options = new MikuSseLoggerOptions();
            configure(options);

            SseLogBroadcaster.Instance.Configure(options);

            return MapMikuLoggerSse(endpoints, options.EndpointPath);
        }

        private static async IAsyncEnumerable<SseItem<SseLogEntry>> GetSseItems(
            SseLogBroadcaster broadcaster,
            [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken)
        {
            var options = broadcaster.Options;
            var reconnectInterval = TimeSpan.FromMilliseconds(options.ReconnectionIntervalMs);

            await foreach (var entry in broadcaster.SubscribeAsync(cancellationToken))
            {
                var eventType = options.IncludeLogLevelInEventType
                    ? $"{options.EventType}-{entry.Level.ToLowerInvariant()}"
                    : options.EventType;

                yield return new SseItem<SseLogEntry>(entry, eventType)
                {
                    ReconnectionInterval = reconnectInterval
                };
            }
        }
    }
}

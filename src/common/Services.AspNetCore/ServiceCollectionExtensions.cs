using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using OpenTelemetry;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Services.Consul;
using Services.Consul.Options;

namespace Services.AspNetCore
{
    public static class ServiceCollectionExtensions
    {
        public static IHostApplicationBuilder AddServiceDefaults(this IHostApplicationBuilder builder)
        {
            builder.Services.Configure<ServiceDiscoveryOption>(builder.Configuration.GetSection(ServiceDiscoveryOption.Section));

            var serviceConfig = builder.Configuration
                .GetSection(ServiceDiscoveryOption.Section)
                .Get<ServiceDiscoveryOption>();

            string serviceName = serviceConfig?.Address;

            builder.AddLogging();
            builder.ConfigureOpenTelemetry(serviceName);
            builder.AddDefaultHealthChecks();

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddConsul(builder.Configuration);


            return builder;
        }

        private static IHostApplicationBuilder AddLogging(this IHostApplicationBuilder builder)
        {
            builder.Logging.ClearProviders(); // Remove default providers to avoid conflicts
            builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging"));
            builder.Logging.AddConsole(options =>
            {
                options.FormatterName = ConsoleFormatterNames.Json;
            });
            builder.Logging.AddDebug();
            builder.Logging.AddEventSourceLogger();

            return builder;
        }

        public static IHostApplicationBuilder ConfigureOpenTelemetry(this IHostApplicationBuilder builder, string serviceName)
        {
            builder.Logging.AddOpenTelemetry(logging =>
            {
                logging.IncludeFormattedMessage = true;
                logging.IncludeScopes = true;
            });

            builder.Services
                .AddOpenTelemetry()
                .ConfigureResource(resource => resource.AddService(serviceName))
                .WithMetrics(metrics =>
                {
                    metrics
                        .AddHttpClientInstrumentation()
                        .AddAspNetCoreInstrumentation();
                })
                .WithTracing(tracing =>
                {
                    tracing
                        .AddSource(serviceName)
                        .AddHttpClientInstrumentation()
                        .AddAspNetCoreInstrumentation();
                });

            builder.Services.AddOpenTelemetry().UseOtlpExporter();

            return builder;
        }

        public static IHostApplicationBuilder AddDefaultHealthChecks(this IHostApplicationBuilder builder)
        {
            builder.Services.AddHealthChecks()
                .AddCheck("self", () => HealthCheckResult.Healthy(), ["live"]);

            return builder;
        }
    }
}

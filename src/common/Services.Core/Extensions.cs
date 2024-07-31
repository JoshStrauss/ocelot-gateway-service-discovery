using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Services.Core.ServiceDiscovery;

namespace Services.Core
{
    public static class Extensions
    {
        public static IHostApplicationBuilder AddServiceDefaults(this IHostApplicationBuilder builder)
        {
            string serviceName = builder.Configuration.GetServiceConfig().Address;

            builder.ConfigureOpenTelemetry(serviceName);
            builder.AddDefaultHealthChecks();

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddConsul(builder.Configuration.GetServiceConfig());

            builder.AddLogging();

            return builder;
        }

        private static IHostApplicationBuilder AddLogging(this IHostApplicationBuilder builder)
        {
            builder.Services.AddLogging();
            builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging"));
            builder.Logging.AddConsole();
            builder.Logging.AddDebug();
            builder.Logging.AddEventSourceLogger();

            return builder;
        }

        public static IHostApplicationBuilder ConfigureOpenTelemetry(this IHostApplicationBuilder builder, string serviceName)
        {
            builder.Services
                .AddOpenTelemetry()
                .ConfigureResource(resource => resource.AddService(serviceName))
                .WithTracing(tracing =>
                {
                    tracing
                        .AddSource(serviceName)
                        .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(serviceName))
                        .AddHttpClientInstrumentation()
                        .AddAspNetCoreInstrumentation()
                        .AddSqlClientInstrumentation(o => o.SetDbStatementForText = true)
                        .AddOtlpExporter();
                });

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

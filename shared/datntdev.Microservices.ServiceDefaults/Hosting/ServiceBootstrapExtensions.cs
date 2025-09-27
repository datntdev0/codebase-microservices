using datntdev.Microservices.Common;
using datntdev.Microservices.Common.Modular;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ServiceDiscovery;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;

namespace datntdev.Microservices.ServiceDefaults.Hosting
{
    public static class ServiceBootstrapExtensions
    {
        public static IServiceCollection AddServiceBootstrap<TStartupModule>(this IServiceCollection services, IConfigurationRoot configs)
            where TStartupModule : BaseModule
        {
            services.AddSingleton(configs);
            var bootstrapper = new ServiceBootstrap<TStartupModule>();
            bootstrapper.ConfigureServices(services, configs);
            return services.AddSingleton(bootstrapper);
        }

        public static IApplicationBuilder UseServiceBootstrap<TStartupModule>(this IApplicationBuilder app, IConfigurationRoot configs)
            where TStartupModule : BaseModule
        {
            var bootstrapper = app.ApplicationServices.GetRequiredService<ServiceBootstrap<TStartupModule>>();
            bootstrapper.Configure(app.ApplicationServices, configs);
            return app;
        }

        public static IServiceCollection AddDefaultHealthChecks(this IServiceCollection services)
        {
            services.AddHealthChecks() // Add a default liveness check to ensure app is responsive
                .AddCheck("self", () => HealthCheckResult.Healthy(), ["live"]);
            return services;
        }

        public static IServiceCollection AddDefaultOpenTelemetry(this IServiceCollection services)
        {
            services.AddLogging(builder =>
            {
                builder.AddOpenTelemetry(config =>
                {
                    config.IncludeFormattedMessage = true;
                    config.IncludeScopes = true;
                });
            });

            services.AddOpenTelemetry()
                .WithMetrics(metrics =>
                {
                    metrics.AddAspNetCoreInstrumentation()
                        .AddHttpClientInstrumentation()
                        .AddRuntimeInstrumentation();
                })
                .WithTracing(tracing =>
                {
                    tracing.AddSource(Constants.Application.Name)
                        .AddAspNetCoreInstrumentation()
                        .AddHttpClientInstrumentation();
                });

            services.AddOpenTelemetry().UseOtlpExporter();

            return services;
        }

        public static IServiceCollection AddDefaultServiceDiscovery(this IServiceCollection services)
        {
            services.AddServiceDiscovery();

            services.ConfigureHttpClientDefaults(http =>
            {
                // Turn on resilience by default
                http.AddStandardResilienceHandler();

                // Turn on service discovery by default
                http.AddServiceDiscovery();
            });

            services.Configure<ServiceDiscoveryOptions>(options =>
            {
                options.AllowedSchemes = ["https"];
            });

            return services;
        }

        public static IEndpointRouteBuilder MapDefaultHealthChecks(this IEndpointRouteBuilder builder)
        {
            // Adding health checks endpoints to applications in non-development environments has security implications.
            // See https://aka.ms/dotnet/aspire/healthchecks for details before enabling these endpoints in non-development environments.
            // All health checks must pass for app to be considered ready to accept traffic after starting
            builder.MapHealthChecks(Constants.Endpoints.Health);
            // Only health checks tagged with the "live" tag must pass for app to be considered alive
            builder.MapHealthChecks(Constants.Endpoints.Liveness, new HealthCheckOptions
            {
                Predicate = r => r.Tags.Contains("live")
            });
            return builder;
        }
    }
}

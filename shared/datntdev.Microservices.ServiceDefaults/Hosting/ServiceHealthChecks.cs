using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace datntdev.Microservices.ServiceDefaults.Hosting
{
    internal class StartupHealthCheck : IHealthCheck
    {
        public Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(HealthCheckResult.Healthy());
        }
    }

    internal class DbContextHealthCheck(IServiceProvider services) : IHealthCheck
    {
        public Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            var dbContextOption = services.GetService<DbContextOptions>();
            if (dbContextOption?.ContextType is not null)
            {
                var dbContext = (DbContext)services.GetRequiredService(dbContextOption.ContextType);
                if (!dbContext.Database.CanConnect())
                {
                    return Task.FromResult(HealthCheckResult.Unhealthy(GetType().Name));
                }
            }

            return Task.FromResult(HealthCheckResult.Healthy());
        }
    }
}

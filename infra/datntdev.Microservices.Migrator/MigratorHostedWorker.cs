using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace datntdev.Microservices.Migrator
{
    internal class MigratorHostedWorker(IServiceProvider services) : IHostedService, IDisposable
    {
        private Timer? _timer;

        public void Dispose() => _timer?.Dispose();

        public Task StartAsync(CancellationToken cancellationToken)
        {
            // Create a timer that will run after 1 second to allow the application to start up
            // and then stop the application lifetime to allow migrations to complete.
            // We use a timer to ensure that the application has fully started before we attempt to run migrations.
            _timer = new Timer(async (services) =>
            {
                var scoped = ((IServiceProvider)services!).CreateScope().ServiceProvider;
                var logger = scoped.GetRequiredService<ILogger<MigratorHostedWorker>>();
                var lifetime = scoped.GetRequiredService<IHostApplicationLifetime>();

                logger.LogInformation("Migrator service is starting...");

                await Task.Delay(5000); // Simulate some work being done, e.g., running migrations

                logger.LogInformation("Migrator service is completed. Stopping application lifetime...");
                lifetime.StopApplication();
            }, services, TimeSpan.FromSeconds(1), Timeout.InfiniteTimeSpan);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }
    }
}

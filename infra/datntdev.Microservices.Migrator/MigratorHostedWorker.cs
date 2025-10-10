using datntdev.Microservices.Common.Repository;
using datntdev.Microservices.Srv.Admin.Web.App;
using datntdev.Microservices.Srv.Identity.Web.App;
using datntdev.Microservices.Srv.Notify.Web.App;
using datntdev.Microservices.Srv.Payment.Web.App;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

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

                await Task.WhenAll(
                    StartMigrationAsync(scoped, scoped.GetRequiredService<SrvIdentityDbContext>()),
                    StartMigrationAsync(scoped, scoped.GetRequiredService<SrvPaymentDbContext>()),
                    StartMigrationAsync(scoped, scoped.GetRequiredService<SrvNotifyDbContext>()),
                    StartMigrationAsync(scoped, scoped.GetRequiredService<SrvAdminDbContext>())
                );

                logger.LogInformation("Database migration is completed. Starting data seeding...");

                await Task.WhenAll(
                    new Seeders.IdentityDataSeeder(scoped).SeedAsync(),
                    new Seeders.AdminDataSeeder(scoped).SeedAsync()
                );

                logger.LogInformation("Migrator service is completed. Stopping application lifetime...");
                lifetime.StopApplication();
            }, services, TimeSpan.FromSeconds(1), Timeout.InfiniteTimeSpan);
            return Task.CompletedTask;
        }

        private static Task StartMigrationAsync<TDbContext>(IServiceProvider scoped, TDbContext dbContext) 
            where TDbContext : DbContext
        {
            var logger = scoped.GetRequiredService<ILogger<TDbContext>>();
            logger.LogInformation("Checking database existed or pending migrations...");

            dbContext.Database.EnsureCreated();

            if (dbContext is IRelationalDbContext)
            {
                var pendingChanges = dbContext.Database.GetPendingMigrations();
                if (pendingChanges.Any()) dbContext.Database.Migrate();
            }

            // For MongoDB, EnsureCreated only creates the database if it contains collections.
            // If there are no collections, you won't see the database in the MongoDb server.

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }
    }
}

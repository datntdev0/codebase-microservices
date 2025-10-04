using datntdev.Microservices.Common.Modular;
using datntdev.Microservices.Srv.Admin.Web.App;
using datntdev.Microservices.Srv.Identity.Web.App;
using datntdev.Microservices.Srv.Notify.Web.App;
using datntdev.Microservices.Srv.Payment.Web.App;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace datntdev.Microservices.Migrator
{
    [DependOn(
        typeof(SrvAdminWebAppModule),
        typeof(SrvNotifyWebAppModule),
        typeof(SrvPaymentWebAppModule),
        typeof(SrvIdentityWebAppModule)
    )]
    internal class MigratorModule : BaseModule
    {
        public override void ConfigureServices(IServiceCollection services, IConfigurationRoot configs)
        {
            ConfigureDbContextSqlServer(services, configs);
        }

        private void ConfigureDbContextSqlServer(IServiceCollection services, IConfigurationRoot configs)
        {
            var migrationsAssembly = GetType().Assembly.GetName().Name;
            services.ConfigureDbContext<SrvIdentityDbContext>(
                opt => opt.UseSqlServer(configs.GetConnectionString("Identity"),
                    o => o.MigrationsAssembly(migrationsAssembly)));
            services.ConfigureDbContext<SrvPaymentDbContext>(
                opt => opt.UseSqlServer(configs.GetConnectionString("Payment"),
                    o => o.MigrationsAssembly(migrationsAssembly)));
        }
    }
}

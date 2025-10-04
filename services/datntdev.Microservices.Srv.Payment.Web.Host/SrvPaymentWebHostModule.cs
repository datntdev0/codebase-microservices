using datntdev.Microservices.Common.Modular;
using datntdev.Microservices.Srv.Payment.Web.App;
using Microsoft.EntityFrameworkCore;

namespace datntdev.Microservices.Srv.Payment.Web.Host
{
    [DependOn(typeof(SrvPaymentWebAppModule))]
    public class SrvPaymentWebHostModule : BaseModule
    {
        public override void ConfigureServices(IServiceCollection services, IConfigurationRoot configs)
        {
            services.ConfigureDbContext<SrvPaymentDbContext>(
                opt => opt.UseSqlServer(configs.GetConnectionString("Default")));
        }
    }
}

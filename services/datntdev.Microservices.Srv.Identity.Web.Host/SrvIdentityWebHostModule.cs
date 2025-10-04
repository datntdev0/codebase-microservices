using datntdev.Microservices.Common.Modular;
using datntdev.Microservices.Srv.Identity.Web.App;
using Microsoft.EntityFrameworkCore;

namespace datntdev.Microservices.Srv.Identity.Web.Host
{
    [DependOn(typeof(SrvIdentityWebAppModule))]
    public class SrvIdentityWebHostModule : BaseModule
    {
        public override void ConfigureServices(IServiceCollection services, IConfigurationRoot configs)
        {
            services.ConfigureDbContext<SrvIdentityDbContext>(
                opt => opt.UseSqlServer(configs.GetConnectionString("Default")));
        }
    }
}

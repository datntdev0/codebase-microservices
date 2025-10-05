using datntdev.Microservices.Common.Modular;
using datntdev.Microservices.Srv.Notify.Web.App;
using Microsoft.EntityFrameworkCore;

namespace datntdev.Microservices.Srv.Notify.Web.Host
{
    [DependOn(typeof(SrvNotifyWebAppModule))]
    public class SrvNotifyWebHostModule : BaseModule
    {
        public override void ConfigureServices(IServiceCollection services, IConfigurationRoot configs)
        {
            services.ConfigureDbContext<SrvNotifyDbContext>(
                opt => opt.UseMongoDB(configs.GetConnectionString("Default")!));
        }
    }
}

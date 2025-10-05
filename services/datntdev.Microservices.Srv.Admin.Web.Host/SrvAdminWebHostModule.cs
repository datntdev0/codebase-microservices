using datntdev.Microservices.Common.Modular;
using datntdev.Microservices.Srv.Admin.Web.App;
using Microsoft.EntityFrameworkCore;

namespace datntdev.Microservices.Srv.Admin.Web.Host
{
    [DependOn(typeof(SrvAdminWebAppModule))]
    public class SrvAdminWebHostModule : BaseModule
    {
        public override void ConfigureServices(IServiceCollection services, IConfigurationRoot configs)
        {
            services.ConfigureDbContext<SrvAdminDbContext>(
                opt => opt.UseMongoDB(configs.GetConnectionString("Default")!));
        }
    }
}

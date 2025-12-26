using datntdev.Microservices.Common.Modular;
using datntdev.Microservices.Common.Web.App;
using datntdev.Microservices.Srv.Admin.Contract;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace datntdev.Microservices.Srv.Admin.Web.App
{
    [DependOn(
        typeof(CommonWebAppModule),
        typeof(SrvAdminContractModule)
    )]
    public class SrvAdminWebAppModule : BaseModule
    {
        public override void ConfigureServices(IServiceCollection services, IConfigurationRoot configs)
        {
            services.AddDbContext<SrvAdminDbContext>();
        }
    }
}

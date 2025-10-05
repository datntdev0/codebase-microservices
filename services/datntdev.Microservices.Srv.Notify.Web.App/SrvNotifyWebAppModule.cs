using datntdev.Microservices.Common.Modular;
using datntdev.Microservices.Srv.Notify.Contract;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace datntdev.Microservices.Srv.Notify.Web.App
{
    [DependOn(typeof(SrvNotifyContractModule))] 
    public class SrvNotifyWebAppModule : BaseModule
    {
        public override void ConfigureServices(IServiceCollection services, IConfigurationRoot configs)
        {
            services.AddDbContext<SrvNotifyDbContext>();
        }
    }
}

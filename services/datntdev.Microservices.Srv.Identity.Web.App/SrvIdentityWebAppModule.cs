using datntdev.Microservices.Common.Modular;
using datntdev.Microservices.Srv.Identity.Contract;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace datntdev.Microservices.Srv.Identity.Web.App
{
    [DependOn(typeof(SrvIdentityContractModule))]
    public class SrvIdentityWebAppModule : BaseModule
    {
        public override void ConfigureServices(IServiceCollection services, IConfigurationRoot configs)
        {
            services.AddDbContext<SrvIdentityDbContext>();
        }
    }
}

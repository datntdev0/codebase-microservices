using datntdev.Microservices.Common.Modular;
using datntdev.Microservices.Srv.Payment.Contract;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace datntdev.Microservices.Srv.Payment.Web.App
{
    [DependOn(typeof(SrvPaymentContractModule))]
    public class SrvPaymentWebAppModule : BaseModule
    {
        public override void ConfigureServices(IServiceCollection services, IConfigurationRoot configs)
        {
            services.AddDbContext<SrvPaymentDbContext>();
        }
    }
}

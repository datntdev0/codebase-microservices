using datntdev.Microservices.Common.Modular;
using datntdev.Microservices.Srv.Payment.Web.App;

namespace datntdev.Microservices.Srv.Payment.Web.Host
{
    [DependOn(typeof(SrvPaymentWebAppModule))]
    public class SrvPaymentWebHostModule : BaseModule
    {
    }
}

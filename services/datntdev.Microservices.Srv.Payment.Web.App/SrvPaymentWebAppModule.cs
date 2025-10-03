using datntdev.Microservices.Common.Modular;
using datntdev.Microservices.Srv.Payment.Contract;

namespace datntdev.Microservices.Srv.Payment.Web.App
{
    [DependOn(typeof(SrvPaymentContractModule))]
    public class SrvPaymentWebAppModule : BaseModule
    {
    }
}

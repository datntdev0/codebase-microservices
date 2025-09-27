using datntdev.Microservices.Common.Modular;
using datntdev.Microservices.Srv.Identity.Contract;

namespace datntdev.Microservices.Srv.Identity.Web.App
{
    [DependOn(typeof(SrvIdentityContractModule))]
    public class SrvIdentityWebAppModule : BaseModule
    {
    }
}

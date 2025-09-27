using datntdev.Microservices.Common.Modular;
using datntdev.Microservices.Srv.Identity.Web.App;

namespace datntdev.Microservices.Srv.Identity.Web.Host
{
    [DependOn(typeof(SrvIdentityWebAppModule))]
    public class SrvIdentityWebHostModule : BaseModule
    {
    }
}

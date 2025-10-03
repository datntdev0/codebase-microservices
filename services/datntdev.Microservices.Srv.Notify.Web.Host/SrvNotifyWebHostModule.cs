using datntdev.Microservices.Common.Modular;
using datntdev.Microservices.Srv.Notify.Web.App;

namespace datntdev.Microservices.Srv.Notify.Web.Host
{
    [DependOn(typeof(SrvNotifyWebAppModule))]
    public class SrvNotifyWebHostModule : BaseModule
    {
    }
}

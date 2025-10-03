using datntdev.Microservices.Common.Modular;
using datntdev.Microservices.Srv.Notify.Contract;

namespace datntdev.Microservices.Srv.Notify.Web.App
{
    [DependOn(typeof(SrvNotifyContractModule))] 
    public class SrvNotifyWebAppModule : BaseModule
    {
    }
}

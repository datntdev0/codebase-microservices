using datntdev.Microservices.Common.Modular;
using datntdev.Microservices.Srv.Admin.Contract;

namespace datntdev.Microservices.Srv.Admin.Web.App
{
    [DependOn(typeof(SrvAdminContractModule))]
    public class SrvAdminWebAppModule : BaseModule
    {
    }
}

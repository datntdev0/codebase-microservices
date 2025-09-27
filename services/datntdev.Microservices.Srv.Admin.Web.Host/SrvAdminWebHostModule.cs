using datntdev.Microservices.Common.Modular;
using datntdev.Microservices.Srv.Admin.Web.App;

namespace datntdev.Microservices.Srv.Admin.Web.Host
{
    [DependOn(typeof(SrvAdminWebAppModule))]
    public class SrvAdminWebHostModule : BaseModule
    {
    }
}

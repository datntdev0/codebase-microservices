using datntdev.Microservices.Common.Modular;
using datntdev.Microservices.Srv.Admin.Web.App;
using datntdev.Microservices.Srv.Identity.Web.App;

namespace datntdev.Microservices.Migrator
{
    [DependOn(
        typeof(SrvAdminWebAppModule),
        typeof(SrvIdentityWebAppModule)
    )]
    internal class MigratorModule : BaseModule
    {
    }
}

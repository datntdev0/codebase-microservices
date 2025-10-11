using datntdev.Microservices.Common.Modular;
using Microsoft.Extensions.DependencyInjection;

namespace datntdev.Microservices.Common.Web.App.Session
{
    [InjectService(ServiceLifetime.Scoped)]
    public class AppSessionContext
    {
        public AppSessionAppInfo AppInfo { get; private set; } = new();
    }

    public class AppSessionAppInfo
    {
        public string? AppName { get; set; } = Constants.Application.Name;
        public string? AppVersion { get; set; } = Constants.Application.Version;
        public string? AppTheme { get; set; } = Constants.Application.DefaultTheme;
    }
}

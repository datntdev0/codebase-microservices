using datntdev.Microservices.Common.Modular;
using Microsoft.Extensions.DependencyInjection;

namespace datntdev.Microservices.Common.Web.App.Session
{
    [InjectableService(ServiceLifetime.Scoped)]
    public class AppSessionContext
    {
        public AppSessionAppInfo AppInfo { get; private set; } = new();
        public AppSessionUserInfo? UserInfo { get; private set; }

        public void SetUserInfo(AppSessionUserInfo? userInfo) => UserInfo = userInfo;
    }

    public class AppSessionAppInfo
    {
        public string? AppName { get; set; } = Constants.Application.Name;
        public string? AppVersion { get; set; } = Constants.Application.Version;
        public string? AppTheme { get; set; } = Constants.Application.DefaultTheme;
    }

    public class AppSessionUserInfo
    {
        public long UserId { get; set; }
        public string EmailAddress { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
    }
}

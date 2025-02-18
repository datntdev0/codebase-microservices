using datntdev.Abp.Authorization;
using datntdev.Abp.Collections;

namespace datntdev.Abp.Configuration.Startup
{
    public class AuthorizationConfiguration : IAuthorizationConfiguration
    {
        public ITypeList<AuthorizationProvider> Providers { get; }

        public bool IsEnabled { get; set; }

        public AuthorizationConfiguration()
        {
            Providers = new TypeList<AuthorizationProvider>();
            IsEnabled = true;
        }
    }
}
using datntdev.Abp.Application.Navigation;
using datntdev.Abp.Collections;

namespace datntdev.Abp.Configuration.Startup
{
    public class NavigationConfiguration : INavigationConfiguration
    {
        public ITypeList<NavigationProvider> Providers { get; private set; }

        public NavigationConfiguration()
        {
            Providers = new TypeList<NavigationProvider>();
        }
    }
}
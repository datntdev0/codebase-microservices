using datntdev.Abp.Application.Navigation;
using datntdev.Abp.Collections;

namespace datntdev.Abp.Configuration.Startup
{
    /// <summary>
    /// Used to configure navigation.
    /// </summary>
    public interface INavigationConfiguration
    {
        /// <summary>
        /// List of navigation providers.
        /// </summary>
        ITypeList<NavigationProvider> Providers { get; }
    }
}
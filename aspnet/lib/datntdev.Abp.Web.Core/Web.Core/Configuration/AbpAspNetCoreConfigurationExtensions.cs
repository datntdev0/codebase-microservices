using datntdev.Abp.Configuration.Startup;

namespace datntdev.Abp.Web.Core.Configuration;

/// <summary>
/// Defines extension methods to <see cref="IModuleConfigurations"/> to allow to configure ABP ASP.NET Core module.
/// </summary>
public static class AbpWebCoreConfigurationExtensions
{
    /// <summary>
    /// Used to configure ABP ASP.NET Core module.
    /// </summary>
    public static IAbpWebCoreConfiguration AbpWebCore(this IModuleConfigurations configurations)
    {
        return configurations.AbpConfiguration.Get<IAbpWebCoreConfiguration>();
    }
}

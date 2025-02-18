using System.Collections.Generic;
using datntdev.Abp.Configuration.Startup;
using datntdev.Abp.Dependency;
using datntdev.Abp.Localization;

namespace datntdev.Abp.Application.Navigation
{
    public class NavigationManager : INavigationManager, ISingletonDependency
    {
        public IDictionary<string, MenuDefinition> Menus { get; private set; }

        public MenuDefinition MainMenu
        {
            get { return Menus["MainMenu"]; }
        }

        private readonly IIocResolver _iocResolver;
        private readonly INavigationConfiguration _configuration;

        public NavigationManager(IIocResolver iocResolver, INavigationConfiguration configuration)
        {
            _iocResolver = iocResolver;
            _configuration = configuration;

            Menus = new Dictionary<string, MenuDefinition>
                    {
                        {"MainMenu", new MenuDefinition("MainMenu", new LocalizableString("MainMenu", AbpConsts.LocalizationSourceName))}
                    };
        }

        public void Initialize()
        {
            var context = new NavigationProviderContext(this);

            foreach (var providerType in _configuration.Providers)
            {
                using (var provider = _iocResolver.ResolveAsDisposable<NavigationProvider>(providerType))
                {
                    provider.Object.SetNavigation(context);
                }
            }
        }
    }
}
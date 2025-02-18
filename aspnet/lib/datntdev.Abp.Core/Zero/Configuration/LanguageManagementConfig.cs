using System.Linq;
using datntdev.Abp.Configuration.Startup;
using datntdev.Abp.Dependency;
using datntdev.Abp.Localization;
using datntdev.Abp.Localization.Dictionaries;
using Castle.Core.Logging;

namespace datntdev.Abp.Zero.Configuration
{
    public class LanguageManagementConfig : ILanguageManagementConfig
    {
        public ILogger Logger { get; set; }

        private readonly IIocManager _iocManager;
        private readonly IAbpStartupConfiguration _configuration;

        public LanguageManagementConfig(IIocManager iocManager, IAbpStartupConfiguration configuration)
        {
            _iocManager = iocManager;
            _configuration = configuration;

            Logger = NullLogger.Instance;
        }

        public void EnableDbLocalization()
        {
            _iocManager.RegisterIfNot<ILanguageProvider, ApplicationLanguageProvider>(DependencyLifeStyle.Transient);

            var sources = _configuration
                .Localization
                .Sources
                .Where(s => s is IDictionaryBasedLocalizationSource)
                .Cast<IDictionaryBasedLocalizationSource>()
                .ToList();
            
            foreach (var source in sources)
            {
                _configuration.Localization.Sources.Remove(source);
                _configuration.Localization.Sources.Add(
                    new MultiTenantLocalizationSource(
                        source.Name,
                        new MultiTenantLocalizationDictionaryProvider(
                            source.DictionaryProvider,
                            _iocManager
                            )
                        )
                    );

                Logger.DebugFormat("Converted {0} ({1}) to MultiTenantLocalizationSource", source.Name, source.GetType());
            }
        }
    }
}
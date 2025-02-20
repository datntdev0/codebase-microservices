using datntdev.Abp.Configuration.Startup;
using datntdev.Abp.Localization.Dictionaries;
using datntdev.Abp.Localization.Dictionaries.Xml;
using datntdev.Abp.Reflection.Extensions;

namespace datntdev.Microservice.Localization;

public static class MicroserviceLocalizationConfigurer
{
    public static void Configure(ILocalizationConfiguration localizationConfiguration)
    {
        localizationConfiguration.Sources.Add(
            new DictionaryBasedLocalizationSource(MicroserviceConsts.LocalizationSourceName,
                new XmlEmbeddedFileLocalizationDictionaryProvider(
                    typeof(MicroserviceLocalizationConfigurer).GetAssembly()
                )
            )
        );
    }
}

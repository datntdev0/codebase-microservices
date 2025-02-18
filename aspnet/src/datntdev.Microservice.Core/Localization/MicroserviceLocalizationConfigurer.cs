using Abp.Configuration.Startup;
using Abp.Localization.Dictionaries;
using Abp.Localization.Dictionaries.Xml;
using Abp.Reflection.Extensions;

namespace datntdev.Microservice.Localization;

public static class MicroserviceLocalizationConfigurer
{
    public static void Configure(ILocalizationConfiguration localizationConfiguration)
    {
        localizationConfiguration.Sources.Add(
            new DictionaryBasedLocalizationSource(MicroserviceConsts.LocalizationSourceName,
                new XmlEmbeddedFileLocalizationDictionaryProvider(
                    typeof(MicroserviceLocalizationConfigurer).GetAssembly(),
                    "datntdev.Microservice.Localization.SourceFiles"
                )
            )
        );
    }
}

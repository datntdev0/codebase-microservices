using System.Globalization;
using System.Linq;
using System.Reflection;

namespace datntdev.Abp.Localization.Dictionaries.Xml
{
    /// <summary>
    /// Provides localization dictionaries from XML files embedded into an <see cref="Assembly"/>.
    /// </summary>
    public class XmlEmbeddedFileLocalizationDictionaryProvider(Assembly assembly) : LocalizationDictionaryProviderBase
    {
        private readonly Assembly _assembly = assembly;

        protected override void InitializeDictionaries()
        {
            var allCultureInfos = CultureInfo.GetCultures(CultureTypes.AllCultures);
            var resourceNames = _assembly.GetManifestResourceNames()
                .Where(resouceName => allCultureInfos
                    .Any(culture => resouceName.EndsWith($"{SourceName}.xml", true, null) ||
                        resouceName.EndsWith($"{SourceName}-{culture.Name}.xml", true, null)))
                .ToList();

            foreach (var resourceName in resourceNames)
            {
                using var stream = _assembly.GetManifestResourceStream(resourceName);
                var dictionary = CreateXmlLocalizationDictionary(Utf8Helper.ReadStringFromStream(stream));
                InitializeDictionary(dictionary, isDefault: resourceName.EndsWith($"{SourceName}.xml"));
            }
        }

        protected virtual XmlLocalizationDictionary CreateXmlLocalizationDictionary(string xmlString)
        {
            return XmlLocalizationDictionary.BuildFomXmlString(xmlString);
        }
    }
}

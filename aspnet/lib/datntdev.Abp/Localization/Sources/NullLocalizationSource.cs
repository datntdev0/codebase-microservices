using System.Collections.Generic;
using System.Globalization;
using datntdev.Abp.Configuration.Startup;
using datntdev.Abp.Dependency;

namespace datntdev.Abp.Localization.Sources
{
    /// <summary>
    /// Null object pattern for <see cref="ILocalizationSource"/>.
    /// </summary>
    public class NullLocalizationSource : ILocalizationSource
    {
        /// <summary>
        /// Singleton instance.
        /// </summary>
        public static NullLocalizationSource Instance { get; } = new NullLocalizationSource();

        public string Name { get { return null; } }

        private readonly IReadOnlyList<LocalizedString> _emptyStringArray = new LocalizedString[0];

        private NullLocalizationSource()
        {

        }

        public void Initialize(ILocalizationConfiguration configuration, IIocResolver iocResolver)
        {

        }

        public string FindKeyOrNull(string value, CultureInfo culture, bool tryDefaults = true)
        {
            return null;
        }

        public string GetString(string name)
        {
            return name;
        }

        public string GetString(string name, CultureInfo culture)
        {
            return name;
        }

        public string GetStringOrNull(string name, bool tryDefaults = true)
        {
            return null;
        }

        public string GetStringOrNull(string name, CultureInfo culture, bool tryDefaults = true)
        {
            return null;
        }

        public List<string> GetStrings(List<string> names)
        {
            return names;
        }

        public List<string> GetStrings(List<string> names, CultureInfo culture)
        {
            return names;
        }

        public List<string> GetStringsOrNull(List<string> names, bool tryDefaults = true)
        {
            return null;
        }

        public List<string> GetStringsOrNull(List<string> names, CultureInfo culture, bool tryDefaults = true)
        {
            return null;
        }

        public IReadOnlyList<LocalizedString> GetAllStrings(bool includeDefaults = true)
        {
            return _emptyStringArray;
        }

        public IReadOnlyList<LocalizedString> GetAllStrings(CultureInfo culture, bool includeDefaults = true)
        {
            return _emptyStringArray;
        }
    }
}

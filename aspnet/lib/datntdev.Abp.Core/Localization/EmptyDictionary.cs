using System.Collections.Generic;
using System.Globalization;
using datntdev.Abp.Localization.Dictionaries;

namespace datntdev.Abp.Localization
{
    public class EmptyDictionary : ILocalizationDictionary
    {
        public CultureInfo CultureInfo { get; private set; }

        public EmptyDictionary(CultureInfo cultureInfo)
        {
            CultureInfo = cultureInfo;
        }

        public string TryGetKey(string value)
        {
            return null;
        }

        public LocalizedString GetOrNull(string name)
        {
            return null;
        }

        public IReadOnlyList<LocalizedString> GetStringsOrNull(List<string> names)
        {
            return new LocalizedString[0];
        }

        public IReadOnlyList<LocalizedString> GetAllStrings()
        {
            return new LocalizedString[0];
        }

        public string this[string name]
        {
            get { return null; }
            set { }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using datntdev.Abp.Collections.Extensions;
using datntdev.Abp.Configuration.Startup;
using datntdev.Abp.Dependency;
using datntdev.Abp.Extensions;
using datntdev.Abp.Localization.Dictionaries;
using Castle.Core.Logging;

namespace datntdev.Abp.Localization
{
    public class MultiTenantLocalizationSource : DictionaryBasedLocalizationSource, IMultiTenantLocalizationSource
    {
        public new MultiTenantLocalizationDictionaryProvider DictionaryProvider
        {
            get { return base.DictionaryProvider.As<MultiTenantLocalizationDictionaryProvider>(); }
        }

        public ILogger Logger { get; set; }

        public MultiTenantLocalizationSource(string name, MultiTenantLocalizationDictionaryProvider dictionaryProvider)
            : base(name, dictionaryProvider)
        {
            Logger = NullLogger.Instance;
        }

        public override void Initialize(ILocalizationConfiguration configuration, IIocResolver iocResolver)
        {
            base.Initialize(configuration, iocResolver);

            if (Logger is NullLogger && iocResolver.IsRegistered(typeof(ILoggerFactory)))
            {
                Logger = iocResolver.Resolve<ILoggerFactory>().Create(typeof (MultiTenantLocalizationSource));
            }
        }

        public string FindKeyOrNull(int? tenantId, string value, CultureInfo culture, bool tryDefaults = true)
        {
            var cultureName = culture.Name;
            var dictionaries = DictionaryProvider.Dictionaries;

            //Try to get from original dictionary (with country code)
            if (dictionaries.TryGetValue(cultureName, out var originalDictionary))
            {
                var keyOriginal = originalDictionary
                    .As<IMultiTenantLocalizationDictionary>()
                    .TryGetKey(tenantId, value);

                if (keyOriginal != null)
                {
                    return keyOriginal;
                }
            }

            if (!tryDefaults)
            {
                return null;
            }

            //Try to get from same language dictionary (without country code)
            if (cultureName.Contains("-")) //Example: "tr-TR" (length=5)
            {
                if (dictionaries.TryGetValue(GetBaseCultureName(cultureName), out var langDictionary))
                {
                    var keyLang = langDictionary.As<IMultiTenantLocalizationDictionary>().TryGetKey(tenantId, value);
                    if (keyLang != null)
                    {
                        return keyLang;
                    }
                }
            }

            //Try to get from default language
            var defaultDictionary = DictionaryProvider.DefaultDictionary;
            if (defaultDictionary == null)
            {
                return null;
            }

            var keyDefault = defaultDictionary.As<IMultiTenantLocalizationDictionary>().TryGetKey(tenantId, value);
            if (keyDefault == null)
            {
                return null;
            }

            return keyDefault;
        }

        public string GetString(int? tenantId, string name, CultureInfo culture)
        {
            var value = GetStringOrNull(tenantId, name, culture);

            if (value == null)
            {
                return ReturnGivenNameOrThrowException(name, culture);
            }

            return value;
        }

        public string GetStringOrNull(int? tenantId, string name, CultureInfo culture, bool tryDefaults = true)
        {
            var cultureName = culture.Name;
            var dictionaries = DictionaryProvider.Dictionaries;

            //Try to get from original dictionary (with country code)
            if (dictionaries.TryGetValue(cultureName, out var originalDictionary))
            {
                var strOriginal = originalDictionary
                    .As<IMultiTenantLocalizationDictionary>()
                    .GetOrNull(tenantId, name);

                if (strOriginal != null)
                {
                    return strOriginal.Value;
                }
            }

            if (!tryDefaults)
            {
                return null;
            }

            //Try to get from same language dictionary (without country code)
            if (cultureName.Contains("-")) //Example: "tr-TR" (length=5)
            {
                if (dictionaries.TryGetValue(GetBaseCultureName(cultureName), out var langDictionary))
                {
                    var strLang = langDictionary.As<IMultiTenantLocalizationDictionary>().GetOrNull(tenantId, name);
                    if (strLang != null)
                    {
                        return strLang.Value;
                    }
                }
            }

            //Try to get from default language
            var defaultDictionary = DictionaryProvider.DefaultDictionary;
            if (defaultDictionary == null)
            {
                return null;
            }

            var strDefault = defaultDictionary.As<IMultiTenantLocalizationDictionary>().GetOrNull(tenantId, name);
            if (strDefault == null)
            {
                return null;
            }

            return strDefault.Value;
        }

        public List<string> GetStrings(int? tenantId, List<string> names, CultureInfo culture)
        {
            var value = GetStringsOrNull(tenantId, names, culture);

            if (value == null)
            {
                return ReturnGivenNamesOrThrowException(names, culture);
            }

            return value;
        }

        public List<string> GetStringsOrNull(int? tenantId, List<string> names, CultureInfo culture, bool tryDefaults = true)
        {
            var cultureName = culture.Name;
            var dictionaries = DictionaryProvider.Dictionaries;

            //Try to get from original dictionary (with country code)
            if (dictionaries.TryGetValue(cultureName, out var originalDictionary))
            {
                var strOriginal = originalDictionary
                    .As<IMultiTenantLocalizationDictionary>()
                    .GetStringsOrNull(tenantId, names);

                if (!strOriginal.IsNullOrEmpty())
                {
                    return strOriginal.Select(x => x.Value).ToList();
                }
            }

            if (!tryDefaults)
            {
                return null;
            }

            //Try to get from same language dictionary (without country code)
            if (cultureName.Contains("-")) //Example: "tr-TR" (length=5)
            {
                if (dictionaries.TryGetValue(GetBaseCultureName(cultureName), out var langDictionary))
                {
                    var strLang = langDictionary.As<IMultiTenantLocalizationDictionary>().GetStringsOrNull(tenantId, names);
                    if (!strLang.IsNullOrEmpty())
                    {
                        return strLang.Select(x => x.Value).ToList();
                    }
                }
            }

            //Try to get from default language
            var defaultDictionary = DictionaryProvider.DefaultDictionary;
            if (defaultDictionary == null)
            {
                return null;
            }

            var strDefault = defaultDictionary.As<IMultiTenantLocalizationDictionary>().GetStringsOrNull(tenantId, names);
            if (strDefault.IsNullOrEmpty())
            {
                return null;
            }

            return strDefault.Select(x => x.Value).ToList();
        }

        public override void Extend(ILocalizationDictionary dictionary)
        {
            DictionaryProvider.Extend(dictionary);
        }

        private static string GetBaseCultureName(string cultureName)
        {
            return cultureName.Contains("-")
                ? cultureName.Left(cultureName.IndexOf("-", StringComparison.Ordinal))
                : cultureName;
        }
    }
}

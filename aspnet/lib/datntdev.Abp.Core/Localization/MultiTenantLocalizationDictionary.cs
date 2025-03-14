using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using datntdev.Abp.Collections.Extensions;
using datntdev.Abp.Domain.Repositories;
using datntdev.Abp.Domain.Uow;
using datntdev.Abp.Localization.Dictionaries;
using datntdev.Abp.Runtime.Caching;
using datntdev.Abp.Runtime.Session;

namespace datntdev.Abp.Localization
{
    /// <summary>
    /// Implements <see cref="IMultiTenantLocalizationDictionary"/>.
    /// </summary>
    public class MultiTenantLocalizationDictionary :
        IMultiTenantLocalizationDictionary
    {
        private readonly string _sourceName;
        private readonly ILocalizationDictionary _internalDictionary;
        private readonly IRepository<ApplicationLanguageText, long> _customLocalizationRepository;
        private readonly ICacheManager _cacheManager;
        private readonly IAbpSession _session;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiTenantLocalizationDictionary"/> class.
        /// </summary>
        public MultiTenantLocalizationDictionary(
            string sourceName,
            ILocalizationDictionary internalDictionary,
            IRepository<ApplicationLanguageText, long> customLocalizationRepository,
            ICacheManager cacheManager,
            IAbpSession session,
            IUnitOfWorkManager unitOfWorkManager)
        {
            _sourceName = sourceName;
            _internalDictionary = internalDictionary;
            _customLocalizationRepository = customLocalizationRepository;
            _cacheManager = cacheManager;
            _session = session;
            _unitOfWorkManager = unitOfWorkManager;
        }

        public CultureInfo CultureInfo
        {
            get { return _internalDictionary.CultureInfo; }
        }

        public string this[string name]
        {
            get { return _internalDictionary[name]; }
            set { _internalDictionary[name] = value; }
        }

        public string TryGetKey(int? tenantId, string value)
        {
            //Get cache
            var cache = _cacheManager.GetMultiTenantLocalizationDictionaryCache();

            //Get for current tenant
            var dictionary = cache.Get(CalculateCacheKey(tenantId), () => GetAllValuesFromDatabase(tenantId));
            var foundValue = dictionary.Values.FirstOrDefault(x => x == value);
            if (foundValue != null)
            {
                return foundValue;
            }

            //Fall back to host
            if (tenantId != null)
            {
                dictionary = cache.Get(CalculateCacheKey(null), () => GetAllValuesFromDatabase(null));
                foundValue = dictionary.Values.FirstOrDefault(x => x == value);
                if (foundValue != null)
                {
                    return foundValue;
                }
            }

            //Not found in database, fall back to public dictionary
            var internalLocalizedString = _internalDictionary.TryGetKey(value);
            if (internalLocalizedString != null)
            {
                return internalLocalizedString;
            }

            //Not found at all
            return null;
        }

        public string TryGetKey(string value)
        {
            return TryGetKey(_session.TenantId, value);
        }

        public LocalizedString GetOrNull(string name)
        {
            return GetOrNull(_session.TenantId, name);
        }

        public IReadOnlyList<LocalizedString> GetStringsOrNull(List<string> names)
        {
            return GetStringsOrNull(_session.TenantId, names);
        }

        public LocalizedString GetOrNull(int? tenantId, string name)
        {
            //Get cache
            var cache = _cacheManager.GetMultiTenantLocalizationDictionaryCache();

            //Get for current tenant
            var dictionary = cache.Get(CalculateCacheKey(tenantId), () => GetAllValuesFromDatabase(tenantId));
            var value = dictionary.GetOrDefault(name);
            if (value != null)
            {
                return new LocalizedString(name, value, CultureInfo);
            }

            //Fall back to host
            if (tenantId != null)
            {
                dictionary = cache.Get(CalculateCacheKey(null), () => GetAllValuesFromDatabase(null));
                value = dictionary.GetOrDefault(name);
                if (value != null)
                {
                    return new LocalizedString(name, value, CultureInfo);
                }
            }

            //Not found in database, fall back to public dictionary
            var internalLocalizedString = _internalDictionary.GetOrNull(name);
            if (internalLocalizedString != null)
            {
                return internalLocalizedString;
            }

            //Not found at all
            return null;
        }

        public IReadOnlyList<LocalizedString> GetStringsOrNull(int? tenantId, List<string> names)
        {
            //Get cache
            var cache = _cacheManager.GetMultiTenantLocalizationDictionaryCache();

            //Create a temp dictionary to build (by underlying dictionary)
            var dictionary = new Dictionary<string, LocalizedString>();

            foreach (var localizedString in _internalDictionary.GetStringsOrNull(names))
            {
                dictionary[localizedString.Name] = localizedString;
            }

            //Override by host
            if (tenantId != null)
            {
                var defaultDictionary = cache.Get(CalculateCacheKey(null), () => GetAllValuesFromDatabase(null));
                foreach (var keyValue in defaultDictionary.Where(x => names.Contains(x.Key)))
                {
                    dictionary[keyValue.Key] = new LocalizedString(keyValue.Key, keyValue.Value, CultureInfo);
                }
            }

            //Override by tenant
            var tenantDictionary = cache.Get(CalculateCacheKey(tenantId), () => GetAllValuesFromDatabase(tenantId));
            foreach (var keyValue in tenantDictionary.Where(x => names.Contains(x.Key)))
            {
                dictionary[keyValue.Key] = new LocalizedString(keyValue.Key, keyValue.Value, CultureInfo);
            }

            return dictionary.Values.ToImmutableList();
        }


        public IReadOnlyList<LocalizedString> GetAllStrings()
        {
            return GetAllStrings(_session.TenantId);
        }

        public IReadOnlyList<LocalizedString> GetAllStrings(int? tenantId)
        {
            //Get cache
            var cache = _cacheManager.GetMultiTenantLocalizationDictionaryCache();

            //Create a temp dictionary to build (by underlying dictionary)
            var dictionary = new Dictionary<string, LocalizedString>();

            foreach (var localizedString in _internalDictionary.GetAllStrings())
            {
                dictionary[localizedString.Name] = localizedString;
            }

            //Override by host
            if (tenantId != null)
            {
                var defaultDictionary = cache.Get(CalculateCacheKey(null), () => GetAllValuesFromDatabase(null));
                foreach (var keyValue in defaultDictionary)
                {
                    dictionary[keyValue.Key] = new LocalizedString(keyValue.Key, keyValue.Value, CultureInfo);
                }
            }

            //Override by tenant
            var tenantDictionary = cache.Get(CalculateCacheKey(tenantId), () => GetAllValuesFromDatabase(tenantId));
            foreach (var keyValue in tenantDictionary)
            {
                dictionary[keyValue.Key] = new LocalizedString(keyValue.Key, keyValue.Value, CultureInfo);
            }

            return dictionary.Values.ToImmutableList();
        }

        private string CalculateCacheKey(int? tenantId)
        {
            return MultiTenantLocalizationDictionaryCacheHelper.CalculateCacheKey(
                tenantId,
                _sourceName,
                CultureInfo.Name
            );
        }

        protected virtual Dictionary<string, string> GetAllValuesFromDatabase(int? tenantId)
        {
            return _unitOfWorkManager.WithUnitOfWork(() =>
            {
                using (_unitOfWorkManager.Current.SetTenantId(tenantId))
                {
                    return _customLocalizationRepository
                        .GetAllList(l => l.Source == _sourceName && l.LanguageName == CultureInfo.Name)
                        .ToDictionary(l => l.Key, l => l.Value);
                }
            });
        }
    }
}

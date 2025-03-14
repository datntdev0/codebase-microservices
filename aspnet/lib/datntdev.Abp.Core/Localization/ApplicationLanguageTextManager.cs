using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using datntdev.Abp.Dependency;
using datntdev.Abp.Domain.Repositories;
using datntdev.Abp.Domain.Uow;
using datntdev.Abp.Extensions;

namespace datntdev.Abp.Localization
{
    /// <summary>
    /// Manages localization texts for host and tenants.
    /// </summary>
    public class ApplicationLanguageTextManager : IApplicationLanguageTextManager, ITransientDependency
    {
        private readonly ILocalizationManager _localizationManager;
        private readonly IRepository<ApplicationLanguageText, long> _applicationTextRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationLanguageTextManager"/> class.
        /// </summary>
        public ApplicationLanguageTextManager(
            ILocalizationManager localizationManager,
            IRepository<ApplicationLanguageText, long> applicationTextRepository,
            IUnitOfWorkManager unitOfWorkManager)
        {
            _localizationManager = localizationManager;
            _applicationTextRepository = applicationTextRepository;
            _unitOfWorkManager = unitOfWorkManager;
        }

        /// <summary>
        /// Gets a localized string value.
        /// </summary>
        /// <param name="tenantId">TenantId or null for host</param>
        /// <param name="sourceName">Source name</param>
        /// <param name="culture">Culture</param>
        /// <param name="key">Localization key</param>
        /// <param name="tryDefaults">True: fallbacks to default languages if can not find in given culture</param>
        public string GetStringOrNull(int? tenantId, string sourceName, CultureInfo culture, string key, bool tryDefaults = true)
        {
            var source = _localizationManager.GetSource(sourceName);

            if (!(source is IMultiTenantLocalizationSource))
            {
                return source.GetStringOrNull(key, culture, tryDefaults);
            }

            return source
                .As<IMultiTenantLocalizationSource>()
                .GetStringOrNull(tenantId, key, culture, tryDefaults);
        }

        public List<string> GetStringsOrNull(int? tenantId, string sourceName, CultureInfo culture, List<string> keys, bool tryDefaults = true)
        {
            var source = _localizationManager.GetSource(sourceName);

            if (!(source is IMultiTenantLocalizationSource))
            {
                return source.GetStringsOrNull(keys, culture, tryDefaults);
            }

            return source
                .As<IMultiTenantLocalizationSource>()
                .GetStringsOrNull(tenantId, keys, culture, tryDefaults);
        }

        /// <summary>
        /// Updates a localized string value.
        /// </summary>
        /// <param name="tenantId">TenantId or null for host</param>
        /// <param name="sourceName">Source name</param>
        /// <param name="culture">Culture</param>
        /// <param name="key">Localization key</param>
        /// <param name="value">New localized value.</param>
        public virtual async Task UpdateStringAsync(int? tenantId, string sourceName, CultureInfo culture, string key, string value)
        {
            await _unitOfWorkManager.WithUnitOfWorkAsync(async () =>
            {
                using (_unitOfWorkManager.Current.SetTenantId(tenantId))
                {
                    var existingEntity = (await _applicationTextRepository.GetAllListAsync(t =>
                            t.Source == sourceName &&
                            t.LanguageName == culture.Name &&
                            t.Key == key))
                        .FirstOrDefault(t => t.Key == key);

                    if (existingEntity != null)
                    {
                        if (existingEntity.Value != value)
                        {
                            existingEntity.Value = value;
                            await _unitOfWorkManager.Current.SaveChangesAsync();
                        }
                    }
                    else
                    {
                        await _applicationTextRepository.InsertAsync(
                            new ApplicationLanguageText
                            {
                                TenantId = tenantId,
                                Source = sourceName,
                                LanguageName = culture.Name,
                                Key = key,
                                Value = value
                            });
                        await _unitOfWorkManager.Current.SaveChangesAsync();
                    }
                }
            });
        }

        /// <summary>
        /// Delete a localized string value for a tenant.
        /// </summary>
        /// <param name="tenantId">TenantId</param>
        /// <param name="sourceName">Source name</param>
        /// <param name="culture">Culture</param>
        /// <param name="key">Localization key</param>
        public virtual async Task DeleteStringAsync(int tenantId, string sourceName, CultureInfo culture, string key)
        {
            await _unitOfWorkManager.WithUnitOfWorkAsync(async () =>
            {
                using (_unitOfWorkManager.Current.SetTenantId(tenantId))
                {
                    var existingEntity = (await _applicationTextRepository.GetAllListAsync(t =>
                            t.Source == sourceName &&
                            t.LanguageName == culture.Name &&
                            t.Key == key))
                        .FirstOrDefault(t => t.Key == key);

                    if (existingEntity != null)
                    {
                        await _applicationTextRepository.DeleteAsync(existingEntity.Id);
                    }
                }
            });
        }
    }
}

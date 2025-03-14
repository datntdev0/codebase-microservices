using System.Collections.Generic;
using System.Threading.Tasks;

namespace datntdev.Abp.Localization
{
    /// <summary>
    /// Manages host and tenant languages.
    /// </summary>
    public interface IApplicationLanguageManager
    {
        /// <summary>
        /// Gets list of all languages available to given tenant (or null for host)
        /// </summary>
        /// <param name="tenantId">TenantId or null for host</param>
        Task<IReadOnlyList<ApplicationLanguage>> GetLanguagesAsync(int? tenantId);

        /// <summary>
        /// Gets list of all active languages available to given tenant (or null for host)
        /// </summary>
        /// <param name="tenantId">TenantId or null for host</param>
        Task<IReadOnlyList<ApplicationLanguage>> GetActiveLanguagesAsync(int? tenantId);

        /// <summary>
        /// Gets list of all languages available to given tenant (or null for host)
        /// </summary>
        /// <param name="tenantId">TenantId or null for host</param>
        IReadOnlyList<ApplicationLanguage> GetLanguages(int? tenantId);

        /// <summary>
        /// Gets list of all active languages available to given tenant (or null for host)
        /// </summary>
        /// <param name="tenantId">TenantId or null for host</param>
        IReadOnlyList<ApplicationLanguage> GetActiveLanguages(int? tenantId);

        /// <summary>
        /// Adds a new language.
        /// </summary>
        /// <param name="language">The language.</param>
        Task AddAsync(ApplicationLanguage language);

        /// <summary>
        /// Adds a new language.
        /// </summary>
        /// <param name="language">The language.</param>
        void Add(ApplicationLanguage language);

        /// <summary>
        /// Deletes a language.
        /// </summary>
        /// <param name="tenantId">Tenant Id or null for host.</param>
        /// <param name="languageName">Name of the language.</param>
        Task RemoveAsync(int? tenantId, string languageName);

        /// <summary>
        /// Deletes a language.
        /// </summary>
        /// <param name="tenantId">Tenant Id or null for host.</param>
        /// <param name="languageName">Name of the language.</param>
        void Remove(int? tenantId, string languageName);

        /// <summary>
        /// Updates a language.
        /// </summary>
        /// <param name="tenantId">Tenant Id or null for host.</param>
        /// <param name="language">The language to be updated</param>
        Task UpdateAsync(int? tenantId, ApplicationLanguage language);

        /// <summary>
        /// Updates a language.
        /// </summary>
        /// <param name="tenantId">Tenant Id or null for host.</param>
        /// <param name="language">The language to be updated</param>
        void Update(int? tenantId, ApplicationLanguage language);

        /// <summary>
        /// Gets the default language or null for a tenant or the host.
        /// </summary>
        /// <param name="tenantId">Tenant Id of null for host</param>
        Task<ApplicationLanguage> GetDefaultLanguageOrNullAsync(int? tenantId);

        /// <summary>
        /// Gets the default language or null for a tenant or the host.
        /// </summary>
        /// <param name="tenantId">Tenant Id of null for host</param>
        ApplicationLanguage GetDefaultLanguageOrNull(int? tenantId);

        /// <summary>
        /// Sets the default language for a tenant or the host.
        /// </summary>
        /// <param name="tenantId">Tenant Id of null for host</param>
        /// <param name="languageName">Name of the language.</param>
        Task SetDefaultLanguageAsync(int? tenantId, string languageName);

        /// <summary>
        /// Sets the default language for a tenant or the host.
        /// </summary>
        /// <param name="tenantId">Tenant Id of null for host</param>
        /// <param name="languageName">Name of the language.</param>
        void SetDefaultLanguage(int? tenantId, string languageName);
    }
}
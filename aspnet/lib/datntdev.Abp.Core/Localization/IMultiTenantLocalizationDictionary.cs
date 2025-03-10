using System.Collections.Generic;
using datntdev.Abp.Localization.Dictionaries;

namespace datntdev.Abp.Localization
{
    /// <summary>
    /// Extends <see cref="ILocalizationDictionary"/> to add tenant and database based localization.
    /// </summary>
    public interface IMultiTenantLocalizationDictionary : ILocalizationDictionary
    {
        /// <summary>
        /// Gets a <see cref="string"/> for given <paramref name="value"/>.
        /// </summary>
        /// <param name="tenantId">TenantId or null for host.</param>
        /// <param name="value">Value to get key</param>
        /// <returns>The key or null</returns>
        string TryGetKey(int? tenantId, string value);

        /// <summary>
        /// Gets a <see cref="LocalizedString"/>.
        /// </summary>
        /// <param name="tenantId">TenantId or null for host.</param>
        /// <param name="name">Localization key name.</param>
        LocalizedString GetOrNull(int? tenantId, string name);

        /// <summary>
        /// Gets a <see cref="LocalizedString"/>.
        /// </summary>
        /// <param name="tenantId">TenantId or null for host.</param>
        /// <param name="names">List of localization key names.</param>
        IReadOnlyList<LocalizedString> GetStringsOrNull(int? tenantId, List<string> names);

        /// <summary>
        /// Gets all <see cref="LocalizedString"/>s.
        /// </summary>
        /// <param name="tenantId">TenantId or null for host.</param>
        IReadOnlyList<LocalizedString> GetAllStrings(int? tenantId);
    }
}

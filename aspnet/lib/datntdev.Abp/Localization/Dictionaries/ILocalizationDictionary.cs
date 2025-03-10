using System.Collections.Generic;
using System.Globalization;

namespace datntdev.Abp.Localization.Dictionaries
{
    /// <summary>
    /// Represents a dictionary that is used to find a localized string.
    /// </summary>
    public interface ILocalizationDictionary
    {
        /// <summary>
        /// Culture of the dictionary.
        /// </summary>
        CultureInfo CultureInfo { get; }

        /// <summary>
        /// Gets/sets a string for this dictionary with given name (key).
        /// </summary>
        /// <param name="name">Name to get/set</param>
        string this[string name] { get; set; }

        /// <summary>
        /// Gets a <see cref="string"/> for given <paramref name="value"/>.
        /// </summary>
        /// <param name="value">Value to get key</param>
        /// <returns>The key or null</returns>
        string TryGetKey(string value);

        /// <summary>
        /// Gets a <see cref="LocalizedString"/> for given <paramref name="name"/>.
        /// </summary>
        /// <param name="name">Name (key) to get localized string</param>
        /// <returns>The localized string or null if not found in this dictionary</returns>
        LocalizedString GetOrNull(string name);

        /// <summary>
        /// Gets a <see cref="LocalizedString"/> for given <paramref name="names"/>.
        /// </summary>
        /// <param name="names">Names (key) to get list of localized strings</param>
        /// <returns>The localized string or null if not found in this dictionary</returns>
        IReadOnlyList<LocalizedString> GetStringsOrNull(List<string> names);

        /// <summary>
        /// Gets a list of all strings in this dictionary.
        /// </summary>
        /// <returns>List of all <see cref="LocalizedString"/> object</returns>
        IReadOnlyList<LocalizedString> GetAllStrings();
    }
}

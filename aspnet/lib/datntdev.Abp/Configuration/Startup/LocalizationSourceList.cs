using System.Collections.Generic;
using datntdev.Abp.Localization.Sources;

namespace datntdev.Abp.Configuration.Startup
{
    /// <summary>
    /// A specialized list to store <see cref="ILocalizationSource"/> object.
    /// </summary>
    public class LocalizationSourceList : List<ILocalizationSource>, ILocalizationSourceList
    {
        public IList<LocalizationSourceExtensionInfo> Extensions { get; private set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        public LocalizationSourceList()
        {
            Extensions = new List<LocalizationSourceExtensionInfo>();
        }
    }
}
using System.Collections.Generic;

namespace datntdev.Abp.Localization
{
    public interface ILanguageProvider
    {
        IReadOnlyList<LanguageInfo> GetLanguages();

        IReadOnlyList<LanguageInfo> GetActiveLanguages();
    }
}
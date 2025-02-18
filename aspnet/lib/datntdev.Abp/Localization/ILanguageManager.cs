using System.Collections.Generic;

namespace datntdev.Abp.Localization
{
    public interface ILanguageManager
    {
        LanguageInfo CurrentLanguage { get; }

        IReadOnlyList<LanguageInfo> GetLanguages();

        IReadOnlyList<LanguageInfo> GetActiveLanguages();
    }
}
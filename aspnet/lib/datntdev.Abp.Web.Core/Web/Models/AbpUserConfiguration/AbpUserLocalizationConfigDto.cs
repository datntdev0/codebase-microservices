using System.Collections.Generic;
using datntdev.Abp.Localization;

namespace datntdev.Abp.Web.Models.AbpUserConfiguration
{
    public class AbpUserLocalizationConfigDto
    {
        public AbpUserCurrentCultureConfigDto CurrentCulture { get; set; }

        public List<LanguageInfo> Languages { get; set; }

        public LanguageInfo CurrentLanguage { get; set; }

        public List<AbpLocalizationSourceDto> Sources { get; set; }

        public Dictionary<string, Dictionary<string, string>> Values { get; set; }
    }
}
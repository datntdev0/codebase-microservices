using System.Collections.Generic;

namespace datntdev.Abp.Web.Models.AbpUserConfiguration
{
    public class AbpUserFeatureConfigDto
    {
        public Dictionary<string, AbpStringValueDto> AllFeatures { get; set; }
    }
}
using System.Collections.Generic;
using datntdev.Abp.Application.Navigation;

namespace datntdev.Abp.Web.Models.AbpUserConfiguration
{
    public class AbpUserNavConfigDto
    {
        public Dictionary<string, UserMenu> Menus { get; set; }
    }
}
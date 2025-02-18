using System;

namespace datntdev.Abp.Web.Core.Mvc.Caching;

public class GetScriptsResponsePerUserConfiguration : IGetScriptsResponsePerUserConfiguration
{
    public bool IsEnabled { get; set; }
    public TimeSpan MaxAge { get; set; } = TimeSpan.FromMinutes(30);
}

using System;

namespace datntdev.Abp.Web.Core.Mvc.Caching;

public interface IGetScriptsResponsePerUserConfiguration
{
    bool IsEnabled { get; set; }

    TimeSpan MaxAge { get; set; }
}

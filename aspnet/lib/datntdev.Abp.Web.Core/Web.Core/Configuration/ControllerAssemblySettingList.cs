using System;
using System.Collections.Generic;
using System.Linq;
using datntdev.Abp.Reflection.Extensions;
using JetBrains.Annotations;

namespace datntdev.Abp.Web.Core.Configuration;

public class ControllerAssemblySettingList : List<AbpControllerAssemblySetting>
{
    public List<AbpControllerAssemblySetting> GetSettings(Type controllerType)
    {
        return this.Where(controllerSetting => controllerSetting.Assembly == controllerType.GetAssembly()).ToList();
    }
}

using System;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace datntdev.Abp.Web.Core.Configuration;

public interface IAbpControllerAssemblySettingBuilder
{
    AbpControllerAssemblySettingBuilder Where(Func<Type, bool> predicate);

    AbpControllerAssemblySettingBuilder ConfigureControllerModel(Action<ControllerModel> configurer);
}

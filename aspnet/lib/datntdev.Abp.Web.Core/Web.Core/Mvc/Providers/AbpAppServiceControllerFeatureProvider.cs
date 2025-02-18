using System.Linq;
using System.Reflection;
using datntdev.Abp.Application.Services;
using datntdev.Abp.Web.Core.Configuration;
using datntdev.Abp.Collections.Extensions;
using datntdev.Abp.Dependency;
using datntdev.Abp.Reflection;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace datntdev.Abp.Web.Core.Mvc.Providers;

/// <summary>
/// Used to add application services as controller.
/// </summary>
public class AbpAppServiceControllerFeatureProvider : ControllerFeatureProvider
{
    private readonly IIocResolver _iocResolver;

    public AbpAppServiceControllerFeatureProvider(IIocResolver iocResolver)
    {
        _iocResolver = iocResolver;
    }

    protected override bool IsController(TypeInfo typeInfo)
    {
        var type = typeInfo.AsType();

        if (!typeof(IApplicationService).IsAssignableFrom(type) ||
            !typeInfo.IsPublic || typeInfo.IsAbstract || typeInfo.IsGenericType)
        {
            return false;
        }

        var remoteServiceAttr = ReflectionHelper.GetSingleAttributeOrDefault<RemoteServiceAttribute>(typeInfo);

        if (remoteServiceAttr != null && !remoteServiceAttr.IsEnabledFor(type))
        {
            return false;
        }

        var settings = _iocResolver.Resolve<AbpWebCoreConfiguration>().ControllerAssemblySettings.GetSettings(type);
        return settings.Any(setting => setting.TypePredicate(type));
    }
}

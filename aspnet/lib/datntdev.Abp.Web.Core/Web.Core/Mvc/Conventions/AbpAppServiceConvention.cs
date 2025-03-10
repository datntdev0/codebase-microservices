﻿using System;
using System.Collections.Generic;
using datntdev.Abp.Application.Services;
using datntdev.Abp.Web.Core.Configuration;
using datntdev.Abp.Extensions;
using Castle.Windsor.MsDependencyInjection;
using datntdev.Abp.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Reflection;
using datntdev.Abp.Collections.Extensions;
using datntdev.Abp.Web.Api.ProxyScripting.Generators;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ActionConstraints;

namespace datntdev.Abp.Web.Core.Mvc.Conventions;

public class AbpAppServiceConvention : IApplicationModelConvention
{
    private static readonly string[] CommonVerbPrefixes = ["Create", "Update", "Delete", "Get", "Patch"];

    private readonly Lazy<AbpWebCoreConfiguration> _configuration;

    public AbpAppServiceConvention(IServiceCollection services)
    {
        _configuration = new Lazy<AbpWebCoreConfiguration>(() =>
        {
            return services
                .GetSingletonService<AbpBootstrapper>()
                .IocManager
                .Resolve<AbpWebCoreConfiguration>();
        }, true);
    }

    public void Apply(ApplicationModel application)
    {
        foreach (var controller in application.Controllers)
        {
            var type = controller.ControllerType.AsType();
            var configuration = GetControllerSettingOrNull(type);

            if (typeof(IApplicationService).GetTypeInfo().IsAssignableFrom(type))
            {
                controller.ControllerName = controller.ControllerName.RemovePostFix(ApplicationService.CommonPostfixes);
                configuration?.ControllerModelConfigurer(controller);

                ConfigureCacheControl(controller, _configuration.Value.DefaultResponseCacheAttributeForAppServices);
                ConfigureArea(controller, configuration);
                ConfigureRemoteService(controller, configuration);
            }
            else
            {
                var remoteServiceAtt = ReflectionHelper.GetSingleAttributeOrDefault<RemoteServiceAttribute>(type.GetTypeInfo());
                if (remoteServiceAtt != null && remoteServiceAtt.IsEnabledFor(type))
                {
                    ConfigureCacheControl(controller, _configuration.Value.DefaultResponseCacheAttributeForControllers);
                    ConfigureRemoteService(controller, configuration);
                }
            }
        }
    }

    private void ConfigureCacheControl(ControllerModel controller, ResponseCacheAttribute responseCacheAttribute)
    {
        if (responseCacheAttribute == null)
        {
            return;
        }

        if (controller.Filters.Any(filter => typeof(ResponseCacheAttribute).IsAssignableFrom(filter.GetType())))
        {
            return;
        }

        controller.Filters.Add(responseCacheAttribute);
    }

    private void ConfigureArea(ControllerModel controller, [CanBeNull] AbpControllerAssemblySetting configuration)
    {
        if (configuration == null)
        {
            return;
        }

        if (controller.RouteValues.ContainsKey("area"))
        {
            return;
        }

        controller.RouteValues["area"] = configuration.ModuleName;
    }

    private void ConfigureRemoteService(ControllerModel controller, [CanBeNull] AbpControllerAssemblySetting configuration)
    {
        ConfigureApiExplorer(controller);
        ConfigureSelector(controller, configuration);
        ConfigureParameters(controller);
    }

    private void ConfigureParameters(ControllerModel controller)
    {
        foreach (var action in controller.Actions)
        {
            foreach (var prm in action.Parameters)
            {
                if (prm.BindingInfo != null)
                {
                    continue;
                }

                if (!TypeHelper.IsPrimitiveExtendedIncludingNullable(prm.ParameterInfo.ParameterType))
                {
                    if (CanUseFormBodyBinding(action, prm))
                    {
                        prm.BindingInfo = BindingInfo.GetBindingInfo(new[] { new FromBodyAttribute() });
                    }
                }
            }
        }
    }

    private bool CanUseFormBodyBinding(ActionModel action, ParameterModel parameter)
    {
        if (_configuration.Value.FormBodyBindingIgnoredTypes.Any(t => t.IsAssignableFrom(parameter.ParameterInfo.ParameterType)))
        {
            return false;
        }

        foreach (var selector in action.Selectors)
        {
            if (selector.ActionConstraints == null)
            {
                continue;
            }

            foreach (var actionConstraint in selector.ActionConstraints)
            {
                var httpMethodActionConstraint = actionConstraint as HttpMethodActionConstraint;
                if (httpMethodActionConstraint == null)
                {
                    continue;
                }

                if (httpMethodActionConstraint.HttpMethods.All(hm => hm.IsIn("GET", "DELETE", "TRACE", "HEAD")))
                {
                    return false;
                }
            }
        }

        return true;
    }

    private void ConfigureApiExplorer(ControllerModel controller)
    {
        if (controller.ApiExplorer.GroupName.IsNullOrEmpty())
        {
            controller.ApiExplorer.GroupName = controller.ControllerName;
        }

        if (controller.ApiExplorer.IsVisible == null)
        {
            var controllerType = controller.ControllerType.AsType();
            var remoteServiceAtt = ReflectionHelper.GetSingleAttributeOrDefault<RemoteServiceAttribute>(controllerType.GetTypeInfo());
            if (remoteServiceAtt != null)
            {
                controller.ApiExplorer.IsVisible =
                    remoteServiceAtt.IsEnabledFor(controllerType) &&
                    remoteServiceAtt.IsMetadataEnabledFor(controllerType);
            }
            else
            {
                controller.ApiExplorer.IsVisible = true;
            }
        }

        foreach (var action in controller.Actions)
        {
            ConfigureApiExplorer(action);
        }
    }

    private void ConfigureApiExplorer(ActionModel action)
    {
        if (action.ApiExplorer.IsVisible == null)
        {
            var remoteServiceAtt = ReflectionHelper.GetSingleAttributeOrDefault<RemoteServiceAttribute>(action.ActionMethod);
            if (remoteServiceAtt != null)
            {
                action.ApiExplorer.IsVisible =
                    remoteServiceAtt.IsEnabledFor(action.ActionMethod) &&
                    remoteServiceAtt.IsMetadataEnabledFor(action.ActionMethod);
            }
        }
    }

    private void ConfigureSelector(ControllerModel controller, [CanBeNull] AbpControllerAssemblySetting configuration)
    {
        RemoveEmptySelectors(controller.Selectors);

        if (controller.Selectors.Any(selector => selector.AttributeRouteModel != null))
        {
            return;
        }

        var moduleName = GetModuleNameOrDefault(controller.ControllerType.AsType());

        foreach (var action in controller.Actions)
        {
            ConfigureSelector(moduleName, controller.ControllerName, action, configuration);
        }
    }

    private void ConfigureSelector(string moduleName, string controllerName, ActionModel action, [CanBeNull] AbpControllerAssemblySetting configuration)
    {
        RemoveEmptySelectors(action.Selectors);

        var remoteServiceAtt = ReflectionHelper.GetSingleAttributeOrDefault<RemoteServiceAttribute>(action.ActionMethod);
        if (remoteServiceAtt != null && !remoteServiceAtt.IsEnabledFor(action.ActionMethod))
        {
            return;
        }

        if (!action.Selectors.Any())
        {
            AddAbpServiceSelector(moduleName, controllerName, action, configuration);
        }
        else
        {
            NormalizeSelectorRoutes(moduleName, controllerName, action, configuration);
        }
    }

    private void AddAbpServiceSelector(string moduleName, string controllerName, ActionModel action, [CanBeNull] AbpControllerAssemblySetting configuration)
    {
        var abpServiceSelectorModel = new SelectorModel
        {
            AttributeRouteModel = CreateAbpServiceAttributeRouteModel(moduleName, controllerName, action)
        };

        var httpMethod = SelectHttpMethod(action, configuration);

        abpServiceSelectorModel.ActionConstraints.Add(new HttpMethodActionConstraint(new[] { httpMethod }));

        action.Selectors.Add(abpServiceSelectorModel);
    }

    private string SelectHttpMethod(ActionModel action, AbpControllerAssemblySetting configuration)
    {
        return configuration?.UseConventionalHttpVerbs == true
            ? ProxyScriptingHelper.GetConventionalVerbForMethodName(action.ActionName)
            : ProxyScriptingHelper.DefaultHttpVerb;
    }

    private void NormalizeSelectorRoutes(string moduleName, string controllerName, ActionModel action, [CanBeNull] AbpControllerAssemblySetting configuration)
    {
        foreach (var selector in action.Selectors)
        {
            if (!selector.ActionConstraints.OfType<HttpMethodActionConstraint>().Any())
            {
                var httpMethod = SelectHttpMethod(action, configuration);
                selector.ActionConstraints.Add(new HttpMethodActionConstraint([httpMethod]));
            }

            if (selector.AttributeRouteModel == null)
            {
                selector.AttributeRouteModel = CreateAbpServiceAttributeRouteModel(
                    moduleName,
                    controllerName,
                    action
                );
            }
        }
    }

    private string GetModuleNameOrDefault(Type controllerType)
    {
        return GetControllerSettingOrNull(controllerType)?.ModuleName ??
               AbpControllerAssemblySetting.DefaultServiceModuleName;
    }

    [CanBeNull]
    private AbpControllerAssemblySetting GetControllerSettingOrNull(Type controllerType)
    {
        var settings = _configuration.Value.ControllerAssemblySettings.GetSettings(controllerType);
        return settings.FirstOrDefault(setting => setting.TypePredicate(controllerType));
    }

    private static AttributeRouteModel CreateAbpServiceAttributeRouteModel(string moduleName, string controllerName, ActionModel action)
    {
        controllerName = controllerName.ToKebabCase();
        if (((string[])["GetAllAsync", "CreateAsync", "UpdateAsync"]).Contains(action.ActionMethod.Name))
        {
            return new AttributeRouteModel(
                new RouteAttribute(
                    $"api/services/{moduleName}/{controllerName}"
                )
            );
        }

        if (((string[])["GetAsync", "DeleteAsync"]).Contains(action.ActionMethod.Name))
        {
            return new AttributeRouteModel(
                new RouteAttribute(
                    $"api/services/{moduleName}/{controllerName}/{{id}}"
                )
            );
        }

        var actionName = action.ActionName.RemovePreFix(CommonVerbPrefixes).ToKebabCase();
        return new AttributeRouteModel(
            new RouteAttribute(
                $"api/services/{moduleName}/{controllerName}/{actionName}"
            )
        );
    }

    private static void RemoveEmptySelectors(IList<SelectorModel> selectors)
    {
        selectors
            .Where(IsEmptySelector)
            .ToList()
            .ForEach(s => selectors.Remove(s));
    }

    private static bool IsEmptySelector(SelectorModel selector)
    {
        return selector.AttributeRouteModel == null
               && selector.ActionConstraints.IsNullOrEmpty()
               && selector.EndpointMetadata.IsNullOrEmpty();
    }
}

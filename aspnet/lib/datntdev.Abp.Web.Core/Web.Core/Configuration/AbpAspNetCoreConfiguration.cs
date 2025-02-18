using System;
using System.Collections.Generic;
using System.Reflection;
using datntdev.Abp.Domain.Uow;
using datntdev.Abp.Web.Models;
using datntdev.Abp.Web.Results.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace datntdev.Abp.Web.Core.Configuration;

public class AbpWebCoreConfiguration : IAbpWebCoreConfiguration
{
    public WrapResultAttribute DefaultWrapResultAttribute { get; }

    public ResponseCacheAttribute DefaultResponseCacheAttributeForControllers { get; set; }

    public ResponseCacheAttribute DefaultResponseCacheAttributeForAppServices { get; set; }

    public UnitOfWorkAttribute DefaultUnitOfWorkAttribute { get; }

    public List<Type> FormBodyBindingIgnoredTypes { get; }

    public ControllerAssemblySettingList ControllerAssemblySettings { get; }

    public bool IsValidationEnabledForControllers { get; set; }

    public bool IsAuditingEnabled { get; set; }

    public bool SetNoCacheForAjaxResponses { get; set; }

    public bool UseMvcDateTimeFormatForAppServices { get; set; }

    public List<string> InputDateTimeFormats { get; set; }

    public string OutputDateTimeFormat { get; set; }

    public List<Action<IEndpointRouteBuilder>> EndpointConfiguration { get; }

    public WrapResultFilterCollection WrapResultFilters { get; }

    public AbpWebCoreConfiguration()
    {
        DefaultWrapResultAttribute = new WrapResultAttribute();
        DefaultResponseCacheAttributeForControllers = null;
        DefaultResponseCacheAttributeForAppServices = null;
        DefaultUnitOfWorkAttribute = new UnitOfWorkAttribute();
        ControllerAssemblySettings = new ControllerAssemblySettingList();
        FormBodyBindingIgnoredTypes = new List<Type>();
        EndpointConfiguration = new List<Action<IEndpointRouteBuilder>>();
        WrapResultFilters = new WrapResultFilterCollection();
        IsValidationEnabledForControllers = true;
        SetNoCacheForAjaxResponses = true;
        IsAuditingEnabled = true;
        UseMvcDateTimeFormatForAppServices = false;
        InputDateTimeFormats = null;
        OutputDateTimeFormat = null;
    }

    public AbpControllerAssemblySettingBuilder CreateControllersForAppServices(
        Assembly assembly,
        string moduleName = AbpControllerAssemblySetting.DefaultServiceModuleName,
        bool useConventionalHttpVerbs = true)
    {
        var setting = new AbpControllerAssemblySetting(moduleName, assembly, useConventionalHttpVerbs);
        ControllerAssemblySettings.Add(setting);
        return new AbpControllerAssemblySettingBuilder(setting);
    }
}

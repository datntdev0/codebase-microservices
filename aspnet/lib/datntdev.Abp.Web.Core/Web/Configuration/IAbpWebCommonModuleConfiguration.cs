﻿using datntdev.Abp.Web.Api.ProxyScripting.Configuration;
using datntdev.Abp.Web.MultiTenancy;
using datntdev.Abp.Web.Results.Filters;
using datntdev.Abp.Web.Security.AntiForgery;

namespace datntdev.Abp.Web.Configuration
{
    /// <summary>
    /// Used to configure ABP Web Common module.
    /// </summary>
    public interface IAbpWebCommonModuleConfiguration
    {
        /// <summary>
        /// If this is set to true, all exception and details are sent directly to clients on an error.
        /// Default: false (ABP hides exception details from clients except special exceptions.)
        /// </summary>
        bool SendAllExceptionsToClients { get; set; }

        /// <summary>
        /// Used to configure Api proxy scripting.
        /// </summary>
        IApiProxyScriptingConfiguration ApiProxyScripting { get; }

        /// <summary>
        /// Used to configure Anti Forgery security settings.
        /// </summary>
        IAbpAntiForgeryConfiguration AntiForgery { get; }

        /// <summary>
        /// Used to configure embedded resource system for web applications.
        /// </summary>
        IWebEmbeddedResourcesConfiguration EmbeddedResources { get; }

        IWebMultiTenancyConfiguration MultiTenancy { get; }
        
        /// <summary>
        /// Used to configure wrap results
        /// </summary>
        WrapResultFilterCollection WrapResultFilters { get; }
    }
}
using System;
using System.Linq;
using datntdev.Abp.Web.Core.Configuration;
using datntdev.Abp.Web.Core.MultiTenancy;
using datntdev.Abp.Web.Core.Mvc.Auditing;
using datntdev.Abp.Web.Core.Mvc.Caching;
using datntdev.Abp.Web.Core.PlugIn;
using datntdev.Abp.Web.Core.Runtime.Session;
using datntdev.Abp.Web.Core.Security.AntiForgery;
using datntdev.Abp.Web.Core.Webhook;
using datntdev.Abp.Auditing;
using datntdev.Abp.Configuration.Startup;
using datntdev.Abp.Dependency;
using datntdev.Abp.Modules;
using datntdev.Abp.Reflection.Extensions;
using datntdev.Abp.Runtime.Session;
using datntdev.Abp.Web;
using datntdev.Abp.Web.Security.AntiForgery;
using datntdev.Abp.Webhooks;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.Options;

namespace datntdev.Abp.Web.Core;

[DependsOn(typeof(AbpWebCommonModule))]
public class AbpWebCoreModule : AbpModule
{
    public override void PreInitialize()
    {
        IocManager.AddConventionalRegistrar(new AbpWebCoreConventionalRegistrar());

        IocManager.Register<IAbpWebCoreConfiguration, AbpWebCoreConfiguration>();

        Configuration.ReplaceService<IPrincipalAccessor, AspNetCorePrincipalAccessor>(DependencyLifeStyle.Transient);
        Configuration.ReplaceService<IAbpAntiForgeryManager, AbpWebCoreAntiForgeryManager>(DependencyLifeStyle.Transient);
        Configuration.ReplaceService<IClientInfoProvider, HttpContextClientInfoProvider>(DependencyLifeStyle.Transient);
        Configuration.ReplaceService<IWebhookSender, AspNetCoreWebhookSender>(DependencyLifeStyle.Transient);

        IocManager.Register<IGetScriptsResponsePerUserConfiguration, GetScriptsResponsePerUserConfiguration>();

        Configuration.Modules.AbpWebCore().FormBodyBindingIgnoredTypes.Add(typeof(IFormFile));

        Configuration.MultiTenancy.Resolvers.Add<DomainTenantResolveContributor>();
        Configuration.MultiTenancy.Resolvers.Add<HttpHeaderTenantResolveContributor>();
        Configuration.MultiTenancy.Resolvers.Add<HttpCookieTenantResolveContributor>();

        Configuration.Caching.Configure(GetScriptsResponsePerUserCache.CacheName, cache => { cache.DefaultSlidingExpireTime = TimeSpan.FromMinutes(30); });
    }

    public override void Initialize()
    {
        IocManager.RegisterAssemblyByConvention(typeof(AbpWebCoreModule).GetAssembly());
    }

    public override void PostInitialize()
    {
        AddApplicationParts();
        ConfigureAntiforgery();
    }

    private void AddApplicationParts()
    {
        var configuration = IocManager.Resolve<AbpWebCoreConfiguration>();
        var partManager = IocManager.Resolve<ApplicationPartManager>();
        var moduleManager = IocManager.Resolve<IAbpModuleManager>();

        partManager.AddApplicationPartsIfNotAddedBefore(typeof(AbpWebCoreModule).Assembly);

        var controllerAssemblies = configuration.ControllerAssemblySettings.Select(s => s.Assembly).Distinct();
        foreach (var controllerAssembly in controllerAssemblies)
        {
            partManager.AddApplicationPartsIfNotAddedBefore(controllerAssembly);
        }

        var plugInAssemblies = moduleManager.Modules.Where(m => m.IsLoadedAsPlugIn).Select(m => m.Assembly).Distinct();
        foreach (var plugInAssembly in plugInAssemblies)
        {
            partManager.AddAbpPlugInAssemblyPartIfNotAddedBefore(new AbpPlugInAssemblyPart(plugInAssembly));
        }
    }

    private void ConfigureAntiforgery()
    {
        IocManager.Using<IOptions<AntiforgeryOptions>>(optionsAccessor =>
        {
            optionsAccessor.Value.HeaderName = Configuration.Modules.AbpWebCommon().AntiForgery.TokenHeaderName;
        });
    }
}

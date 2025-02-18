using System;
using System.Collections.Generic;
using System.Text.Json;
using datntdev.Abp.Web.Core.Configuration;
using datntdev.Abp.Web.Core.EmbeddedResources;
using datntdev.Abp.Web.Core.Mvc;
using datntdev.Abp.Dependency;
using Castle.Windsor.MsDependencyInjection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using datntdev.Abp.Web.Core.Mvc.Providers;
using datntdev.Abp.Web.Core.Webhook;
using datntdev.Abp.Auditing;
using datntdev.Abp.Domain.Uow;
using datntdev.Abp.Json.SystemTextJson;
using datntdev.Abp.Modules;
using datntdev.Abp.Runtime.Validation;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation;

namespace datntdev.Abp.Web.Core;

public static class AbpServiceCollectionExtensions
{
    /// <summary>
    /// Integrates ABP to AspNet Core.
    /// </summary>
    /// <typeparam name="TStartupModule">Startup module of the application which depends on other used modules. Should be derived from <see cref="AbpModule"/>.</typeparam>
    /// <param name="services">Services.</param>
    /// <param name="optionsAction">An action to get/modify options</param>
    /// <param name="removeConventionalInterceptors">Removes the conventional interceptors</param>
    public static IServiceProvider AddAbp<TStartupModule>(this IServiceCollection services,
        [CanBeNull] Action<AbpBootstrapperOptions> optionsAction = null,
        bool removeConventionalInterceptors = true)
        where TStartupModule : AbpModule
    {
        if (removeConventionalInterceptors)
        {
            RemoveConventionalInterceptionSelectors();
        }

        var abpBootstrapper = AddAbpBootstrapper<TStartupModule>(services, optionsAction);
        ConfigureAspNetCore(services, abpBootstrapper.IocManager);

        return WindsorRegistrationHelper.CreateServiceProvider(abpBootstrapper.IocManager.IocContainer, services);
    }

    /// <summary>
    /// Integrates ABP to AspNet Core without creating a IServiceProvider.
    /// </summary>
    /// <typeparam name="TStartupModule">Startup module of the application which depends on other used modules. Should be derived from <see cref="AbpModule"/>.</typeparam>
    /// <param name="services">Services.</param>
    /// <param name="optionsAction">An action to get/modify options</param>
    /// <param name="removeConventionalInterceptors">Removes the conventional interceptors</param>
    public static void AddAbpWithoutCreatingServiceProvider<TStartupModule>(this IServiceCollection services,
        [CanBeNull] Action<AbpBootstrapperOptions> optionsAction = null,
        bool removeConventionalInterceptors = true)
        where TStartupModule : AbpModule
    {
        if (removeConventionalInterceptors)
        {
            RemoveConventionalInterceptionSelectors();
        }

        var abpBootstrapper = AddAbpBootstrapper<TStartupModule>(services, optionsAction);
        ConfigureAspNetCore(services, abpBootstrapper.IocManager);
    }

    private static void RemoveConventionalInterceptionSelectors()
    {
        UnitOfWorkDefaultOptions.ConventionalUowSelectorList = new List<Func<Type, bool>>();
        AbpAuditingDefaultOptions.ConventionalAuditingSelectorList = new List<Func<Type, bool>>();
        AbpValidationDefaultOptions.ConventionalValidationSelectorList = new List<Func<Type, bool>>();
    }

    private static void ConfigureAspNetCore(IServiceCollection services, IIocResolver iocResolver)
    {
        //See https://github.com/aspnet/Mvc/issues/3936 to know why we added these services.
        services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();

        //Use DI to create controllers
        services.Replace(ServiceDescriptor.Transient<IControllerActivator, ServiceBasedControllerActivator>());

        //Use DI to create page models
        services.Replace(ServiceDescriptor
            .Singleton<IPageModelActivatorProvider, ServiceBasedPageModelActivatorProvider>());

        //Use DI to create view components
        services.Replace(ServiceDescriptor
            .Singleton<IViewComponentActivator, ServiceBasedViewComponentActivator>());

        //Add feature providers
        var partManager = services.GetSingletonServiceOrNull<ApplicationPartManager>();
        partManager?.FeatureProviders.Add(new AbpAppServiceControllerFeatureProvider(iocResolver));

        //Configure System Text JSON serializer
        services.AddOptions<JsonOptions>()
            .Configure<IServiceProvider>((options, rootServiceProvider) =>
        {
            options.JsonSerializerOptions.ReadCommentHandling = JsonCommentHandling.Skip;
            options.JsonSerializerOptions.AllowTrailingCommas = true;

            options.JsonSerializerOptions.Converters.Add(new AbpStringToEnumFactory());
            options.JsonSerializerOptions.Converters.Add(new AbpStringToBooleanConverter());
            options.JsonSerializerOptions.Converters.Add(new AbpStringToGuidConverter());
            options.JsonSerializerOptions.Converters.Add(new AbpNullableStringToGuidConverter());
            options.JsonSerializerOptions.Converters.Add(new AbpNullableFromEmptyStringConverterFactory());
            options.JsonSerializerOptions.Converters.Add(new ObjectToInferredTypesConverter());

            var aspNetCoreConfiguration = rootServiceProvider.GetRequiredService<IAbpWebCoreConfiguration>();
            options.JsonSerializerOptions.TypeInfoResolver = new AbpDateTimeJsonTypeInfoResolver(aspNetCoreConfiguration.InputDateTimeFormats, aspNetCoreConfiguration.OutputDateTimeFormat);
        });

        //Configure MVC
        services.Configure<MvcOptions>(mvcOptions => { mvcOptions.AddAbp(services); });

        //Configure Razor
        services.Insert(0,
            ServiceDescriptor.Singleton<IConfigureOptions<MvcRazorRuntimeCompilationOptions>>(
                new ConfigureOptions<MvcRazorRuntimeCompilationOptions>(
                    (options) => { options.FileProviders.Add(new EmbeddedResourceViewFileProvider(iocResolver)); }
                )
            )
        );

        services.AddHttpClient(AspNetCoreWebhookSender.WebhookSenderHttpClientName);
    }

    private static AbpBootstrapper AddAbpBootstrapper<TStartupModule>(IServiceCollection services,
        Action<AbpBootstrapperOptions> optionsAction)
        where TStartupModule : AbpModule
    {
        var abpBootstrapper = AbpBootstrapper.Create<TStartupModule>(optionsAction);

        services.AddSingleton(abpBootstrapper);

        return abpBootstrapper;
    }
}

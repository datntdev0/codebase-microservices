﻿using datntdev.Abp.Dependency;
using Castle.MicroKernel.Registration;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using Castle.Windsor.MsDependencyInjection;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace datntdev.Abp.Web.Core;

public class AbpWebCoreConventionalRegistrar : IConventionalDependencyRegistrar
{
    public void RegisterAssembly(IConventionalRegistrationContext context)
    {
        //Razor Pages
        context.IocManager.IocContainer.Register(
            Classes.FromAssembly(context.Assembly)
                .BasedOn<PageModel>()
                .If(type => !type.GetTypeInfo().IsGenericTypeDefinition && !type.IsAbstract)
                .LifestyleTransient()
        );

        //ViewComponents
        context.IocManager.IocContainer.Register(
            Classes.FromAssembly(context.Assembly)
                .BasedOn<ViewComponent>()
                .If(type => !type.GetTypeInfo().IsGenericTypeDefinition)
                .LifestyleTransient()
        );

        //PerWebRequest
        context.IocManager.IocContainer.Register(
            Classes.FromAssembly(context.Assembly)
                .IncludeNonPublicTypes()
                .BasedOn<IPerWebRequestDependency>()
                .If(type => !type.GetTypeInfo().IsGenericTypeDefinition)
                .WithService.Self()
                .WithService.DefaultInterfaces()
                .LifestyleCustom<MsScopedLifestyleManager>()
        );
    }
}

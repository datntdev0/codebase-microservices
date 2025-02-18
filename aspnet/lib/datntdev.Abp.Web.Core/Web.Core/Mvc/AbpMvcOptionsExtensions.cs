using datntdev.Abp.Web.Core.Mvc.Auditing;
using datntdev.Abp.Web.Core.Mvc.Authorization;
using datntdev.Abp.Web.Core.Mvc.Conventions;
using datntdev.Abp.Web.Core.Mvc.ExceptionHandling;
using datntdev.Abp.Web.Core.Mvc.ModelBinding;
using datntdev.Abp.Web.Core.Mvc.Results;
using datntdev.Abp.Web.Core.Mvc.Uow;
using datntdev.Abp.Web.Core.Mvc.Validation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace datntdev.Abp.Web.Core.Mvc;

public static class AbpMvcOptionsExtensions
{
    public static void AddAbp(this MvcOptions options, IServiceCollection services)
    {
        AddConventions(options, services);
        AddActionFilters(options);
        AddPageFilters(options);
        AddModelBinders(options);
    }

    private static void AddConventions(MvcOptions options, IServiceCollection services)
    {
        options.Conventions.Add(new AbpAppServiceConvention(services));
    }

    private static void AddActionFilters(MvcOptions options)
    {
        options.Filters.AddService(typeof(AbpAuthorizationFilter));
        options.Filters.AddService(typeof(AbpAuditActionFilter));
        options.Filters.AddService(typeof(AbpValidationActionFilter));
        options.Filters.AddService(typeof(AbpUowActionFilter));
        options.Filters.AddService(typeof(AbpExceptionFilter));
        options.Filters.AddService(typeof(AbpResultFilter));
    }

    private static void AddPageFilters(MvcOptions options)
    {
        options.Filters.AddService(typeof(AbpUowPageFilter));
        options.Filters.AddService(typeof(AbpAuditPageFilter));
        options.Filters.AddService(typeof(AbpResultPageFilter));
        options.Filters.AddService(typeof(AbpExceptionPageFilter));
    }

    private static void AddModelBinders(MvcOptions options)
    {
        options.ModelBinderProviders.Insert(0, new AbpDateTimeModelBinderProvider());
    }
}

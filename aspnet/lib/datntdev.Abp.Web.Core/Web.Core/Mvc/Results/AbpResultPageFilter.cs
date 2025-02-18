using System;
using System.Threading.Tasks;
using datntdev.Abp.Web.Core.Configuration;
using datntdev.Abp.Web.Core.Mvc.Results.Wrapping;
using datntdev.Abp.Dependency;
using datntdev.Abp.Reflection;
using datntdev.Abp.Web.Configuration;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc.Filters;

namespace datntdev.Abp.Web.Core.Mvc.Results;

public class AbpResultPageFilter : IAsyncPageFilter, ITransientDependency
{
    private readonly IAbpWebCoreConfiguration _configuration;
    private readonly IAbpActionResultWrapperFactory _actionResultWrapperFactory;
    private readonly IAbpWebCommonModuleConfiguration _abpWebCommonModuleConfiguration;

    public AbpResultPageFilter(IAbpWebCoreConfiguration configuration,
        IAbpActionResultWrapperFactory actionResultWrapperFactory,
        IAbpWebCommonModuleConfiguration abpWebCommonModuleConfiguration)
    {
        _configuration = configuration;
        _actionResultWrapperFactory = actionResultWrapperFactory;
        _abpWebCommonModuleConfiguration = abpWebCommonModuleConfiguration;
    }

    public Task OnPageHandlerSelectionAsync(PageHandlerSelectedContext context)
    {
        return Task.CompletedTask;
    }

    public async Task OnPageHandlerExecutionAsync(PageHandlerExecutingContext context, PageHandlerExecutionDelegate next)
    {
        if (context.HandlerMethod == null)
        {
            await next();
            return;
        }

        var pageHandlerExecutedContext = await next();

        var methodInfo = context.HandlerMethod.MethodInfo;

        /*
        * Here is the check order,
        * 1) Configuration
        * 2) Attribute
        */

        var displayUrl = context.HttpContext.Request.GetDisplayUrl();

        if (_abpWebCommonModuleConfiguration.WrapResultFilters.HasFilterForWrapOnSuccess(displayUrl, out var wrapOnSuccess))
        {
            //there is a configuration for that method use configuration
            if (!wrapOnSuccess)
            {
                return;
            }

            _actionResultWrapperFactory.CreateFor(pageHandlerExecutedContext).Wrap(pageHandlerExecutedContext);
            return;
        }

        var wrapResultAttribute =
            ReflectionHelper.GetSingleAttributeOfMemberOrDeclaringTypeOrDefault(
                methodInfo,
                _configuration.DefaultWrapResultAttribute
            );

        if (!wrapResultAttribute.WrapOnSuccess)
        {
            return;
        }

        _actionResultWrapperFactory.CreateFor(pageHandlerExecutedContext).Wrap(pageHandlerExecutedContext);
    }

}

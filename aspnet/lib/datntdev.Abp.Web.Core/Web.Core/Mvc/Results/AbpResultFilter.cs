using System;
using datntdev.Abp.Web.Core.Configuration;
using datntdev.Abp.Web.Core.Mvc.Extensions;
using datntdev.Abp.Web.Core.Mvc.Results.Wrapping;
using datntdev.Abp.Dependency;
using datntdev.Abp.Reflection;
using datntdev.Abp.Web.Configuration;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc.Filters;

namespace datntdev.Abp.Web.Core.Mvc.Results;

public class AbpResultFilter : IResultFilter, ITransientDependency
{
    private readonly IAbpWebCoreConfiguration _configuration;
    private readonly IAbpActionResultWrapperFactory _actionResultWrapperFactory;
    private readonly IAbpWebCommonModuleConfiguration _abpWebCommonModuleConfiguration;

    public AbpResultFilter(IAbpWebCoreConfiguration configuration,
        IAbpActionResultWrapperFactory actionResultWrapper,
        IAbpWebCommonModuleConfiguration abpWebCommonModuleConfiguration)
    {
        _configuration = configuration;
        _actionResultWrapperFactory = actionResultWrapper;
        _abpWebCommonModuleConfiguration = abpWebCommonModuleConfiguration;
    }

    public virtual void OnResultExecuting(ResultExecutingContext context)
    {
        if (!context.ActionDescriptor.IsControllerAction())
        {
            return;
        }

        var methodInfo = context.ActionDescriptor.GetMethodInfo();

        var displayUrl = context.HttpContext.Request.GetDisplayUrl();
        if (_abpWebCommonModuleConfiguration.WrapResultFilters.HasFilterForWrapOnSuccess(displayUrl, out var wrapOnSuccess))
        {
            if (!wrapOnSuccess)
            {
                return;
            }

            _actionResultWrapperFactory.CreateFor(context).Wrap(context);
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

        _actionResultWrapperFactory.CreateFor(context).Wrap(context);
    }

    public virtual void OnResultExecuted(ResultExecutedContext context)
    {
        //no action
    }
}

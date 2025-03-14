﻿using System.Net;
using datntdev.Abp.Web.Core.Configuration;
using datntdev.Abp.Web.Core.Mvc.Extensions;
using datntdev.Abp.Web.Core.Mvc.Results;
using datntdev.Abp.Authorization;
using datntdev.Abp.Dependency;
using datntdev.Abp.Domain.Entities;
using datntdev.Abp.Events.Bus;
using datntdev.Abp.Events.Bus.Exceptions;
using datntdev.Abp.Logging;
using datntdev.Abp.Reflection;
using datntdev.Abp.Runtime.Validation;
using datntdev.Abp.Web.Configuration;
using datntdev.Abp.Web.Models;
using Castle.Core.Logging;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace datntdev.Abp.Web.Core.Mvc.ExceptionHandling;

public class AbpExceptionFilter : IExceptionFilter, ITransientDependency
{
    public ILogger Logger { get; set; }

    public IEventBus EventBus { get; set; }

    private readonly IErrorInfoBuilder _errorInfoBuilder;
    private readonly IAbpWebCoreConfiguration _configuration;
    private readonly IAbpWebCommonModuleConfiguration _abpWebCommonModuleConfiguration;

    public AbpExceptionFilter(
        IErrorInfoBuilder errorInfoBuilder,
        IAbpWebCoreConfiguration configuration,
        IAbpWebCommonModuleConfiguration abpWebCommonModuleConfiguration)
    {
        _errorInfoBuilder = errorInfoBuilder;
        _configuration = configuration;
        _abpWebCommonModuleConfiguration = abpWebCommonModuleConfiguration;

        Logger = NullLogger.Instance;
        EventBus = NullEventBus.Instance;
    }

    public void OnException(ExceptionContext context)
    {
        if (!context.ActionDescriptor.IsControllerAction())
        {
            return;
        }

        var wrapResultAttribute = ReflectionHelper.GetSingleAttributeOfMemberOrDeclaringTypeOrDefault(
            context.ActionDescriptor.GetMethodInfo(),
            _configuration.DefaultWrapResultAttribute
        );

        if (wrapResultAttribute.LogError)
        {
            LogHelper.LogException(Logger, context.Exception);
        }

        HandleAndWrapException(context, wrapResultAttribute);
    }

    protected virtual void HandleAndWrapException(ExceptionContext context, WrapResultAttribute wrapResultAttribute)
    {
        if (!ActionResultHelper.IsObjectResult(context.ActionDescriptor.GetMethodInfo().ReturnType))
        {
            return;
        }

        var displayUrl = context.HttpContext.Request.GetDisplayUrl();
        if (_abpWebCommonModuleConfiguration.WrapResultFilters.HasFilterForWrapOnError(displayUrl,
            out var wrapOnError))
        {
            context.HttpContext.Response.StatusCode = GetStatusCode(context, wrapOnError);

            if (!wrapOnError)
            {
                return;
            }

            HandleError(context);
            return;
        }

        context.HttpContext.Response.StatusCode = GetStatusCode(context, wrapResultAttribute.WrapOnError);

        if (!wrapResultAttribute.WrapOnError)
        {
            return;
        }

        HandleError(context);
    }

    private void HandleError(ExceptionContext context)
    {
        context.Result = new ObjectResult(
            new AjaxResponse(
                _errorInfoBuilder.BuildForException(context.Exception),
                context.Exception is AbpAuthorizationException
            )
        );

        EventBus.Trigger(this, new AbpHandledExceptionData(context.Exception));

        context.Exception = null; // Handled!
    }

    protected virtual int GetStatusCode(ExceptionContext context, bool wrapOnError)
    {
        if (context.Exception is AbpAuthorizationException)
        {
            return context.HttpContext.User.Identity.IsAuthenticated
                ? (int)HttpStatusCode.Forbidden
                : (int)HttpStatusCode.Unauthorized;
        }

        if (context.Exception is AbpValidationException)
        {
            return (int)HttpStatusCode.BadRequest;
        }

        if (context.Exception is EntityNotFoundException)
        {
            return (int)HttpStatusCode.NotFound;
        }

        if (wrapOnError)
        {
            return (int)HttpStatusCode.InternalServerError;
        }

        return context.HttpContext.Response.StatusCode;
    }
}

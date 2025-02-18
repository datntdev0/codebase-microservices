using System.Net;
using System.Threading.Tasks;
using datntdev.Abp.Authorization;
using datntdev.Abp.Dependency;
using datntdev.Abp.Events.Bus;
using datntdev.Abp.Events.Bus.Exceptions;
using datntdev.Abp.Json;
using datntdev.Abp.Localization;
using datntdev.Abp.Web;
using datntdev.Abp.Web.Models;
using Castle.Core.Logging;
using Microsoft.AspNetCore.Http;

namespace datntdev.Abp.Web.Core.ExceptionHandling;

public class AbpAuthorizationExceptionHandlingMiddleware : IMiddleware, ITransientDependency
{
    private readonly IErrorInfoBuilder _errorInfoBuilder;
    private readonly ILocalizationManager _localizationManager;

    public ILogger Logger { get; set; }

    public IEventBus EventBus { get; set; }

    public AbpAuthorizationExceptionHandlingMiddleware(
        IErrorInfoBuilder errorInfoBuilder,
        ILocalizationManager localizationManager)
    {
        _errorInfoBuilder = errorInfoBuilder;
        _localizationManager = localizationManager;

        EventBus = NullEventBus.Instance;
        Logger = NullLogger.Instance;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        await next(context);

        if (IsAuthorizationExceptionStatusCode(context))
        {
            var exception = new AbpAuthorizationException(GetAuthorizationExceptionMessage(context));

            Logger.Error(exception.Message);

            await context.Response.WriteAsync(
                new AjaxResponse(
                    _errorInfoBuilder.BuildForException(exception),
                    true
                ).ToJsonString()
            );

            await EventBus.TriggerAsync(this, new AbpHandledExceptionData(exception));
        }
    }

    protected virtual string GetAuthorizationExceptionMessage(HttpContext context)
    {
        if (context.Response.StatusCode == (int)HttpStatusCode.Forbidden)
        {
            _localizationManager.GetString(AbpWebConsts.LocalizationSourceName, "DefaultError403");
        }

        return _localizationManager.GetString(AbpWebConsts.LocalizationSourceName, "DefaultError401");
    }

    protected virtual bool IsAuthorizationExceptionStatusCode(HttpContext context)
    {
        return context.Response.StatusCode == (int)HttpStatusCode.Forbidden
               || context.Response.StatusCode == (int)HttpStatusCode.Unauthorized;
    }
}

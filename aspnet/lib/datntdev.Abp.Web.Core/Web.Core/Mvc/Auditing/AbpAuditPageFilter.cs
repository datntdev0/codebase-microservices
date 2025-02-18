using System;
using System.Diagnostics;
using System.Threading.Tasks;
using datntdev.Abp.Aspects;
using datntdev.Abp.Web.Core.Configuration;
using datntdev.Abp.Web.Core.Mvc.Extensions;
using datntdev.Abp.Auditing;
using datntdev.Abp.Dependency;
using datntdev.Abp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace datntdev.Abp.Web.Core.Mvc.Auditing;

public class AbpAuditPageFilter : IAsyncPageFilter, ITransientDependency
{
    private readonly IAbpWebCoreConfiguration _configuration;
    private readonly IAuditingHelper _auditingHelper;
    private readonly IAuditingConfiguration _auditingConfiguration;
    private readonly IAuditSerializer _auditSerializer;

    public AbpAuditPageFilter(IAbpWebCoreConfiguration configuration,
        IAuditingHelper auditingHelper,
        IAuditingConfiguration auditingConfiguration,
        IAuditSerializer auditSerializer)
    {
        _configuration = configuration;
        _auditingHelper = auditingHelper;
        _auditingConfiguration = auditingConfiguration;
        _auditSerializer = auditSerializer;
    }

    public Task OnPageHandlerSelectionAsync(PageHandlerSelectedContext context)
    {
        return Task.CompletedTask;
    }

    public async Task OnPageHandlerExecutionAsync(PageHandlerExecutingContext context, PageHandlerExecutionDelegate next)
    {
        if (context.HandlerMethod == null || !ShouldSaveAudit(context))
        {
            await next();
            return;
        }

        using (AbpCrossCuttingConcerns.Applying(context.HandlerInstance, AbpCrossCuttingConcerns.Auditing))
        {
            var auditInfo = _auditingHelper.CreateAuditInfo(
                context.HandlerInstance.GetType(),
                context.HandlerMethod.MethodInfo,
                context.GetBoundPropertiesAsDictionary()
            );

            var stopwatch = Stopwatch.StartNew();

            PageHandlerExecutedContext result = null;
            try
            {
                result = await next();
                if (result.Exception != null && !result.ExceptionHandled)
                {
                    auditInfo.Exception = result.Exception;
                }
            }
            catch (Exception ex)
            {
                auditInfo.Exception = ex;
                throw;
            }
            finally
            {
                stopwatch.Stop();
                auditInfo.ExecutionDuration = Convert.ToInt32(stopwatch.Elapsed.TotalMilliseconds);

                if (_auditingConfiguration.SaveReturnValues && result != null)
                {
                    switch (result.Result)
                    {
                        case ObjectResult objectResult:
                            if (objectResult.Value is AjaxResponse ajaxObjectResponse)
                            {
                                auditInfo.ReturnValue = _auditSerializer.Serialize(ajaxObjectResponse.Result);
                            }
                            else
                            {
                                auditInfo.ReturnValue = _auditSerializer.Serialize(objectResult.Value);
                            }
                            break;

                        case JsonResult jsonResult:
                            if (jsonResult.Value is AjaxResponse ajaxJsonResponse)
                            {
                                auditInfo.ReturnValue = _auditSerializer.Serialize(ajaxJsonResponse.Result);
                            }
                            else
                            {
                                auditInfo.ReturnValue = _auditSerializer.Serialize(jsonResult.Value);
                            }
                            break;

                        case ContentResult contentResult:
                            auditInfo.ReturnValue = contentResult.Content;
                            break;
                    }
                }

                await _auditingHelper.SaveAsync(auditInfo);
            }
        }
    }

    private bool ShouldSaveAudit(PageHandlerExecutingContext actionContext)
    {
        return _configuration.IsAuditingEnabled &&
               _auditingHelper.ShouldSaveAudit(actionContext.HandlerMethod.MethodInfo, true);
    }
}

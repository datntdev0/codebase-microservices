﻿using System.Threading.Tasks;
using datntdev.Abp.Web.Core.Configuration;
using datntdev.Abp.Dependency;
using datntdev.Abp.Domain.Uow;
using Microsoft.AspNetCore.Mvc.Filters;

namespace datntdev.Abp.Web.Core.Mvc.Uow;

public class AbpUowPageFilter : IAsyncPageFilter, ITransientDependency
{
    private readonly IUnitOfWorkManager _unitOfWorkManager;
    private readonly IAbpWebCoreConfiguration _aspnetCoreConfiguration;
    private readonly IUnitOfWorkDefaultOptions _unitOfWorkDefaultOptions;

    public AbpUowPageFilter(
        IUnitOfWorkManager unitOfWorkManager,
        IAbpWebCoreConfiguration aspnetCoreConfiguration,
        IUnitOfWorkDefaultOptions unitOfWorkDefaultOptions)
    {
        _unitOfWorkManager = unitOfWorkManager;
        _aspnetCoreConfiguration = aspnetCoreConfiguration;
        _unitOfWorkDefaultOptions = unitOfWorkDefaultOptions;
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

        var unitOfWorkAttr = _unitOfWorkDefaultOptions
                                 .GetUnitOfWorkAttributeOrNull(context.HandlerMethod.MethodInfo) ??
                             _aspnetCoreConfiguration.DefaultUnitOfWorkAttribute;

        if (unitOfWorkAttr.IsDisabled)
        {
            await next();
            return;
        }

        var uowOpts = new UnitOfWorkOptions
        {
            IsTransactional = unitOfWorkAttr.IsTransactional,
            IsolationLevel = unitOfWorkAttr.IsolationLevel,
            Timeout = unitOfWorkAttr.Timeout,
            Scope = unitOfWorkAttr.Scope
        };

        using (var uow = _unitOfWorkManager.Begin(uowOpts))
        {
            var result = await next();
            if (result.Exception == null || result.ExceptionHandled)
            {
                await uow.CompleteAsync();
            }
        }
    }
}

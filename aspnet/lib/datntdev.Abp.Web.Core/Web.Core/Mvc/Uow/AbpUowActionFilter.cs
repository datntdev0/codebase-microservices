using System.Threading.Tasks;
using datntdev.Abp.Web.Core.Configuration;
using datntdev.Abp.Web.Core.Mvc.Extensions;
using datntdev.Abp.Dependency;
using datntdev.Abp.Domain.Uow;
using Microsoft.AspNetCore.Mvc.Filters;

namespace datntdev.Abp.Web.Core.Mvc.Uow;

public class AbpUowActionFilter : IAsyncActionFilter, ITransientDependency
{
    private readonly IUnitOfWorkManager _unitOfWorkManager;
    private readonly IAbpWebCoreConfiguration _aspnetCoreConfiguration;
    private readonly IUnitOfWorkDefaultOptions _unitOfWorkDefaultOptions;

    public AbpUowActionFilter(
        IUnitOfWorkManager unitOfWorkManager,
        IAbpWebCoreConfiguration aspnetCoreConfiguration,
        IUnitOfWorkDefaultOptions unitOfWorkDefaultOptions)
    {
        _unitOfWorkManager = unitOfWorkManager;
        _aspnetCoreConfiguration = aspnetCoreConfiguration;
        _unitOfWorkDefaultOptions = unitOfWorkDefaultOptions;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (!context.ActionDescriptor.IsControllerAction())
        {
            await next();
            return;
        }

        var unitOfWorkAttr = _unitOfWorkDefaultOptions
            .GetUnitOfWorkAttributeOrNull(context.ActionDescriptor.GetMethodInfo()) ??
            _aspnetCoreConfiguration.DefaultUnitOfWorkAttribute;

        if (unitOfWorkAttr.IsDisabled)
        {
            await next();
            return;
        }

        using (var uow = _unitOfWorkManager.Begin(unitOfWorkAttr.CreateOptions()))
        {
            var result = await next();
            if (result.Exception == null || result.ExceptionHandled)
            {
                await uow.CompleteAsync();
            }
        }
    }
}

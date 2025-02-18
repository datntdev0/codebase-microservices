using System.Threading.Tasks;
using datntdev.Abp.Application.Services;
using datntdev.Abp.Aspects;
using datntdev.Abp.Web.Core.Configuration;
using datntdev.Abp.Web.Core.Mvc.Extensions;
using datntdev.Abp.Dependency;
using Microsoft.AspNetCore.Mvc.Filters;

namespace datntdev.Abp.Web.Core.Mvc.Validation;

public class AbpValidationActionFilter : IAsyncActionFilter, ITransientDependency
{
    private readonly IIocResolver _iocResolver;
    private readonly IAbpWebCoreConfiguration _configuration;

    public AbpValidationActionFilter(IIocResolver iocResolver, IAbpWebCoreConfiguration configuration)
    {
        _iocResolver = iocResolver;
        _configuration = configuration;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (!_configuration.IsValidationEnabledForControllers || !context.ActionDescriptor.IsControllerAction())
        {
            await next();
            return;
        }

        using (AbpCrossCuttingConcerns.Applying(context.Controller, AbpCrossCuttingConcerns.Validation))
        {
            using (var validator = _iocResolver.ResolveAsDisposable<MvcActionInvocationValidator>())
            {
                validator.Object.Initialize(context);
                validator.Object.Validate();
            }

            await next();
        }
    }
}

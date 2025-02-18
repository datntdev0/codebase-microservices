using System.ComponentModel.DataAnnotations;
using datntdev.Abp.Web.Core.Mvc.Extensions;
using datntdev.Abp.Collections.Extensions;
using datntdev.Abp.Configuration.Startup;
using datntdev.Abp.Dependency;
using datntdev.Abp.Web.Validation;
using Microsoft.AspNetCore.Mvc.Filters;

namespace datntdev.Abp.Web.Core.Mvc.Validation;

public class MvcActionInvocationValidator : ActionInvocationValidatorBase
{
    protected ActionExecutingContext ActionContext { get; private set; }

    public MvcActionInvocationValidator(IValidationConfiguration configuration, IIocResolver iocResolver)
        : base(configuration, iocResolver)
    {
    }

    public void Initialize(ActionExecutingContext actionContext)
    {
        ActionContext = actionContext;

        base.Initialize(actionContext.ActionDescriptor.GetMethodInfo());
    }

    protected override object GetParameterValue(string parameterName)
    {
        return ActionContext.ActionArguments.GetOrDefault(parameterName);
    }

    protected override void SetDataAnnotationAttributeErrors()
    {
        foreach (var state in ActionContext.ModelState)
        {
            foreach (var error in state.Value.Errors)
            {
                ValidationErrors.Add(new ValidationResult(error.ErrorMessage, new[] { state.Key }));
            }
        }
    }
}

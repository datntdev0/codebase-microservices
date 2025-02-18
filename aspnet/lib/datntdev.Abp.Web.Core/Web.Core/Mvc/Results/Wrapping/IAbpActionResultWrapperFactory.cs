using datntdev.Abp.Dependency;
using Microsoft.AspNetCore.Mvc.Filters;

namespace datntdev.Abp.Web.Core.Mvc.Results.Wrapping;

public interface IAbpActionResultWrapperFactory : ITransientDependency
{
    IAbpActionResultWrapper CreateFor(FilterContext context);
}

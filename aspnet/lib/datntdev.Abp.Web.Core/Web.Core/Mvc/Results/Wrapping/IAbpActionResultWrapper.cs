using Microsoft.AspNetCore.Mvc.Filters;

namespace datntdev.Abp.Web.Core.Mvc.Results.Wrapping;

public interface IAbpActionResultWrapper
{
    void Wrap(FilterContext context);
}

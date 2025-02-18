using Microsoft.AspNetCore.Mvc.Filters;

namespace datntdev.Abp.Web.Core.Mvc.Results.Wrapping;

public class NullAbpActionResultWrapper : IAbpActionResultWrapper
{
    public void Wrap(FilterContext context)
    {

    }

}

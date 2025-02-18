using System.Security.Claims;
using datntdev.Abp.Runtime.Session;
using Microsoft.AspNetCore.Http;

namespace datntdev.Abp.Web.Core.Runtime.Session;

public class AspNetCorePrincipalAccessor : DefaultPrincipalAccessor
{
    public override ClaimsPrincipal Principal => _httpContextAccessor.HttpContext?.User ?? base.Principal;

    private readonly IHttpContextAccessor _httpContextAccessor;

    public AspNetCorePrincipalAccessor(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
}

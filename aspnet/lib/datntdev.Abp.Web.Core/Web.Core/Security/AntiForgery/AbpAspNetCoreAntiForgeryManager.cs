using datntdev.Abp.Web.Security.AntiForgery;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;

namespace datntdev.Abp.Web.Core.Security.AntiForgery;

public class AbpWebCoreAntiForgeryManager : IAbpAntiForgeryManager
{
    public IAbpAntiForgeryConfiguration Configuration { get; }

    private readonly IAntiforgery _antiforgery;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AbpWebCoreAntiForgeryManager(
        IAntiforgery antiforgery,
        IHttpContextAccessor httpContextAccessor,
        IAbpAntiForgeryConfiguration configuration)
    {
        Configuration = configuration;
        _antiforgery = antiforgery;
        _httpContextAccessor = httpContextAccessor;
    }

    public string GenerateToken()
    {
        return _antiforgery.GetAndStoreTokens(_httpContextAccessor.HttpContext).RequestToken;
    }
}

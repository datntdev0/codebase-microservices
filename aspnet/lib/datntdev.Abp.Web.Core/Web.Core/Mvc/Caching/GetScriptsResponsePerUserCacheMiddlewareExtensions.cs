using Microsoft.AspNetCore.Builder;

namespace datntdev.Abp.Web.Core.Mvc.Caching;

public static class GetScriptsResponsePerUserCacheMiddlewareExtensions
{
    /// <summary>
    /// Implements GetScriptsResponsePerUserCacheMiddleware middleware with given maxAge
    /// </summary>
    /// <param name="builder"></param>
    public static IApplicationBuilder UseGetScriptsResponsePerUserCache(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<GetScriptsResponsePerUserCacheMiddleware>();
    }
}

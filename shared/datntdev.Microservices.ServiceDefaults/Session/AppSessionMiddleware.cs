using datntdev.Microservices.Common.Web.App.Session;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

namespace datntdev.Microservices.ServiceDefaults.Session
{
    public class AppSessionMiddleware : IMiddleware
    {
        public Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            context.RequestServices.GetService<AppSessionContext>()?
                .SetUserInfo(GetAppSessionUserInfo(context));

            return next(context);
        }

        private static AppSessionUserInfo? GetAppSessionUserInfo(HttpContext context)
        {
            var currentUser = context.User;
            return currentUser.Identity?.IsAuthenticated == false ? null :
                new AppSessionUserInfo
                {
                    UserId = long.Parse(currentUser.FindFirstValue(ClaimTypes.NameIdentifier)!),
                    Username = currentUser.FindFirstValue(ClaimTypes.Name)!,
                    EmailAddress = currentUser.FindFirstValue(ClaimTypes.Email)!,
                    FirstName = currentUser.FindFirstValue(ClaimTypes.GivenName)!,
                    LastName = currentUser.FindFirstValue(ClaimTypes.Surname)!,
                };
        }
    }
}

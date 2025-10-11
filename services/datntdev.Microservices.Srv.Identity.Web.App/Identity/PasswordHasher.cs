using datntdev.Microservices.Srv.Identity.Web.App.Authorization.Users.Models;
using Microsoft.AspNetCore.Identity;

namespace datntdev.Microservices.Srv.Identity.Web.App.Identity
{
    public class PasswordHasher : PasswordHasher<AppUserEntity>
    {
    }
}

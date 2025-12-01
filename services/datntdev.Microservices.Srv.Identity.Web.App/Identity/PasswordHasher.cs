using datntdev.Microservices.Srv.Identity.Web.App.Authorization.Users.Models;
using Microsoft.AspNetCore.Identity;

namespace datntdev.Microservices.Srv.Identity.Web.App.Identity
{
    public class PasswordHasher : PasswordHasher<AppUserEntity>
    {
        public AppUserEntity SetPassword(AppUserEntity userEntity, string password)
        {
            if (!string.IsNullOrEmpty(password))
            {
                userEntity.PasswordHash = base.HashPassword(userEntity, password);
            }
            return userEntity;
        }
    }
}

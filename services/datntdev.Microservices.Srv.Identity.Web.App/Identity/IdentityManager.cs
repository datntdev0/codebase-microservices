using datntdev.Microservices.Common;
using datntdev.Microservices.Srv.Identity.Web.App.Authorization.Users;
using datntdev.Microservices.Srv.Identity.Web.App.Authorization.Users.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;
using IdentityResult = datntdev.Microservices.Srv.Identity.Web.App.Identity.Models.IdentityResult;

namespace datntdev.Microservices.Srv.Identity.Web.App.Identity
{
    public class IdentityManager(IServiceProvider services)
    {
        private readonly IHttpContextAccessor _contextAccessor = services.GetRequiredService<IHttpContextAccessor>();
        private readonly UserManager _userManager = services.GetRequiredService<UserManager>();
        private readonly PasswordHasher _passwordHasher = services.GetRequiredService<PasswordHasher>();

        public async Task<IdentityResult> SignInWithPassword(string username, string password)
        {
            var userEntity = await _userManager.FindAsync(username);
            if (userEntity == null) return IdentityResult.Failure;

            var passwordVerification = _passwordHasher.VerifyHashedPassword(
                userEntity, userEntity.PasswordHash, password);

            if (passwordVerification == PasswordVerificationResult.Success)
            {
                var claims = new Claim[]
                {
                    new(ClaimTypes.Name, userEntity.Username),
                    new(ClaimTypes.NameIdentifier, userEntity.Id.ToString()),
                    new(ClaimTypes.Email, userEntity.EmailAddress ?? string.Empty),
                    new(ClaimTypes.GivenName, userEntity.FirstName ?? string.Empty),
                    new(ClaimTypes.Surname, userEntity.LastName ?? string.Empty),
                };

                var claimsIdentity = new ClaimsIdentity(claims, Constants.Application.AuthenticationScheme);
                await _contextAccessor.HttpContext!.SignInAsync(new ClaimsPrincipal(claimsIdentity));
            }

            return passwordVerification switch
            {
                PasswordVerificationResult.Success => IdentityResult.Success,
                PasswordVerificationResult.Failed => IdentityResult.Failure,
                _ => throw new NotImplementedException(),
            };
        }

        public async Task<IdentityResult> SignUpWithPassword(AppUserEntity user, string password)
        {
            // Ignore query filters to allow querying all users cross tenants
            var existingUser = await _userManager.FindAsync(user.Username);
            if (existingUser != null) return IdentityResult.Duplicated;

            await _userManager.CreateAsync(user, password);
            return IdentityResult.Success;
        }
    }
}

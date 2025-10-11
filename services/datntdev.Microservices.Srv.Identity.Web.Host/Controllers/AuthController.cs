using datntdev.Microservices.Common;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using System.Security.Claims;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace datntdev.Microservices.Srv.Identity.Web.Host.Controllers
{
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpGet(Constants.Endpoints.OAuth2Auth)]
        [HttpPost(Constants.Endpoints.OAuth2Auth)]
        public async Task<IActionResult> AuthorizeAsync()
        {
            var request = HttpContext.GetOpenIddictServerRequest() ??
                throw new InvalidOperationException("The OpenID Connect request cannot be retrieved.");

            // Retrieve the user principal stored in the authentication cookie.
            var result = await HttpContext.AuthenticateAsync();

            // If the user principal can't be extracted, redirect the user to the login page.
            if (!result.Succeeded) return Challenge();

            var authenticationScheme = OpenIddictServerAspNetCoreDefaults.AuthenticationScheme;
            var subjectClaim = new Claim(Claims.Subject, result.Principal.Identity!.Name!);
            var claims = result.Principal.Claims.Append(subjectClaim);
            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claims, authenticationScheme));

            // Set requested scopes (this is not done automatically)
            claimsPrincipal.SetScopes(request.GetScopes());

            // Signing in with the OpenIddict authentiction scheme trigger
            // OpenIddict to issue a code (which can be exchanged for an access token)
            return SignIn(claimsPrincipal, authenticationScheme);
        }

        [HttpPost(Constants.Endpoints.OAuth2Token)]
        public async Task<IActionResult> TokenAsync()
        {
            var request = HttpContext.GetOpenIddictServerRequest() ??
                throw new InvalidOperationException("The OpenID Connect request cannot be retrieved.");

            var authenticationScheme = OpenIddictServerAspNetCoreDefaults.AuthenticationScheme;
            ClaimsPrincipal claimsPrincipal;

            if (request.IsAuthorizationCodeGrantType())
            {
                // Retrieve the claims principal stored in the authorization code
                claimsPrincipal = (await HttpContext.AuthenticateAsync(authenticationScheme)).Principal ??
                    throw new InvalidOperationException("Can't retrieve the claims principal stored in the authorization code");
            }
            else if (request.IsClientCredentialsGrantType())
            {
                var claims = new Claim[] { new(Claims.Subject, "Service Principal") };
                claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claims, authenticationScheme));
            }
            else
            {
                throw new InvalidOperationException("The specified grant type is not supported.");
            }

            // Returning a SignInResult will ask OpenIddict to issue the appropriate access/identity tokens.
            return SignIn(claimsPrincipal, authenticationScheme);
        }

        [HttpPost("/me/signout")]
        public async Task<IActionResult> SignOutAsync()
        {
            await HttpContext.SignOutAsync();
            return Redirect(Constants.Endpoints.AuthSignIn);
        }
    }
}

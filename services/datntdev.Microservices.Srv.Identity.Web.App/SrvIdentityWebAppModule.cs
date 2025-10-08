using datntdev.Microservices.Common;
using datntdev.Microservices.Common.Modular;
using datntdev.Microservices.Srv.Identity.Contract;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace datntdev.Microservices.Srv.Identity.Web.App
{
    [DependOn(typeof(SrvIdentityContractModule))]
    public class SrvIdentityWebAppModule : BaseModule
    {
        public override void ConfigureServices(IServiceCollection services, IConfigurationRoot configs)
        {
            services.AddDbContext<SrvIdentityDbContext>(opt => opt.UseOpenIddict());
        }
    }

    internal static class SrvIdentityWebAppModuleExtensions
    {
        public static IServiceCollection AddOpenIddictServices(
            this IServiceCollection services, IConfigurationRoot configs)
        {
            var encryptionKey = Convert.FromBase64String(configs["OpenIddict:EncryptionKey"]!);

            services.AddOpenIddict()
                .AddCore(opt => opt.UseEntityFrameworkCore().UseDbContext<SrvIdentityDbContext>())
                .AddServer(options =>
                {
                    // TODO: Disable access token encryption for debug
                    // edit this code for applying higher environments
                    options.DisableAccessTokenEncryption()
                        .AddEphemeralSigningKey()
                        .AddEncryptionKey(new SymmetricSecurityKey(encryptionKey));

                    options
                        .RequireProofKeyForCodeExchange()
                        .AllowAuthorizationCodeFlow()
                        .AllowClientCredentialsFlow();

                    options
                        .SetTokenEndpointUris(Constants.Endpoints.OAuth2Token)
                        .SetAuthorizationEndpointUris(Constants.Endpoints.OAuth2Auth);

                    options.UseAspNetCore()
                        .EnableTokenEndpointPassthrough()
                        .EnableAuthorizationEndpointPassthrough();
                });
            return services;
        }
    }
}

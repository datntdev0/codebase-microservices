using datntdev.Microservices.Common;
using datntdev.Microservices.Common.Configuration;
using datntdev.Microservices.Common.Modular;
using datntdev.Microservices.Common.Web.App;
using datntdev.Microservices.Srv.Identity.Contract;
using datntdev.Microservices.Srv.Identity.Web.App.Authorization.Users;
using datntdev.Microservices.Srv.Identity.Web.App.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace datntdev.Microservices.Srv.Identity.Web.App
{
    [DependOn(
        typeof(CommonWebAppModule),
        typeof(SrvIdentityContractModule)
    )]
    public class SrvIdentityWebAppModule : BaseModule
    {
        public override void ConfigureServices(IServiceCollection services, IConfigurationRoot configs)
        {
            services.AddDbContext<SrvIdentityDbContext>(opt => opt.UseOpenIddict());
            services.AddOpenIddictServices(configs);
            services.AddIdentityServices();
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
                    if (AppConfiguration.IsDevelopment()) options.DisableAccessTokenEncryption();

                    options
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

        public static IServiceCollection AddIdentityServices(this IServiceCollection services)
        {
            services.AddScoped<IdentityManager>()
                .AddSingleton<PasswordHasher>()
                .AddScoped<UserManager>()
                .AddHttpContextAccessor();
            return services;
        }
    }
}

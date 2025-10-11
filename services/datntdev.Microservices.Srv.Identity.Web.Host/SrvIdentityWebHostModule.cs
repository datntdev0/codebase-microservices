using datntdev.Microservices.Common;
using datntdev.Microservices.Common.Modular;
using datntdev.Microservices.Srv.Identity.Web.App;
using Microsoft.EntityFrameworkCore;

namespace datntdev.Microservices.Srv.Identity.Web.Host
{
    [DependOn(typeof(SrvIdentityWebAppModule))]
    public class SrvIdentityWebHostModule : BaseModule
    {
        public override void ConfigureServices(IServiceCollection services, IConfigurationRoot configs)
        {
            // Configure the DbContext to use SQL Server with the connection string from configuration
            services.ConfigureDbContext<SrvIdentityDbContext>(
                opt => opt.UseSqlServer(configs.GetConnectionString("Default")));

            // Configure authentication with cookie scheme
            services.AddAuthentication(Constants.Application.AuthenticationScheme)
                .AddCookie(Constants.Application.AuthenticationScheme, options =>
                {
                    options.LoginPath = Constants.Endpoints.AuthSignIn;
                });
            services.AddAuthorization().AddAuthenticationCore();
            services.AddCascadingAuthenticationState();
        }
    }
}

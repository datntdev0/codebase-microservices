using datntdev.Microservices.Srv.Identity.Web.App;
using datntdev.Microservices.Srv.Identity.Web.App.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenIddict.Abstractions;

namespace datntdev.Microservices.Migrator.Seeders
{
    internal class IdentityDataSeeder(IServiceProvider services)
    {
        private readonly IConfigurationRoot _configuration = services.GetRequiredService<IConfigurationRoot>();
        private readonly SrvIdentityDbContext _dbContext = services.GetRequiredService<SrvIdentityDbContext>();

        public async Task SeedAsync()
        {
            await EnsureOpenIddictApplicationExistsAsync();
            await EnsureDefaultAdminUserExistsAsync();
        }

        private async Task EnsureDefaultAdminUserExistsAsync()
        {
            var defaultAdminUsername = _configuration.GetValue<string>("DefaultAdmin:Username");
            var defaultAdminEmail = _configuration.GetValue<string>("DefaultAdmin:Username");
            var defaultAdminPassword = _configuration.GetValue<string>("DefaultAdmin:Password");
            var defaultAdminFirstName = _configuration.GetValue<string>("DefaultAdmin:FirstName");
            var defaultAdminLastName = _configuration.GetValue<string>("DefaultAdmin:LastName");

            ArgumentNullException.ThrowIfNull(defaultAdminUsername, nameof(defaultAdminUsername));
            ArgumentNullException.ThrowIfNull(defaultAdminPassword, nameof(defaultAdminPassword));

            var passwordHasher = services.GetRequiredService<PasswordHasher>();

            // Recreate the default admin user althgough it exists.
            var existingUser = await _dbContext.AppUsers.FirstOrDefaultAsync(x => x.Username == defaultAdminUsername);
            if (existingUser != null) _dbContext.AppUsers.Remove(existingUser);
            var newUser = passwordHasher.SetPassword(new Srv.Identity.Web.App.Authorization.Users.Models.AppUserEntity
                {
                    Username = defaultAdminUsername,
                    EmailAddress = defaultAdminEmail ?? string.Empty,
                    FirstName = defaultAdminFirstName ?? string.Empty,
                    LastName = defaultAdminLastName ?? string.Empty,
                }, defaultAdminPassword);

            await _dbContext.AppUsers.AddAsync(newUser);
            await _dbContext.SaveChangesAsync();
        }

        private async Task EnsureOpenIddictApplicationExistsAsync()
        {
            var openIddictConfiguration = _configuration.GetSection("OpenIddict");
            var openIddictClientId = openIddictConfiguration.GetValue<string>("ClientId");
            var openIddictClientSecret = openIddictConfiguration.GetValue<string>("ClientSecret");
            var openIddictRedirectUris = openIddictConfiguration.GetValue<string>("RedirectUris");
            var openIddictLogoutRedirectUris = openIddictConfiguration.GetValue<string>("LogoutRedirectUris");

            ArgumentNullException.ThrowIfNull(openIddictClientId, nameof(openIddictClientId));
            ArgumentNullException.ThrowIfNull(openIddictClientSecret, nameof(openIddictClientSecret));

            var manager = services.GetRequiredService<IOpenIddictApplicationManager>();

            // Create a new OpenIddict application with the specified client ID.
            // The application type is set to Web, and the client type is set to Public.
            var newApplication = CreatePublicApplication(
                openIddictClientId, openIddictRedirectUris, openIddictLogoutRedirectUris);
            var existingApplication = await manager.FindByClientIdAsync(newApplication.ClientId!);
            if (existingApplication != null) await manager.DeleteAsync(existingApplication);
            await manager.CreateAsync(newApplication);

            // Create a new OpenIddict application for the confidential client.
            // The application type is set to Web, and the client type is set to Confidential.
            newApplication = CreateConfidentialApplication(
                openIddictClientId, openIddictClientSecret, openIddictRedirectUris, openIddictLogoutRedirectUris);
            existingApplication = await manager.FindByClientIdAsync(newApplication.ClientId!);
            if (existingApplication != null) await manager.DeleteAsync(existingApplication);
            await manager.CreateAsync(newApplication);
        }

        private static OpenIddictApplicationDescriptor CreateConfidentialApplication(
            string clientId, string clientSecret, string? redirectUris, string? logoutRedirectUris)
        {
            var application = new OpenIddictApplicationDescriptor
            {
                DisplayName = $"{clientId}.Confidential",
                ClientId = $"{clientId}.Confidential",
                ClientSecret = clientSecret,
                ClientType = OpenIddictConstants.ClientTypes.Confidential,
                ApplicationType = OpenIddictConstants.ApplicationTypes.Web,
                Permissions =
                {
                    OpenIddictConstants.Permissions.Endpoints.Token,
                    OpenIddictConstants.Permissions.Endpoints.EndSession,
                    OpenIddictConstants.Permissions.GrantTypes.ClientCredentials,
                }
            };
            redirectUris?.Split(';').Select(x => new Uri(x))
                .ToList().ForEach(x => application.RedirectUris.Add(x));
            logoutRedirectUris?.Split(";").Select(x => new Uri(x))
                .ToList().ForEach(x => application.PostLogoutRedirectUris.Add(x));
            return application;
        }

        private static OpenIddictApplicationDescriptor CreatePublicApplication(
            string clientId, string? redirectUris, string? logoutRedirectUris)
        {
            var application = new OpenIddictApplicationDescriptor
            {
                DisplayName = $"{clientId}.Public",
                ClientId = $"{clientId}.Public",
                ClientType = OpenIddictConstants.ClientTypes.Public,
                ApplicationType = OpenIddictConstants.ApplicationTypes.Web,
                Permissions =
                {
                    OpenIddictConstants.Permissions.Endpoints.Token,
                    OpenIddictConstants.Permissions.Endpoints.EndSession,
                    OpenIddictConstants.Permissions.Endpoints.Authorization,
                    OpenIddictConstants.Permissions.GrantTypes.AuthorizationCode,
                    OpenIddictConstants.Permissions.ResponseTypes.Code,
                }
            };
            redirectUris?.Split(";").Select(x => new Uri(x))
                .ToList().ForEach(x => application.RedirectUris.Add(x));
            logoutRedirectUris?.Split(";").Select(x => new Uri(x))
                .ToList().ForEach(x => application.PostLogoutRedirectUris.Add(x));
            return application;
        }
    }
}

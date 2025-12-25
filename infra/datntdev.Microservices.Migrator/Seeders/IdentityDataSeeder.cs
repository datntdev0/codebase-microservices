using datntdev.Microservices.Common;
using datntdev.Microservices.Srv.Identity.Web.App;
using datntdev.Microservices.Srv.Identity.Web.App.Authorization.Permissions;
using datntdev.Microservices.Srv.Identity.Web.App.Authorization.Roles.Models;
using datntdev.Microservices.Srv.Identity.Web.App.Authorization.Users.Models;
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
        private readonly PermissionProvider _permissionProvider = services.GetRequiredService<PermissionProvider>();

        public async Task SeedAsync()
        {
            await EnsureOpenIddictApplicationExistsAsync();
            await EnsureDefaultAdminRoleExistsAsync();
            await EnsureDefaultAdminUserExistsAsync();
        }

        private async Task EnsureDefaultAdminRoleExistsAsync()
        {
            var existingRole = await _dbContext.AppRoles
                .Where(x => x.Name == Constants.Authorization.DefaultAdminRole).ToListAsync();
            if (existingRole.Count != 0) _dbContext.AppRoles.RemoveRange(existingRole);
            await _dbContext.AppRoles.AddRangeAsync(
                CreateDefaultHostAdminRole(),
                CreateDefaultTenantAdminRole());
            await _dbContext.SaveChangesAsync();
        }

        private AppRoleEntity CreateDefaultHostAdminRole()
        {
            return new AppRoleEntity
            {
                TenantId = null,
                Name = Constants.Authorization.DefaultAdminRole,
                Description = "Default administrator role with full permissions.",
                Permissions = _permissionProvider.GetAllHostAppPermissions(),
            };
        }

        private AppRoleEntity CreateDefaultTenantAdminRole()
        {
            return new AppRoleEntity
            {
                TenantId = Constants.MultiTenancy.DefaultTenantId,
                Name = Constants.Authorization.DefaultAdminRole,
                Description = "Default tenant administrator role with full permissions.",
                Permissions = _permissionProvider.GetAllTenantAppPermissions(),
            };
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

            var newUser = GetDefaultAdminUser(
                defaultAdminUsername, 
                defaultAdminPassword,
                defaultAdminFirstName ?? string.Empty, 
                defaultAdminLastName ?? string.Empty);

            // Lookup the default administrator roles
            newUser.Roles = await _dbContext.AppRoles.Where(x => x.Name == Constants.Authorization.DefaultAdminRole)
                .ToListAsync();

            newUser = passwordHasher.SetPassword(newUser, defaultAdminPassword);

            await _dbContext.AppUsers.AddAsync(newUser);
            await _dbContext.SaveChangesAsync();
        }

        private AppUserEntity GetDefaultAdminUser(
            string username, string password, string firstName, string lastName)
        {
            return new AppUserEntity()
            {
                Username = username,
                EmailAddress = username,
                FirstName = firstName,
                LastName = lastName,
                Permissions = _permissionProvider.GetAllHostAppPermissions(),
            };
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

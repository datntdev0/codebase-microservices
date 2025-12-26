using datntdev.Microservices.Srv.Identity.Authorization;
using datntdev.Microservices.Srv.Identity.Web.App;
using datntdev.Microservices.Srv.Identity.Web.App.Authorization.Permissions;
using datntdev.Microservices.Srv.Identity.Web.App.Authorization.Permissions.Models;
using datntdev.Microservices.Srv.Identity.Web.App.Authorization.Roles.Models;
using datntdev.Microservices.Srv.Identity.Web.App.Authorization.Users.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace datntdev.Microservices.Srv.Identity
{
    [TestClass]
    public abstract class SrvIdentityTestBase
    {
        protected readonly static HttpClient _client = GetHttpClient();

        public static HttpClient GetHttpClient()
        {
            return SrvIdentityStartupTests.Factory.CreateClient(new()
            {
                AllowAutoRedirect = false,
                BaseAddress = new Uri("https://localhost"),
            });
        }

        public static SrvIdentityDbContext GetDbContext()
        {
            return SrvIdentityStartupTests.Factory.Services.CreateScope()
                .ServiceProvider.GetRequiredService<SrvIdentityDbContext>();
        }

        protected static AppRoleEntity GetRoleByName(string roleName, int? tenantId = null)
        {
            using var dbContext = GetDbContext();
            return dbContext.AppRoles.Include(r => r.Users)
                .First(r => r.Name == roleName && r.TenantId == tenantId);
        }

        protected static AppUserEntity GetUserByName(string username)
        {
            using var dbContext = GetDbContext();
            return dbContext.AppUsers.Include(u => u.Roles)
                .First(u => u.Username == username);
        }

        protected static AppUserEntity GetAdminUser()
        {
            var adminUsername = SrvIdentityStartupTests.Factory.Services
                .GetRequiredService<IConfiguration>()
                .GetValue<string>("DefaultAdmin:Username")!;
            return GetUserByName(adminUsername);
        }

        protected static PermissionModel[] GetPermissions()
        {
            return SrvIdentityStartupTests.Factory.Services
                .GetRequiredService<PermissionProvider>()
                .GetAllPermissions();
        }

        protected static void CleanupUserAppServiceTests()
        {
            using var dbContext = GetDbContext();
            var users = dbContext.AppUsers
                .Where(u => u.Username.StartsWith(UserAppServiceTests.UsernamePrefix))
                .ToList();
            if (users.Count > 0)
            {
                dbContext.AppUsers.RemoveRange(users);
                dbContext.SaveChanges();
            }
        }
    }
}

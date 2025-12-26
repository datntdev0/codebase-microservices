using datntdev.Microservices.Srv.Identity.Authorization;
using datntdev.Microservices.Srv.Identity.Web.Host;
using Microsoft.AspNetCore.Mvc.Testing;

namespace datntdev.Microservices.Srv.Identity
{
    [TestClass]
    public class SrvIdentityStartupTests
    {
        public static readonly WebApplicationFactory<Program> Factory = new();

        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {
            CleanupUserAppServiceTests();
            CleanupRoleAppServiceTests();
            Factory.Dispose();
        }

        [TestMethod]
        public async Task GetHealthChecks_ReturnOkStatusCode()
        {
            // Arrange 
            using var httpClient = SrvIdentityTestBase.GetHttpClient();

            // Act
            var response = await httpClient.GetAsync("/health");

            // Assert
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            Assert.AreEqual("Healthy", content);
        }

        protected static void CleanupUserAppServiceTests()
        {
            using var dbContext = SrvIdentityTestBase.GetDbContext();
            var users = dbContext.AppUsers
                .Where(u => u.Username.StartsWith(UserAppServiceTests.UsernamePrefix))
                .ToList();
            if (users.Count > 0)
            {
                dbContext.AppUsers.RemoveRange(users);
                dbContext.SaveChanges();
            }
        }

        protected static void CleanupRoleAppServiceTests()
        {
            using var dbContext = SrvIdentityTestBase.GetDbContext();
            var roles = dbContext.AppRoles
                .Where(r => r.Name.StartsWith(RoleAppServiceTests.RoleNamePrefix))
                .ToList();
            if (roles.Count > 0)
            {
                dbContext.AppRoles.RemoveRange(roles);
                dbContext.SaveChanges();
            }
        }
    }
}

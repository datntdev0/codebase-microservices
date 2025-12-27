using datntdev.Microservices.Srv.Admin.MultiTenancy;
using datntdev.Microservices.Srv.Admin.Web.Host;
using Microsoft.AspNetCore.Mvc.Testing;


namespace datntdev.Microservices.Srv.Admin
{
    [TestClass]
    public class SrvAdminStartupTests
    {
        public static readonly WebApplicationFactory<Program> Factory = new();

        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {
            CleanupTenantAppServiceTests();
            Factory.Dispose();
        }

        [TestMethod]
        public async Task GetHealthChecks_ReturnOkStatusCode()
        {
            // Arrange 
            using var httpClient = SrvAdminTestBase.GetHttpClient();

            // Act
            var response = await httpClient.GetAsync("/health");

            // Assert
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            Assert.AreEqual("Healthy", content);
        }

        protected static void CleanupTenantAppServiceTests()
        {
            using var dbContext = SrvAdminTestBase.GetDbContext();
            var tenants = dbContext.AppTenants
                .Where(t => t.TenantName.StartsWith(TenantAppServiceTests.TenantNamePrefix))
                .ToList();
            if (tenants.Count > 0)
            {
                dbContext.AppTenants.RemoveRange(tenants);
                dbContext.SaveChanges();
            }
        }
    }
}

using datntdev.Microservices.Srv.Admin.Web.App;
using datntdev.Microservices.Srv.Admin.Web.App.MultiTenancy.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace datntdev.Microservices.Srv.Admin
{
    [TestClass]
    public abstract class SrvAdminTestBase
    {
        protected readonly static HttpClient _client = GetHttpClient();

        public static HttpClient GetHttpClient()
        {
            return SrvAdminStartupTests.Factory.CreateClient(new()
            {
                AllowAutoRedirect = false,
                BaseAddress = new Uri("https://localhost"),
            });
        }

        public static SrvAdminDbContext GetDbContext()
        {
            return SrvAdminStartupTests.Factory.Services.CreateScope()
                .ServiceProvider.GetRequiredService<SrvAdminDbContext>();
        }

        protected static AppTenantEntity GetTenantByName(string tenantName)
        {
            using var dbContext = GetDbContext();
            return dbContext.AppTenants.First(t => t.TenantName == tenantName);
        }

        protected static AppTenantEntity GetTenantById(int id)
        {
            using var dbContext = GetDbContext();
            return dbContext.AppTenants.First(t => t.Id == id);
        }
    }
}

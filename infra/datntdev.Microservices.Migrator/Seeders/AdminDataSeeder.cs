using datntdev.Microservices.Common;
using datntdev.Microservices.Srv.Admin.Web.App;
using datntdev.Microservices.Srv.Admin.Web.App.MultiTenancy.Models;
using Microsoft.Extensions.DependencyInjection;

namespace datntdev.Microservices.Migrator.Seeders
{
    internal class AdminDataSeeder(IServiceProvider services)
    {
        private readonly SrvAdminDbContext _dbContext = services.GetRequiredService<SrvAdminDbContext>();

        public async Task SeedAsync()
        {
            await EnsureDefaultTenantExistsAsync();
        }

        private async Task EnsureDefaultTenantExistsAsync()
        {
            var defaultTenant = new AppTenantEntity() 
            {
                Id = Constants.MultiTenancy.DefaultTenantId,
                TenantName = Constants.MultiTenancy.DefaultTenantName,
            };

            var existingTenant = await _dbContext.AppTenants.FindAsync(defaultTenant.Id);
            if (existingTenant != null)
            {
                _dbContext.AppTenants.Remove(existingTenant);
                await _dbContext.SaveChangesAsync();
            }
            await _dbContext.AppTenants.AddAsync(defaultTenant);
            await _dbContext.SaveChangesAsync();
        }
    }
}

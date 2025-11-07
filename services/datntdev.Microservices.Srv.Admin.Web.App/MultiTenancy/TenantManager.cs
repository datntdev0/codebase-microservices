using datntdev.Microservices.Common.Web.App.Application;
using datntdev.Microservices.Common.Web.App.Exceptions;
using datntdev.Microservices.Srv.Admin.Web.App.MultiTenancy.Models;
using Microsoft.Extensions.DependencyInjection;

namespace datntdev.Microservices.Srv.Admin.Web.App.MultiTenancy
{
    internal class TenantManager(IServiceProvider services) : BaseManager<int, AppTenantEntity, SrvAdminDbContext>
    {
        private readonly SrvAdminDbContext _dbContext = services.GetRequiredService<SrvAdminDbContext>();

        public override async Task<AppTenantEntity> CreateEntityAsync(AppTenantEntity entity)
        {
            var createdEntity = await _dbContext.AppTenants.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return createdEntity.Entity;
        }

        public override async Task DeleteEntityAsync(int id)
        {
            var entity = await GetEntityAsync(id);
            _dbContext.AppTenants.Remove(entity);
        }

        public override async Task<AppTenantEntity> GetEntityAsync(int id)
        {
            var entity = await _dbContext.AppTenants.FindAsync(id);
            return entity is null ? throw new ExceptionNotFound() : entity!;
        }

        public override async Task<AppTenantEntity> UpdateEntityAsync(AppTenantEntity entity)
        {
            var updatedEntity = _dbContext.AppTenants.Update(entity);
            await _dbContext.SaveChangesAsync();
            return updatedEntity.Entity;
        }
    }
}

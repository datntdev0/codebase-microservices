using datntdev.Microservices.Common;
using datntdev.Microservices.Common.Web.App.Application;
using datntdev.Microservices.Common.Web.App.Exceptions;
using datntdev.Microservices.Srv.Admin.Web.App.MultiTenancy.Models;
using Microsoft.Extensions.DependencyInjection;

namespace datntdev.Microservices.Srv.Admin.Web.App.MultiTenancy
{
    internal class TenantManager(IServiceProvider services) : BaseManager<int, AppTenantEntity, SrvAdminDbContext>
    {
        private readonly SrvAdminDbContext _dbContext = services.GetRequiredService<SrvAdminDbContext>();

        public IQueryable<AppTenantEntity> GetQueryable() => _dbContext.AppTenants.AsQueryable();

        public override async Task<AppTenantEntity> CreateEntityAsync(AppTenantEntity entity)
        {
            var unixTimeSeconds = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            var combinedString = $"{entity.TenantName}{unixTimeSeconds}";
            entity.Id = combinedString.GetHashCode();

            var createdEntity = await _dbContext.AppTenants.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return createdEntity.Entity;
        }

        public override async Task DeleteEntityAsync(int id)
        {
            if (Constants.MultiTenancy.DefaultTenantId == id)
                throw new ExceptionConflict("The default tenant cannot be deleted.");

            var entity = await GetEntityAsync(id);
            _dbContext.AppTenants.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public override async Task<AppTenantEntity> GetEntityAsync(int id)
        {
            var entity = await _dbContext.AppTenants.FindAsync(id);
            return entity is null ? throw new ExceptionNotFound() : entity!;
        }

        public override async Task<AppTenantEntity> UpdateEntityAsync(AppTenantEntity entity)
        {
            if (Constants.MultiTenancy.DefaultTenantId == entity.Id)
                throw new ExceptionConflict("The default tenant cannot be updated.");

            var updatedEntity = _dbContext.AppTenants.Update(entity);
            await _dbContext.SaveChangesAsync();
            return updatedEntity.Entity;
        }
    }
}

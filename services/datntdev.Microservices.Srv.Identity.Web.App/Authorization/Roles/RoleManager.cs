using datntdev.Microservices.Common;
using datntdev.Microservices.Common.Web.App.Application;
using datntdev.Microservices.Common.Web.App.Exceptions;
using datntdev.Microservices.Srv.Identity.Web.App.Authorization.Roles.Models;
using datntdev.Microservices.Srv.Identity.Web.App.Authorization.Users.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace datntdev.Microservices.Srv.Identity.Web.App.Authorization.Roles
{
    public class RoleManager(IServiceProvider services) : BaseManager<int, AppRoleEntity, SrvIdentityDbContext>
    {
        private readonly SrvIdentityDbContext _dbContext = services.GetRequiredService<SrvIdentityDbContext>();

        public IQueryable<AppRoleEntity> GetQueryable() => _dbContext.AppRoles.Where(r => !r.IsDeleted).AsQueryable();

        public Task<AppRoleEntity?> FindAsync(string name, int? tenantId = null)
        {
            return _dbContext.AppRoles.FirstOrDefaultAsync(r => r.Name == name && r.TenantId == tenantId && !r.IsDeleted);
        }

        public override async Task<AppRoleEntity> GetEntityAsync(int id)
        {
            var entity = await _dbContext.AppRoles.Include(x => x.Users)
                .FirstOrDefaultAsync(r => r.Id == id && !r.IsDeleted);
            return entity is null ? throw new ExceptionNotFound() : entity!;
        }

        public override async Task<AppRoleEntity> CreateEntityAsync(AppRoleEntity entity)
        {
            var createdEntity = _dbContext.AppRoles.Add(entity);
            await _dbContext.SaveChangesAsync();
            return createdEntity.Entity;
        }

        public override async Task<AppRoleEntity> UpdateEntityAsync(AppRoleEntity entity)
        {
            if (entity.Name == Constants.Authorization.DefaultAdminRole)
                throw new ExceptionConflict("The default admin role cannot be updated.");

            var updatedEntity = _dbContext.AppRoles.Update(entity);
            await _dbContext.SaveChangesAsync();
            return updatedEntity.Entity;
        }

        public override async Task DeleteEntityAsync(int id)
        {
            var entity = await GetEntityAsync(id);
            
            if (entity.Name == Constants.Authorization.DefaultAdminRole)
                throw new ExceptionConflict("The default admin role cannot be deleted.");

            entity.IsDeleted = true;
            _dbContext.AppRoles.Update(entity);
            await _dbContext.SaveChangesAsync();
        }
    }
}

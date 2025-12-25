using datntdev.Microservices.Common.Web.App.Application;
using datntdev.Microservices.Common.Web.App.Exceptions;
using datntdev.Microservices.Srv.Identity.Web.App.Authorization.Users.Models;
using datntdev.Microservices.Srv.Identity.Web.App.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace datntdev.Microservices.Srv.Identity.Web.App.Authorization.Users
{
    public class UserManager(IServiceProvider services) : BaseManager<long, AppUserEntity, SrvIdentityDbContext>
    {
        private readonly IConfiguration _configuration = services.GetRequiredService<IConfiguration>();
        private readonly SrvIdentityDbContext _dbContext = services.GetRequiredService<SrvIdentityDbContext>();
        private readonly PasswordHasher _passwordHasher = services.GetRequiredService<PasswordHasher>();

        public IQueryable<AppUserEntity> GetQueryable() => _dbContext.AppUsers.Where(u => !u.IsDeleted).AsQueryable();

        public Task<AppUserEntity?> FindAsync(string username)
        {
            return _dbContext.AppUsers.FirstOrDefaultAsync(u => u.Username == username && !u.IsDeleted);
        }

        public override async Task<AppUserEntity> GetEntityAsync(long id)
        {
            var entity = await _dbContext.AppUsers.Include(x => x.Roles)
                .FirstOrDefaultAsync(u => u.Id == id && !u.IsDeleted);
            return entity is null ? throw new ExceptionNotFound() : entity!;
        }

        public override async Task<AppUserEntity> CreateEntityAsync(AppUserEntity entity)
        {
            await CheckUsernameExistedAsync(entity.Username);
            await CheckEmailExistedAsync(entity.EmailAddress);
            _passwordHasher.SetPassword(entity, entity.PasswordPlainText);
            var createdEntity = _dbContext.AppUsers.Add(entity);
            await _dbContext.SaveChangesAsync();
            return createdEntity.Entity;
        }

        public override async Task<AppUserEntity> UpdateEntityAsync(AppUserEntity entity)
        {
            if (_configuration.GetValue<string>("DefaultAdmin:Username") == entity.Username)
                throw new ExceptionConflict("The default admin user cannot be updated.");

            await CheckEmailExistedAsync(entity.EmailAddress, entity.Id);
            _passwordHasher.SetPassword(entity, entity.PasswordPlainText);
            var updatedEntity = _dbContext.AppUsers.Update(entity);
            await _dbContext.SaveChangesAsync();
            return updatedEntity.Entity;
        }

        public override async Task DeleteEntityAsync(long id)
        {
            var entity = await GetEntityAsync(id);
            
            if (_configuration.GetValue<string>("DefaultAdmin:Username") == entity.Username)
                throw new ExceptionConflict("The default admin user cannot be deleted.");

            entity.IsDeleted = true;
            _dbContext.AppUsers.Update(entity);
            await _dbContext.SaveChangesAsync();
        }

        private async Task CheckUsernameExistedAsync(string username)
        {
            var existed = await _dbContext.AppUsers.AnyAsync(u => u.Username == username && !u.IsDeleted);
            if (existed) throw new ExceptionConflict($"The username '{username}' is already existed.");
        }

        private async Task CheckEmailExistedAsync(string emailAddress, long? excludeId = null)
        {
            var existed = await _dbContext.AppUsers.AnyAsync(u
                => u.EmailAddress == emailAddress && !u.IsDeleted && (!excludeId.HasValue || u.Id != excludeId.Value));
            if (existed) throw new ExceptionConflict($"The email address '{emailAddress}' is already existed.");
        }
    }
}

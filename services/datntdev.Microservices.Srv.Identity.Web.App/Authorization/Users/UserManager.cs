using datntdev.Microservices.Srv.Identity.Web.App.Authorization.Users.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace datntdev.Microservices.Srv.Identity.Web.App.Authorization.Users
{
    public class UserManager(IServiceProvider services)
    {
        private readonly SrvIdentityDbContext _dbContext = services.GetRequiredService<SrvIdentityDbContext>();
        private readonly PasswordHasher<AppUserEntity> _passwordHasher = services.GetRequiredService<PasswordHasher<AppUserEntity>>();

        public Task<AppUserEntity?> FindAsync(string username)
        {
            return _dbContext.AppUsers.FirstOrDefaultAsync(u => u.Username == username);
        }

        public Task<AppUserEntity> GetAsync(string username)
        {
            return _dbContext.AppUsers.SingleAsync(u => u.Username == username);
        }

        public Task<AppUserEntity> CreateAsync(AppUserEntity userEntity, string password)
        {
            userEntity.PasswordHash = _passwordHasher.HashPassword(userEntity, password);
            var userEntityEntry = _dbContext.AppUsers.Add(userEntity);
            return _dbContext.SaveChangesAsync().ContinueWith(t => userEntityEntry.Entity);
        }

        public async Task DeleteAsync(string username)
        {
            _dbContext.AppUsers.Remove(await GetAsync(username));
            await _dbContext.SaveChangesAsync();
        }
    }
}

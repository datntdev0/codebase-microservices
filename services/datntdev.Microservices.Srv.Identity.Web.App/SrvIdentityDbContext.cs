using datntdev.Microservices.Common.Repository;
using datntdev.Microservices.Srv.Identity.Web.App.Authorization.Roles.Models;
using datntdev.Microservices.Srv.Identity.Web.App.Authorization.Users.Models;
using Microsoft.EntityFrameworkCore;

namespace datntdev.Microservices.Srv.Identity.Web.App
{
    public class SrvIdentityDbContext(DbContextOptions<SrvIdentityDbContext> options) 
        : BaseDbContext(options), IRelationalDbContext
    {
        public DbSet<AppUserEntity> AppUsers { get; set; }
        public DbSet<AppRoleEntity> AppRoles { get; set; }

        override protected void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<AppUserEntity>(b =>
            {
                b.HasIndex(e => e.Username);
                b.HasIndex(e => e.EmailAddress);
                b.Ignore(e => e.PasswordPlainText);
            });
            modelBuilder.Entity<AppRoleEntity>(b =>
            {
                b.HasIndex(e => e.Name);
            });
        }
    }
}

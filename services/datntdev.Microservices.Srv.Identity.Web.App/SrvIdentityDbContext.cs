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
        public DbSet<AppUserRoleEntity> AppUserRoles { get; set; }

        override protected void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<AppUserEntity>(b =>
            {
                b.HasIndex(e => e.Username);
                b.HasIndex(e => e.EmailAddress);
                b.Ignore(e => e.PasswordPlainText);
                b.HasMany(e => e.Roles).WithMany(e => e.Users)
                    .UsingEntity<AppUserRoleEntity>(
                        r => r.HasOne(e => e.Role).WithMany(e => e.UserRoles).HasForeignKey(e => e.RoleId),
                        l => l.HasOne(e => e.User).WithMany(e => e.UserRoles).HasForeignKey(e => e.UserId), 
                        j => j.HasIndex(nameof(AppUserRoleEntity.RoleId), nameof(AppUserRoleEntity.UserId)));
            });
            modelBuilder.Entity<AppRoleEntity>(b =>
            {
                b.HasIndex(e => e.Name);
            });
        }
    }
}

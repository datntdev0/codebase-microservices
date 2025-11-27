using datntdev.Microservices.Common.Models;
using datntdev.Microservices.Common.Repository;
using datntdev.Microservices.Srv.Admin.Web.App.MultiTenancy.Models;
using Microsoft.EntityFrameworkCore;
using MongoDB.EntityFrameworkCore.Extensions;

namespace datntdev.Microservices.Srv.Admin.Web.App
{
    public class SrvAdminDbContext(DbContextOptions<SrvAdminDbContext> options)
        : DbContext(options), IDocumentDbContext
    {
        public DbSet<AppTenantEntity> AppTenants { get; init; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<AppTenantEntity>().ToCollection(nameof(AppTenants));
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess = true)
        {
            SetDefaults();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            SetDefaults();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void SetDefaults()
        {
            var changedEntities = ChangeTracker.Entries().Where(x => x.State != EntityState.Unchanged);

            foreach (var entry in changedEntities.Where(x => x.State == EntityState.Modified))
            {
                if (entry.Entity is IUpdated entity)
                {
                    entity.UpdatedAt = DateTime.UtcNow;
                }
            }

            foreach (var entry in changedEntities.Where(x => x.State == EntityState.Added))
            {
                if (entry.Entity is ICreated entity)
                {
                    entity.CreatedAt = DateTime.UtcNow;
                }
            }
        }
    }
}

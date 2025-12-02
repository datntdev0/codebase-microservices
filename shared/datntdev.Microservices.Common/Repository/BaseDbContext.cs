using datntdev.Microservices.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace datntdev.Microservices.Common.Repository
{
    public abstract class BaseDbContext(DbContextOptions options): DbContext(options)
    {
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

using datntdev.Microservices.Common.Models;
using datntdev.Microservices.Common.Repository;
using datntdev.Microservices.Srv.Admin.Web.App.MultiTenancy.Models;
using Microsoft.EntityFrameworkCore;
using MongoDB.EntityFrameworkCore.Extensions;

namespace datntdev.Microservices.Srv.Admin.Web.App
{
    public class SrvAdminDbContext(DbContextOptions<SrvAdminDbContext> options)
        : BaseDbContext(options), IDocumentDbContext
    {
        public DbSet<AppTenantEntity> AppTenants { get; init; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<AppTenantEntity>().ToCollection(nameof(AppTenants));
        }
    }
}

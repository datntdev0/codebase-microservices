using System.Threading.Tasks;
using datntdev.Abp.Dependency;
using datntdev.Abp.Domain.Uow;
using Microsoft.EntityFrameworkCore;

namespace datntdev.Abp.EntityFrameworkCore.Uow;

public interface IEfCoreTransactionStrategy
{
    void InitOptions(UnitOfWorkOptions options);

    Task<DbContext> CreateDbContextAsync<TDbContext>(
        string connectionString,
        IDbContextResolver dbContextResolver) where TDbContext : DbContext;

    void Commit();

    void Dispose(IIocResolver iocResolver);

    DbContext CreateDbContext<TDbContext>(
        string connectionString,
        IDbContextResolver dbContextResolver) where TDbContext : DbContext;
}
using datntdev.Abp.Domain.Entities;
using datntdev.Abp.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace datntdev.Abp.EntityFrameworkCore.Repositories;

public class EfCoreRepositoryBase<TDbContext, TEntity> : EfCoreRepositoryBase<TDbContext, TEntity, int>, IRepository<TEntity>
    where TEntity : class, IEntity<int>
    where TDbContext : DbContext
{
    public EfCoreRepositoryBase(IDbContextProvider<TDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }
}
using datntdev.Microservices.Common.Models;
using datntdev.Microservices.Common.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace datntdev.Microservices.Common.Web.App.Application
{
    public abstract class BaseAppManager { }

    public abstract class BaseManager<TKey, TEntity, TDbContext>(IServiceProvider services) : BaseAppManager
        where TKey : IEquatable<TKey>
        where TEntity : BaseEntity<TKey>
        where TDbContext : IDbContext
    {
        protected readonly TDbContext _dbContext = services.GetRequiredService<TDbContext>();

        public abstract Task<TEntity> GetEntityAsync(TKey id);
        public abstract Task<TEntity> CreateEntityAsync(TEntity entity);
        public abstract Task<TEntity> UpdateEntityAsync(TEntity entity);
        public abstract Task DeleteEntityAsync(TKey id);
    }
}

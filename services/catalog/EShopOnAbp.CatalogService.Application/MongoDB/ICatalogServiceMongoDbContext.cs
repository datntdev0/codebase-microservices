using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace EShopOnAbp.CatalogService.Application
{
    [ConnectionStringName(CatalogServiceDbProperties.ConnectionStringName)]
    public interface ICatalogServiceMongoDbContext : IAbpMongoDbContext
    {
        /* Define mongo collections here. Example:
         * IMongoCollection<Question> Questions { get; }
         */
    }
}

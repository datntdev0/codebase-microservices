using EShopOnAbp.CatalogService.Application;
using Xunit;

namespace EShopOnAbp.CatalogService;

[CollectionDefinition(CatalogServiceTestConsts.CollectionDefinitionName)]
public class CatalogServiceApplicationCollection : CatalogServiceMongoDbCollectionFixtureBase
{

}
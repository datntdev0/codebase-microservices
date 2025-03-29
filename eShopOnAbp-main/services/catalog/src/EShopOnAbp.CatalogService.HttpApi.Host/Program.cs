using EShopOnAbp.Shared.Hosting.AspNetCore;
using EShopOnAbp.CatalogService;

var assembly = typeof(Program).Assembly;
await ApplicationBuilderHelper.RunApplicationAsync<CatalogServiceHttpApiHostModule>(args, assembly);

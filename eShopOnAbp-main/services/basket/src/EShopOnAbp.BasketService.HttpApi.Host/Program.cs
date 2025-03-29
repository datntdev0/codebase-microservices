using EShopOnAbp.BasketService;
using EShopOnAbp.Shared.Hosting.AspNetCore;

var assembly = typeof(Program).Assembly;
await ApplicationBuilderHelper.RunApplicationAsync<BasketServiceHttpApiHostModule>(args, assembly);

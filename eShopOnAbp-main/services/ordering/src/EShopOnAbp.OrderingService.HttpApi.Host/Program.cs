using EShopOnAbp.OrderingService;
using EShopOnAbp.Shared.Hosting.AspNetCore;

var assembly = typeof(Program).Assembly;
await ApplicationBuilderHelper.RunApplicationAsync<OrderingServiceHttpApiHostModule>(args, assembly);

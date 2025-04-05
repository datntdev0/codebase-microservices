using EShopOnAbp.OrderingService;
using EShopOnAbp.Shared.Hosting.AspNetCore;

var assembly = typeof(Program).Assembly;
await ApplicationBuilderHelper.RunApplicationAsync<OrderingServiceWebApiHostModule>(args, assembly);

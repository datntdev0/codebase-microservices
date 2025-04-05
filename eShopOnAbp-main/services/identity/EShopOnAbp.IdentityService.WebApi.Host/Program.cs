using EShopOnAbp.IdentityService;
using EShopOnAbp.Shared.Hosting.AspNetCore;

var assembly = typeof(Program).Assembly;
await ApplicationBuilderHelper.RunApplicationAsync<IdentityServiceWebApiHostModule>(args, assembly);

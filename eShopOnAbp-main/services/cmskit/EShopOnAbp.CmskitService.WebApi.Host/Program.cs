using EShopOnAbp.CmskitService;
using EShopOnAbp.Shared.Hosting.AspNetCore;

var assembly = typeof(Program).Assembly;
await ApplicationBuilderHelper.RunApplicationAsync<CmskitServiceWebApiHostModule>(args, assembly);

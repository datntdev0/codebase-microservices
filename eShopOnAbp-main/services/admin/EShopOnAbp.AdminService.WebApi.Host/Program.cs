using EShopOnAbp.Shared.Hosting.AspNetCore;
using EShopOnAbp.AdminService;

var assembly = typeof(Program).Assembly;
await ApplicationBuilderHelper.RunApplicationAsync<AdminServiceWebApiHostModule>(args, assembly);

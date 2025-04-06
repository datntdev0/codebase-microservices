using EShopOnAbp.PaymentService;
using EShopOnAbp.Shared.Hosting.AspNetCore;

var assembly = typeof(Program).Assembly;
await ApplicationBuilderHelper.RunApplicationAsync<PaymentServiceWebApiHostModule>(args, assembly);

using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Volo.Abp.Application;
using Microsoft.Extensions.Options;
using EShopOnAbp.PaymentService.PayPal;
using PayPalCheckoutSdk.Core;
using System;
using EShopOnAbp.PaymentService.PaymentRequests;
using Microsoft.Extensions.Logging;
using EShopOnAbp.PaymentService.PaymentMethods;
using Volo.Abp.Domain;
using EShopOnAbp.PaymentService.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace EShopOnAbp.PaymentService
{
    [DependsOn(
        typeof(PaymentServiceContractsModule),
        typeof(AbpDddApplicationModule),
        typeof(AbpDddDomainModule),
        typeof(AbpAutoMapperModule)
    )]
    public class PaymentServiceApplicationModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            PaymentServiceEfCoreEntityExtensionMappings.Configure();
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAutoMapperObjectMapper<PaymentServiceApplicationModule>();
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<PaymentServiceApplicationModule>(validate: true);
            });

            Configure<PayPalOptions>(context.Services.GetConfiguration().GetSection("Payment:PayPal"));

            context.Services.AddTransient(provider =>
            {
                var options = provider.GetService<IOptions<PayPalOptions>>().Value;

                if (options.Environment.IsNullOrWhiteSpace() || options.Environment == PayPalConsts.Environment.Sandbox)
                {
                    return new PayPalHttpClient(new SandboxEnvironment(options.ClientId, options.Secret));
                }

                return new PayPalHttpClient(new LiveEnvironment(options.ClientId, options.Secret));
            });

            context.Services.AddTransient<PaymentMethodResolver>(provider => new PaymentMethodResolver(
                provider.GetServices<IPaymentMethod>(),
                provider.GetRequiredService<ILogger<PaymentMethodResolver>>()
            ));

            context.Services.AddAbpDbContext<PaymentServiceDbContext>(options =>
            {
                options.AddRepository<PaymentRequest, EfCorePaymentRequestRepository>();
            });

            // https://www.npgsql.org/efcore/release-notes/6.0.html#opting-out-of-the-new-timestamp-mapping-logic
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

            Configure<AbpDbContextOptions>(options =>
            {
                /* The main point to change your DBMS.
                 * See also PaymentServiceMigrationsDbContextFactory for EF Core tooling. */
                options.UseNpgsql(b =>
                {
                    b.MigrationsHistoryTable("__PaymentService_Migrations");
                });
            });
        }
    }
}
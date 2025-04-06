using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;
using Volo.Abp.Modularity;
using System.Reflection;

namespace EShopOnAbp.Shared.Hosting.AspNetCore;

public static class ApplicationBuilderHelper
{
    public static WebApplicationBuilder CreateApplicationBuilder<TStartupModule>(string[] args)
        where TStartupModule : IAbpModule
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Host
            .AddAppSettingsSecretsJson()
            .UseAutofac()
            .UseSerilog();

        builder.AddServiceDefaults();
        builder.WebHost.ConfigureKestrel((context, options) =>
        {
            var kestrelConfig = context.Configuration.GetSection("Kestrel:Endpoints");

            foreach (var endpoint in kestrelConfig.GetChildren())
            {
                var url = endpoint.GetValue<string>("Url")!;
                var protocols = endpoint.GetValue<string>("Protocols")!;

                options.ListenAnyIP(new Uri(url).Port, listenOptions =>
                {
                    listenOptions.Protocols = Enum.Parse<HttpProtocols>(protocols);
                    if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
                    {
                        listenOptions.UseHttps();
                    }
                });
            }
        });

        return builder;
    }

    public static async Task<WebApplication> BuildApplicationAsync<TStartupModule>(string[] args)
        where TStartupModule : IAbpModule
    {
        var builder = CreateApplicationBuilder<TStartupModule>(args);
        await builder.AddApplicationAsync<TStartupModule>();
        return builder.Build();
    }

    public static async Task<int> RunApplicationAsync<TStartupModule>(string[] args, Assembly assembly)
        where TStartupModule : IAbpModule
    {
        var assemblyName = assembly.GetName().Name ?? throw new ArgumentNullException(nameof(assembly));

        SerilogConfigurationHelper.Configure(assemblyName);

        try
        {
            Log.Information($"Starting {assemblyName}.");
            var app = await BuildApplicationAsync<TStartupModule>(args);
            await app.InitializeApplicationAsync();
            await app.RunAsync();

            return 0;
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, $"{assemblyName} terminated unexpectedly!");
            return 1;
        }
        finally
        {
            await Log.CloseAndFlushAsync();
        }
    }
}
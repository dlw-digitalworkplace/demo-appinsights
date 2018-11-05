using System.Threading.Tasks;
using Demo.AppInsights.Core.Configuration;
using Demo.AppInsights.Core.Telemetry;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Demo.AppInsights.WebJob
{
    internal static class Program
    {
        private static async Task Main(string[] args)
        {
            var builder = new HostBuilder() // Create a HostBuilder
                
                .ConfigureHostConfiguration(config =>
                {
                    config.AddEnvironmentVariables(); // Add environment variables to the Host
                })
                
                .ConfigureWebJobs(b =>
                {
                    b.AddAzureStorageCoreServices().AddAzureStorage(options => { options.BatchSize = 5; }); // Add AzureStorage configuration
                })

                .ConfigureAppConfiguration((context, b) =>
                {
                    var env = context.HostingEnvironment;
                    
                    // Add app settings from various sources
                    b.SetBasePath(env.ContentRootPath)
                        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                        .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true,
                            reloadOnChange: true)
                        .AddEnvironmentVariables();

                    if (env.IsDevelopment())
                    {
                        // Add personal app settings when in development mode
                        b.AddJsonFile("appsettings.Personal.json", optional: true, reloadOnChange: true);
                    }
                })

                .ConfigureLogging((context, b) =>
                {
                    b.AddConfiguration(context.Configuration.GetSection("Logging")); // Configure log level settings

                    // Get ApplicationInsights configuration settings
                    var appInsightsSettings = context.Configuration.GetSection("ApplicationInsights")
                        .Get<AppInsightsConfiguration>();
                    var appInsightsInstrumentationKey =
                        context.Configuration.GetSection("APPINSIGHTS_INSTRUMENTATIONKEY")?.Value
                        ?? appInsightsSettings.InstrumentationKey;

                    // Add ApplicationInsights as logging provider
                    b.AddApplicationInsights(options =>
                    {
                        options.InstrumentationKey = appInsightsInstrumentationKey;
                    });

                    b.AddConsole(); // Add console as logging provider
                })

                .ConfigureServices((context, services) =>
                {
                    var appInsightsConfig = context.Configuration.GetSection("ApplicationInsights")
                        .Get<AppInsightsConfiguration>();

                    // Add TelemetryInitializer implementations
                    services.AddSingleton<ITelemetryInitializer, CorrelationTelemetryInitializer>();
                    services.AddSingleton<ITelemetryInitializer, WebJobNameTelemetryInitializer>();
                    services.AddSingleton<ITelemetryInitializer>(
                        new CloudRoleNameInitializer(appInsightsConfig.CloudRoleName ?? nameof(WebJob)));

                    // Add Services
                    services.AddSingleton<Services.ValuesService>();
                })

                .UseConsoleLifetime();

            var host = builder.Build();

            using (host)
                await host.RunAsync();
        }
    }
}
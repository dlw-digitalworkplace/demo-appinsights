using Demo.AppInsights.Core.Configuration;
using Demo.AppInsights.Core.Telemetry;
using Demo.AppInsights.Services;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Demo.AppInsights.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1); // Add MVC

            // Add CORS options
            services.AddCors(options => options
                .AddPolicy("AllOrigins",
                    builder => builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                ));

            var appInsightsConfig = Configuration.GetSection("ApplicationInsights").Get<AppInsightsConfiguration>(); // Get the ApplicationInsights settings
            services.AddSingleton<ITelemetryInitializer>(new CloudRoleNameInitializer(appInsightsConfig.CloudRoleName ?? nameof(API))); // Add CloudRoleName telemetry initializer
            services.AddApplicationInsightsTelemetry(Configuration); // Initialize ApplicationInsights configuration

            services.Configure<AzureStorageConfiguration>(Configuration.GetSection("AzureStorage")); // Configure AzureStorageConfiguration dependecy injection

            services.AddSingleton<ValuesService>(); // Configure ValuesService dependecy injection
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory log)
        {
            app.UseHsts();

            app.UseHttpsRedirection();
            app.UseCors("AllOrigins");
            app.UseMvc();

            log.AddApplicationInsights(app.ApplicationServices, LogLevel.Trace); // Add ApplicationInsights as logging provider
        }
    }
}
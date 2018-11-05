using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Demo.AppInsights.API
{
    public static class Program
    {
        private static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run(); // Run the WebHost
        }

        /// <summary>
        /// Creates a WebHost to run the application.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args) // Create a WebHost with default configuration
                .ConfigureAppConfiguration((context, builder) =>
                {
                    if (context.HostingEnvironment.IsDevelopment())
                    {
                        // Add personal app settings when in development mode
                        builder.AddJsonFile("appsettings.Personal.json", optional: true, reloadOnChange: true);
                    }
                })
                .UseStartup<Startup>();
    }
}
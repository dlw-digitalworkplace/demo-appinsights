using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.Configuration;

namespace Demo.AppInsights.Core.Telemetry
{
    /// <summary>
    /// Initializes the WebJobName property.
    /// </summary>
    public class WebJobNameTelemetryInitializer : ITelemetryInitializer
    {
        private readonly IConfiguration _configuration;

        public WebJobNameTelemetryInitializer(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Initialize(ITelemetry telemetry)
        {
            var webJobName = _configuration["WEBJOBS_NAME"];
            telemetry.Context.GlobalProperties["WebJobName"] = webJobName;
        }
    }
}
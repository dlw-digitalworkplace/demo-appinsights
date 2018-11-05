using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;

namespace Demo.AppInsights.Core.Telemetry
{
    /// <summary>
    /// Changes the Operation Id to the current value from the CorrelationManager.
    /// </summary>
    public class CorrelationTelemetryInitializer : ITelemetryInitializer
    {
        public void Initialize(ITelemetry telemetry)
        {
            telemetry.Context.Operation.Id = CorrelationManager.GetOperationId();
        }
    }
}
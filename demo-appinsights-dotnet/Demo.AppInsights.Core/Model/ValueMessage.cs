using Demo.AppInsights.Core.Telemetry;

namespace Demo.AppInsights.Core.Model
{
    public class ValueMessage : ICorrelationMessage
    {
        public string OperationId { get; set; }

        public string Value { get; set; }
    }
}
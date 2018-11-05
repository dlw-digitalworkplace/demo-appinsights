namespace Demo.AppInsights.Core.Telemetry
{
    public interface ICorrelationMessage
    {
        string OperationId { get; }
    }
}
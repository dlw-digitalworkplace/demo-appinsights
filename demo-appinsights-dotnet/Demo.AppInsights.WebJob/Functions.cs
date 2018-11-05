using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Demo.AppInsights.Core.Model;
using Demo.AppInsights.Core.Telemetry;
using Demo.AppInsights.Services;
using Microsoft.ApplicationInsights;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using CorrelationManager = Demo.AppInsights.Core.Telemetry.CorrelationManager;

namespace Demo.AppInsights.WebJob
{
    public class Functions
    {
        public TelemetryClient TelemetryClient { get; }
        public ValuesService ValuesService { get; }

        public Functions(TelemetryClient telemetryClient, Services.ValuesService valuesService)
        {
            TelemetryClient = telemetryClient;
            ValuesService = valuesService;
        }

        public void ProcessQueueMessage([QueueTrigger("demoqueue")] ValueMessage message, ILogger log)
        {
            // Set operation id correlation
            CorrelationManager.SetOperationId(message.OperationId);

            TelemetryClient.TrackEvent(GlobalMetricNames.QueueMessageReceived, new Dictionary<string, string>
            {
                [nameof(message)] = JsonConvert.SerializeObject(message)
            });

            var stopWatch = Stopwatch.StartNew();

            // Process queue message
            var i = 0;
            const int max = 5;

            while (i++ < max)
            {
                log.LogTrace($"Tracing a test message... ({i}/{max})");
                Thread.Sleep(2500);
            }

            stopWatch.Stop();

            TelemetryClient.TrackEvent(nameof(ProcessQueueMessage), metrics: new Dictionary<string, double>
            {
                [GlobalMetricNames.Elapsed] = stopWatch.ElapsedMilliseconds
            });
        }
    }
}
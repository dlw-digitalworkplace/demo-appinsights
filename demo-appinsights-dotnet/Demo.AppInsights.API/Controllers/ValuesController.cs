using System;
using System.Collections.Generic;
using Demo.AppInsights.Services;
using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Demo.AppInsights.API.Controllers
{
    [CorrelationInitializer]
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private ILogger<ValuesController> Logger { get; }
        private TelemetryClient TelemetryClient { get; }
        private ValuesService ValuesService { get; }

        public ValuesController(ILogger<ValuesController> logger, TelemetryClient telemetryClient, ValuesService valuesService)
        {
            Logger = logger;
            TelemetryClient = telemetryClient;
            ValuesService = valuesService;
        }

        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            Logger.LogInformation("ValuesController.Get has been called (info)");

            TelemetryClient.TrackEvent(nameof(Get));
            TelemetryClient.TrackMetric(nameof(Get), 0);

            var returnValue = new[] { ValuesService.GetRandomValue(), ValuesService.GetRandomValue() };

            Logger.LogDebug("ValuesController.Get returns with value {returnValue}",
                JsonConvert.SerializeObject(returnValue));

            return returnValue;
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
            Logger.LogDebug("ValuesController.Post received input value {inputValue}",
                JsonConvert.SerializeObject(value));

            ValuesService.CreateValue(value);
        }

        // DELETE api/values?value={value}
        [HttpDelete]
        public void Delete(string value)
        {
            Logger.LogDebug("ValuesController.Delete received input value {inputValue}",
                JsonConvert.SerializeObject(value));

            throw new NotSupportedException("Deleting values is not supported in the application.");
        }
    }
}

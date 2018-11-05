using System;
using Demo.AppInsights.Core.Configuration;
using Demo.AppInsights.Core.Model;
using Demo.AppInsights.Core.Telemetry;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;

namespace Demo.AppInsights.Services
{
    public class ValuesService
    {
        private IOptions<AzureStorageConfiguration> AzureStorageConfiguration { get; }

        public ValuesService(IOptions<AzureStorageConfiguration> azureStorageConfiguration)
        {
            AzureStorageConfiguration = azureStorageConfiguration;
        }

        public void CreateValue(string value)
        {
            var storageAccount = CloudStorageAccount.Parse(AzureStorageConfiguration.Value.QueueConnectionString);
            var queueClient = storageAccount.CreateCloudQueueClient();
            var queue = queueClient.GetQueueReference(AzureStorageConfiguration.Value.QueueName);
            queue.CreateIfNotExists();

            var message = new CloudQueueMessage(JsonConvert.SerializeObject(new ValueMessage
            {
                OperationId = CorrelationManager.GetOperationId(),
                Value = value
            }));

            queue.AddMessage(message);
        }

        public string GetRandomValue()
        {
            return Guid.NewGuid().ToString("N");
        }
    }
}
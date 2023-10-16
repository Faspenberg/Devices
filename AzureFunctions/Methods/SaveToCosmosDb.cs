using System;
using System.Text;
using Azure.Messaging.EventHubs;
using AzureFunctions.Models;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AzureFunctions.Methods
{
    public class SaveToCosmosDb
    {
        private readonly ILogger<SaveToCosmosDb> _logger;
        private readonly CosmosClient _cosmosClient;
        private readonly Container _lampContainer;
        private readonly Container _fanContainer;
        private readonly Container _speakerContainer;

        public SaveToCosmosDb(ILogger<SaveToCosmosDb> logger)
        {
            _logger = logger;

            _cosmosClient =
                new CosmosClient(
                    "AccountEndpoint=https://kyh-smartunitcosmosdb.documents.azure.com:443/;AccountKey=ImCYA5pgXRqg6qSGDu7bkaOn3HRrrlp6nfeyyzDfYizs45y6Esi19ZSbYqGETqXWrnLIiswGjaRHACDbWY58pw==;");
            var database = _cosmosClient.GetDatabase("CosmosDb");
            _lampContainer = database.GetContainer("lampData");
            _fanContainer = database.GetContainer("fanData");
            _speakerContainer = database.GetContainer("speakerData");
        }

            [Function(nameof(SaveToCosmosDb))]
        public async Task Run([EventHubTrigger("iothub-ehub-kyh-smartu-25230147-8f58cd7876", Connection = "IotHubEndpoint")] EventData[] events)
        {
            foreach (EventData @event in events)
            {
                try
                {

                    var json = Encoding.UTF8.GetString(@event.Body.ToArray());

                    dynamic data = null;

                    var container = JsonConvert.DeserializeObject<CosmosContainer>(json);

                    switch (container.ContainerName)
                    {
                        case "lampData":
                            data = JsonConvert.DeserializeObject<LampDataMessage>(json)!;
                            await _lampContainer.CreateItemAsync(data, new PartitionKey(data.id));
                            break;

                        case "fanData":
                            data = JsonConvert.DeserializeObject<FanDataMessage>(json)!;
                            await _fanContainer.CreateItemAsync(data, new PartitionKey(data.id));
                            break;
                        case "speakerData":
                            data = JsonConvert.DeserializeObject<SpeakerDataMessage>(json)!;
                            await _speakerContainer.CreateItemAsync(data, new PartitionKey(data.id));
                            break;
                        default:
                            _logger.LogWarning($"Error: {container.ContainerName}");
                            break;
                    }



                    _logger.LogInformation($"Saved Message: {data}");
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Could not save: {ex.Message}");
                }
            }
        }
    }
}

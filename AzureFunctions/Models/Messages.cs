using System;
using System.Text;
using Azure.Messaging.EventHubs;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace AzureFunctions.Models
{
    public class Messages
    {
        private readonly ILogger<Messages> _logger;

        public Messages(ILogger<Messages> logger)
        {
            _logger = logger;
        }

        [Function(nameof(Messages))]
        public void Run([EventHubTrigger("iothub-ehub-kyh-smartu-25230147-8f58cd7876", Connection = "IotHubEndpoint")] EventData[] events)
        {
            foreach (EventData @event in events)
            {
                var data = Encoding.UTF8.GetString(@event.Body.ToArray());
                _logger.LogInformation("Event Body: {body}", data);
            }
        }
    }
}

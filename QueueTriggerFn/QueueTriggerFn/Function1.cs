using Azure.Messaging.EventHubs;
using Azure.Storage.Queues.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System;
using System.Text;
using System.Text.Json;

namespace QueueTriggerFn;

public class Function1
{
    private readonly ILogger<Function1> _logger;

    public Function1(ILogger<Function1> logger)
    {
        _logger = logger;
    }

    [Function(nameof(Function1))]
    [EventHubOutput("my-test-eventhub", Connection = "EventHubConnection")]
    public async Task<string> RunAsync([QueueTrigger("my-az-fn-queue", Connection = "AzureWebJobsStorage")] QueueMessage message)
    {
        _logger.LogInformation("C# Queue trigger function processed: {messageText}", message.MessageText);

        var eventData = new { Id = Guid.NewGuid(), Message = message.MessageText, ProcessedAt = DateTime.UtcNow.ToString("o") };

        return JsonSerializer.Serialize(eventData);
    }
}
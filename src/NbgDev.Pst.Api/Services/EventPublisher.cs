using Azure.Storage.Queues;
using NbgDev.Pst.Events.Contract.Base;
using NbgDev.Pst.Events.Contract.Models;
using System.Text.Json;

namespace NbgDev.Pst.Api.Services;

public interface IEventPublisher
{
    Task PublishAsync(BaseEvent @event, CancellationToken cancellationToken = default);
}

public class EventPublisher(QueueServiceClient queueServiceClient, ILogger<EventPublisher> logger) : IEventPublisher
{
    private const string QueueName = "api-events";

    public async Task PublishAsync(BaseEvent @event, CancellationToken cancellationToken = default)
    {
        var queue = queueServiceClient.GetQueueClient(QueueName);
        await queue.CreateIfNotExistsAsync(cancellationToken: cancellationToken);

        var message = JsonSerializer.Serialize(@event, @event.GetType());
        await queue.SendMessageAsync(message, cancellationToken: cancellationToken);

        logger.LogInformation(
            "Published event of type {EventType}",
            @event.EventType);
    }
}

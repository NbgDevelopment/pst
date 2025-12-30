using Azure.Storage.Queues;
using NbgDev.Pst.Events.Contract.Base;
using System.Text.Json;

namespace NbgDev.Pst.Processing.Services;

public interface IEventPublisher
{
    Task PublishAsync(BaseEvent @event, CancellationToken cancellationToken = default);
}

public class EventPublisher(QueueServiceClient queueServiceClient, ILogger<EventPublisher> logger) : IEventPublisher
{
    private const string QueueName = "processing-events";

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

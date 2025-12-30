using Azure.Storage.Queues;
using NbgDev.Pst.Events.Contract.Models;
using System.Text.Json;

namespace NbgDev.Pst.Api.Services;

public interface IEventPublisher
{
    Task PublishProjectCreatedAsync(ProjectCreatedEvent projectCreatedEvent, CancellationToken cancellationToken = default);
}

public class EventPublisher(QueueServiceClient queueServiceClient, ILogger<EventPublisher> logger) : IEventPublisher
{
    private const string QueueName = "api-events";

    public async Task PublishProjectCreatedAsync(ProjectCreatedEvent projectCreatedEvent, CancellationToken cancellationToken = default)
    {
        var queue = queueServiceClient.GetQueueClient(QueueName);
        await queue.CreateIfNotExistsAsync(cancellationToken: cancellationToken);

        var message = JsonSerializer.Serialize(projectCreatedEvent);
        await queue.SendMessageAsync(message, cancellationToken: cancellationToken);

        logger.LogInformation(
            "Published ProjectCreatedEvent for project {ProjectId} - {ProjectName}",
            projectCreatedEvent.ProjectId,
            projectCreatedEvent.ProjectName);
    }
}

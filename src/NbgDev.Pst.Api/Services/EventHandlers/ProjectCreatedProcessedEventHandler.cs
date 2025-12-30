using NbgDev.Pst.Events.Contract.Base;
using NbgDev.Pst.Events.Contract.Models;

namespace NbgDev.Pst.Api.Services.EventHandlers;

public class ProjectCreatedProcessedEventHandler(
    ILogger<ProjectCreatedProcessedEventHandler> logger) : IEventHandler
{
    public bool CanHandle(string eventType) => eventType == nameof(ProjectCreatedProcessedEvent);

    public Task HandleAsync(BaseEvent @event, CancellationToken cancellationToken = default)
    {
        var processedEvent = @event as ProjectCreatedProcessedEvent;
        if (processedEvent == null)
        {
            logger.LogError("Event is not a ProjectCreatedProcessedEvent");
            return Task.CompletedTask;
        }

        logger.LogInformation(
            "Received project created processed event for project {ProjectId}. Success: {Success}, Message: {Message}",
            processedEvent.ProjectId,
            processedEvent.Success,
            processedEvent.Message);

        return Task.CompletedTask;
    }
}

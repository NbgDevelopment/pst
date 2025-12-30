using NbgDev.Pst.Events.Contract.Base;
using NbgDev.Pst.Events.Contract.Models;
using NbgDev.Pst.Processing.Services;

namespace NbgDev.Pst.Processing.Handlers;

public class ProjectCreatedEventHandler(
    IEventPublisher eventPublisher,
    ILogger<ProjectCreatedEventHandler> logger) : IEventHandler
{
    public bool CanHandle(string eventType) => eventType == nameof(ProjectCreatedEvent);

    public async Task HandleAsync(BaseEvent @event, CancellationToken cancellationToken = default)
    {
        var projectCreatedEvent = @event as ProjectCreatedEvent;
        if (projectCreatedEvent == null)
        {
            logger.LogError("Event is not a ProjectCreatedEvent");
            return;
        }

        logger.LogInformation(
            "Processing project created event for project {ProjectId} - {ProjectName} ({ShortName})",
            projectCreatedEvent.ProjectId,
            projectCreatedEvent.ProjectName,
            projectCreatedEvent.ShortName);

        // Send confirmation event
        var processedEvent = new ProjectCreatedProcessedEvent
        {
            EventType = nameof(ProjectCreatedProcessedEvent),
            ProjectId = projectCreatedEvent.ProjectId,
            Success = true,
            Message = "Project creation processed successfully"
        };

        await eventPublisher.PublishAsync(processedEvent, cancellationToken);

        logger.LogInformation(
            "Sent confirmation event for project {ProjectId}",
            projectCreatedEvent.ProjectId);
    }
}

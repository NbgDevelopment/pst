using NbgDev.Pst.Events.Contract.Base;
using NbgDev.Pst.Events.Contract.Models;
using NbgDev.Pst.Processing.Services;

namespace NbgDev.Pst.Processing.Handlers;

public class ProjectCreatedEventHandler(
    IEntraIdGroupService entraIdGroupService,
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

        try
        {
            // Create EntraId group for the project
            var (groupId, groupName) = await entraIdGroupService.CreateGroupForProjectAsync(
                projectCreatedEvent.ProjectId,
                projectCreatedEvent.ProjectName,
                projectCreatedEvent.ShortName,
                cancellationToken);

            // Send confirmation event
            var processedEvent = new ProjectCreatedProcessedEvent
            {
                EventType = nameof(ProjectCreatedProcessedEvent),
                ProjectId = projectCreatedEvent.ProjectId,
                Success = true,
                Message = "Project creation processed successfully",
                GroupId = groupId,
                GroupName = groupName
            };

            await eventPublisher.PublishAsync(processedEvent, cancellationToken);

            logger.LogInformation(
                "Sent confirmation event for project {ProjectId}",
                projectCreatedEvent.ProjectId);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to create EntraId group for project {ProjectId}", projectCreatedEvent.ProjectId);

            var processedEvent = new ProjectCreatedProcessedEvent
            {
                EventType = nameof(ProjectCreatedProcessedEvent),
                ProjectId = projectCreatedEvent.ProjectId,
                Success = false,
                Message = $"Failed to create EntraId group: {ex.Message}"
            };

            await eventPublisher.PublishAsync(processedEvent, cancellationToken);
        }
    }
}

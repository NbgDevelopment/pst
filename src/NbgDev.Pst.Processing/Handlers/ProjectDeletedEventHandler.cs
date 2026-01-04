using NbgDev.Pst.Events.Contract.Base;
using NbgDev.Pst.Events.Contract.Models;
using NbgDev.Pst.Processing.Services;

namespace NbgDev.Pst.Processing.Handlers;

public class ProjectDeletedEventHandler(
    IEntraIdGroupService entraIdGroupService,
    ILogger<ProjectDeletedEventHandler> logger) : IEventHandler
{
    public bool CanHandle(string eventType) => eventType == nameof(ProjectDeletedEvent);

    public async Task HandleAsync(BaseEvent @event, CancellationToken cancellationToken = default)
    {
        var projectDeletedEvent = @event as ProjectDeletedEvent;
        if (projectDeletedEvent == null)
        {
            logger.LogError("Event is not a ProjectDeletedEvent");
            return;
        }

        logger.LogInformation(
            "Processing project deleted event for project {ProjectId}",
            projectDeletedEvent.ProjectId);

        try
        {
            // Find the group for this project
            var groupId = await entraIdGroupService.GetGroupIdForProjectAsync(
                projectDeletedEvent.ProjectId,
                cancellationToken);

            if (groupId == null)
            {
                logger.LogWarning(
                    "No group found for project {ProjectId}, nothing to delete",
                    projectDeletedEvent.ProjectId);
                return;
            }

            // Delete the group
            await entraIdGroupService.DeleteGroupAsync(groupId, cancellationToken);

            logger.LogInformation(
                "Successfully deleted group {GroupId} for project {ProjectId}",
                groupId,
                projectDeletedEvent.ProjectId);
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Failed to delete group for project {ProjectId}",
                projectDeletedEvent.ProjectId);
        }
    }
}

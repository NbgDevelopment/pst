using NbgDev.Pst.Events.Contract.Base;
using NbgDev.Pst.Events.Contract.Models;
using NbgDev.Pst.Processing.Services;

namespace NbgDev.Pst.Processing.Handlers;

public class ProjectMemberRemovedEventHandler(
    IEntraIdGroupService entraIdGroupService,
    ILogger<ProjectMemberRemovedEventHandler> logger) : IEventHandler
{
    public bool CanHandle(string eventType) => eventType == nameof(ProjectMemberRemovedEvent);

    public async Task HandleAsync(BaseEvent @event, CancellationToken cancellationToken = default)
    {
        var memberRemovedEvent = @event as ProjectMemberRemovedEvent;
        if (memberRemovedEvent == null)
        {
            logger.LogError("Event is not a ProjectMemberRemovedEvent");
            return;
        }

        logger.LogInformation(
            "Processing member removed event for project {ProjectId} - removing user {UserId}",
            memberRemovedEvent.ProjectId,
            memberRemovedEvent.UserId);

        try
        {
            // Find the group for this project
            var groupId = await entraIdGroupService.GetGroupIdForProjectAsync(
                memberRemovedEvent.ProjectId,
                cancellationToken);

            if (groupId == null)
            {
                logger.LogWarning(
                    "No group found for project {ProjectId}, cannot remove member {UserId}",
                    memberRemovedEvent.ProjectId,
                    memberRemovedEvent.UserId);
                return;
            }

            // Remove member from the group
            await entraIdGroupService.RemoveMemberFromGroupAsync(
                groupId,
                memberRemovedEvent.UserId,
                cancellationToken);

            logger.LogInformation(
                "Successfully removed user {UserId} from group {GroupId} for project {ProjectId}",
                memberRemovedEvent.UserId,
                groupId,
                memberRemovedEvent.ProjectId);
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Failed to remove user {UserId} from group for project {ProjectId}",
                memberRemovedEvent.UserId,
                memberRemovedEvent.ProjectId);
        }
    }
}

using NbgDev.Pst.Events.Contract.Base;
using NbgDev.Pst.Events.Contract.Models;
using NbgDev.Pst.Processing.Services;

namespace NbgDev.Pst.Processing.Handlers;

public class ProjectMemberAddedEventHandler(
    IEntraIdGroupService entraIdGroupService,
    ILogger<ProjectMemberAddedEventHandler> logger) : IEventHandler
{
    public bool CanHandle(string eventType) => eventType == nameof(ProjectMemberAddedEvent);

    public async Task HandleAsync(BaseEvent @event, CancellationToken cancellationToken = default)
    {
        var memberAddedEvent = @event as ProjectMemberAddedEvent;
        if (memberAddedEvent == null)
        {
            logger.LogError("Event is not a ProjectMemberAddedEvent");
            return;
        }

        logger.LogInformation(
            "Processing member added event for project {ProjectId} - adding user {UserId} ({FirstName} {LastName})",
            memberAddedEvent.ProjectId,
            memberAddedEvent.UserId,
            memberAddedEvent.FirstName,
            memberAddedEvent.LastName);

        try
        {
            // Find the group for this project
            var groupId = await entraIdGroupService.GetGroupIdForProjectAsync(
                memberAddedEvent.ProjectId,
                cancellationToken);

            if (groupId == null)
            {
                logger.LogWarning(
                    "No group found for project {ProjectId}, cannot add member {UserId}",
                    memberAddedEvent.ProjectId,
                    memberAddedEvent.UserId);
                return;
            }

            // Add member to the group
            await entraIdGroupService.AddMemberToGroupAsync(
                groupId,
                memberAddedEvent.UserId,
                cancellationToken);

            logger.LogInformation(
                "Successfully added user {UserId} to group {GroupId} for project {ProjectId}",
                memberAddedEvent.UserId,
                groupId,
                memberAddedEvent.ProjectId);
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Failed to add user {UserId} to group for project {ProjectId}",
                memberAddedEvent.UserId,
                memberAddedEvent.ProjectId);
        }
    }
}

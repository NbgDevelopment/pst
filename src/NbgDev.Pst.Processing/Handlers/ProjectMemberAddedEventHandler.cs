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

        // Check if group ID is available
        if (string.IsNullOrEmpty(memberAddedEvent.GroupId))
        {
            logger.LogWarning(
                "No group ID found for project {ProjectId}, cannot add member {UserId}",
                memberAddedEvent.ProjectId,
                memberAddedEvent.UserId);
            return;
        }

        try
        {
            // Add member to the group
            await entraIdGroupService.AddMemberToGroupAsync(
                memberAddedEvent.GroupId,
                memberAddedEvent.UserId,
                cancellationToken);

            logger.LogInformation(
                "Successfully added user {UserId} to group {GroupId} for project {ProjectId}",
                memberAddedEvent.UserId,
                memberAddedEvent.GroupId,
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

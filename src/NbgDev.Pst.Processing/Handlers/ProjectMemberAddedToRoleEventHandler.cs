using NbgDev.Pst.Events.Contract.Base;
using NbgDev.Pst.Events.Contract.Models;

namespace NbgDev.Pst.Processing.Handlers;

public class ProjectMemberAddedToRoleEventHandler(
    ILogger<ProjectMemberAddedToRoleEventHandler> logger) : IEventHandler
{
    public bool CanHandle(string eventType) => eventType == nameof(ProjectMemberAddedToRoleEvent);

    public Task HandleAsync(BaseEvent @event, CancellationToken cancellationToken = default)
    {
        var memberAddedEvent = @event as ProjectMemberAddedToRoleEvent;
        if (memberAddedEvent == null)
        {
            logger.LogError("Event is not a ProjectMemberAddedToRoleEvent");
            return Task.CompletedTask;
        }

        logger.LogInformation(
            "Processing member added to role event - adding user {UserId} ({FirstName} {LastName}) to role {RoleId}",
            memberAddedEvent.UserId,
            memberAddedEvent.FirstName,
            memberAddedEvent.LastName,
            memberAddedEvent.RoleId);

        // Placeholder for future processing logic (e.g., adding member to corresponding Entra ID resources)

        return Task.CompletedTask;
    }
}

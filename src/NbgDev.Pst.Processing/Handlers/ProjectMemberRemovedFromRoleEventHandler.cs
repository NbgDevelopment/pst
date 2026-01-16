using NbgDev.Pst.Events.Contract.Base;
using NbgDev.Pst.Events.Contract.Models;

namespace NbgDev.Pst.Processing.Handlers;

public class ProjectMemberRemovedFromRoleEventHandler(
    ILogger<ProjectMemberRemovedFromRoleEventHandler> logger) : IEventHandler
{
    public bool CanHandle(string eventType) => eventType == nameof(ProjectMemberRemovedFromRoleEvent);

    public Task HandleAsync(BaseEvent @event, CancellationToken cancellationToken = default)
    {
        var memberRemovedEvent = @event as ProjectMemberRemovedFromRoleEvent;
        if (memberRemovedEvent == null)
        {
            logger.LogError("Event is not a ProjectMemberRemovedFromRoleEvent");
            return Task.CompletedTask;
        }

        logger.LogInformation(
            "Processing member removed from role event - removing user {UserId} from role {RoleId}",
            memberRemovedEvent.UserId,
            memberRemovedEvent.RoleId);

        // Placeholder for future processing logic (e.g., removing member from corresponding Entra ID resources)

        return Task.CompletedTask;
    }
}

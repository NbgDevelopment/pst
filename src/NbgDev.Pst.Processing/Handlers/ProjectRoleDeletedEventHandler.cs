using NbgDev.Pst.Events.Contract.Base;
using NbgDev.Pst.Events.Contract.Models;

namespace NbgDev.Pst.Processing.Handlers;

public class ProjectRoleDeletedEventHandler(
    ILogger<ProjectRoleDeletedEventHandler> logger) : IEventHandler
{
    public bool CanHandle(string eventType) => eventType == nameof(ProjectRoleDeletedEvent);

    public Task HandleAsync(BaseEvent @event, CancellationToken cancellationToken = default)
    {
        var roleDeletedEvent = @event as ProjectRoleDeletedEvent;
        if (roleDeletedEvent == null)
        {
            logger.LogError("Event is not a ProjectRoleDeletedEvent");
            return Task.CompletedTask;
        }

        logger.LogInformation(
            "Processing role deleted event for project {ProjectId} - role {RoleId}",
            roleDeletedEvent.ProjectId,
            roleDeletedEvent.RoleId);

        // Placeholder for future processing logic (e.g., cleaning up corresponding Entra ID resources)

        return Task.CompletedTask;
    }
}

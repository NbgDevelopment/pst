using NbgDev.Pst.Events.Contract.Base;
using NbgDev.Pst.Events.Contract.Models;

namespace NbgDev.Pst.Processing.Handlers;

public class ProjectRoleCreatedEventHandler(
    ILogger<ProjectRoleCreatedEventHandler> logger) : IEventHandler
{
    public bool CanHandle(string eventType) => eventType == nameof(ProjectRoleCreatedEvent);

    public Task HandleAsync(BaseEvent @event, CancellationToken cancellationToken = default)
    {
        var roleCreatedEvent = @event as ProjectRoleCreatedEvent;
        if (roleCreatedEvent == null)
        {
            logger.LogError("Event is not a ProjectRoleCreatedEvent");
            return Task.CompletedTask;
        }

        logger.LogInformation(
            "Processing role created event for project {ProjectId} - role {RoleId} named '{RoleName}'",
            roleCreatedEvent.ProjectId,
            roleCreatedEvent.RoleId,
            roleCreatedEvent.Name);

        // Placeholder for future processing logic (e.g., creating corresponding Entra ID resources)

        return Task.CompletedTask;
    }
}

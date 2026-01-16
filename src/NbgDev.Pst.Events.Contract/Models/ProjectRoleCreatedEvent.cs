using NbgDev.Pst.Events.Contract.Base;

namespace NbgDev.Pst.Events.Contract.Models;

public class ProjectRoleCreatedEvent : BaseEvent
{
    public required Guid RoleId { get; set; }
    public required Guid ProjectId { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
}

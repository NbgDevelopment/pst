using NbgDev.Pst.Events.Contract.Base;

namespace NbgDev.Pst.Events.Contract.Models;

public class ProjectRoleDeletedEvent : BaseEvent
{
    public required Guid RoleId { get; set; }
    public required Guid ProjectId { get; set; }
}

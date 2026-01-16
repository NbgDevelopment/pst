using NbgDev.Pst.Events.Contract.Base;

namespace NbgDev.Pst.Events.Contract.Models;

public class ProjectMemberRemovedFromRoleEvent : BaseEvent
{
    public required Guid RoleId { get; set; }
    public required string UserId { get; set; }
}

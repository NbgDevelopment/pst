using NbgDev.Pst.Events.Contract.Base;

namespace NbgDev.Pst.Events.Contract.Models;

public class ProjectMemberAddedToRoleEvent : BaseEvent
{
    public required Guid RoleId { get; set; }
    public required string UserId { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public string? Email { get; set; }
}

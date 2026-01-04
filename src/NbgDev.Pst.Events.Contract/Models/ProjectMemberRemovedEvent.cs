using NbgDev.Pst.Events.Contract.Base;

namespace NbgDev.Pst.Events.Contract.Models;

public class ProjectMemberRemovedEvent : BaseEvent
{
    public required Guid ProjectId { get; set; }
    public required string UserId { get; set; }
    public string? GroupId { get; set; }
}

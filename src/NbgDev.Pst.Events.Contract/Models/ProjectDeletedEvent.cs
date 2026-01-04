using NbgDev.Pst.Events.Contract.Base;

namespace NbgDev.Pst.Events.Contract.Models;

public class ProjectDeletedEvent : BaseEvent
{
    public required Guid ProjectId { get; set; }
    public string? GroupId { get; set; }
}

using NbgDev.Pst.Events.Contract.Base;

namespace NbgDev.Pst.Events.Contract.Models;

public class ProjectCreatedEvent : BaseEvent
{
    public required Guid ProjectId { get; set; }
    public required string ProjectName { get; set; }
    public required string ShortName { get; set; }
}

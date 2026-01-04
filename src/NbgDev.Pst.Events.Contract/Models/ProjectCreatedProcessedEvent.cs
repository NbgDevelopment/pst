using NbgDev.Pst.Events.Contract.Base;

namespace NbgDev.Pst.Events.Contract.Models;

public class ProjectCreatedProcessedEvent : BaseEvent
{
    public required Guid ProjectId { get; set; }
    public required bool Success { get; set; }
    public string? Message { get; set; }
    public string? GroupId { get; set; }
}

namespace NbgDev.Pst.Events.Contract.Models;

public class ProjectProcessedEvent
{
    public required Guid ProjectId { get; set; }
    public required bool Success { get; set; }
    public string? Message { get; set; }
}

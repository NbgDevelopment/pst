namespace NbgDev.Pst.Events.Contract.Models;

public class ProjectCreatedEvent
{
    public required Guid ProjectId { get; set; }
    public required string ProjectName { get; set; }
    public required string ShortName { get; set; }
}

namespace NbgDev.Pst.App.Models;

public class Role
{
    public required Guid Id { get; set; }

    public required Guid ProjectId { get; set; }

    public required string Name { get; set; }

    public required string Description { get; set; }
}

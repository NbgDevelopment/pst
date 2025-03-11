namespace NbgDev.Pst.Projects.Contract.Models;

public class Project
{
    public required Guid Id { get; set; }

    public required string Name { get; set; }

    public required string ShortName { get; set; }
}

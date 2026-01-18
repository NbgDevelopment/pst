namespace NbgDev.Pst.Projects.Contract.Models;

public class ProjectMember
{
    public required Guid ProjectId { get; set; }

    public required string UserId { get; set; }

    public required string FirstName { get; set; }

    public required string LastName { get; set; }

    public string? Email { get; set; }
}

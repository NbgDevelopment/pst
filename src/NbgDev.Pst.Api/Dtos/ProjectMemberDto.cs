namespace NbgDev.Pst.Api.Dtos;

public class ProjectMemberDto
{
    public required Guid ProjectId { get; set; }

    public required string UserId { get; set; }

    public required string FirstName { get; set; }

    public required string LastName { get; set; }

    public string? Email { get; set; }
}

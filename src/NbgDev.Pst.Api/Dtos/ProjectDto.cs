namespace NbgDev.Pst.Api.Dtos;

public class ProjectDto
{
    public required Guid Id { get; set; }

    public required string Name { get; set; }

    public required string ShortName { get; set; }

    public string? GroupId { get; set; }
}

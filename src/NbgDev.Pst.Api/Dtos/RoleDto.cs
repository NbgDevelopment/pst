namespace NbgDev.Pst.Api.Dtos;

public class RoleDto
{
    public required Guid Id { get; set; }

    public required Guid ProjectId { get; set; }

    public required string Name { get; set; }

    public string Description { get; set; } = string.Empty;
}

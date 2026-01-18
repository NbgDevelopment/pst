namespace NbgDev.Pst.Api.Dtos;

public class UpdateRoleDto
{
    public required string Name { get; set; }

    public string Description { get; set; } = string.Empty;
}

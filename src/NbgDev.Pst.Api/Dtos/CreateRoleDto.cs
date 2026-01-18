namespace NbgDev.Pst.Api.Dtos;

public class CreateRoleDto
{
    public required string Name { get; set; }

    public string Description { get; set; } = string.Empty;
}

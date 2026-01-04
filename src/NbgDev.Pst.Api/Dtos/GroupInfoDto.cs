namespace NbgDev.Pst.Api.Dtos;

public class GroupInfoDto
{
    public required string Id { get; set; }
    public required string Name { get; set; }
    public List<string>? AssignedRoleIds { get; set; }
}

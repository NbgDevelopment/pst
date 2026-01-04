namespace NbgDev.Pst.Projects.Contract.Models;

public class GroupInfo
{
    public required string Id { get; set; }
    public required string Name { get; set; }
    public List<string>? AssignedRoleIds { get; set; }
}

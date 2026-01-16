namespace NbgDev.Pst.App.Models;

public class RoleMember
{
    public required Guid RoleId { get; set; }

    public required string UserId { get; set; }

    public required string FirstName { get; set; }

    public required string LastName { get; set; }

    public required string Email { get; set; }
}

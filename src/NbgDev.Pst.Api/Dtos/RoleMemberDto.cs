namespace NbgDev.Pst.Api.Dtos;

public class RoleMemberDto
{
    public required Guid RoleId { get; set; }

    public required string UserId { get; set; }

    public required string FirstName { get; set; }

    public required string LastName { get; set; }

    public string? Email { get; set; }
}

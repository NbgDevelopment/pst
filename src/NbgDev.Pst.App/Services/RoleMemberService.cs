using NbgDev.Pst.Api.Client;
using NbgDev.Pst.App.Models;

namespace NbgDev.Pst.App.Services;

public class RoleMemberService(IProjectRoleMemberClient roleMemberClient) : IRoleMemberService
{
    public async Task<IReadOnlyList<RoleMember>> GetRoleMembers(Guid roleId)
    {
        var members = await roleMemberClient.GetMembersAsync(roleId);
        return members.Select(Map).ToList();
    }

    public async Task<RoleMember> AddRoleMember(Guid roleId, string userId, string firstName, string lastName, string? email)
    {
        var dto = new AddRoleMemberDto
        {
            UserId = userId,
            FirstName = firstName,
            LastName = lastName,
            Email = email
        };

        var result = await roleMemberClient.AddMemberAsync(roleId, dto);
        return Map(result);
    }

    public async Task RemoveRoleMember(Guid roleId, string userId)
    {
        await roleMemberClient.RemoveMemberAsync(roleId, userId);
    }

    private static RoleMember Map(RoleMemberDto dto)
    {
        return new RoleMember
        {
            RoleId = dto.RoleId,
            UserId = dto.UserId,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email
        };
    }
}

using NbgDev.Pst.App.Models;

namespace NbgDev.Pst.App.Services;

public interface IRoleMemberService
{
    Task<IReadOnlyList<RoleMember>> GetRoleMembers(Guid roleId);

    Task<RoleMember> AddRoleMember(Guid roleId, string userId, string firstName, string lastName, string email);

    Task RemoveRoleMember(Guid roleId, string userId);
}

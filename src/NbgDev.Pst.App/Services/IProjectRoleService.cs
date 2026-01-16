using NbgDev.Pst.App.Models;

namespace NbgDev.Pst.App.Services;

public interface IProjectRoleService
{
    Task<IReadOnlyList<Role>> GetRoles(Guid projectId);

    Task<Role> CreateRole(Guid projectId, string name, string description);

    Task DeleteRole(Guid projectId, Guid roleId);
}

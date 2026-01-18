using NbgDev.Pst.Api.Client;
using NbgDev.Pst.App.Models;

namespace NbgDev.Pst.App.Services;

public class ProjectRoleService(IProjectRoleClient projectRoleClient) : IProjectRoleService
{
    public async Task<IReadOnlyList<Role>> GetRoles(Guid projectId)
    {
        var roles = await projectRoleClient.GetRolesAsync(projectId);
        return roles.Select(Map).ToList();
    }

    public async Task<Role> CreateRole(Guid projectId, string name, string description)
    {
        var dto = new CreateRoleDto
        {
            Name = name,
            Description = description
        };

        var result = await projectRoleClient.CreateRoleAsync(projectId, dto);
        return Map(result);
    }

    public async Task<Role?> UpdateRole(Guid projectId, Guid roleId, string name, string description)
    {
        var dto = new UpdateRoleDto
        {
            Name = name,
            Description = description
        };

        try
        {
            var result = await projectRoleClient.UpdateRoleAsync(projectId, roleId, dto);
            return Map(result);
        }
        catch (PstApiException ex) when (ex.StatusCode == 404)
        {
            return null;
        }
    }

    public async Task DeleteRole(Guid projectId, Guid roleId)
    {
        await projectRoleClient.DeleteRoleAsync(projectId, roleId);
    }

    private static Role Map(RoleDto dto)
    {
        return new Role
        {
            Id = dto.Id,
            ProjectId = dto.ProjectId,
            Name = dto.Name,
            Description = dto.Description
        };
    }
}

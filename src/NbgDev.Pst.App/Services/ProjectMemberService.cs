using NbgDev.Pst.Api.Client;
using NbgDev.Pst.App.Models;

namespace NbgDev.Pst.App.Services;

public class ProjectMemberService(IProjectMemberClient projectMemberClient) : IProjectMemberService
{
    public async Task<IReadOnlyList<ProjectMember>> GetProjectMembers(Guid projectId)
    {
        var members = await projectMemberClient.GetMembersAsync(projectId);
        return members.Select(Map).ToList();
    }

    public async Task<ProjectMember> AddProjectMember(Guid projectId, string userId, string firstName, string lastName, string email)
    {
        var dto = new AddProjectMemberDto
        {
            UserId = userId,
            FirstName = firstName,
            LastName = lastName,
            Email = email
        };

        var result = await projectMemberClient.AddMemberAsync(projectId, dto);
        return Map(result);
    }

    public async Task RemoveProjectMember(Guid projectId, string userId)
    {
        await projectMemberClient.RemoveMemberAsync(projectId, userId);
    }

    private static ProjectMember Map(ProjectMemberDto dto)
    {
        return new ProjectMember
        {
            ProjectId = dto.ProjectId,
            UserId = dto.UserId,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email
        };
    }
}

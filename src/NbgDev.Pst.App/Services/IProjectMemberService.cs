using NbgDev.Pst.App.Models;

namespace NbgDev.Pst.App.Services;

public interface IProjectMemberService
{
    Task<IReadOnlyList<ProjectMember>> GetProjectMembers(Guid projectId);

    Task<ProjectMember> AddProjectMember(Guid projectId, string userId, string firstName, string lastName, string email);

    Task RemoveProjectMember(Guid projectId, string userId);
}

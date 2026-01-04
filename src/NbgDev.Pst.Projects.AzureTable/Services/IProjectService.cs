using NbgDev.Pst.Projects.Contract.Models;

namespace NbgDev.Pst.Projects.AzureTable.Services;

internal interface IProjectService
{
    Task<IReadOnlyList<Project>> GetProjects();

    Task<Project?> GetProject(Guid id);

    Task<Project> CreateProject(string name, string shortName);

    Task UpdateProjectGroup(Guid projectId, GroupInfo group);

    Task<bool> DeleteProject(Guid projectId);

    Task<IReadOnlyList<ProjectMember>> GetProjectMembers(Guid projectId);

    Task<ProjectMember> AddProjectMember(Guid projectId, string userId, string firstName, string lastName, string email);

    Task<bool> RemoveProjectMember(Guid projectId, string userId);
}

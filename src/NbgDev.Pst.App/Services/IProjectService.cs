using NbgDev.Pst.App.Models;

namespace NbgDev.Pst.App.Services;

public interface IProjectService
{
    event Action<Project>? ProjectCreated;

    Task<IReadOnlyList<Project>> GetProjects();

    Task<Project?> GetProject(Guid id);

    Task<Project> CreateProject(ProjectToCreate project);

    Task DeleteProject(Guid id);
}

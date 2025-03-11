using NbgDev.Pst.Projects.Contract.Models;

namespace NbgDev.Pst.Projects.AzureTable.Services;

internal interface IProjectService
{
    Task<IReadOnlyList<Project>> GetProjects();

    Task<Project?> GetProject(Guid id);

    Task<Project> CreateProject(string name);
}

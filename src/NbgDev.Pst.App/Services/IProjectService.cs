using NbgDev.Pst.App.Models;

namespace NbgDev.Pst.App.Services;

public interface IProjectService
{
    IReadOnlyList<Project> GetProjects();
}

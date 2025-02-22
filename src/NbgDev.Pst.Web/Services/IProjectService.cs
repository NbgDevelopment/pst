using NbgDev.Pst.Web.Models;

namespace NbgDev.Pst.Web.Services;

public interface IProjectService
{
    IReadOnlyList<Project> GetProjects();
}

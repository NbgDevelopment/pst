using NbgDev.Pst.App.Models;
using NbgDev.Pst.App.Services;

namespace NbgDev.Pst.App.Layout;

public partial class MyNavMenu(IProjectService projectService)
{
    private IReadOnlyList<Project> Projects => projectService.GetProjects();
}

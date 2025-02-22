using NbgDev.Pst.Web.Models;
using NbgDev.Pst.Web.Services;

namespace NbgDev.Pst.Web.Layout;

public partial class MyNavMenu(IProjectService projectService)
{
    private IReadOnlyList<Project> Projects => projectService.GetProjects();
}

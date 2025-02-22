using Microsoft.AspNetCore.Components;
using NbgDev.Pst.Web.Models;
using NbgDev.Pst.Web.Services;

namespace NbgDev.Pst.Web.Pages;

public partial class ProjectPage (IProjectService projectService, NavigationManager navigation)
{
    [Parameter]
    public required Guid Id { get; set; }

    private Project? Project { get; set; }

    protected override Task OnParametersSetAsync()
    {
        Project = projectService.GetProjects().FirstOrDefault(p => p.Id == Id);
        return Task.CompletedTask;
    }

    private void GoHome()
    {
        navigation.NavigateTo("/");
    }
}

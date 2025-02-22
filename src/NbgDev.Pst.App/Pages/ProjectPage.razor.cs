using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using NbgDev.Pst.App.Models;
using NbgDev.Pst.App.Services;

namespace NbgDev.Pst.App.Pages;

[Authorize]
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

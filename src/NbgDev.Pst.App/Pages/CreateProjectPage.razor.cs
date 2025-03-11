using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using NbgDev.Pst.App.Models;
using NbgDev.Pst.App.Services;

namespace NbgDev.Pst.App.Pages;

partial class CreateProjectPage(IProjectService projectService, NavigationManager navigation)
{
    private readonly ProjectToCreate _project = new();

    private async Task OnValidSubmit(EditContext context)
    {
        var project = await projectService.CreateProject(_project);

        navigation.NavigateTo($"/project/{project.Id}");
    }
}

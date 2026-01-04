using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using NbgDev.Pst.App.Models;
using NbgDev.Pst.App.Services;

namespace NbgDev.Pst.App.Pages;

[Authorize]
public partial class ProjectPage (IProjectService projectService, NavigationManager navigation, ISnackbar snackbar)
{
    [Parameter]
    public required Guid Id { get; set; }

    private Project? Project { get; set; }
    private bool deleteDialogVisible = false;
    private bool isDeleting = false;
    private DialogOptions dialogOptions = new() { CloseOnEscapeKey = true };

    protected override async Task OnParametersSetAsync()
    {
        Project = await projectService.GetProject(Id);
    }

    private void GoHome()
    {
        navigation.NavigateTo("/");
    }

    private void OpenDeleteDialog()
    {
        deleteDialogVisible = true;
    }

    private void CloseDeleteDialog()
    {
        deleteDialogVisible = false;
    }

    private async Task DeleteProject()
    {
        if (Project == null) return;

        isDeleting = true;
        try
        {
            await projectService.DeleteProject(Id);
            deleteDialogVisible = false;
            snackbar.Add($"Project '{Project.Name}' deleted successfully", Severity.Success);
            navigation.NavigateTo("/");
        }
        catch (Exception ex)
        {
            snackbar.Add($"Failed to delete project: {ex.Message}", Severity.Error);
            isDeleting = false;
        }
    }
}

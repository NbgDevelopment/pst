using Microsoft.AspNetCore.Components;
using MudBlazor;
using NbgDev.Pst.App.Models;
using NbgDev.Pst.App.Services;

namespace NbgDev.Pst.App.Components;

public partial class SettingsPanel
{
    [Parameter]
    public required Guid ProjectId { get; set; }

    [Parameter]
    public Project? Project { get; set; }

    [Inject]
    private IProjectService ProjectService { get; set; } = default!;

    [Inject]
    private NavigationManager Navigation { get; set; } = default!;

    [Inject]
    private ISnackbar Snackbar { get; set; } = default!;

    private bool _deleteDialogVisible = false;
    private bool _isDeleting = false;
    private DialogOptions _dialogOptions = new() { CloseOnEscapeKey = true };

    private void OpenDeleteDialog()
    {
        _deleteDialogVisible = true;
    }

    private void CloseDeleteDialog()
    {
        _deleteDialogVisible = false;
    }

    private async Task DeleteProject()
    {
        if (Project == null) return;

        _isDeleting = true;
        try
        {
            await ProjectService.DeleteProject(ProjectId);
            _deleteDialogVisible = false;
            Snackbar.Add($"Project '{Project.Name}' deleted successfully", Severity.Success);
            Navigation.NavigateTo("/");
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Failed to delete project: {ex.Message}", Severity.Error);
            _isDeleting = false;
        }
    }
}

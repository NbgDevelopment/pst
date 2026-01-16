using Microsoft.AspNetCore.Components;
using MudBlazor;
using NbgDev.Pst.App.Services;

namespace NbgDev.Pst.App.Components;

public partial class CreateRoleDialog
{
    [Parameter]
    public required Guid ProjectId { get; set; }

    [Inject]
    private IProjectRoleService ProjectRoleService { get; set; } = default!;

    [Inject]
    private ISnackbar Snackbar { get; set; } = default!;

    private string _roleName = string.Empty;

    [CascadingParameter]
    public IMudDialogInstance? DialogInstance { get; set; }

    private void Cancel()
    {
        DialogInstance?.Close();
    }

    private async Task CreateRole()
    {
        if (string.IsNullOrWhiteSpace(_roleName))
        {
            return;
        }

        try
        {
            await ProjectRoleService.CreateRole(ProjectId, _roleName);
            DialogInstance?.Close(DialogResult.Ok(true));
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Failed to create role: {ex.Message}", Severity.Error);
        }
    }
}

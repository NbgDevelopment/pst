using Microsoft.AspNetCore.Components;
using MudBlazor;
using NbgDev.Pst.App.Models;
using NbgDev.Pst.App.Services;

namespace NbgDev.Pst.App.Components;

public partial class PermissionsPanel
{
    [Parameter]
    public Project? Project { get; set; }

    [Inject]
    private IProjectRoleService ProjectRoleService { get; set; } = default!;

    [Inject]
    private IDialogService DialogService { get; set; } = default!;

    [Inject]
    private ISnackbar Snackbar { get; set; } = default!;

    private List<Role> _roles = new();
    private bool _isLoadingRoles = true;

    protected override async Task OnParametersSetAsync()
    {
        if (Project != null)
        {
            await LoadRoles();
        }
    }

    private async Task LoadRoles()
    {
        _isLoadingRoles = true;
        try
        {
            if (Project != null)
            {
                var roles = await ProjectRoleService.GetRoles(Project.Id);
                _roles = roles.ToList();
            }
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Failed to load roles: {ex.Message}", Severity.Error);
        }
        finally
        {
            _isLoadingRoles = false;
        }
    }

    private async Task OpenCreateRoleDialog()
    {
        var parameters = new DialogParameters<CreateRoleDialog>
        {
            { x => x.ProjectId, Project!.Id }
        };

        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true };
        var dialog = await DialogService.ShowAsync<CreateRoleDialog>("Create Role", parameters, options);
        var result = await dialog.Result;

        if (result != null && !result.Canceled)
        {
            await LoadRoles();
            Snackbar.Add("Role created successfully", Severity.Success);
        }
    }

    private async Task OpenRoleMembersDialog(Role role)
    {
        var parameters = new DialogParameters<RoleMembersDialog>
        {
            { x => x.Role, role }
        };

        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true };
        await DialogService.ShowAsync<RoleMembersDialog>($"Manage Members - {role.Name}", parameters, options);
    }

    private async Task ConfirmDeleteRole(Role role)
    {
        var result = await DialogService.ShowMessageBox(
            "Confirm Deletion",
            $"Are you sure you want to delete the role '{role.Name}'? This will also remove all members from this role.",
            yesText: "Delete",
            cancelText: "Cancel");

        if (result == true)
        {
            await DeleteRole(role);
        }
    }

    private async Task DeleteRole(Role role)
    {
        try
        {
            await ProjectRoleService.DeleteRole(role.ProjectId, role.Id);
            _roles.Remove(role);
            Snackbar.Add("Role deleted successfully", Severity.Success);
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Failed to delete role: {ex.Message}", Severity.Error);
        }
    }
}

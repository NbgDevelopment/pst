using Microsoft.AspNetCore.Components;
using MudBlazor;
using NbgDev.Pst.App.Models;
using NbgDev.Pst.App.Services;

namespace NbgDev.Pst.App.Components;

public partial class ManageRoleDialog
{
    [Parameter]
    public required Role Role { get; set; }

    [Inject]
    private IRoleMemberService RoleMemberService { get; set; } = default!;

    [Inject]
    private IProjectRoleService ProjectRoleService { get; set; } = default!;

    [Inject]
    private IDialogService DialogService { get; set; } = default!;

    [Inject]
    private ISnackbar Snackbar { get; set; } = default!;

    private List<RoleMember> _members = new();
    private bool _isLoading = true;
    private string _roleName = string.Empty;
    private string _roleDescription = string.Empty;

    [CascadingParameter]
    public IMudDialogInstance? DialogInstance { get; set; }

    protected override async Task OnParametersSetAsync()
    {
        _roleName = Role.Name;
        _roleDescription = Role.Description;
        await LoadMembers();
    }

    private async Task LoadMembers()
    {
        _isLoading = true;
        try
        {
            var members = await RoleMemberService.GetRoleMembers(Role.Id);
            _members = members.ToList();
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Failed to load members: {ex.Message}", Severity.Error);
        }
        finally
        {
            _isLoading = false;
        }
    }

    private async Task UpdateRole()
    {
        if (string.IsNullOrWhiteSpace(_roleName) || string.IsNullOrWhiteSpace(_roleDescription))
        {
            return;
        }

        try
        {
            var updatedRole = await ProjectRoleService.UpdateRole(Role.ProjectId, Role.Id, _roleName, _roleDescription);
            if (updatedRole != null)
            {
                Role.Name = updatedRole.Name;
                Role.Description = updatedRole.Description;
                Snackbar.Add("Role updated successfully", Severity.Success);
                StateHasChanged();
            }
            else
            {
                Snackbar.Add("Failed to update role: Role not found", Severity.Error);
            }
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Failed to update role: {ex.Message}", Severity.Error);
        }
    }

    private async Task OpenAddMemberDialog()
    {
        var parameters = new DialogParameters<AddRoleMemberDialog>
        {
            { x => x.RoleId, Role.Id },
            { x => x.ProjectId, Role.ProjectId }
        };

        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true };
        var dialog = await DialogService.ShowAsync<AddRoleMemberDialog>("Add Role Member", parameters, options);
        var result = await dialog.Result;

        if (result != null && !result.Canceled)
        {
            await LoadMembers();
            Snackbar.Add("Member added to role successfully", Severity.Success);
        }
    }

    private async Task ConfirmRemoveMember(RoleMember member)
    {
        var result = await DialogService.ShowMessageBox(
            "Confirm Removal",
            $"Are you sure you want to remove {member.FirstName} {member.LastName} from this role?",
            yesText: "Remove",
            cancelText: "Cancel");

        if (result == true)
        {
            await RemoveMember(member);
        }
    }

    private async Task RemoveMember(RoleMember member)
    {
        try
        {
            await RoleMemberService.RemoveRoleMember(Role.Id, member.UserId);
            _members.Remove(member);
            Snackbar.Add("Member removed from role successfully", Severity.Success);
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Failed to remove member: {ex.Message}", Severity.Error);
        }
    }

    private void Close()
    {
        DialogInstance?.Close(DialogResult.Ok(true));
    }
}

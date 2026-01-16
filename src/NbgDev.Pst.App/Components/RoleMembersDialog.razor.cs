using Microsoft.AspNetCore.Components;
using MudBlazor;
using NbgDev.Pst.App.Models;
using NbgDev.Pst.App.Services;

namespace NbgDev.Pst.App.Components;

public partial class RoleMembersDialog
{
    [Parameter]
    public required Role Role { get; set; }

    [Inject]
    private IRoleMemberService RoleMemberService { get; set; } = default!;

    [Inject]
    private IDialogService DialogService { get; set; } = default!;

    [Inject]
    private ISnackbar Snackbar { get; set; } = default!;

    private List<RoleMember> _members = new();
    private bool _isLoading = true;

    [CascadingParameter]
    public IMudDialogInstance? DialogInstance { get; set; }

    protected override async Task OnParametersSetAsync()
    {
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

    private async Task OpenAddMemberDialog()
    {
        var parameters = new DialogParameters<AddRoleMemberDialog>
        {
            { x => x.RoleId, Role.Id }
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
        DialogInstance?.Close();
    }
}

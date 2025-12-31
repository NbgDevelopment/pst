using Microsoft.AspNetCore.Components;
using MudBlazor;
using NbgDev.Pst.App.Models;
using NbgDev.Pst.App.Services;

namespace NbgDev.Pst.App.Components;

public partial class MembersPanel
{
    [Parameter]
    public required Guid ProjectId { get; set; }

    [Inject]
    private IProjectMemberService ProjectMemberService { get; set; } = default!;

    [Inject]
    private IDialogService DialogService { get; set; } = default!;

    [Inject]
    private ISnackbar Snackbar { get; set; } = default!;

    private List<ProjectMember> _members = new();
    private bool _isLoading = true;
    private AddMemberDialog? _addMemberDialog;

    protected override async Task OnParametersSetAsync()
    {
        await LoadMembers();
    }

    private async Task LoadMembers()
    {
        _isLoading = true;
        try
        {
            var members = await ProjectMemberService.GetProjectMembers(ProjectId);
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

    private void OpenAddMemberDialog()
    {
        _addMemberDialog?.Open();
    }

    private async Task HandleMemberAdded()
    {
        await LoadMembers();
        Snackbar.Add("Member added successfully", Severity.Success);
    }

    private async Task ConfirmRemoveMember(ProjectMember member)
    {
        var result = await DialogService.ShowMessageBox(
            "Confirm Removal",
            $"Are you sure you want to remove {member.FirstName} {member.LastName} from this project?",
            yesText: "Remove",
            cancelText: "Cancel");

        if (result == true)
        {
            await RemoveMember(member);
        }
    }

    private async Task RemoveMember(ProjectMember member)
    {
        try
        {
            await ProjectMemberService.RemoveProjectMember(ProjectId, member.UserId);
            _members.Remove(member);
            Snackbar.Add("Member removed successfully", Severity.Success);
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Failed to remove member: {ex.Message}", Severity.Error);
        }
    }
}

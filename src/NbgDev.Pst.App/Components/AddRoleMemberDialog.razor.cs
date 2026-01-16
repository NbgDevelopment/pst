using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using NbgDev.Pst.App.Models;
using NbgDev.Pst.App.Services;

namespace NbgDev.Pst.App.Components;

public partial class AddRoleMemberDialog
{
    [Parameter]
    public required Guid RoleId { get; set; }

    [Parameter]
    public required Guid ProjectId { get; set; }

    [Inject]
    private IProjectMemberService ProjectMemberService { get; set; } = default!;

    [Inject]
    private IRoleMemberService RoleMemberService { get; set; } = default!;

    [Inject]
    private ISnackbar Snackbar { get; set; } = default!;

    private string _searchTerm = string.Empty;
    private List<ProjectMember> _projectMembers = new();
    private List<ProjectMember> _filteredMembers = new();
    private bool _isLoading = true;

    [CascadingParameter]
    public IMudDialogInstance? DialogInstance { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await LoadProjectMembers();
    }

    private async Task LoadProjectMembers()
    {
        _isLoading = true;
        try
        {
            var members = await ProjectMemberService.GetProjectMembers(ProjectId);
            _projectMembers = members.ToList();
            _filteredMembers = _projectMembers;
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Failed to load project members: {ex.Message}", Severity.Error);
        }
        finally
        {
            _isLoading = false;
        }
    }

    private void Cancel()
    {
        DialogInstance?.Close();
    }

    private Task HandleKeyDown(KeyboardEventArgs e)
    {
        if (e.Key == "Enter")
        {
            FilterMembers();
        }
        return Task.CompletedTask;
    }

    private void FilterMembers()
    {
        if (string.IsNullOrWhiteSpace(_searchTerm))
        {
            _filteredMembers = _projectMembers;
        }
        else
        {
            var searchLower = _searchTerm.ToLowerInvariant();
            _filteredMembers = _projectMembers
                .Where(m => 
                    m.FirstName.ToLowerInvariant().Contains(searchLower) ||
                    m.LastName.ToLowerInvariant().Contains(searchLower) ||
                    (m.Email?.ToLowerInvariant().Contains(searchLower) ?? false))
                .ToList();
        }
    }

    private async Task SelectMember(ProjectMember member)
    {
        try
        {
            await RoleMemberService.AddRoleMember(
                RoleId,
                member.UserId,
                member.FirstName,
                member.LastName,
                member.Email
            );

            DialogInstance?.Close(DialogResult.Ok(true));
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Failed to add member to role: {ex.Message}", Severity.Error);
        }
    }

    private static string GetInitials(ProjectMember member)
    {
        var firstInitial = !string.IsNullOrEmpty(member.FirstName) ? member.FirstName[0] : '?';
        var lastInitial = !string.IsNullOrEmpty(member.LastName) ? member.LastName[0] : '?';
        return $"{firstInitial}{lastInitial}";
    }
}

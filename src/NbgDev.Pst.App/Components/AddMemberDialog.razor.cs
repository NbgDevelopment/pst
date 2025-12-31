using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using NbgDev.Pst.App.Models;
using NbgDev.Pst.App.Services;

namespace NbgDev.Pst.App.Components;

public partial class AddMemberDialog
{
    [Parameter]
    public required Guid ProjectId { get; set; }

    [Parameter]
    public EventCallback OnMemberAdded { get; set; }

    [Inject]
    private IEntraIdService EntraIdService { get; set; } = default!;

    [Inject]
    private IProjectMemberService ProjectMemberService { get; set; } = default!;

    [Inject]
    private ISnackbar Snackbar { get; set; } = default!;

    private bool _isVisible = false;
    private string _searchTerm = string.Empty;
    private List<EntraIdUser> _searchResults = new();
    private bool _isSearching = false;

    public void Open()
    {
        _isVisible = true;
        _searchTerm = string.Empty;
        _searchResults.Clear();
        StateHasChanged();
    }

    private void Cancel()
    {
        _isVisible = false;
        _searchTerm = string.Empty;
        _searchResults.Clear();
    }

    private async Task HandleKeyDown(KeyboardEventArgs e)
    {
        if (e.Key == "Enter")
        {
            await SearchUsers();
        }
    }

    private async Task SearchUsers()
    {
        if (string.IsNullOrWhiteSpace(_searchTerm))
        {
            return;
        }

        _isSearching = true;
        try
        {
            var users = await EntraIdService.SearchUsers(_searchTerm);
            _searchResults = users.ToList();
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Failed to search users: {ex.Message}", Severity.Error);
        }
        finally
        {
            _isSearching = false;
        }
    }

    private async Task SelectUser(EntraIdUser user)
    {
        try
        {
            await ProjectMemberService.AddProjectMember(
                ProjectId,
                user.Id,
                user.GivenName,
                user.Surname,
                user.Mail
            );

            _isVisible = false;
            _searchTerm = string.Empty;
            _searchResults.Clear();

            await OnMemberAdded.InvokeAsync();
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Failed to add member: {ex.Message}", Severity.Error);
        }
    }

    private static string GetInitials(EntraIdUser user)
    {
        var firstInitial = !string.IsNullOrEmpty(user.GivenName) ? user.GivenName[0] : '?';
        var lastInitial = !string.IsNullOrEmpty(user.Surname) ? user.Surname[0] : '?';
        return $"{firstInitial}{lastInitial}";
    }
}

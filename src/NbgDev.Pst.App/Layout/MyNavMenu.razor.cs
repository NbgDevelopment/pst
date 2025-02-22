using Microsoft.AspNetCore.Components.Authorization;
using NbgDev.Pst.App.Models;
using NbgDev.Pst.App.Services;

namespace NbgDev.Pst.App.Layout;

public partial class MyNavMenu : IDisposable
{
    private readonly IProjectService _projectService;
    private readonly AuthenticationStateProvider _authenticationStateProvider;

    public MyNavMenu(IProjectService projectService, AuthenticationStateProvider authenticationStateProvider)
    {
        _projectService = projectService;
        _authenticationStateProvider = authenticationStateProvider;
        _authenticationStateProvider.AuthenticationStateChanged += OnAuthenticationStateChanged;
    }

    private async void OnAuthenticationStateChanged(Task<AuthenticationState> task)
    {
        var authenticationState = await task;
        await LoadProjects(authenticationState.User.Identity?.IsAuthenticated == true);
    }

    private IReadOnlyList<Project> Projects { get; set; } = [];

    protected override async Task OnParametersSetAsync()
    {
        var authenticationState = await _authenticationStateProvider.GetAuthenticationStateAsync();
        await LoadProjects(authenticationState.User.Identity?.IsAuthenticated == true);
    }

    private async Task LoadProjects(bool isAuthenticated)
    {
        Projects = isAuthenticated
            ? await _projectService.GetProjects()
            : [];
    }

    public void Dispose()
    {
        _authenticationStateProvider.AuthenticationStateChanged -= OnAuthenticationStateChanged;
    }
}

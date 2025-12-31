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
        _projectService.ProjectCreated += OnProjectCreated;
        _authenticationStateProvider = authenticationStateProvider;
        _authenticationStateProvider.AuthenticationStateChanged += OnAuthenticationStateChanged;
    }

    private async void OnAuthenticationStateChanged(Task<AuthenticationState> task)
    {
        var authenticationState = await task;
        await LoadProjects(authenticationState.User.Identity?.IsAuthenticated == true);
    }

    private List<Project> Projects { get; set; } = [];
    private bool IsLoading { get; set; }

    protected override async Task OnParametersSetAsync()
    {
        var authenticationState = await _authenticationStateProvider.GetAuthenticationStateAsync();
        await LoadProjects(authenticationState.User.Identity?.IsAuthenticated == true);
    }

    private async Task LoadProjects(bool isAuthenticated)
    {
        IsLoading = true;
        StateHasChanged();

        try
        {
            Projects.Clear();

            if (isAuthenticated)
            {
                Projects.AddRange(await _projectService.GetProjects());
            }
        }
        finally
        {
            IsLoading = false;
            StateHasChanged();
        }
    }

    private void OnProjectCreated(Project project)
    {
        Projects.Add(project);
        StateHasChanged();
    }

    public void Dispose()
    {
        _projectService.ProjectCreated -= OnProjectCreated;
        _authenticationStateProvider.AuthenticationStateChanged -= OnAuthenticationStateChanged;
    }
}

using NbgDev.Pst.Api.Client;
using NbgDev.Pst.App.Models;

namespace NbgDev.Pst.App.Services;

public class ProjectService(IProjectClient projectClient) : IProjectService
{
    public async Task<IReadOnlyList<Project>> GetProjects()
    {
        var projects = await projectClient.GetAsync();

        return projects.Select(Map).ToList();
    }

    private Project Map(ProjectDto source)
    {
        return new()
        {
            Id = source.Id,
            Name = source.Name
        };
    }
}

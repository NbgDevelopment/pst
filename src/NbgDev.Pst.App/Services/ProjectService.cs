using System.Net;
using NbgDev.Pst.Api.Client;
using NbgDev.Pst.App.Models;

namespace NbgDev.Pst.App.Services;

public class ProjectService(IProjectClient projectClient) : IProjectService
{
    public event Action<Project>? ProjectCreated;

    public async Task<IReadOnlyList<Project>> GetProjects()
    {
        var projects = await projectClient.GetAllAsync();

        return projects.Select(Map).ToList();
    }

    public async Task<Project?> GetProject(Guid id)
    {
        try
        {
            var projectDto = await projectClient.GetAsync(id);

            return Map(projectDto);
        }
        catch (PstApiException ex) when(ex.StatusCode == (int)HttpStatusCode.NotFound)
        {
            return null;
        }
    }

    public async Task<Project> CreateProject(ProjectToCreate project)
    {
        var created = Map(await projectClient.CreateAsync(new CreateProjectRequest
        {
            Name = project.Name,
            ShortName = project.ShortName
        }));

        ProjectCreated?.Invoke(created);

        return created;
    }

    private Project Map(ProjectDto source)
    {
        GroupInfo? group = null;
        if (source.Group != null)
        {
            group = new GroupInfo
            {
                Id = source.Group.Id,
                Name = source.Group.Name
            };
        }

        return new()
        {
            Id = source.Id,
            Name = source.Name,
            ShortName = source.ShortName,
            Group = group
        };
    }
}

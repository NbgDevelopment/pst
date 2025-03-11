using Azure.Data.Tables;
using NbgDev.Pst.Projects.AzureTable.Entities;
using NbgDev.Pst.Projects.Contract.Models;

namespace NbgDev.Pst.Projects.AzureTable.Services;

internal class ProjectService(TableServiceClient tableServiceClient) : IProjectService
{
    public async Task<IReadOnlyList<Project>> GetProjects()
    {
        var tableClient = await GetTableClient();

        return tableClient.Query<ProjectEntity>()
            .Select(Map)
            .ToArray();
    }

    public async Task<Project?> GetProject(Guid id)
    {
        var tableClient = await GetTableClient();

        var project = await tableClient.GetEntityAsync<ProjectEntity>(ProjectEntity.EntityPartitionKey, id.ToString());

        return project.HasValue ? Map(project.Value) : null;
    }

    public async Task<Project> CreateProject(string name, string shortName)
    {
        var project = new ProjectEntity()
        {
            Id = Guid.NewGuid(),
            Name = name,
            ShortName = shortName
        };

        var tableClient = await GetTableClient();

        await tableClient.AddEntityAsync(project);

        return Map(project);
    }

    private async Task<TableClient> GetTableClient()
    {
        var table = await tableServiceClient.CreateTableIfNotExistsAsync("Projects");

        var tableClient = tableServiceClient.GetTableClient(table.Value.Name);
        return tableClient;
    }

    private static Project Map(ProjectEntity project)
    {
        return new Project
        {
            Id = project.Id,
            Name = project.Name,
            ShortName = project.ShortName
        };
    }
}

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
            .Select(p => new Project
            {
                Id = p.Id,
                Name = p.Name
            })
            .ToArray();
    }

    public async Task<Project?> GetProject(Guid id)
    {
        var tableClient = await GetTableClient();

        var project = await tableClient.GetEntityAsync<ProjectEntity>(ProjectEntity.EntityPartitionKey, id.ToString());

        return project.HasValue ? new Project
        {
            Id = project.Value.Id,
            Name = project.Value.Name
        } : null;
    }

    public async Task<Project> CreateProject(string name)
    {
        var project = new ProjectEntity()
        {
            Id = Guid.NewGuid(),
            Name = name
        };

        var tableClient = await GetTableClient();

        await tableClient.AddEntityAsync(project);

        return new Project
        {
            Id = project.Id,
            Name = project.Name
        };
    }

    private async Task<TableClient> GetTableClient()
    {
        var table = await tableServiceClient.CreateTableIfNotExistsAsync("Projects");

        var tableClient = tableServiceClient.GetTableClient(table.Value.Name);
        return tableClient;
    }
}

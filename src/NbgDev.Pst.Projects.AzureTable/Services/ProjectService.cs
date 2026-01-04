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

    public async Task UpdateProjectGroupId(Guid projectId, string groupId)
    {
        var tableClient = await GetTableClient();

        var project = await tableClient.GetEntityAsync<ProjectEntity>(ProjectEntity.EntityPartitionKey, projectId.ToString());
        
        if (!project.HasValue)
        {
            throw new InvalidOperationException($"Project {projectId} not found");
        }

        var projectEntity = project.Value;
        projectEntity.GroupId = groupId;

        await tableClient.UpdateEntityAsync(projectEntity, projectEntity.ETag);
    }

    public async Task<bool> DeleteProject(Guid projectId)
    {
        var tableClient = await GetTableClient();

        try
        {
            // Delete the project
            await tableClient.DeleteEntityAsync(ProjectEntity.EntityPartitionKey, projectId.ToString());

            // Delete all members of the project
            var partitionKey = ProjectMemberEntity.EntityPartitionKeyPrefix + projectId;
            var members = tableClient.Query<ProjectMemberEntity>(e => e.PartitionKey == partitionKey);

            foreach (var member in members)
            {
                await tableClient.DeleteEntityAsync(partitionKey, member.UserId);
            }

            return true;
        }
        catch (Azure.RequestFailedException ex) when (ex.Status == 404)
        {
            // Entity doesn't exist
            return false;
        }
    }

    public async Task<IReadOnlyList<ProjectMember>> GetProjectMembers(Guid projectId)
    {
        var tableClient = await GetTableClient();

        var partitionKey = ProjectMemberEntity.EntityPartitionKeyPrefix + projectId;
        var members = tableClient.Query<ProjectMemberEntity>(e => e.PartitionKey == partitionKey);

        return members.Select(MapMember).ToArray();
    }

    public async Task<ProjectMember> AddProjectMember(Guid projectId, string userId, string firstName, string lastName, string email)
    {
        var member = new ProjectMemberEntity
        {
            ProjectId = projectId,
            UserId = userId,
            FirstName = firstName,
            LastName = lastName,
            Email = email
        };

        var tableClient = await GetTableClient();
        await tableClient.UpsertEntityAsync(member);

        return MapMember(member);
    }

    public async Task<bool> RemoveProjectMember(Guid projectId, string userId)
    {
        var tableClient = await GetTableClient();
        var partitionKey = ProjectMemberEntity.EntityPartitionKeyPrefix + projectId;

        try
        {
            await tableClient.DeleteEntityAsync(partitionKey, userId);
            return true;
        }
        catch (Azure.RequestFailedException ex) when (ex.Status == 404)
        {
            // Entity doesn't exist
            return false;
        }
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
            ShortName = project.ShortName,
            GroupId = project.GroupId
        };
    }

    private static ProjectMember MapMember(ProjectMemberEntity member)
    {
        return new ProjectMember
        {
            ProjectId = member.ProjectId,
            UserId = member.UserId,
            FirstName = member.FirstName,
            LastName = member.LastName,
            Email = member.Email
        };
    }
}

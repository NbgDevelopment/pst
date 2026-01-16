using Azure.Data.Tables;
using NbgDev.Pst.Projects.AzureTable.Entities;
using NbgDev.Pst.Projects.Contract.Models;
using System.Text.Json;

namespace NbgDev.Pst.Projects.AzureTable.Services;

internal class ProjectService(TableServiceClient tableServiceClient) : IProjectService
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        WriteIndented = false
    };

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

    public async Task UpdateProjectGroup(Guid projectId, GroupInfo group)
    {
        var tableClient = await GetTableClient();

        var project = await tableClient.GetEntityAsync<ProjectEntity>(ProjectEntity.EntityPartitionKey, projectId.ToString());
        
        if (!project.HasValue)
        {
            throw new InvalidOperationException($"Failed to update Group: Project {projectId} not found");
        }

        var projectEntity = project.Value;
        projectEntity.GroupJson = JsonSerializer.Serialize(group, JsonOptions);

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
            var partitionKey = $"{ProjectMemberEntity.EntityPartitionKeyPrefix}{projectId}";
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
        GroupInfo? group = null;
        if (!string.IsNullOrEmpty(project.GroupJson))
        {
            try
            {
                group = JsonSerializer.Deserialize<GroupInfo>(project.GroupJson, JsonOptions);
            }
            catch (JsonException)
            {
                // If deserialization fails, group remains null
            }
        }

        return new Project
        {
            Id = project.Id,
            Name = project.Name,
            ShortName = project.ShortName,
            Group = group
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

    public async Task<IReadOnlyList<Role>> GetRoles(Guid projectId)
    {
        var tableClient = await GetTableClient();

        var partitionKey = RoleEntity.EntityPartitionKeyPrefix + projectId;
        var roles = tableClient.Query<RoleEntity>(e => e.PartitionKey == partitionKey);

        return roles.Select(MapRole).ToArray();
    }

    public async Task<Role> CreateRole(Guid projectId, string name)
    {
        var role = new RoleEntity
        {
            Id = Guid.NewGuid(),
            ProjectId = projectId,
            Name = name
        };

        var tableClient = await GetTableClient();
        await tableClient.AddEntityAsync(role);

        return MapRole(role);
    }

    public async Task<bool> DeleteRole(Guid roleId)
    {
        var tableClient = await GetTableClient();

        // Find the role first to get its ProjectId
        var allRoles = tableClient.Query<RoleEntity>(e => e.Id == roleId);
        var roleEntity = allRoles.FirstOrDefault();

        if (roleEntity == null)
        {
            return false;
        }

        try
        {
            // Delete the role
            var partitionKey = RoleEntity.EntityPartitionKeyPrefix + roleEntity.ProjectId;
            await tableClient.DeleteEntityAsync(partitionKey, roleId.ToString());

            // Delete all members of the role
            var roleMemberPartitionKey = RoleMemberEntity.EntityPartitionKeyPrefix + roleId;
            var members = tableClient.Query<RoleMemberEntity>(e => e.PartitionKey == roleMemberPartitionKey);

            foreach (var member in members)
            {
                await tableClient.DeleteEntityAsync(roleMemberPartitionKey, member.UserId);
            }

            return true;
        }
        catch (Azure.RequestFailedException ex) when (ex.Status == 404)
        {
            return false;
        }
    }

    public async Task<IReadOnlyList<RoleMember>> GetRoleMembers(Guid roleId)
    {
        var tableClient = await GetTableClient();

        var partitionKey = RoleMemberEntity.EntityPartitionKeyPrefix + roleId;
        var members = tableClient.Query<RoleMemberEntity>(e => e.PartitionKey == partitionKey);

        return members.Select(MapRoleMember).ToArray();
    }

    public async Task<RoleMember> AddRoleMember(Guid roleId, string userId, string firstName, string lastName, string email)
    {
        var member = new RoleMemberEntity
        {
            RoleId = roleId,
            UserId = userId,
            FirstName = firstName,
            LastName = lastName,
            Email = email
        };

        var tableClient = await GetTableClient();
        await tableClient.UpsertEntityAsync(member);

        return MapRoleMember(member);
    }

    public async Task<bool> RemoveRoleMember(Guid roleId, string userId)
    {
        var tableClient = await GetTableClient();
        var partitionKey = RoleMemberEntity.EntityPartitionKeyPrefix + roleId;

        try
        {
            await tableClient.DeleteEntityAsync(partitionKey, userId);
            return true;
        }
        catch (Azure.RequestFailedException ex) when (ex.Status == 404)
        {
            return false;
        }
    }

    private static Role MapRole(RoleEntity role)
    {
        return new Role
        {
            Id = role.Id,
            ProjectId = role.ProjectId,
            Name = role.Name
        };
    }

    private static RoleMember MapRoleMember(RoleMemberEntity member)
    {
        return new RoleMember
        {
            RoleId = member.RoleId,
            UserId = member.UserId,
            FirstName = member.FirstName,
            LastName = member.LastName,
            Email = member.Email
        };
    }
}

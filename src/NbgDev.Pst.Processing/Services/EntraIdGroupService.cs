using Microsoft.Graph;
using Microsoft.Graph.Models;

namespace NbgDev.Pst.Processing.Services;

public class EntraIdGroupService(
    GraphServiceClient graphClient,
    IConfiguration configuration,
    ILogger<EntraIdGroupService> logger) : IEntraIdGroupService
{
    public async Task<string> CreateGroupForProjectAsync(
        Guid projectId,
        string projectName,
        string shortName,
        CancellationToken cancellationToken = default)
    {
        var stage = GetStage();
        var groupDisplayName = BuildGroupDisplayName(projectName, stage);
        var groupMailNickname = BuildGroupMailNickname(shortName, stage);

        logger.LogInformation(
            "Creating EntraId group for project {ProjectId} with display name '{DisplayName}' and mail nickname '{MailNickname}'",
            projectId,
            groupDisplayName,
            groupMailNickname);

        var group = new Group
        {
            DisplayName = groupDisplayName,
            Description = $"Members of project {projectName} (Project ID: {projectId})",
            MailEnabled = false,
            MailNickname = groupMailNickname,
            SecurityEnabled = true,
            GroupTypes = new List<string>()
        };

        var createdGroup = await graphClient.Groups.PostAsync(group, cancellationToken: cancellationToken);

        if (createdGroup?.Id == null)
        {
            throw new InvalidOperationException($"Failed to create group for project {projectId}");
        }

        logger.LogInformation(
            "Created EntraId group {GroupId} for project {ProjectId}",
            createdGroup.Id,
            projectId);

        return createdGroup.Id;
    }

    public async Task AddMemberToGroupAsync(
        string groupId,
        string userId,
        CancellationToken cancellationToken = default)
    {
        logger.LogInformation(
            "Adding user {UserId} to group {GroupId}",
            userId,
            groupId);

        var directoryObject = new ReferenceCreate
        {
            OdataId = $"https://graph.microsoft.com/v1.0/directoryObjects/{userId}"
        };

        await graphClient.Groups[groupId].Members.Ref.PostAsync(directoryObject, cancellationToken: cancellationToken);

        logger.LogInformation(
            "Added user {UserId} to group {GroupId}",
            userId,
            groupId);
    }

    public async Task RemoveMemberFromGroupAsync(
        string groupId,
        string userId,
        CancellationToken cancellationToken = default)
    {
        logger.LogInformation(
            "Removing user {UserId} from group {GroupId}",
            userId,
            groupId);

        await graphClient.Groups[groupId].Members[userId].Ref.DeleteAsync(cancellationToken: cancellationToken);

        logger.LogInformation(
            "Removed user {UserId} from group {GroupId}",
            userId,
            groupId);
    }

    public async Task DeleteGroupAsync(
        string groupId,
        CancellationToken cancellationToken = default)
    {
        logger.LogInformation(
            "Deleting group {GroupId}",
            groupId);

        await graphClient.Groups[groupId].DeleteAsync(cancellationToken: cancellationToken);

        logger.LogInformation(
            "Deleted group {GroupId}",
            groupId);
    }

    public async Task<string?> GetGroupIdForProjectAsync(
        Guid projectId,
        CancellationToken cancellationToken = default)
    {
        logger.LogInformation(
            "Searching for group for project {ProjectId}",
            projectId);

        var groups = await graphClient.Groups
            .GetAsync(requestConfiguration =>
            {
                requestConfiguration.QueryParameters.Filter = $"contains(description, 'Project ID: {projectId}')";
                requestConfiguration.QueryParameters.Select = new[] { "id" };
                requestConfiguration.QueryParameters.Top = 1;
            }, cancellationToken: cancellationToken);

        var groupId = groups?.Value?.FirstOrDefault()?.Id;

        if (groupId != null)
        {
            logger.LogInformation(
                "Found group {GroupId} for project {ProjectId}",
                groupId,
                projectId);
        }
        else
        {
            logger.LogInformation(
                "No group found for project {ProjectId}",
                projectId);
        }

        return groupId;
    }

    private string GetStage()
    {
        var stage = configuration["Stage"];
        
        if (string.IsNullOrWhiteSpace(stage))
        {
            logger.LogInformation("Stage not configured, using default value 'Debug'");
            return "Debug";
        }

        return stage;
    }

    private static string BuildGroupDisplayName(string projectName, string stage)
    {
        // Don't add stage suffix for Production or Prod
        if (string.Equals(stage, "Production", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(stage, "Prod", StringComparison.OrdinalIgnoreCase))
        {
            return $"PST - {projectName}";
        }

        return $"PST - {projectName} ({stage})";
    }

    private static string BuildGroupMailNickname(string shortName, string stage)
    {
        // Sanitize shortName to be a valid mail nickname (alphanumeric and hyphens only)
        var sanitizedShortName = new string(shortName
            .Where(c => char.IsLetterOrDigit(c) || c == '-')
            .ToArray());

        // Ensure we have a valid shortName after sanitization
        if (string.IsNullOrWhiteSpace(sanitizedShortName))
        {
            sanitizedShortName = "project";
        }

        // Don't add stage suffix for Production or Prod
        if (string.Equals(stage, "Production", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(stage, "Prod", StringComparison.OrdinalIgnoreCase))
        {
            return $"pst-{sanitizedShortName}".ToLowerInvariant();
        }

        // Sanitize stage name as well
        var sanitizedStage = new string(stage
            .Where(c => char.IsLetterOrDigit(c) || c == '-')
            .ToArray());

        // Ensure we have a valid stage after sanitization
        if (string.IsNullOrWhiteSpace(sanitizedStage))
        {
            sanitizedStage = "env";
        }

        return $"pst-{sanitizedShortName}-{sanitizedStage}".ToLowerInvariant();
    }
}

namespace NbgDev.Pst.Processing.Services;

public interface IEntraIdGroupService
{
    Task<string> CreateGroupForProjectAsync(Guid projectId, string projectName, string shortName, CancellationToken cancellationToken = default);
    Task AddMemberToGroupAsync(string groupId, string userId, CancellationToken cancellationToken = default);
    Task RemoveMemberFromGroupAsync(string groupId, string userId, CancellationToken cancellationToken = default);
    Task DeleteGroupAsync(string groupId, CancellationToken cancellationToken = default);
    Task<string?> GetGroupIdForProjectAsync(Guid projectId, CancellationToken cancellationToken = default);
}

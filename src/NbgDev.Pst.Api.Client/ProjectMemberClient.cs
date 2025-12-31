using System.Text.Json;

namespace NbgDev.Pst.Api.Client;

public partial interface IProjectMemberClient
{
    Task<ICollection<ProjectMemberDto>> GetMembersAsync(Guid projectId);
    Task<ICollection<ProjectMemberDto>> GetMembersAsync(Guid projectId, CancellationToken cancellationToken);
    Task<ProjectMemberDto> AddMemberAsync(Guid projectId, AddProjectMemberDto dto);
    Task<ProjectMemberDto> AddMemberAsync(Guid projectId, AddProjectMemberDto dto, CancellationToken cancellationToken);
    Task RemoveMemberAsync(Guid projectId, string userId);
    Task RemoveMemberAsync(Guid projectId, string userId, CancellationToken cancellationToken);
}

internal partial class ProjectMemberClient : PstApiClient, IProjectMemberClient
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;

    public ProjectMemberClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _baseUrl = httpClient.BaseAddress?.ToString() ?? string.Empty;
    }

    public Task<ICollection<ProjectMemberDto>> GetMembersAsync(Guid projectId)
    {
        return GetMembersAsync(projectId, CancellationToken.None);
    }

    public async Task<ICollection<ProjectMemberDto>> GetMembersAsync(Guid projectId, CancellationToken cancellationToken)
    {
        var urlBuilder = new System.Text.StringBuilder();
        urlBuilder.Append(_baseUrl != null ? _baseUrl.TrimEnd('/') : "").Append("/api/project/{projectId}/members");
        urlBuilder.Replace("{projectId}", Uri.EscapeDataString(projectId.ToString()));

        var request = new HttpRequestMessage(HttpMethod.Get, urlBuilder.ToString());
        await PrepareRequestAsync(_httpClient, request, urlBuilder.ToString(), cancellationToken);

        var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
        await ProcessResponseAsync(_httpClient, response, cancellationToken);

        var responseData = await response.Content.ReadAsStringAsync(cancellationToken);
        response.EnsureSuccessStatusCode();

        var result = JsonSerializer.Deserialize<ICollection<ProjectMemberDto>>(responseData, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        return result ?? new List<ProjectMemberDto>();
    }

    public Task<ProjectMemberDto> AddMemberAsync(Guid projectId, AddProjectMemberDto dto)
    {
        return AddMemberAsync(projectId, dto, CancellationToken.None);
    }

    public async Task<ProjectMemberDto> AddMemberAsync(Guid projectId, AddProjectMemberDto dto, CancellationToken cancellationToken)
    {
        var urlBuilder = new System.Text.StringBuilder();
        urlBuilder.Append(_baseUrl != null ? _baseUrl.TrimEnd('/') : "").Append("/api/project/{projectId}/members");
        urlBuilder.Replace("{projectId}", Uri.EscapeDataString(projectId.ToString()));

        var request = new HttpRequestMessage(HttpMethod.Post, urlBuilder.ToString());
        var content = new StringContent(JsonSerializer.Serialize(dto), System.Text.Encoding.UTF8, "application/json");
        request.Content = content;

        await PrepareRequestAsync(_httpClient, request, urlBuilder.ToString(), cancellationToken);

        var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
        await ProcessResponseAsync(_httpClient, response, cancellationToken);

        var responseData = await response.Content.ReadAsStringAsync(cancellationToken);
        response.EnsureSuccessStatusCode();

        var result = JsonSerializer.Deserialize<ProjectMemberDto>(responseData, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        return result!;
    }

    public Task RemoveMemberAsync(Guid projectId, string userId)
    {
        return RemoveMemberAsync(projectId, userId, CancellationToken.None);
    }

    public async Task RemoveMemberAsync(Guid projectId, string userId, CancellationToken cancellationToken)
    {
        var urlBuilder = new System.Text.StringBuilder();
        urlBuilder.Append(_baseUrl != null ? _baseUrl.TrimEnd('/') : "").Append("/api/project/{projectId}/members/{userId}");
        urlBuilder.Replace("{projectId}", Uri.EscapeDataString(projectId.ToString()));
        urlBuilder.Replace("{userId}", Uri.EscapeDataString(userId));

        var request = new HttpRequestMessage(HttpMethod.Delete, urlBuilder.ToString());
        await PrepareRequestAsync(_httpClient, request, urlBuilder.ToString(), cancellationToken);

        var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
        await ProcessResponseAsync(_httpClient, response, cancellationToken);

        response.EnsureSuccessStatusCode();
    }
}

public class ProjectMemberDto
{
    public required Guid ProjectId { get; set; }
    public required string UserId { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
}

public class AddProjectMemberDto
{
    public required string UserId { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
}

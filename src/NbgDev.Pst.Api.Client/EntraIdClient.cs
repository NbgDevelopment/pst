using System.Text.Json;

namespace NbgDev.Pst.Api.Client;

public partial interface IEntraIdClient
{
    Task<ICollection<EntraIdUserDto>> SearchUsersAsync(string searchTerm);
    Task<ICollection<EntraIdUserDto>> SearchUsersAsync(string searchTerm, CancellationToken cancellationToken);
}

internal partial class EntraIdClient : PstApiClient, IEntraIdClient
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;

    public EntraIdClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _baseUrl = httpClient.BaseAddress?.ToString() ?? string.Empty;
    }

    public Task<ICollection<EntraIdUserDto>> SearchUsersAsync(string searchTerm)
    {
        return SearchUsersAsync(searchTerm, CancellationToken.None);
    }

    public async Task<ICollection<EntraIdUserDto>> SearchUsersAsync(string searchTerm, CancellationToken cancellationToken)
    {
        var urlBuilder = new System.Text.StringBuilder();
        urlBuilder.Append(_baseUrl != null ? _baseUrl.TrimEnd('/') : "").Append("/api/entraid/users/search?");
        if (!string.IsNullOrEmpty(searchTerm))
        {
            urlBuilder.Append("searchTerm=").Append(Uri.EscapeDataString(searchTerm));
        }

        var request = new HttpRequestMessage(HttpMethod.Get, urlBuilder.ToString());
        await PrepareRequestAsync(_httpClient, request, urlBuilder.ToString(), cancellationToken);

        var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
        await ProcessResponseAsync(_httpClient, response, cancellationToken);

        var responseData = await response.Content.ReadAsStringAsync(cancellationToken);
        response.EnsureSuccessStatusCode();

        var result = JsonSerializer.Deserialize<ICollection<EntraIdUserDto>>(responseData, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        return result ?? new List<EntraIdUserDto>();
    }
}

public class EntraIdUserDto
{
    public required string Id { get; set; }
    public required string DisplayName { get; set; }
    public required string GivenName { get; set; }
    public required string Surname { get; set; }
    public required string Mail { get; set; }
}

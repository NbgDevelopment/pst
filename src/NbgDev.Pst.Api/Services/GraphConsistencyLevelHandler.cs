namespace NbgDev.Pst.Api.Services;

/// <summary>
/// HTTP handler that adds the ConsistencyLevel header to Graph API requests.
/// This is required for advanced queries like $search.
/// </summary>
public class GraphConsistencyLevelHandler : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        // Add ConsistencyLevel header for all Graph API requests
        if (!request.Headers.Contains("ConsistencyLevel"))
        {
            request.Headers.Add("ConsistencyLevel", "eventual");
        }

        return await base.SendAsync(request, cancellationToken);
    }
}

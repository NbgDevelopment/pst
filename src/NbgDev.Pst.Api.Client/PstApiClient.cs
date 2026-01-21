using System.Net;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Abstractions;

namespace NbgDev.Pst.Api.Client;

public class PstApiClient
{
    public required IAuthorizationHeaderProvider AuthorizationHeaderProvider { get; init; }
    public required NavigationManager NavigationManager { get; init; }
    public required IOptions<PstApiClientOptions> Options { get; init; }
    
    protected async Task PrepareRequestAsync(HttpClient client, HttpRequestMessage request, string url, CancellationToken cancellationToken)
    {
        await PrepareRequestAsync(request, cancellationToken);
    }

    protected async Task PrepareRequestAsync(HttpClient client, HttpRequestMessage request, StringBuilder urlBuilder, CancellationToken cancellationToken)
    {
        await PrepareRequestAsync(request, cancellationToken);
    }

    protected Task ProcessResponseAsync(HttpClient client, HttpResponseMessage response, CancellationToken cancellationToken)
    {
        // Don't try to navigate on 401 - let the caller handle it
        // Navigating with forceLoad in Blazor Server during request processing throws NavigationException
        return Task.CompletedTask;
    }

    private async Task PrepareRequestAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        // Get the scope from configuration
        var scope = Options.Value.Scope;
        var scopes = !string.IsNullOrEmpty(scope) ? new[] { scope } : Array.Empty<string>();
        
        var authorizationHeader = await AuthorizationHeaderProvider.CreateAuthorizationHeaderForUserAsync(
            scopes: scopes,
            cancellationToken: cancellationToken);
        
        if (!string.IsNullOrEmpty(authorizationHeader))
        {
            // Remove any existing Authorization header before adding new one
            request.Headers.Remove("Authorization");
            request.Headers.TryAddWithoutValidation("Authorization", authorizationHeader);
        }
    }
}

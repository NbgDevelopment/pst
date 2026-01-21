using System.Net;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Components;
using Microsoft.Identity.Abstractions;

namespace NbgDev.Pst.Api.Client;

public class PstApiClient
{
    public required IAuthorizationHeaderProvider AuthorizationHeaderProvider { get; init; }
    public required NavigationManager NavigationManager { get; init; }
    
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
        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            NavigationManager.NavigateTo("MicrosoftIdentity/Account/SignOut", forceLoad: true);
        }
        
        return Task.CompletedTask;
    }

    private async Task PrepareRequestAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var authorizationHeader = await AuthorizationHeaderProvider.CreateAuthorizationHeaderForUserAsync(
            scopes: [],
            cancellationToken: cancellationToken);
        
        if (!string.IsNullOrEmpty(authorizationHeader))
        {
            request.Headers.Add("Authorization", authorizationHeader);
        }
    }
}

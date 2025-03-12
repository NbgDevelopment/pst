﻿using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace NbgDev.Pst.Api.Client;

public class PstApiClient
{
    public required IAccessTokenProvider AccessTokenProvider { get; init; }
    protected async Task PrepareRequestAsync(HttpClient client, HttpRequestMessage request, string url, CancellationToken cancellationToken)
    {
        await PrepareRequestAsync(request);
    }

    protected async Task PrepareRequestAsync(HttpClient client, HttpRequestMessage request, StringBuilder urlBuilder, CancellationToken cancellationToken)
    {
        await PrepareRequestAsync(request);
    }

    protected Task ProcessResponseAsync(HttpClient client, HttpResponseMessage request, CancellationToken cancellationToken) => Task.CompletedTask;

    private async Task PrepareRequestAsync(HttpRequestMessage request)
    {
        var accessTokenResult = await AccessTokenProvider.RequestAccessToken();
        if (accessTokenResult.TryGetToken(out var accessToken))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken.Value);
        }
    }
}

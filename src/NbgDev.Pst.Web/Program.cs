using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using NbgDev.Pst.App;
using NbgDev.Pst.Web;
using NbgDev.Pst.Web.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

Console.WriteLine($"BaseUrl: {builder.HostEnvironment.BaseAddress}");

using (var client = new HttpClient{ BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) })
{
    using var response = await client.GetAsync("appsettings.backend.json");
    if (response.IsSuccessStatusCode)
    {
        await using var stream = await response.Content.ReadAsStreamAsync();
        builder.Configuration.AddJsonStream(stream);
    }
}

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddApp(builder.Configuration);

// Register token expiration service
builder.Services.AddScoped<TokenExpirationService>();

builder.Services.AddMsalAuthentication(options =>
{
    builder.Configuration.Bind("AzureAd", options.ProviderOptions.Authentication);
    var scope = builder.Configuration["PstApi:Scope"];

    if (string.IsNullOrWhiteSpace(scope))
    {
        Console.Error.WriteLine("Configuration for [PstApi:Scope] not found.");
        return;
    }

    options.ProviderOptions.DefaultAccessTokenScopes.Add(scope);
    
    // Configure MSAL to use localStorage for persistent login across browser restarts
    options.ProviderOptions.Cache.CacheLocation = "localStorage";
});

var host = builder.Build();

// Clear expired tokens before running the app
var tokenExpirationService = host.Services.GetRequiredService<TokenExpirationService>();
await tokenExpirationService.ClearExpiredTokensAsync();

await host.RunAsync();

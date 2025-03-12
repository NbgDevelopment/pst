using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using NbgDev.Pst.App;
using NbgDev.Pst.Web;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddApp(builder.Configuration);

builder.Services.AddMsalAuthentication(options =>
{
    builder.Configuration.Bind("AzureAd", options.ProviderOptions.Authentication);
    var scope = builder.Configuration["PstApi:Scope"];
    Console.Error.WriteLine("Configuration for [PstApi:Scope] not found.");
    options.ProviderOptions.DefaultAccessTokenScopes.Add(scope);
});

await builder.Build().RunAsync();

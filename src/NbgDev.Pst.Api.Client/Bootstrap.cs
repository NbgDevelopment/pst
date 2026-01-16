using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace NbgDev.Pst.Api.Client;

public static class Bootstrap
{
    public static IServiceCollection AddPstApiClient(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<PstApiClientOptions>().Configure(options => configuration.GetSection("PstApi").Bind(options));

        services.AddScoped<IProjectClient>(serviceProvider =>
        {
            var options = serviceProvider.GetRequiredService<IOptions<PstApiClientOptions>>();
            Console.WriteLine($"API Url: {options.Value.ApiUrl}");
            var accessTokenProvider = serviceProvider.GetRequiredService<IAccessTokenProvider>();
            var navigationManager = serviceProvider.GetRequiredService<NavigationManager>();
            return new ProjectClient(options.Value.ApiUrl, new HttpClient())
            {
                AccessTokenProvider = accessTokenProvider,
                NavigationManager = navigationManager
            };
        });

        services.AddScoped<IProjectMemberClient>(serviceProvider =>
        {
            var options = serviceProvider.GetRequiredService<IOptions<PstApiClientOptions>>();
            var accessTokenProvider = serviceProvider.GetRequiredService<IAccessTokenProvider>();
            var navigationManager = serviceProvider.GetRequiredService<NavigationManager>();
            return new ProjectMemberClient(options.Value.ApiUrl, new HttpClient())
            {
                AccessTokenProvider = accessTokenProvider,
                NavigationManager = navigationManager
            };
        });

        services.AddScoped<IEntraIdClient>(serviceProvider =>
        {
            var options = serviceProvider.GetRequiredService<IOptions<PstApiClientOptions>>();
            var accessTokenProvider = serviceProvider.GetRequiredService<IAccessTokenProvider>();
            var navigationManager = serviceProvider.GetRequiredService<NavigationManager>();
            return new EntraIdClient(options.Value.ApiUrl, new HttpClient())
            {
                AccessTokenProvider = accessTokenProvider,
                NavigationManager = navigationManager
            };
        });

        services.AddScoped<IProjectRoleClient>(serviceProvider =>
        {
            var options = serviceProvider.GetRequiredService<IOptions<PstApiClientOptions>>();
            var accessTokenProvider = serviceProvider.GetRequiredService<IAccessTokenProvider>();
            var navigationManager = serviceProvider.GetRequiredService<NavigationManager>();
            return new ProjectRoleClient(options.Value.ApiUrl, new HttpClient())
            {
                AccessTokenProvider = accessTokenProvider,
                NavigationManager = navigationManager
            };
        });

        services.AddScoped<IProjectRoleMemberClient>(serviceProvider =>
        {
            var options = serviceProvider.GetRequiredService<IOptions<PstApiClientOptions>>();
            var accessTokenProvider = serviceProvider.GetRequiredService<IAccessTokenProvider>();
            var navigationManager = serviceProvider.GetRequiredService<NavigationManager>();
            return new ProjectRoleMemberClient(options.Value.ApiUrl, new HttpClient())
            {
                AccessTokenProvider = accessTokenProvider,
                NavigationManager = navigationManager
            };
        });

        return services;
    }
}

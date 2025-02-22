using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor.Services;
using NbgDev.Pst.Api.Client;
using NbgDev.Pst.App.Services;

namespace NbgDev.Pst.App;

public static class Bootstrap
{
    public static IServiceCollection AddApp(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMudServices();

        services.AddPstApiClient(configuration);

        services.AddSingleton<IProjectService, ProjectService>();

        return services;
    }
}

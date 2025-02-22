using Microsoft.Extensions.DependencyInjection;
using MudBlazor.Services;
using NbgDev.Pst.App.Services;

namespace NbgDev.Pst.App;

public static class Bootstrap
{
    public static IServiceCollection AddApp(this IServiceCollection services)
    {
        services.AddMudServices();

        services.AddSingleton<IProjectService, ProjectService>();

        return services;
    }
}

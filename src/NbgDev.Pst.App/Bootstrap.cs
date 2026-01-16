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

        services.AddScoped<IProjectService, ProjectService>();
        services.AddScoped<IProjectMemberService, ProjectMemberService>();
        services.AddScoped<IEntraIdService, EntraIdService>();
        services.AddScoped<IProjectRoleService, ProjectRoleService>();
        services.AddScoped<IRoleMemberService, RoleMemberService>();

        return services;
    }
}

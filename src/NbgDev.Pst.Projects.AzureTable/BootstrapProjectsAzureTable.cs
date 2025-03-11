using Microsoft.Extensions.DependencyInjection;
using NbgDev.Pst.Projects.AzureTable.Services;

namespace NbgDev.Pst.Projects.AzureTable;

public static class BootstrapProjectsAzureTable
{
    public static IServiceCollection AddProjectsAzureTable(this IServiceCollection services)
    {
        services.AddSingleton<IProjectService, ProjectService>();

        return services;
    }
}

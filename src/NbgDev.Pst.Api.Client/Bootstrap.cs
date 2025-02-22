using Microsoft.Extensions.DependencyInjection;

namespace NbgDev.Pst.Api.Client;

public static class Bootstrap
{
    public static IServiceCollection AddPstApiClient(this IServiceCollection services)
    {
        var clients = typeof(Bootstrap).Assembly.GetTypes()
            .Where(t => t.IsClass)
            .Where(t => t.Name.EndsWith("Client"))
            .Select(t => new { Type = t, Interfaces = t.GetInterfaces() })
            .Where(t => t.Interfaces.Length == 1)
            .Select(t => new { Implementation = t.Type, Interface = t.Interfaces[0] })
            .ToList();

        foreach (var client in clients)
        {
            services.AddSingleton(client.Interface, client.Implementation);
        }

        return services;
    }
}

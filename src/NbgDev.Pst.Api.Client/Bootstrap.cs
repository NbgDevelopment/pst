using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace NbgDev.Pst.Api.Client;

public static class Bootstrap
{
    public static IServiceCollection AddPstApiClient(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<PstApiClientOptions>().Configure(options => configuration.GetSection("PstApi").Bind(options));

        //var clients = typeof(Bootstrap).Assembly.GetTypes()
        //    .Where(t => t.IsClass)
        //    .Where(t => t.Name.EndsWith("Client"))
        //    .Select(t => new { Type = t, Interfaces = t.GetInterfaces() })
        //    .Where(t => t.Interfaces.Length == 1)
        //    .Select(t => new { Implementation = t.Type, Interface = t.Interfaces[0] })
        //    .ToList();

        //var lazyType = typeof(Lazy<>);
        //foreach (var client in clients)
        //{
        //    services.AddSingleton(client.Interface, serviceProvider =>
        //    {
        //        var options = serviceProvider.GetRequiredService<IOptions<PstApiClientOptions>>();
        //        var constructor = client.Implementation.GetConstructor([typeof(string), typeof(HttpClient)])!;
        //        return constructor.Invoke(null, [options.Value.ApiUrl, new HttpClient()])!;
        //    });
        //}

        services.AddSingleton<IProjectClient>(serviceProvider =>
        {
            var options = serviceProvider.GetRequiredService<IOptions<PstApiClientOptions>>();
            return new ProjectClient(options.Value.ApiUrl, new HttpClient());
        });

        return services;
    }
}

using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace NbgDev.Pst.Projects.Contract.Mediator;

/// <summary>
/// Extension methods for registering mediator services
/// </summary>
public static class MediatorServiceCollectionExtensions
{
    /// <summary>
    /// Registers mediator and handlers from the specified assemblies
    /// </summary>
    /// <param name="services">Service collection</param>
    /// <param name="assemblies">Assemblies to scan for handlers</param>
    /// <returns>Service collection for chaining</returns>
    public static IServiceCollection AddMediator(this IServiceCollection services, params Assembly[] assemblies)
    {
        // Register the mediator
        services.AddScoped<IMediator, Mediator>();

        // Register all handlers
        foreach (var assembly in assemblies)
        {
            RegisterHandlers(services, assembly);
        }

        return services;
    }

    private static void RegisterHandlers(IServiceCollection services, Assembly assembly)
    {
        var handlerTypes = assembly.GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract)
            .Select(t => new
            {
                Type = t,
                Interfaces = t.GetInterfaces()
                    .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>))
                    .ToList()
            })
            .Where(x => x.Interfaces.Any())
            .ToList();

        foreach (var handlerType in handlerTypes)
        {
            foreach (var handlerInterface in handlerType.Interfaces)
            {
                services.AddTransient(handlerInterface, handlerType.Type);
            }
        }
    }
}

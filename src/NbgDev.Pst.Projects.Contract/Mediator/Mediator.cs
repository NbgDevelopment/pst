using Microsoft.Extensions.DependencyInjection;

namespace NbgDev.Pst.Projects.Contract.Mediator;

/// <summary>
/// Default mediator implementation that uses the service provider to resolve handlers
/// </summary>
internal sealed class Mediator(IServiceProvider serviceProvider) : IMediator
{
    public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        var requestType = request.GetType();
        var handlerType = typeof(IRequestHandler<,>).MakeGenericType(requestType, typeof(TResponse));
        
        var handler = serviceProvider.GetRequiredService(handlerType);
        
        var handleMethod = handlerType.GetMethod(nameof(IRequestHandler<IRequest<TResponse>, TResponse>.Handle));
        
        if (handleMethod == null)
        {
            throw new InvalidOperationException($"Handle method not found on handler for request type {requestType.Name}");
        }

        var result = handleMethod.Invoke(handler, [request, cancellationToken]);
        
        if (result is Task<TResponse> task)
        {
            return task;
        }

        throw new InvalidOperationException($"Handler for {requestType.Name} did not return a Task<{typeof(TResponse).Name}>");
    }
}

using Microsoft.Extensions.DependencyInjection;
using System.Collections.Concurrent;

namespace NbgDev.Pst.Projects.Contract.Mediator;

/// <summary>
/// Default mediator implementation that uses the service provider to resolve handlers
/// </summary>
internal sealed class Mediator(IServiceProvider serviceProvider) : IMediator
{
    private delegate Task<object> HandlerDelegate(object handler, object request, CancellationToken cancellationToken);
    private static readonly ConcurrentDictionary<Type, HandlerDelegate> _handlerCache = new();

    public async Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        var requestType = request.GetType();
        var handlerType = typeof(IRequestHandler<,>).MakeGenericType(requestType, typeof(TResponse));
        
        var handler = serviceProvider.GetRequiredService(handlerType);
        
        // Get or create the compiled delegate for this handler type
        var invoker = _handlerCache.GetOrAdd(handlerType, ht =>
        {
            var handleMethod = ht.GetMethod(nameof(IRequestHandler<IRequest<TResponse>, TResponse>.Handle));
            
            if (handleMethod == null)
            {
                throw new InvalidOperationException($"Handle method not found on handler type {ht.Name}");
            }

            return async (h, r, ct) =>
            {
                var result = handleMethod.Invoke(h, [r, ct]);
                if (result is Task<TResponse> task)
                {
                    return await task;
                }
                throw new InvalidOperationException($"Handler did not return expected Task<TResponse>");
            };
        });
        
        var responseObj = await invoker(handler, request, cancellationToken);
        return (TResponse)responseObj!;
    }
}

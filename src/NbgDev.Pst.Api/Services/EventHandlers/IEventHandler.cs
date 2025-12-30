using NbgDev.Pst.Events.Contract.Base;

namespace NbgDev.Pst.Api.Services.EventHandlers;

public interface IEventHandler
{
    Task HandleAsync(BaseEvent @event, CancellationToken cancellationToken = default);
    bool CanHandle(string eventType);
}

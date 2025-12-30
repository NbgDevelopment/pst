using NbgDev.Pst.Events.Contract.Base;

namespace NbgDev.Pst.Processing.Handlers;

public interface IEventHandler
{
    Task HandleAsync(BaseEvent @event, CancellationToken cancellationToken = default);
    bool CanHandle(string eventType);
}

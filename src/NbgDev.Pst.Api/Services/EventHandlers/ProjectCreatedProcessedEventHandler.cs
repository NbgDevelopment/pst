using MediatR;
using NbgDev.Pst.Events.Contract.Base;
using NbgDev.Pst.Events.Contract.Models;
using NbgDev.Pst.Projects.Contract.Requests;

namespace NbgDev.Pst.Api.Services.EventHandlers;

public class ProjectCreatedProcessedEventHandler(
    IMediator mediator,
    ILogger<ProjectCreatedProcessedEventHandler> logger) : IEventHandler
{
    public bool CanHandle(string eventType) => eventType == nameof(ProjectCreatedProcessedEvent);

    public async Task HandleAsync(BaseEvent @event, CancellationToken cancellationToken = default)
    {
        var processedEvent = @event as ProjectCreatedProcessedEvent;
        if (processedEvent == null)
        {
            logger.LogError("Event is not a ProjectCreatedProcessedEvent");
            return;
        }

        logger.LogInformation(
            "Received project created processed event for project {ProjectId}. Success: {Success}, Message: {Message}, GroupId: {GroupId}",
            processedEvent.ProjectId,
            processedEvent.Success,
            processedEvent.Message,
            processedEvent.GroupId);

        if (processedEvent.Success && !string.IsNullOrEmpty(processedEvent.GroupId))
        {
            try
            {
                await mediator.Send(new UpdateProjectGroupIdRequest(
                    processedEvent.ProjectId,
                    processedEvent.GroupId), cancellationToken);

                logger.LogInformation(
                    "Updated project {ProjectId} with GroupId {GroupId}",
                    processedEvent.ProjectId,
                    processedEvent.GroupId);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, 
                    "Failed to update project {ProjectId} with GroupId {GroupId}",
                    processedEvent.ProjectId,
                    processedEvent.GroupId);
            }
        }
    }
}

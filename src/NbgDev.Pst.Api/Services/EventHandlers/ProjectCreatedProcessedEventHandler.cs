using NbgDev.Pst.Projects.Contract.Mediator;
using NbgDev.Pst.Events.Contract.Base;
using NbgDev.Pst.Events.Contract.Models;
using NbgDev.Pst.Projects.Contract.Models;
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
            "Received project created processed event for project {ProjectId}. Success: {Success}, Message: {Message}, GroupId: {GroupId}, GroupName: {GroupName}",
            processedEvent.ProjectId,
            processedEvent.Success,
            processedEvent.Message,
            processedEvent.GroupId,
            processedEvent.GroupName);

        if (processedEvent.Success && !string.IsNullOrEmpty(processedEvent.GroupId) && !string.IsNullOrEmpty(processedEvent.GroupName))
        {
            try
            {
                var groupInfo = new GroupInfo
                {
                    Id = processedEvent.GroupId,
                    Name = processedEvent.GroupName
                };

                await mediator.Send(new UpdateProjectGroupRequest(
                    processedEvent.ProjectId,
                    groupInfo), cancellationToken);

                logger.LogInformation(
                    "Updated project {ProjectId} with Group {GroupName} ({GroupId})",
                    processedEvent.ProjectId,
                    processedEvent.GroupName,
                    processedEvent.GroupId);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, 
                    "Failed to update project {ProjectId} with Group {GroupName} ({GroupId})",
                    processedEvent.ProjectId,
                    processedEvent.GroupName,
                    processedEvent.GroupId);
            }
        }
    }
}

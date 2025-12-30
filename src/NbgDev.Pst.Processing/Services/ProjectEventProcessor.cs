using Azure.Storage.Queues;
using NbgDev.Pst.Events.Contract.Models;
using System.Text.Json;

namespace NbgDev.Pst.Processing.Services;

public class ProjectEventProcessor(
    QueueServiceClient apiQueueClient,
    QueueServiceClient processingQueueClient,
    ILogger<ProjectEventProcessor> logger) : BackgroundService
{
    private const string ApiQueueName = "api-events";
    private const string ProcessingQueueName = "processing-events";

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var apiQueue = apiQueueClient.GetQueueClient(ApiQueueName);
        await apiQueue.CreateIfNotExistsAsync(cancellationToken: stoppingToken);

        var processingQueue = processingQueueClient.GetQueueClient(ProcessingQueueName);
        await processingQueue.CreateIfNotExistsAsync(cancellationToken: stoppingToken);

        logger.LogInformation("Project event processor started");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var messages = await apiQueue.ReceiveMessagesAsync(
                    maxMessages: 10,
                    cancellationToken: stoppingToken);

                if (messages.Value == null || messages.Value.Length == 0)
                {
                    await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
                    continue;
                }

                foreach (var message in messages.Value)
                {
                    try
                    {
                        ProjectCreatedEvent? projectCreatedEvent = null;
                        try
                        {
                            projectCreatedEvent = JsonSerializer.Deserialize<ProjectCreatedEvent>(message.MessageText);
                        }
                        catch (JsonException ex)
                        {
                            logger.LogError(ex, "Failed to deserialize message {MessageId}. Invalid JSON format", message.MessageId);
                            await apiQueue.DeleteMessageAsync(message.MessageId, message.PopReceipt, stoppingToken);
                            continue;
                        }
                        
                        if (projectCreatedEvent != null)
                        {
                            logger.LogInformation(
                                "Processing project created event for project {ProjectId} - {ProjectName} ({ShortName})",
                                projectCreatedEvent.ProjectId,
                                projectCreatedEvent.ProjectName,
                                projectCreatedEvent.ShortName);

                            // Send confirmation event
                            var processedEvent = new ProjectProcessedEvent
                            {
                                ProjectId = projectCreatedEvent.ProjectId,
                                Success = true,
                                Message = "Project creation processed successfully"
                            };

                            var confirmationMessage = JsonSerializer.Serialize(processedEvent);
                            await processingQueue.SendMessageAsync(confirmationMessage, cancellationToken: stoppingToken);

                            logger.LogInformation(
                                "Sent confirmation event for project {ProjectId}",
                                projectCreatedEvent.ProjectId);
                        }

                        await apiQueue.DeleteMessageAsync(message.MessageId, message.PopReceipt, stoppingToken);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "Error processing message {MessageId}", message.MessageId);
                    }
                }

                await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                logger.LogError(ex, "Error in event processing loop");
                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
        }

        logger.LogInformation("Project event processor stopped");
    }
}

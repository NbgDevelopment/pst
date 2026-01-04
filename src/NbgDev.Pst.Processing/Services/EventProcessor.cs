using Azure.Storage.Queues;
using NbgDev.Pst.Events.Contract.Base;
using NbgDev.Pst.Processing.Handlers;
using System.Text.Json;

namespace NbgDev.Pst.Processing.Services;

public class EventProcessor(
    QueueServiceClient apiQueueClient,
    IServiceScopeFactory serviceScopeFactory,
    ILogger<EventProcessor> logger) : BackgroundService
{
    private const string ApiQueueName = "api-events";

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var apiQueue = apiQueueClient.GetQueueClient(ApiQueueName);
        await apiQueue.CreateIfNotExistsAsync(cancellationToken: stoppingToken);

        logger.LogInformation("Event processor started");

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
                        using var scope = serviceScopeFactory.CreateScope();
                        var eventHandlers = scope.ServiceProvider.GetServices<IEventHandler>();

                        BaseEvent? @event = null;
                        string? eventType = null;

                        try
                        {
                            // First, deserialize to get the event type
                            using var doc = JsonDocument.Parse(message.MessageText);
                            if (doc.RootElement.TryGetProperty("EventType", out var eventTypeElement))
                            {
                                eventType = eventTypeElement.GetString();
                            }

                            if (string.IsNullOrEmpty(eventType))
                            {
                                logger.LogError("Message {MessageId} does not contain EventType property", message.MessageId);
                                await apiQueue.DeleteMessageAsync(message.MessageId, message.PopReceipt, stoppingToken);
                                continue;
                            }

                            // Find handler for this event type
                            var handler = eventHandlers.FirstOrDefault(h => h.CanHandle(eventType));
                            if (handler == null)
                            {
                                logger.LogInformation("No handler found for event type {EventType}. Message: {Message}", eventType, message.MessageText);
                                await apiQueue.DeleteMessageAsync(message.MessageId, message.PopReceipt, stoppingToken);
                                continue;
                            }

                            // Deserialize based on event type
                            @event = eventType switch
                            {
                                nameof(Events.Contract.Models.ProjectCreatedEvent) => 
                                    JsonSerializer.Deserialize<Events.Contract.Models.ProjectCreatedEvent>(message.MessageText),
                                nameof(Events.Contract.Models.ProjectMemberAddedEvent) => 
                                    JsonSerializer.Deserialize<Events.Contract.Models.ProjectMemberAddedEvent>(message.MessageText),
                                nameof(Events.Contract.Models.ProjectMemberRemovedEvent) => 
                                    JsonSerializer.Deserialize<Events.Contract.Models.ProjectMemberRemovedEvent>(message.MessageText),
                                _ => null
                            };

                            if (@event != null)
                            {
                                await handler.HandleAsync(@event, stoppingToken);
                            }
                        }
                        catch (JsonException ex)
                        {
                            logger.LogError(ex, "Failed to deserialize message {MessageId}. Invalid JSON format", message.MessageId);
                            await apiQueue.DeleteMessageAsync(message.MessageId, message.PopReceipt, stoppingToken);
                            continue;
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

        logger.LogInformation("Event processor stopped");
    }
}

using NbgDev.Pst.Processing.Handlers;
using NbgDev.Pst.Processing.Services;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.AddAzureQueueClient("ApiQueues");
builder.AddAzureQueueClient("ProcessingQueues");

builder.Services.AddScoped<IEventHandler, ProjectCreatedEventHandler>();
builder.Services.AddScoped<NbgDev.Pst.Processing.Services.IEventPublisher, NbgDev.Pst.Processing.Services.EventPublisher>();
builder.Services.AddHostedService<EventProcessor>();

var app = builder.Build();

app.MapDefaultEndpoints();

app.Run();

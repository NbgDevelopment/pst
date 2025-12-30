using NbgDev.Pst.Processing.Services;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.AddAzureQueueClient("ApiQueues");
builder.AddAzureQueueClient("ProcessingQueues");

builder.Services.AddHostedService<ProjectEventProcessor>();

var app = builder.Build();

app.MapDefaultEndpoints();

app.Run();

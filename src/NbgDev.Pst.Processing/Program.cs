using Azure.Identity;
using Microsoft.Graph;
using NbgDev.Pst.Processing.Handlers;
using NbgDev.Pst.Processing.Services;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.AddAzureQueueServiceClient("ApiQueues");
builder.AddAzureQueueServiceClient("ProcessingQueues");

// Configure GraphServiceClient with app-only authentication (client credentials)
builder.Services.AddSingleton<GraphServiceClient>(sp =>
{
    var config = builder.Configuration;
    var tenantId = config["AzureAd:TenantId"];
    var clientId = config["AzureAd:ClientId"];
    var clientSecret = config["AzureAd:ClientSecret"];
    
    if (string.IsNullOrEmpty(tenantId))
    {
        throw new InvalidOperationException("AzureAd:TenantId is not configured. Please set it in configuration, environment variables, or Azure Key Vault.");
    }
    if (string.IsNullOrEmpty(clientId))
    {
        throw new InvalidOperationException("AzureAd:ClientId is not configured. Please set it in configuration, environment variables, or Azure Key Vault.");
    }
    if (string.IsNullOrEmpty(clientSecret))
    {
        throw new InvalidOperationException("AzureAd:ClientSecret is not configured. Please set it in user secrets, environment variables, or Azure Key Vault. Never commit secrets to source control.");
    }
    
    var credential = new ClientSecretCredential(tenantId, clientId, clientSecret);
    
    return new GraphServiceClient(credential);
});

builder.Services.AddScoped<IEntraIdGroupService, EntraIdGroupService>();
builder.Services.AddScoped<IEventHandler, ProjectCreatedEventHandler>();
builder.Services.AddScoped<IEventHandler, ProjectMemberAddedEventHandler>();
builder.Services.AddScoped<IEventHandler, ProjectMemberRemovedEventHandler>();
builder.Services.AddScoped<IEventHandler, ProjectDeletedEventHandler>();
builder.Services.AddScoped<IEventHandler, ProjectRoleCreatedEventHandler>();
builder.Services.AddScoped<IEventHandler, ProjectRoleDeletedEventHandler>();
builder.Services.AddScoped<IEventHandler, ProjectMemberAddedToRoleEventHandler>();
builder.Services.AddScoped<IEventHandler, ProjectMemberRemovedFromRoleEventHandler>();
builder.Services.AddScoped<NbgDev.Pst.Processing.Services.IEventPublisher, NbgDev.Pst.Processing.Services.EventPublisher>();
builder.Services.AddHostedService<EventProcessor>();

var app = builder.Build();

app.MapDefaultEndpoints();

app.Run();

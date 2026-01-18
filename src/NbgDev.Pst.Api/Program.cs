using Azure.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Graph;
using Microsoft.Identity.Web;
using NbgDev.Pst.Api.Services;
using NbgDev.Pst.Api.Services.EventHandlers;
using NbgDev.Pst.Projects.AzureTable;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add authentication for incoming API requests
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

// Configure GraphServiceClient with app-only authentication (client credentials)
builder.Services.AddSingleton<GraphServiceClient>(sp =>
{
    var config = builder.Configuration;
    var tenantId = config["AzureAd:TenantId"];
    var clientId = config["AzureAd:ClientId"];
    var clientSecret = config["AzureAd:ClientSecret"];
    
    if (string.IsNullOrEmpty(tenantId))
    {
        throw new InvalidOperationException("AzureAd:TenantId is not configured. Please set it in appsettings.json or user secrets.");
    }
    if (string.IsNullOrEmpty(clientId))
    {
        throw new InvalidOperationException("AzureAd:ClientId is not configured. Please set it in appsettings.json or user secrets.");
    }
    if (string.IsNullOrEmpty(clientSecret))
    {
        throw new InvalidOperationException("AzureAd:ClientSecret is not configured. Please set it in user secrets, environment variables, or Azure Key Vault. Never commit secrets to source control.");
    }
    
    var credential = new ClientSecretCredential(tenantId, clientId, clientSecret);
    
    return new GraphServiceClient(credential);
});

builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssemblyContaining(typeof(BootstrapProjectsAzureTable));
    config.RegisterServicesFromAssemblyContaining(typeof(Program));
});

builder.AddAzureTableServiceClient("Projects");
builder.Services.AddProjectsAzureTable();
builder.Services.AddScoped<IEntraIdService, EntraIdService>();

builder.AddAzureQueueServiceClient("ApiQueues");
builder.AddAzureQueueServiceClient("ProcessingQueues");
builder.Services.AddScoped<IEventPublisher, EventPublisher>();
builder.Services.AddScoped<IEventHandler, ProjectCreatedProcessedEventHandler>();
builder.Services.AddHostedService<EventProcessor>();

builder.Services.AddControllers();
builder.Services.AddCors(cors =>
{
    cors.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.AllowAnyHeader();
            policy.AllowAnyMethod();
            policy.AllowCredentials();
            policy.WithOrigins(builder.Configuration["Cors:AllowedOrigins"]!.Split(',', StringSplitOptions.RemoveEmptyEntries));
        });
});

builder.Services.AddOpenApiDocument();

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
}

app.UseCors("AllowFrontend");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers().RequireAuthorization();

app.Run();

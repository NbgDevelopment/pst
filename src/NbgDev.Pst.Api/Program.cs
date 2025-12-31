using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using NbgDev.Pst.Api.Services;
using NbgDev.Pst.Api.Services.EventHandlers;
using NbgDev.Pst.Projects.AzureTable;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

// Add Microsoft Graph with client credentials flow (application permissions)
builder.Services.AddMicrosoftGraph(builder.Configuration.GetSection("MicrosoftGraph"));

builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssemblyContaining(typeof(BootstrapProjectsAzureTable));
    config.RegisterServicesFromAssemblyContaining(typeof(Program));
});

builder.AddAzureTableClient("Projects");
builder.Services.AddProjectsAzureTable();
builder.Services.AddScoped<IEntraIdService, EntraIdService>();

builder.AddAzureQueueClient("ApiQueues");
builder.AddAzureQueueClient("ProcessingQueues");
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

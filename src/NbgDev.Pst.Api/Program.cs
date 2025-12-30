using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using NbgDev.Pst.Api.Services;
using NbgDev.Pst.Projects.AzureTable;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssemblyContaining(typeof(BootstrapProjectsAzureTable));
});

builder.AddAzureTableClient("Projects");
builder.Services.AddProjectsAzureTable();

builder.AddAzureQueueClient("ApiQueues");
builder.Services.AddScoped<IEventPublisher, EventPublisher>();

builder.Services.AddControllers();
builder.Services.AddCors(cors =>
{
    cors.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.WithHeaders("content-type", "authorization");
            policy.AllowAnyMethod();
            policy.WithOrigins(builder.Configuration["Cors:AllowedOrigins"]!);
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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers().RequireAuthorization();

app.UseCors("AllowFrontend");

app.Run();

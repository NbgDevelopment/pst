using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using NbgDev.Pst.App;
using NbgDev.Pst.Web.Components;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

Console.WriteLine($"BaseUrl: {builder.Environment.ContentRootPath}");

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddMudServices();

// Add Azure AD authentication
// Get the authentication cookie expiration configuration once
var authCookieExpireDays = Math.Clamp(
    builder.Configuration.GetValue<int?>("AuthenticationCookieExpireDays") ?? 7,
    1, 30);

builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApp(options =>
    {
        builder.Configuration.GetSection("AzureAd").Bind(options);
        
        // Enable persistent cookies to keep login state across browser restarts
        options.SaveTokens = true;
        
        // Configure the authentication ticket to be persistent
        options.Events = new OpenIdConnectEvents
        {
            OnTicketReceived = context =>
            {
                // Make the authentication ticket persistent across browser sessions
                if (context.Properties != null)
                {
                    context.Properties.IsPersistent = true;
                    
                    // Set the expiration time for the authentication ticket
                    context.Properties.ExpiresUtc = DateTimeOffset.UtcNow.AddDays(authCookieExpireDays);
                }
                
                return Task.CompletedTask;
            }
        };
    })
    .EnableTokenAcquisitionToCallDownstreamApi()
    .AddInMemoryTokenCaches();

// Configure cookie authentication to persist across browser sessions
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = builder.Environment.IsDevelopment() 
        ? CookieSecurePolicy.SameAsRequest 
        : CookieSecurePolicy.Always;
    options.Cookie.SameSite = SameSiteMode.Lax;
    
    // Make cookies persistent - survive browser close
    // Use the same expiration as the authentication ticket
    options.ExpireTimeSpan = TimeSpan.FromDays(authCookieExpireDays);
    options.SlidingExpiration = true;
    
    // Keep the cookie persistent across browser sessions
    options.Cookie.IsEssential = true;
});

builder.Services.AddControllersWithViews()
    .AddMicrosoftIdentityUI();

builder.Services.AddAuthorization();
builder.Services.AddCascadingAuthenticationState();

builder.Services.AddHttpClient();
builder.Services.AddScoped<HttpClient>(sp =>
{
    var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
    return httpClientFactory.CreateClient();
});

builder.Services.AddApp(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddAdditionalAssemblies(typeof(NbgDev.Pst.App.Layout.MainLayout).Assembly);

app.MapControllers();

app.Run();

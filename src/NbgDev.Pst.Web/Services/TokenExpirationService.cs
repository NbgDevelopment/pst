using Microsoft.JSInterop;

namespace NbgDev.Pst.Web.Services;

/// <summary>
/// Service that checks for expired tokens in localStorage and clears them on application startup.
/// </summary>
public class TokenExpirationService
{
    private readonly IJSRuntime _jsRuntime;
    private IJSObjectReference? _module;

    public TokenExpirationService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    /// <summary>
    /// Checks localStorage for expired MSAL tokens and clears them if found.
    /// This ensures the app starts in a logged-out state when tokens are expired.
    /// </summary>
    public async Task ClearExpiredTokensAsync()
    {
        try
        {
            // Import the JavaScript module
            _module = await _jsRuntime.InvokeAsync<IJSObjectReference>(
                "import", "./js/tokenExpiration.js");
            
            // Call the function to clear expired tokens
            await _module.InvokeVoidAsync("clearExpiredMsalTokens");
        }
        catch (Exception ex)
        {
            // Log the error but don't throw - this is a best-effort check
            Console.Error.WriteLine($"Error checking/clearing expired tokens: {ex.Message}");
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_module is not null)
        {
            await _module.DisposeAsync();
        }
    }
}

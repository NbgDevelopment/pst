using Microsoft.JSInterop;

namespace NbgDev.Pst.Web.Services;

/// <summary>
/// Service that checks for expired tokens in localStorage and clears them on application startup.
/// </summary>
public class TokenExpirationService
{
    private readonly IJSRuntime _jsRuntime;

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
            await _jsRuntime.InvokeVoidAsync("eval",
                @"(() => {
                    try {
                        const now = Date.now();
                        const keysToRemove = [];
                        
                        // Iterate through localStorage to find MSAL cache entries
                        for (let i = 0; i < localStorage.length; i++) {
                            const key = localStorage.key(i);
                            if (key && key.includes('msal')) {
                                const value = localStorage.getItem(key);
                                if (value) {
                                    try {
                                        const parsed = JSON.parse(value);
                                        
                                        // Check if this entry has an expiration time
                                        // MSAL stores different types of data; we're looking for tokens with expiration
                                        if (parsed.secret || parsed.credentialType === 'AccessToken' || parsed.credentialType === 'IdToken') {
                                            // Check for expiration in various formats MSAL might use
                                            let expirationTime = null;
                                            
                                            if (parsed.expiresOn) {
                                                // expiresOn is typically a string timestamp
                                                expirationTime = new Date(parsed.expiresOn).getTime();
                                            } else if (parsed.expires_on) {
                                                // expires_on might be in seconds (Unix timestamp)
                                                expirationTime = parsed.expires_on * 1000;
                                            } else if (parsed.extendedExpiresOn) {
                                                expirationTime = new Date(parsed.extendedExpiresOn).getTime();
                                            }
                                            
                                            // If we found an expiration time and it's in the past, mark for removal
                                            if (expirationTime && expirationTime < now) {
                                                console.log('Found expired token in localStorage, will clear MSAL cache');
                                                // When we find any expired token, we should clear all MSAL cache
                                                // to ensure a clean logged-out state
                                                for (let j = 0; j < localStorage.length; j++) {
                                                    const k = localStorage.key(j);
                                                    if (k && k.includes('msal')) {
                                                        keysToRemove.push(k);
                                                    }
                                                }
                                                break; // No need to check further
                                            }
                                        }
                                    } catch (parseError) {
                                        // If we can't parse this entry, skip it
                                    }
                                }
                            }
                        }
                        
                        // Remove all marked keys
                        keysToRemove.forEach(key => {
                            localStorage.removeItem(key);
                        });
                        
                        if (keysToRemove.length > 0) {
                            console.log('Cleared ' + keysToRemove.length + ' expired MSAL cache entries from localStorage');
                        }
                    } catch (e) {
                        console.error('Error checking/clearing expired tokens:', e);
                    }
                })()");
        }
        catch (Exception ex)
        {
            // Log the error but don't throw - this is a best-effort check
            Console.Error.WriteLine($"Error checking/clearing expired tokens: {ex.Message}");
        }
    }
}

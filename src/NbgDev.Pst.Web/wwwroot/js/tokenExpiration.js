// Token expiration checker for MSAL authentication
// This module checks localStorage for expired MSAL tokens and clears them

export function clearExpiredMsalTokens() {
    try {
        const now = Date.now();
        const keysToRemove = [];
        let foundExpired = false;
        
        // First, collect all localStorage keys to avoid issues with changing collection
        const allKeys = [];
        for (let i = 0; i < localStorage.length; i++) {
            const key = localStorage.key(i);
            if (key) {
                allKeys.push(key);
            }
        }
        
        // Iterate through collected keys to find MSAL cache entries
        for (const key of allKeys) {
            if (key.includes('msal')) {
                const value = localStorage.getItem(key);
                if (value) {
                    try {
                        const parsed = JSON.parse(value);
                        
                        // Check if this entry has an expiration time
                        // MSAL stores different types of data; we're looking for tokens with expiration
                        const tokenTypes = ['AccessToken', 'IdToken'];
                        if (parsed.secret || tokenTypes.includes(parsed.credentialType)) {
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
                            
                            // If we found an expiration time and it's in the past
                            if (expirationTime && expirationTime < now) {
                                foundExpired = true;
                                console.log('Found expired token in localStorage');
                                break; // No need to check further
                            }
                        }
                    } catch (parseError) {
                        // If we can't parse this entry, skip it
                    }
                }
            }
        }
        
        // If we found any expired token, clear all MSAL cache entries
        // This ensures a clean logged-out state
        if (foundExpired) {
            for (const key of allKeys) {
                if (key.includes('msal')) {
                    keysToRemove.push(key);
                }
            }
            
            // Remove all marked keys
            keysToRemove.forEach(key => {
                localStorage.removeItem(key);
            });
            
            console.log(`Cleared ${keysToRemove.length} expired MSAL cache entries from localStorage`);
        }
        
        return true;
    } catch (e) {
        console.error('Error checking/clearing expired tokens:', e);
        return false;
    }
}

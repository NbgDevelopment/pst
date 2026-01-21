# Azure AD App Registration Setup for Blazor Server

This guide explains how to configure Azure AD app registration for the PST Blazor Server application after migrating from Blazor WebAssembly.

## Overview

The Blazor Server application uses Microsoft.Identity.Web for authentication with Azure AD. The migration from Blazor WebAssembly (MSAL) to Blazor Server (OpenID Connect) requires different app registration settings.

## Changes from WebAssembly to Blazor Server

| Aspect | Blazor WebAssembly (Old) | Blazor Server (New) |
|--------|-------------------------|---------------------|
| Authentication Flow | Implicit Grant + SPA | Authorization Code Flow |
| Token Acquisition | Client-side (MSAL.js) | Server-side (Microsoft.Identity.Web) |
| Redirect URIs | SPA redirect URIs | Web redirect URIs |
| Client Type | Public client (SPA) | Confidential client (Web App) |

## Prerequisites

- Azure subscription with appropriate permissions
- Permission to create/modify Azure AD app registrations
- Access to the Azure Portal or Azure CLI

## App Registration Configuration

### Required Settings

The application uses the following configuration (from `appsettings.json`):

```json
{
  "AzureAd": {
    "Instance": "https://login.microsoftonline.com/",
    "TenantId": "d683dd3f-22b5-4905-8d15-03e25604e757",
    "ClientId": "aa1b1263-6e41-4236-a260-877fc2607f78",
    "CallbackPath": "/signin-oidc"
  }
}
```

### 1. Create or Update App Registration

#### Using Azure Portal

1. Navigate to **Azure Active Directory** > **App registrations**
2. Either create a new registration or select the existing one:
   - **Name**: `pst-web-app` (or your preferred name)
   - **Supported account types**: Accounts in this organizational directory only
   - **Redirect URI**: Select **Web** platform
3. Note the following values:
   - **Application (client) ID**: Use this for `ClientId` in appsettings.json
   - **Directory (tenant) ID**: Use this for `TenantId` in appsettings.json

#### Using Azure CLI

```bash
# Create a new app registration
az ad app create --display-name "pst-web-app" \
  --sign-in-audience "AzureADMyOrg" \
  --web-redirect-uris "https://localhost:7004/signin-oidc"

# Note the appId and use it as ClientId
```

### 2. Configure Platform Settings

#### Using Azure Portal

1. In your App Registration, go to **Authentication**
2. Under **Platform configurations**, click **Add a platform**
3. Select **Web**
4. Configure the following:

   **Redirect URIs** (add all environments):
   - Development: `https://localhost:7004/signin-oidc`
   - Production: `https://your-production-domain/signin-oidc`

   **Front-channel logout URL**:
   - Development: `https://localhost:7004/signout-oidc`
   - Production: `https://your-production-domain/signout-oidc`

5. Under **Implicit grant and hybrid flows**:
   - ✅ Check **ID tokens** (used for sign-in)
   - ⬜ Uncheck **Access tokens** (not needed for Authorization Code Flow)

6. Click **Save**

#### Using Azure CLI

```bash
APP_ID="<your-app-id>"

# Add redirect URIs
az ad app update --id $APP_ID \
  --web-redirect-uris "https://localhost:7004/signin-oidc" "https://your-production-domain/signin-oidc"

# Configure logout URL
az ad app update --id $APP_ID \
  --web-home-page-url "https://localhost:7004" \
  --identifier-uris "api://$APP_ID"
```

### 3. Configure API Permissions (for calling downstream API)

The Blazor Server app needs permissions to call the PST API on behalf of the signed-in user.

#### Using Azure Portal

1. In your App Registration, go to **API permissions**
2. Click **Add a permission**
3. Select **My APIs** tab
4. Find and select your PST API app registration
5. Select **Delegated permissions**
6. Check the required scopes (e.g., `Manage`)
7. Click **Add permissions**
8. Click **Grant admin consent** (if you have admin permissions)

The application is configured to request this scope:
```json
{
  "PstApi": {
    "Scope": "https://nbgdev.onmicrosoft.com/pst-api-dev/Manage"
  }
}
```

#### Using Azure CLI

```bash
APP_ID="<your-app-id>"
API_APP_ID="<your-pst-api-app-id>"

# Get the API's scope ID (replace with your actual scope ID)
SCOPE_ID=$(az ad app show --id $API_APP_ID --query "oauth2Permissions[0].id" -o tsv)

# Add the API permission
az ad app permission add --id $APP_ID \
  --api $API_APP_ID \
  --api-permissions $SCOPE_ID=Scope

# Grant admin consent
az ad app permission admin-consent --id $APP_ID
```

### 4. Configure Token Configuration (Optional)

For enhanced security and functionality, you can configure optional claims:

#### Using Azure Portal

1. In your App Registration, go to **Token configuration**
2. Click **Add optional claim**
3. Select **ID** token type
4. Add the following claims:
   - `email`
   - `preferred_username`
   - `family_name`
   - `given_name`
5. Click **Add**

### 5. Client Secret (Production Only)

For production deployments, you may need to configure a client secret:

#### Using Azure Portal

1. In your App Registration, go to **Certificates & secrets**
2. Click **New client secret**
3. Enter a description: `pst-web-app-secret`
4. Select expiration: 24 months (or your organization's policy)
5. Click **Add**
6. **Important**: Copy the secret value immediately (it won't be shown again)
7. Store the secret securely in Azure Key Vault or your secret management system

#### Using Azure CLI

```bash
APP_ID="<your-app-id>"

# Create a client secret
az ad app credential reset --id $APP_ID \
  --append \
  --display-name "pst-web-app-secret" \
  --years 2

# Note: The secret value is shown only once. Store it securely.
```

For production, add the secret to your configuration:
```json
{
  "AzureAd": {
    "Instance": "https://login.microsoftonline.com/",
    "TenantId": "your-tenant-id",
    "ClientId": "your-client-id",
    "ClientSecret": "your-client-secret",
    "CallbackPath": "/signin-oidc"
  }
}
```

## Development Configuration

For local development, update `src/NbgDev.Pst.Web/appsettings.Development.json`:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning",
      "Microsoft.Identity": "Debug"
    }
  },
  "PstApi": {
    "Scope": "https://nbgdev.onmicrosoft.com/pst-api-dev/Manage",
    "ApiUrl": "https://localhost:7236"
  }
}
```

## Verification

After configuration, verify the setup:

1. **Start the application**:
   ```bash
   cd src/NbgDev.Pst/NbgDev.Pst.AppHost
   dotnet run
   ```

2. **Navigate to the web app**: `https://localhost:7004`

3. **Sign in**: Click the login link and authenticate with your Azure AD account

4. **Check the logs** for successful authentication:
   - Look for `Microsoft.Identity.Web` log entries
   - Verify token acquisition succeeds
   - Check that API calls include authorization headers

## Troubleshooting

### Common Issues

#### Issue: "AADSTS50011: The reply URL specified in the request does not match"

**Solution**: Ensure the redirect URI in Azure AD matches exactly (including protocol, domain, port, and path):
- Azure AD: `https://localhost:7004/signin-oidc`
- Application: Uses `/signin-oidc` as CallbackPath

#### Issue: "IDW10106: The 'ClientId' option must be provided"

**Solution**: Verify `appsettings.json` contains the correct ClientId and the file is in the project root (not wwwroot).

#### Issue: "No service for type 'IAuthorizationHeaderProvider' has been registered"

**Solution**: This is already fixed in the current implementation. Ensure `Program.cs` includes:
```csharp
builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAd"))
    .EnableTokenAcquisitionToCallDownstreamApi()
    .AddInMemoryTokenCaches();
```

#### Issue: "AADSTS65001: The user or administrator has not consented to use the application"

**Solution**: 
1. Ensure API permissions are properly configured
2. Grant admin consent for the required permissions
3. Or configure user consent settings in Azure AD

#### Issue: Tokens not being acquired for downstream API

**Solution**: 
1. Verify the scope is correctly configured in `appsettings.Development.json`
2. Check that the API permission is granted in Azure AD
3. Ensure the API app registration exposes the scope correctly

## Security Best Practices

1. **Use Client Secrets in Production**: Never commit secrets to source control
2. **Rotate Secrets Regularly**: Set expiration policies and rotate before expiry
3. **Use Azure Key Vault**: Store secrets in Azure Key Vault for production
4. **Limit Token Lifetime**: Configure appropriate token lifetimes in Azure AD
5. **Monitor Authentication**: Enable Azure AD sign-in logs and monitoring
6. **Use Managed Identity**: Where possible, use managed identity for Azure resources

## Additional Resources

- [Microsoft Identity Platform documentation](https://docs.microsoft.com/en-us/azure/active-directory/develop/)
- [Microsoft.Identity.Web documentation](https://github.com/AzureAD/microsoft-identity-web/wiki)
- [Blazor Server authentication and authorization](https://docs.microsoft.com/en-us/aspnet/core/blazor/security/server/)
- [Azure AD app registration reference](https://docs.microsoft.com/en-us/azure/active-directory/develop/quickstart-register-app)

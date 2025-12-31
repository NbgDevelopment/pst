# Azure Setup for CD Deployment

This guide explains how to set up Azure authentication for the CD (Continuous Deployment) workflow without using Personal Access Tokens (PATs) or client secrets. The workflow uses OpenID Connect (OIDC) with federated identity credentials for secure, secret-free authentication.

## Overview

The CD workflow authenticates to Azure using:
- **Azure AD App Registration** with federated identity credentials (OIDC) - no secrets required
- **SAS Token** for Terraform state storage backend (required for state management)
- **GitHub Container Registry credentials** for pulling Docker images

## Prerequisites

- Azure subscription with appropriate permissions
- GitHub repository admin access
- Azure CLI installed (for setup steps)
- Owner or User Access Administrator role on the Azure subscription

## 1. Create Azure AD App Registration

First, create an App Registration that will be used by GitHub Actions to authenticate to Azure.

### Using Azure Portal

1. Navigate to **Azure Active Directory** > **App registrations**
2. Click **New registration**
3. Enter the following details:
   - **Name**: `pst-github-actions` (or your preferred name)
   - **Supported account types**: Accounts in this organizational directory only
   - **Redirect URI**: Leave blank
4. Click **Register**
5. Note down the following values (you'll need them later):
   - **Application (client) ID** - this is your `DEPLOYMENT_CLIENT_ID`
   - **Directory (tenant) ID** - this is your `DEPLOYMENT_TENANT_ID`

### Using Azure CLI

#### Bash

```bash
# Create the app registration
az ad app create --display-name "pst-github-actions"

# Note the appId (DEPLOYMENT_CLIENT_ID) and copy it for the next step
APP_ID="<your-app-id>"

# Create a service principal for the app
az ad sp create --id $APP_ID

# Get your tenant ID (DEPLOYMENT_TENANT_ID)
az account show --query tenantId -o tsv
```

#### PowerShell

```powershell
# Create the app registration
az ad app create --display-name "pst-github-actions"

# Note the appId (DEPLOYMENT_CLIENT_ID) and copy it for the next step
$APP_ID = "<your-app-id>"

# Create a service principal for the app
az ad sp create --id $APP_ID

# Get your tenant ID (DEPLOYMENT_TENANT_ID)
az account show --query tenantId -o tsv
```

## 2. Configure Federated Identity Credentials

Federated identity credentials allow GitHub Actions to authenticate to Azure using OIDC tokens, eliminating the need for secrets.

### Using Azure Portal

1. In your App Registration, go to **Certificates & secrets**
2. Click the **Federated credentials** tab
3. Click **Add credential**
4. Select **GitHub Actions deploying Azure resources**
5. Configure the credential:
   - **Organization**: `NbgDevelopment` (your GitHub organization)
   - **Repository**: `pst` (your repository name)
   - **Entity type**: `Environment`
   - **Environment name**: `dev`
   - **Name**: `pst-dev-environment` (or your preferred name)
6. Click **Add**

> **Note**: If you deploy to multiple environments (e.g., staging, production), create a separate federated credential for each environment.

### Using Azure CLI

#### Bash

```bash
# Set variables
APP_ID="<your-app-id>"
GITHUB_ORG="NbgDevelopment"
GITHUB_REPO="pst"
ENVIRONMENT="dev"

# Create federated credential for the dev environment
az ad app federated-credential create \
  --id $APP_ID \
  --parameters "{
    \"name\": \"pst-dev-environment\",
    \"issuer\": \"https://token.actions.githubusercontent.com\",
    \"subject\": \"repo:${GITHUB_ORG}/${GITHUB_REPO}:environment:${ENVIRONMENT}\",
    \"description\": \"GitHub Actions federated credential for dev environment\",
    \"audiences\": [\"api://AzureADTokenExchange\"]
  }"
```

#### PowerShell

```powershell
# Set variables
$APP_ID = "<your-app-id>"
$GITHUB_ORG = "NbgDevelopment"
$GITHUB_REPO = "pst"
$ENVIRONMENT = "dev"

# Create federated credential for the dev environment
$subject = "repo:$GITHUB_ORG/$GITHUB_REPO:environment:$ENVIRONMENT"
az ad app federated-credential create `
  --id $APP_ID `
  --parameters @"
{
  \"name\": \"pst-dev-environment\",
  \"issuer\": \"https://token.actions.githubusercontent.com\",
  \"subject\": \"$subject\",
  \"description\": \"GitHub Actions federated credential for dev environment\",
  \"audiences\": [\"api://AzureADTokenExchange\"]
}
"@
```

## 3. Assign Azure RBAC Permissions

The App Registration's service principal needs permissions to manage Azure resources.

### Using Azure Portal

1. Navigate to your **Azure Subscription**
2. Go to **Access control (IAM)**
3. Click **Add** > **Add role assignment**
4. On the **Role** tab:
   - Select **Contributor** role (or a custom role with required permissions)
5. On the **Members** tab:
   - Select **User, group, or service principal**
   - Click **Select members**
   - Search for `pst-github-actions` (the app registration name)
   - Select it and click **Select**
6. Click **Review + assign**

### Using Azure CLI

#### Bash

```bash
# Set variables
APP_ID="<your-app-id>"
SUBSCRIPTION_ID="<your-subscription-id>"

# Get the service principal object ID
SP_OBJECT_ID=$(az ad sp show --id $APP_ID --query id -o tsv)

# Assign Contributor role at subscription level
az role assignment create \
  --assignee-object-id $SP_OBJECT_ID \
  --assignee-principal-type ServicePrincipal \
  --role Contributor \
  --scope /subscriptions/$SUBSCRIPTION_ID
```

#### PowerShell

```powershell
# Set variables
$APP_ID = "<your-app-id>"
$SUBSCRIPTION_ID = "<your-subscription-id>"

# Get the service principal object ID
$SP_OBJECT_ID = az ad sp show --id $APP_ID --query id -o tsv

# Assign Contributor role at subscription level
az role assignment create `
  --assignee-object-id $SP_OBJECT_ID `
  --assignee-principal-type ServicePrincipal `
  --role Contributor `
  --scope /subscriptions/$SUBSCRIPTION_ID
```

### Required Permissions

The service principal needs the following permissions:
- Create and manage Resource Groups
- Create and manage Container Apps
- Create and manage Storage Accounts
- Create and manage Log Analytics Workspaces
- Create and manage Application Insights

The **Contributor** role provides these permissions. If you prefer least-privilege access, create a custom role with only the required permissions.

## 4. Set Up Terraform State Storage

Terraform requires a storage backend to maintain state. We use Azure Blob Storage with SAS token authentication.

### Create Storage Resources

If you haven't already created the storage resources, create them:

#### Bash

```bash
# Set variables
LOCATION="germanywestcentral"
RG_NAME="rg-pst-management"
STORAGE_ACCOUNT="stpstmanagement"
CONTAINER_NAME="terraform-states"

# Create resource group
az group create --name $RG_NAME --location $LOCATION

# Create storage account
az storage account create \
  --name $STORAGE_ACCOUNT \
  --resource-group $RG_NAME \
  --location $LOCATION \
  --sku Standard_LRS \
  --allow-blob-public-access false

# Create container
az storage container create \
  --name $CONTAINER_NAME \
  --account-name $STORAGE_ACCOUNT
```

#### PowerShell

```powershell
# Set variables
$LOCATION = "germanywestcentral"
$RG_NAME = "rg-pst-management"
$STORAGE_ACCOUNT = "stpstmanagement"
$CONTAINER_NAME = "terraform-states"

# Create resource group
az group create --name $RG_NAME --location $LOCATION

# Create storage account
az storage account create `
  --name $STORAGE_ACCOUNT `
  --resource-group $RG_NAME `
  --location $LOCATION `
  --sku Standard_LRS `
  --allow-blob-public-access false

# Create container
az storage container create `
  --name $CONTAINER_NAME `
  --account-name $STORAGE_ACCOUNT
```

### Generate SAS Token

Create a SAS token with appropriate permissions and expiration:

#### Bash

```bash
# Set variables
STORAGE_ACCOUNT="stpstmanagement"

# For Linux/GNU systems:
EXPIRY_DATE=$(date -u -d "+1 year" '+%Y-%m-%dT%H:%M:%SZ')

# For macOS/BSD systems, use:
# EXPIRY_DATE=$(date -u -v+1y '+%Y-%m-%dT%H:%M:%SZ')

# Or specify the date manually (recommended for cross-platform compatibility):
# EXPIRY_DATE="2026-12-30T23:59:59Z"

# Generate SAS token
az storage account generate-sas \
  --account-name $STORAGE_ACCOUNT \
  --permissions cdlruwap \
  --services b \
  --resource-types sco \
  --expiry $EXPIRY_DATE \
  --https-only \
  --output tsv
```

#### PowerShell

```powershell
# Set variables
$STORAGE_ACCOUNT = "stpstmanagement"

# Calculate expiry date (1 year from now)
$EXPIRY_DATE = (Get-Date).AddYears(1).ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss") + "Z"

# Or specify the date manually:
# $EXPIRY_DATE = "2026-12-30T23:59:59Z"

# Generate SAS token
az storage account generate-sas `
  --account-name $STORAGE_ACCOUNT `
  --permissions cdlruwap `
  --services b `
  --resource-types sco `
  --expiry $EXPIRY_DATE `
  --https-only `
  --output tsv
```

> **Important**: Store this SAS token securely. You'll need to add it to GitHub Secrets as `SAS_TOKEN_STATES`.

> **Note**: SAS tokens have an expiration date. You'll need to rotate the token before it expires by generating a new one and updating the GitHub Secret.

## 5. Configure GitHub Repository

### Variables

Add the following variables to your GitHub repository (Settings > Secrets and variables > Actions > Variables):

| Variable Name | Description | Example Value |
|--------------|-------------|---------------|
| `DEPLOYMENT_CLIENT_ID` | Azure App Registration Application (client) ID | `12345678-1234-1234-1234-123456789abc` |
| `DEPLOYMENT_TENANT_ID` | Azure AD Directory (tenant) ID | `87654321-4321-4321-4321-cba987654321` |
| `SUBSCRIPTION_ID` | Azure Subscription ID | `abcdef12-3456-7890-abcd-ef1234567890` |
| `STAGE` | Environment/stage name | `dev` |
| `REGISTRY_USERNAME` | GitHub Container Registry username | `NbgDevelopment` |

### Secrets

Add the following secrets to your GitHub repository (Settings > Secrets and variables > Actions > Secrets):

| Secret Name | Description | Source |
|------------|-------------|--------|
| `SAS_TOKEN_STATES` | SAS token for Terraform state storage | Generated in step 4 |
| `REGISTRY_PASSWORD` | GitHub Container Registry password (PAT with packages:read) | GitHub Personal Access Token |

> **Note**: `REGISTRY_PASSWORD` is still required for Azure Container Apps to pull images from GitHub Container Registry. This is a GitHub PAT with `read:packages` permission, not an Azure credential.

### Environment Configuration

The workflow uses the `dev` environment. Configure it in your repository:

1. Go to repository **Settings** > **Environments**
2. Create or edit the `dev` environment
3. Optionally configure:
   - **Protection rules** (require reviewers, wait timer)
   - **Deployment branches** (restrict which branches can deploy)
   - **Environment secrets** (if you want environment-specific secrets)

## 6. Verify the Setup

After completing the setup:

1. Navigate to your GitHub repository
2. Go to **Actions** tab
3. Manually trigger the **Project Setup Tool CD** workflow (workflow_dispatch)
4. Monitor the workflow execution to ensure:
   - The Azure Login step succeeds
   - Terraform init connects to the state storage
   - Terraform apply completes successfully

## Troubleshooting

### Common Issues

**Error: "No subscription found"**
- Verify `SUBSCRIPTION_ID` is correct
- Ensure the service principal has access to the subscription

**Error: "AADSTS70021: No matching federated identity record found"**
- Check that the federated credential configuration matches exactly:
  - Organization name
  - Repository name
  - Environment name
- Ensure the GitHub environment name matches the workflow configuration

**Error: "Authorization failed"**
- Verify the service principal has the Contributor role (or required permissions)
- Check the role assignment scope (should be at subscription level)

**Error: "Failed to get existing workspaces: storage: service returned error"**
- Verify `SAS_TOKEN_STATES` is valid and not expired
- Check that the storage account and container exist
- Ensure the SAS token has the correct permissions (cdlruwap)

### Rotating SAS Token

When the SAS token approaches expiration:

1. Generate a new SAS token (see step 4)
2. Update the `SAS_TOKEN_STATES` secret in GitHub
3. Test by triggering a deployment

### Additional Environments

To deploy to additional environments (e.g., staging, production):

1. Create a new federated credential for each environment (step 2)
2. Create corresponding GitHub environments
3. Add environment-specific jobs to the CD workflow

## Security Considerations

- **Least Privilege**: Consider using a custom Azure role with minimum required permissions instead of Contributor
- **Token Expiration**: Set appropriate expiration dates for SAS tokens and rotate them regularly
- **Environment Protection**: Use GitHub environment protection rules to require approvals for production deployments
- **Audit Logging**: Enable Azure Activity Log and review deployment activities regularly
- **Secrets Management**: Never commit secrets to version control; always use GitHub Secrets

## 7. Configure Entra ID API Permissions for Member Management

The Project Setup Tool includes a member management feature that searches for users in your Entra ID tenant. To enable this functionality, you need to grant the API's App Registration the necessary Microsoft Graph permissions.

### Create API App Registration

If you haven't already created a separate App Registration for the API itself (different from the deployment app registration), create one:

#### Using Azure Portal

1. Navigate to **Azure Active Directory** > **App registrations**
2. Click **New registration**
3. Enter the following details:
   - **Name**: `pst-api` (or your preferred name)
   - **Supported account types**: Accounts in this organizational directory only
   - **Redirect URI**: Leave blank
4. Click **Register**
5. Note down the **Application (client) ID** and **Directory (tenant) ID**

#### Using Azure CLI

```bash
# Create the app registration for the API
az ad app create --display-name "pst-api"

# Note the appId and copy it for the next step
API_APP_ID="<your-api-app-id>"

# Create a service principal for the app
az ad sp create --id $API_APP_ID
```

### Grant Microsoft Graph Permissions

The API needs the `User.Read.All` permission to search for users in your tenant.

#### Using Azure Portal

1. Navigate to your API App Registration (`pst-api`)
2. Go to **API permissions**
3. Click **Add a permission**
4. Select **Microsoft Graph**
5. Choose **Application permissions** (not Delegated)
6. Search for and select **User.Read.All**
7. Click **Add permissions**
8. Click **Grant admin consent for [Your Organization]** (requires admin privileges)
   - This step is critical - without admin consent, the API won't be able to search for users

#### Using Azure CLI

```bash
# Set variables
API_APP_ID="<your-api-app-id>"

# Add the User.Read.All application permission to the app
# The permission ID for User.Read.All is df021288-bdef-4463-88db-98f22de89214
az ad app permission add \
  --id $API_APP_ID \
  --api 00000003-0000-0000-c000-000000000000 \
  --api-permissions df021288-bdef-4463-88db-98f22de89214=Role

# Grant admin consent for the permissions
az ad app permission admin-consent --id $API_APP_ID
```

### Create a Client Secret or Certificate

The API needs credentials to authenticate to Microsoft Graph. You can use either a client secret or a certificate.

#### Option 1: Client Secret (Simpler, but requires rotation)

##### Using Azure Portal

1. In your API App Registration, go to **Certificates & secrets**
2. Click **New client secret**
3. Enter a description (e.g., "API Microsoft Graph Access")
4. Select an expiration period (recommended: 24 months or less)
5. Click **Add**
6. **Copy the secret value immediately** - it won't be shown again
7. Add this secret to your application configuration as `AzureAd:ClientSecret`

##### Using Azure CLI

```bash
# Create a client secret (expires in 2 years)
az ad app credential reset --id $API_APP_ID --years 2
```

#### Option 2: Certificate (More secure, no rotation needed)

1. Generate a certificate or use an existing one
2. In the Azure Portal, go to **Certificates & secrets** > **Certificates**
3. Upload your certificate
4. Configure your application to use certificate-based authentication

### Update Application Configuration

Update your API's `appsettings.json` or Azure Configuration to include:

```json
{
  "AzureAd": {
    "Instance": "https://login.microsoftonline.com/",
    "TenantId": "<your-tenant-id>",
    "ClientId": "<your-api-app-id>",
    "ClientSecret": "<your-client-secret>",
    "Audience": "<your-api-audience>"
  },
  "MicrosoftGraph": {
    "BaseUrl": "https://graph.microsoft.com/v1.0",
    "Scopes": "https://graph.microsoft.com/.default"
  }
}
```

**Important Notes:**
- The `ClientSecret` should be stored securely (use Azure Key Vault, user secrets, or environment variables in production)
- Never commit secrets to source control
- The `Scopes` value should be `https://graph.microsoft.com/.default` for application permissions (not `User.Read.All`)
- The application uses **application permissions** (client credentials flow), not delegated permissions

### Security Best Practices

- **Minimize Permissions**: The API only requests `User.Read.All`, which is the minimum permission needed to search for users
- **Use Managed Identity**: In production, consider using Azure Managed Identity instead of client secrets when running in Azure
- **Rotate Secrets**: If using client secrets, rotate them regularly before they expire
- **Monitor Usage**: Review the API's Microsoft Graph usage regularly through Azure AD audit logs
- **Conditional Access**: Consider applying Conditional Access policies to the API's service principal

### Troubleshooting

**Error: "Insufficient privileges to complete the operation"**
- Ensure admin consent has been granted for the `User.Read.All` permission
- Verify the App Registration has the correct permissions assigned

**Error: "AADSTS7000215: Invalid client secret"**
- The client secret may have expired or is incorrect
- Generate a new client secret and update your configuration

**No users returned from search**
- Verify the API is using the correct tenant ID
- Check that the search term matches users in your directory
- Ensure the users have the required properties (mail, givenName, surname)

## Resources

- [Azure Federated Identity Credentials](https://learn.microsoft.com/en-us/azure/active-directory/develop/workload-identity-federation)
- [GitHub Actions OIDC](https://docs.github.com/en/actions/deployment/security-hardening-your-deployments/about-security-hardening-with-openid-connect)
- [Azure Login GitHub Action](https://github.com/Azure/login)
- [Terraform Azure Backend](https://developer.hashicorp.com/terraform/language/settings/backends/azurerm)
- [Microsoft Graph Permissions Reference](https://learn.microsoft.com/en-us/graph/permissions-reference)

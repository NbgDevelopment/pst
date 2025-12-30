# Azure Infrastructure Deployment

This directory contains the Terraform configuration for deploying the PST (Project Setup Tool) infrastructure to Azure.

## Container Apps Configuration

All container apps in this project are configured to scale down to 0 replicas when idle:

- **API Container App** (`resources/api/main.tf`): min_replicas = 0
- **Web Container App** (`resources/web/main.tf`): min_replicas = 0  
- **Processing Container App** (`resources/processing/main.tf`): min_replicas = 0

### Important: Always Scale to 0

**All container apps must always be configured to scale down to 0 replicas.** This ensures cost optimization by not consuming resources when the applications are not in use.

When adding new container apps or modifying existing ones, ensure that the `min_replicas` value in the `template` block is set to `0`:

```hcl
template {
  min_replicas = 0
  max_replicas = <desired_max>
  # ... rest of configuration
}
```

## Deployment

### Local Deployment (Debug)

To deploy the infrastructure locally:

1. Navigate to this directory
2. Run `Deploy-Debug.ps1`
3. Review the Terraform plan
4. Confirm deployment when prompted

### Production Deployment

Production deployments are handled via GitHub Actions (see `.github/workflows/CD.yml`).

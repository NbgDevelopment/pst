resource "azurerm_container_app" "processing" {
  name                         = "aca-pst-${var.stage}-processing"
  container_app_environment_id = var.container_app_environment_id
  resource_group_name          = var.resource_group_name
  tags                         = var.tags
  revision_mode                = "Single"

  template {
    min_replicas = 0
    max_replicas = 2
    container {
      name   = "pst-processing"
      image  = var.image
      cpu    = 0.25
      memory = "0.5Gi"
      env {
        name  = "APPINSIGHTS_INSTRUMENTATIONKEY"
        value = var.app_insights_instrumentation_key
      }
      env {
        name        = "ConnectionStrings__ApiQueues"
        secret_name = "connectionstring-queues"
      }
      env {
        name        = "ConnectionStrings__ProcessingQueues"
        secret_name = "connectionstring-queues"
      }
      env {
        name  = "ASPNETCORE_ENVIRONMENT"
        value = var.stage == "dev" ? "Development" : var.stage == "debug" ? "Development" : "Production"
      }
      env {
        name  = "AzureAd__TenantId"
        value = var.azure_ad_tenant_id
      }
      env {
        name  = "AzureAd__ClientId"
        value = var.azure_ad_client_id
      }
      env {
        name        = "AzureAd__ClientSecret"
        secret_name = "azure-ad-client-secret"
      }
      env {
        name  = "Stage"
        value = var.stage
      }
    }
  }

  secret {
    name  = "registry-password"
    value = var.registry_password
  }

  secret {
    name  = "connectionstring-queues"
    value = var.connectionstring_queues
  }

  secret {
    name  = "azure-ad-client-secret"
    value = var.azure_ad_client_secret
  }

  registry {
    server               = "ghcr.io"
    username             = var.registry_username
    password_secret_name = "registry-password"
  }

  ingress {
    target_port      = 8080
    external_enabled = false
    transport        = "http"
    traffic_weight {
      latest_revision = true
      percentage      = 100
    }
  }
}

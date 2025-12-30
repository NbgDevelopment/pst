resource "azurerm_container_app" "processing" {
  name                         = "aca-pst-${var.stage}-processing"
  container_app_environment_id = var.container_app_environment_id
  resource_group_name          = var.resource_group_name
  tags                         = var.tags
  revision_mode                = "Single"

  template {
    min_replicas = 1
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

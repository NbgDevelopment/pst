resource "azurerm_container_app" "api" {
  name                         = "aca-pst-${var.stage}-api"
  container_app_environment_id = var.container_app_environment_id
  resource_group_name          = var.resource_group_name
  tags                         = var.tags
  revision_mode                = "Single"

  template {
    min_replicas = 0
    max_replicas = 2
    http_scale_rule {
      name                = "http-scale-rule"
      concurrent_requests = 100
    }
    container {
      name   = "pst-api"
      image  = var.image
      cpu    = 0.25
      memory = "0.5Gi"
      env {
        name  = "APPINSIGHTS_INSTRUMENTATIONKEY"
        value = var.app_insights_instrumentation_key
      }
      env {
        name  = "ConnectionStrings__Projects"
        secret_name = "connectionstring-projects"
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
    name = "connectionstring-projects"
    value = var.connectionstring_projects
  }

  registry {
    server   = "ghcr.io"
    username = var.registry_username
    password_secret_name = "registry-password"
  }

  ingress {
    target_port      = 8080
    external_enabled = true
    transport        = "http"
    traffic_weight {
      latest_revision = true
      percentage = 100
    }
  }
}
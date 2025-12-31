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
        name        = "ConnectionStrings__Projects"
        secret_name = "connectionstring-projects"
      }
      env {
        name        = "ConnectionStrings__ApiQueues"
        secret_name = "connectionstring-queues"
      }
      env {
        name  = "ASPNETCORE_ENVIRONMENT"
        value = var.stage == "dev" ? "Development" : var.stage == "debug" ? "Development" : "Production"
      }
      env {
        name  = "Cors__AllowedOrigins"
        value = var.web_fqdn
      }
    }
  }

  secret {
    name  = "registry-password"
    value = var.registry_password
  }

  secret {
    name  = "connectionstring-projects"
    value = var.connectionstring_projects
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
    external_enabled = true
    transport        = "http"
    traffic_weight {
      latest_revision = true
      percentage      = 100
    }
    cors {
      allowed_origins     = [var.web_fqdn]
      allowed_methods     = ["GET", "POST", "PUT", "DELETE", "OPTIONS", "PATCH"]
      allowed_headers     = ["Content-Type", "Authorization", "Accept", "Origin", "X-Requested-With"]
      expose_headers      = ["Content-Length", "Content-Type"]
      allow_credentials   = true
      max_age             = 3600
    }
  }
}
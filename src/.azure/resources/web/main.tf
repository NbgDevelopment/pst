resource "azurerm_container_app" "web" {
  name                         = "aca-pst-${var.stage}-web"
  container_app_environment_id = var.container_app_environment_id
  resource_group_name          = var.resource_group_name
  tags                         = var.tags
  revision_mode                = "Single"

  template {
    min_replicas = 0
    max_replicas = 1
    http_scale_rule {
      name                = "http-scale-rule"
      concurrent_requests = 100
    }
    container {
      name   = "pst-web"
      image  = var.image
      cpu    = 0.25
      memory = "0.5Gi"
      env {
        name  = "stage"
        value = var.stage
      }
      env {
        name  = "API_URL"
        value = var.api_url
      }
    }
  }

  secret {
    name  = "registry-password"
    value = var.registry_password
  }

  registry {
    server               = "ghcr.io"
    username             = var.registry_username
    password_secret_name = "registry-password"
  }

  ingress {
    target_port      = 80
    external_enabled = true
    transport        = "http"
    traffic_weight {
      latest_revision = true
      percentage      = 100
    }
  }
}
resource "azurerm_container_app" "redis" {
  name                         = "aca-pst-${var.stage}-redis"
  container_app_environment_id = var.container_app_environment_id
  resource_group_name          = var.resource_group_name
  tags                         = var.tags
  revision_mode                = "Single"

  template {
    min_replicas = 1
    max_replicas = 1

    container {
      name   = "redis"
      image  = "redis:7-alpine"
      cpu    = 0.25
      memory = "0.5Gi"

      # Add volume mount for data persistence
      volume_mounts {
        name = "redis-data"
        path = "/data"
      }
    }

    # Define volume for persistent storage
    volume {
      name         = "redis-data"
      storage_type = "AzureFile"
      storage_name = azurerm_container_app_environment_storage.redis_storage.name
    }
  }

  # Internal ingress only - not exposed externally
  ingress {
    target_port      = 6379
    external_enabled = false
    transport        = "tcp"
    traffic_weight {
      latest_revision = true
      percentage      = 100
    }
  }
}

# Create persistent storage for Redis data
resource "azurerm_container_app_environment_storage" "redis_storage" {
  name                         = "redis-data"
  container_app_environment_id = var.container_app_environment_id
  account_name                 = azurerm_storage_account.redis_storage.name
  share_name                   = azurerm_storage_share.redis_data.name
  access_key                   = azurerm_storage_account.redis_storage.primary_access_key
  access_mode                  = "ReadWrite"
}

resource "azurerm_storage_account" "redis_storage" {
  name                     = "stpstredis${var.stage}"
  resource_group_name      = var.resource_group_name
  location                 = var.location
  account_tier             = "Standard"
  account_replication_type = "LRS"
  tags                     = var.tags
}

resource "azurerm_storage_share" "redis_data" {
  name                 = "redis-data"
  storage_account_name = azurerm_storage_account.redis_storage.name
  quota                = 1
}

resource "azurerm_storage_account" "storage" {
  name                     = "stpst${var.stage}"
  resource_group_name      = var.resource_group_name
  location                 = var.location
  tags                     = var.tags
  account_tier             = "Standard"
  account_replication_type = "LRS"
}

resource "azurerm_storage_queue" "api_events" {
  name               = "api-events"
  storage_account_id = azurerm_storage_account.storage.id
}

resource "azurerm_storage_queue" "processing_events" {
  name               = "processing-events"
  storage_account_id = azurerm_storage_account.storage.id
}
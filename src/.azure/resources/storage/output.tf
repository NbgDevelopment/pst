output "primary_table_connectionstring" {
  value     = "DefaultEndpointsProtocol=http;AccountName=${azurerm_storage_account.storage.name};AccountKey=${azurerm_storage_account.storage.primary_access_key};TableEndpoint=${azurerm_storage_account.storage.primary_table_endpoint};"
  sensitive = false
}

output "primary_queue_connectionstring" {
  value     = "DefaultEndpointsProtocol=http;AccountName=${azurerm_storage_account.storage.name};AccountKey=${azurerm_storage_account.storage.primary_access_key};QueueEndpoint=${azurerm_storage_account.storage.primary_queue_endpoint};"
  sensitive = false
}
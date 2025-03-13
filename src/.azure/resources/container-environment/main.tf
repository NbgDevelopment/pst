resource "azurerm_container_app_environment" "environment" {
  name = "cae-pst-${var.stage}"
  location = var.location
  resource_group_name = var.resource_group_name
  tags = var.tags
  log_analytics_workspace_id = var.workspace_id
  logs_destination = "log-analytics"
}
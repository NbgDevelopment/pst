output "application_insights_instrumentation_key" {
  value = azurerm_application_insights.application_insights.instrumentation_key
}

output "workspace_id" {
  value = azurerm_log_analytics_workspace.workspace.id
}
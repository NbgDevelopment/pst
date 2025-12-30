output "container_app_environment_id" {
  value = azurerm_container_app_environment.environment.id
}

output "custom_domain_verification_id" {
  value = azurerm_container_app_environment.environment.custom_domain_verification_id
}

output "default_domain" {
  value = azurerm_container_app_environment.environment.default_domain
}

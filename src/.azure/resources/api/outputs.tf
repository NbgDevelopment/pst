output "api_fqdn" {
  value = "https://${azurerm_container_app.api.ingress[0].fqdn}"
}

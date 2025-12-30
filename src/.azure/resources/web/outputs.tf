output "web_fqdn" {
  value = "https://${azurerm_container_app.web.ingress[0].fqdn}"
}

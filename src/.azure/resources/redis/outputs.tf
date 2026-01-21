output "redis_fqdn" {
  value       = azurerm_container_app.redis.ingress[0].fqdn
  description = "The FQDN of the Redis container app"
}

output "redis_connection_string" {
  value       = "${azurerm_container_app.redis.ingress[0].fqdn}:6379"
  description = "The Redis connection string"
  sensitive   = true
}

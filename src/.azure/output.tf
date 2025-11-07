output "container_environment_custom_domain_verification_id" {
  value = module.container-environment.custom_domain_verification_id
}

output "connectionstring_projects" {
  value     = module.storage.primary_table_connectionstring
  sensitive = true
}
variable "stage" {
  type = string
  validation {
    condition     = length(var.stage) >= 3 && length(var.stage) <= 5
    error_message = "The length of the stage parameter must be between 3 and 5 characters"
  }
}

variable "tags" {
  type = map(string)
}

variable "location" {
  type = string
}

variable "resource_group_name" {
  type = string
}

variable "container_app_environment_id" {
  type = string
}

variable "registry_username" {
  type = string
}

variable "registry_password" {
  type      = string
  sensitive = true
}

variable "image" {
  type = string
}

variable "app_insights_instrumentation_key" {
  type = string
}

variable "connectionstring_projects" {
  type      = string
  sensitive = true
}

variable "connectionstring_queues" {
  type      = string
  sensitive = true
}

variable "web_fqdn" {
  type        = string
  description = "The FQDN of the web application for CORS configuration"
}

variable "azure_ad_tenant_id" {
  type        = string
  description = "Azure AD Tenant ID for API authentication"
}

variable "azure_ad_client_id" {
  type        = string
  description = "Azure AD Client ID (App Registration) for the API"
}

variable "azure_ad_client_secret" {
  type        = string
  sensitive   = true
  description = "Azure AD Client Secret for the API to authenticate to Microsoft Graph"
}

variable "azure_ad_audience" {
  type        = string
  description = "Azure AD Audience (API identifier URI)"
}

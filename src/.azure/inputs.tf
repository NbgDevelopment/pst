variable "subscription_id" {
  type = string
}

variable "stage" {
  type = string
  validation {
    condition     = length(var.stage) >= 3 && length(var.stage) <= 5
    error_message = "The length of the stage parameter must be between 3 and 5 characters"
  }
}

variable "registry_username" {
  type = string
}

variable "registry_password" {
  type      = string
  sensitive = true
}

variable "image_api" {
  type = string
}

variable "image_web" {
  type = string
}

variable "image_processing" {
  type = string
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

variable "processing_azure_ad_client_id" {
  type        = string
  description = "Azure AD Client ID (App Registration) for the Processing app"
}

variable "processing_azure_ad_client_secret" {
  type        = string
  sensitive   = true
  description = "Azure AD Client Secret for the Processing app to authenticate to Microsoft Graph"
}
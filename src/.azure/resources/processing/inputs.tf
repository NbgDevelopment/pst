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
  type = string
  sensitive = true
}

variable "image" {
  type = string
}

variable "app_insights_instrumentation_key" {
  type = string
}

variable "connectionstring_queues" {
  type      = string
  sensitive = true
}

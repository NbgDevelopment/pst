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
# Configure the Azure provider
terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "4.22.0"
    }
  }

  backend "azurerm" {
    resource_group_name = "rg-pst-management"
    storage_account_name = "stpstmanagement"
    container_name = "terraform-states"
  }

  required_version = ">= 1.11.2"
}

provider "azurerm" {
  subscription_id = var.subscription_id
  features {}
}
resource "azurerm_resource_group" "rg" {
  name     = "rg-pst-${var.stage}"
  location = "germanywestcentral"
}

module "monitoring" {
  source = "./stages/resources/monitoring"
  stage = var.stage
  location = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name
  tags = local.tags
}

module "container-environment" {
  source = "./stages/resources/container-environment"
  stage = var.stage
  location = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name
  tags = local.tags
  workspace_id = module.monitoring.workspace_id
}
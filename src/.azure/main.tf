resource "azurerm_resource_group" "rg" {
  name     = "rg-pst-${var.stage}"
  location = "germanywestcentral"
}

module "monitoring" {
  source              = "./resources/monitoring"
  stage               = var.stage
  location            = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name
  tags                = local.tags
}

module "storage" {
  source              = "./resources/storage"
  stage               = var.stage
  location            = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name
  tags                = local.tags
}

module "container-environment" {
  source              = "./resources/container-environment"
  stage               = var.stage
  location            = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name
  tags                = local.tags
  workspace_id        = module.monitoring.workspace_id
}

module "api" {
  source                           = "./resources/api"
  stage                            = var.stage
  location                         = azurerm_resource_group.rg.location
  resource_group_name              = azurerm_resource_group.rg.name
  tags                             = local.tags
  container_app_environment_id     = module.container-environment.container_app_environment_id
  image                            = var.image_api
  app_insights_instrumentation_key = module.monitoring.application_insights_instrumentation_key
  connectionstring_projects        = module.storage.primary_table_connectionstring
  registry_username                = var.registry_username
  registry_password                = var.registry_password
}

module "web" {
  source                           = "./resources/web"
  stage                            = var.stage
  location                         = azurerm_resource_group.rg.location
  resource_group_name              = azurerm_resource_group.rg.name
  tags                             = local.tags
  container_app_environment_id     = module.container-environment.container_app_environment_id
  image                            = var.image_web
  registry_username                = var.registry_username
  registry_password                = var.registry_password
}
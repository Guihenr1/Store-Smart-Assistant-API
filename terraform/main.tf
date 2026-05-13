resource "azurerm_resource_group" "main" {
  name     = var.resource_group_name
  location = var.location
}

module "openai" {
  source = "./modules/openai"

  resource_group_name = azurerm_resource_group.main.name
  location            = azurerm_resource_group.main.location
  openai_name         = "${var.project_name}-openai"
}

module "postgres" {
  source = "./modules/postgres"

  resource_group_name = azurerm_resource_group.main.name
  location            = azurerm_resource_group.main.location
  project_name        = var.project_name
  admin_password      = var.postgres_admin_password
}

module "app_service" {
  source = "./modules/app-service"

  resource_group_name = azurerm_resource_group.main.name
  location            = azurerm_resource_group.main.location
  project_name        = var.project_name
  app_service_sku     = var.app_service_sku

  openai_endpoint     = var.openai_endpoint
  openai_key          = var.openai_key

  depends_on = [module.postgres]
}


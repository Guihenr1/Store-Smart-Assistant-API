resource "azurerm_service_plan" "main" {
  name                = "${var.project_name}-plan"
  resource_group_name = var.resource_group_name
  location            = var.location
  os_type             = "Linux"
  sku_name            = var.app_service_sku
}

resource "azurerm_linux_web_app" "main" {
  name                = "${var.project_name}-api"
  resource_group_name = var.resource_group_name
  location            = var.location
  service_plan_id     = azurerm_service_plan.main.id

  site_config {
    application_stack {
      dotnet_version = "10.0"        # Change to 9.0 when .NET 9 is fully supported
    }

    always_on = true
  }

  app_settings = {
    "ASPNETCORE_ENVIRONMENT"          = "Production"
    "ConnectionStrings__DefaultConnection" = "@Microsoft.KeyVault(SecretUri=${var.key_vault_uri}secrets/postgres-connection-string/)"
    "AzureOpenAI__Endpoint"           = var.openai_endpoint
    "AzureOpenAI__ApiKey"             = var.openai_key
    "AzureOpenAI__ChatDeployment"     = "gpt-4o"
    "AzureOpenAI__EmbeddingDeployment" = "text-embedding-3-large"
  }

  identity {
    type = "SystemAssigned"
  }

  tags = {
    Environment = "Production"
  }
}

output "app_service_url" {
  value = "https://${azurerm_linux_web_app.main.default_hostname}"
}

output "app_service_name" {
  value = azurerm_linux_web_app.main.name
}

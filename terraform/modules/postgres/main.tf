resource "azurerm_postgresql_flexible_server" "main" {
  name                = "${var.project_name}-postgres"
  resource_group_name = var.resource_group_name
  location            = var.location
  version             = "16"
  sku_name            = "Standard_B1ms"   # Adjust based on needs

  storage_mb = 32768
  backup_retention_days = 7

  administrator_login    = "postgres"
  administrator_password = var.admin_password

  tags = {
    Environment = "Production"
  }
}

resource "azurerm_postgresql_flexible_server_firewall_rule" "allow_all_azure" {
  name             = "AllowAllAzureServices"
  server_id        = azurerm_postgresql_flexible_server.main.id
  start_ip_address = "0.0.0.0"
  end_ip_address   = "0.0.0.0"
}

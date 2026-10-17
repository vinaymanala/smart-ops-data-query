resource "azurerm_service_plan" "sp" {
  name                = "${var.name}-sp"
  resource_group_name = var.resource_group_name
  location            = var.location
  os_type             = "Linux"
  sku_name            = "B1" # B1 - Basic tier supports VNet integration & access restrictions
}


resource "azurerm_app_service_virtual_network_swift_connection" "app_service_vent_integration" {
  app_service_id = azurerm_service_plan.sp.id
  subnet_id      = var.subnet_id
}

# Web app
resource "azurerm_linux_web_app" "web-app" {
  name                = var.name
  resource_group_name = var.resource_group_name
  location            = var.location
  service_plan_id     = azurerm_service_plan.sp.id
  identity {
    type         = "UserAssigned"
    identity_ids = [var.uami]
  }
  site_config {
    application_stack {
      dotnet_version = "10.0"
    }

    ip_restriction {
      name        = "AllowOnlySWA"
      service_tag = "AzureStaticWebApps"
      priority    = 100
      action      = "Allow"
    }
  }
  tags = var.tags
}

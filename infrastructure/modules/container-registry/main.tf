resource "azurerm_container_registry" "acr" {
  name                = var.name
  resource_group_name = var.resource_group_name
  location            = var.location
  sku                 = "Basic"
  admin_enabled       = false
}

# RBAC for arcpull webapp_to_container_registry
resource "azurerm_role_assignment" "webapp_to_container_registry" {
  principal_id                     = var.uami
  role_definition_name             = "AcrPull"
  scope                            = azurerm_container_registry.acr.id
  skip_service_principal_aad_check = true
}

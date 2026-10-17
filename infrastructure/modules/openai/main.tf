resource "azurerm_cognitive_account" "openai" {
  name                = var.name
  resource_group_name = var.resource_group_name
  location            = var.location
  kind                = "OpenAI"
  sku_name            = "S0"
  tags                = var.tags

  identity {
    type = "SystemAssigned"
  }

  public_network_access_enabled = var.enable_private_endpoint ? false : true
  custom_subdomain_name         = var.name # Require for private endpoints
}

resource "azurerm_cognitive_deployment" "models" {
  for_each = var.deployments

  name                 = each.key
  cognitive_account_id = azurerm_cognitive_account.openai.id

  model {
    format  = "OpenAI"
    name    = each.value.model_name
    version = each.value.model_version
  }

  sku {
    name     = each.value.model_name
    capacity = each.value.capacity
    size     = each.value.scale_type
  }

  rai_policy_name = "Microsoft.Default"
}

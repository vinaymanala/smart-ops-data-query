
resource "azurerm_search_service" "main" {
  name = var.name
  resource_group_name = var.resource_group_name
  location = var.location
  sku = var.sku
  tags = var.tags

  # Semantic search (required for RAG)
  semantic_search_sku = var.enable_semantic_search ? "standard" : null

  # Security settings
  public_network_access_enabled = var.enable_private_endpoint ? false : true

  identity {
    type = "SystemAssigned"
  }
}
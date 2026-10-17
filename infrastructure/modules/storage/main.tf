resource "azurerm_storage_account" "main" {
  name                     = var.name
  resource_group_name      = var.resource_group_name
  location                 = var.location
  account_tier             = "Standard"
  account_replication_type = var.replication_type
  account_kind             = "StorageV2"
  tags                     = var.tags

  https_traffic_only_enabled      = true
  allow_nested_items_to_be_public = false

  identity {
    type = "SystemAssigned"
  }

  # Blob properties
  blob_properties {
    versioning_enabled = true
    delete_retention_policy {
      days = var.soft_delete_retention_days
    }
    container_delete_retention_policy {
      days = var.soft_delete_retention_days
    }
  }
}

# Container for RAG documents
resource "azurerm_storage_container" "documents" {
  name                  = "documents"
  storage_account_id = azurerm_storage_account.main.id  
  container_access_type = "private"  
}

resource "azurerm_storage_container" "embeddings" {
  name                  = "embeddings"
  storage_account_id  = azurerm_storage_account.main.id
  container_access_type = "private"
}

# RBAC for AI Search to read blobs
resource "azurerm_role_assignment" "search_to_storage" {  
  scope                = azurerm_storage_account.main.id
  role_definition_name = "Storage Blob Data Reader"
  principal_id         = var.search_principal_id
}

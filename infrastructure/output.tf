output "search_endpoint" {
  description = "Azure AI Search endpoint"
  value       = module.ai_search.endpoint
}

output "openai_endpoint" {
  description = "Azure OpenAI endpoint"
  value       = module.openai.endpoint
}

output "storage_account_name" {
  description = "Storage account name"
  value       = module.storage.name
}

output "key_vault_uri" {
  description = "Key Vault URI for retrieving secrets"
  value       = module.key_vault.uri
}

output "resource_group_name" {
  description = "Resource group name"
  value       = azurerm_resource_group.main.name
}

# Sensitive outputs
output "connection_info" {
  description = "Connection information for applications"
  value = {
    search_endpoint   = module.ai_search.endpoint
    openai_endpoint   = module.openai.endpoint
    storage_account   = module.storage.name
    key_vault_uri     = module.key_vault.uri
  }
  sensitive = true
}

output "id" {
  description = "Key Vault id"
  value = azurerm_key_vault.kv.id
}

output "uri" {
  description = "Key Vault uri"
  value = azurerm_key_vault.kv.vault_uri
}
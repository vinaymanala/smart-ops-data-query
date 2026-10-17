output "primary_connection_string" {
  description = "primary_connection_string"
  value = azurerm_storage_account.main.primary_connection_string
}

output "name" {
  description = "Storage name"
  value = azurerm_storage_account.main.name
}
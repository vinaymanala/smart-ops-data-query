output "id" {
  description = "OpenAI service ID"
  value       = azurerm_cognitive_account.openai.id
}

output "name" {
  description = "OpenAI service name"
  value       = azurerm_cognitive_account.openai.name
}

output "primary_key" {
  description = "OpenAI primary key"
  value = azurerm_cognitive_account.openai.primary_access_key
}

output "endpoint" {
  description = "Search service endpoint"
  value       = "https://${azurerm_cognitive_account.openai.name}.search.windows.net"
}
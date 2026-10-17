output "uami" {
  description = "User assigned identity"  
  value = azurerm_user_assigned_identity.uami.id
}
# Configure the Azure provider
terraform {
  required_providers {
    azurerm = {
        source = "hashicorp/azurerm",
        version = "~>5.0.0"
    }
  }
}

provider "azurerm" {
  features {}
}


# Create a resource group
# resource "azurerm_resource_group" "example" {
#   name     = "example-resources"
#   location = "East Asia"
# }
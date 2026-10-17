
# Create a resource group
resource "random_string" "suffix" {
  length  = 8
  special = false
  upper   = false
}

locals {
  name_prefix = var.project_name
  name_suffix = random_string.suffix.result

  common_tags = merge(
    var.tags,
    {
      Environment = "dev/test"
      DeployedBy  = "Terraform"
      DeployDate  = timestamp()
    }
  )
}

# Resource Group
resource "azurerm_resource_group" "main" {
  name     = "${local.name_prefix}-rg"
  location = var.location
  tags     = local.common_tags
}

# Storage Account for documents
module "storage" {
  source = "./modules/storage"

  name                       = "${var.project_name}${var.environment}${local.name_suffix}"
  resource_group_name        = azurerm_resource_group.main.name
  location                   = azurerm_resource_group.main.location
  tags                       = local.common_tags
  replication_type           = var.replication_type
  soft_delete_retention_days = var.soft_delete_retention_days
  enable_private_endpoint    = var.enable_private_endpoints
  # Search AI Principal Id
  search_principal_id = module.ai_search.identity_principal_id
}

module "uami_resource" {
  source              = "./resources"
  name                = "${local.name_prefix}-WebApp-ACR-User-${local.name_suffix}"
  resource_group_name = azurerm_resource_group.main.name
  location            = azurerm_resource_group.main.location
}
# Azure AI Search
module "ai_search" {
  source = "./modules/ai-search"

  name                = "${local.name_prefix}-search-${local.name_suffix}"
  resource_group_name = azurerm_resource_group.main.name
  location            = azurerm_resource_group.main.location
  sku                 = var.search_sku
  tags                = local.common_tags

  enable_semantic_search  = true
  enable_private_endpoint = var.enable_private_endpoints
}

# Azure OpenAI
module "openai" {
  source = "./modules/openai"

  name                = "${local.name_prefix}-openai-${local.name_suffix}"
  resource_group_name = azurerm_resource_group.main.name
  location            = var.location
  tags                = local.common_tags

  deployments             = var.openai_deployments
  enable_private_endpoint = var.enable_private_endpoints
}

# Azure Network - VNet & Subnet
module "vnet" {
  source = "./modules/network"

  name                = "${local.name_prefix}-vnet-${local.name_suffix}"
  resource_group_name = azurerm_resource_group.main.name
  location            = var.location
  tags                = local.common_tags
  address_space       = var.address_space

  subnet_name      = "${local.name_prefix}-subnet-${local.name_suffix}"
  address_prefixes = var.address_prefixes

  nsg_name = "${local.name_prefix}-nsg-${local.name_suffix}"
}

module "webapp" {
  source              = "./modules/webapp"
  name                = "${local.name_prefix}-webapp-${local.name_suffix}"
  resource_group_name = azurerm_resource_group.main.name
  location            = var.location
  uami                = module.uami_resource.uami
  tags                = local.common_tags

  subnet_id = module.vnet.subnet_id
}

module "container-registry" {
  source              = "./modules/container-registry"
  name                = "${local.name_prefix}-acr-${local.name_suffix}"
  resource_group_name = azurerm_resource_group.main.name
  location            = var.location
  uami                = module.uami_resource.uami
  tags                = local.common_tags
}

# Key Vault for secrets
module "key_vault" {
  source = "./modules/key-vault"

  name                = "${local.name_prefix}-kv-${local.name_suffix}"
  resource_group_name = azurerm_resource_group.main.name
  location            = azurerm_resource_group.main.location
  tags                = local.common_tags

  enable_private_endpoint = var.enable_private_endpoints
}

# Store connection strings and keys in Key Vault
resource "azurerm_key_vault_secret" "storage_connection_string" {
  name         = "storage-connection-string"
  value        = module.storage.primary_connection_string
  key_vault_id = module.key_vault.id

  depends_on = [module.key_vault]
}

resource "azurerm_key_vault_secret" "search_admin_key" {
  name         = "search-admin-key"
  value        = module.ai_search.primary_admin_key
  key_vault_id = module.key_vault.id

  depends_on = [module.key_vault]
}

resource "azurerm_key_vault_secret" "openai_key" {
  name         = "openai-key"
  value        = module.openai.primary_key
  key_vault_id = module.key_vault.id

  depends_on = [module.key_vault]
}


variable "location" {
  description = "Azure region for resources"
  type        = string  
}

variable "project_name" {
  description = "Project name for resource naming"
  type        = string  
}

variable "tags" {
  description = "Tags to apply to all resources"
  type        = map(string)  
}

variable "environment" {
  description = "Environment Name"
  type        = string  
}


# Security settings
variable "enable_private_endpoints" {
  description = "Enable private endpoints for services"
  type        = bool
  default     = false # Set to true for production
}


# OpenAI model deployments
variable "openai_deployments" {
  description = "OpenAI model deployments"
  type = map(object({
    model_name    = string
    model_version = string
    scale_type    = string
    capacity      = number
  }))  
}

# Search service tier
variable "search_sku" {
  description = "Azure AI Search SKU"
  type        = string
  default     = "basic" # standard, basic, standard2, standard3
}

variable "replication_type" {
  description = "Storage account replication service"
  type        = string  
}

variable "soft_delete_retention_days" {
  description = "Soft Delete Retention Days"
  type        = number  
}

variable "address_space" {
  description = "vNET address space"
  type        = list(string)  
}

variable "address_prefixes" {
  description = "Subnet address prefixes"
  type        = list(string)  
}

variable "allowed_ip_ranges" {
  description = "IP ranges allowed to access services (for firewall)"
  type        = list(string)
  default     = [] # Empty = allow from Azure services only
}

# Monitoring
variable "enable_diagnostic_logs" {
  description = "Enable diagnostic logging"
  type        = bool
  default     = true
}

variable "log_retention_days" {
  description = "Number of days to retain logs"
  type        = number
  default     = 30
}

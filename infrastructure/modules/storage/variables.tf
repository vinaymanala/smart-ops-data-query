variable "name" {
  description = "Name of the storage account"
  type        = string
}

variable "resource_group_name" {
  description = "Resource group name"
  type        = string
}

variable "location" {
  description = "Azure region"
  type        = string
  default     = "southindia"
}

variable "replication_type" {
  description = "Account storage replication service"
  type = string  
}

variable "soft_delete_retention_days" {
  description = "Soft Delete Retention period"
  type = number
}

variable "enable_private_endpoint" {
  description = "Enable private endpoint"
  type        = bool
  default     = false
}


variable "tags" {
  description = "Resource tags"
  type        = map(string)
  default     = {}
}

variable "search_principal_id" {
  description = "Seach AI principal id"
  type = string  
}

variable "name" {
  description = "Name of the search service"
  type        = string
}

variable "resource_group_name" {
  description = "Resource group name"
  type        = string
}

variable "location" {
  description = "Azure region"
  type        = string
}

variable "sku" {
  description = "Search service SKU"
  type        = string  

  validation {
    condition     = contains(["free", "basic", "standard", "standard2", "standard3", "storage_optimized_l1", "storage_optimized_l2"], var.sku)
    error_message = "Invalid SKU. Choose from: free, basic, standard, standard2, standard3, storage_optimized_l1, storage_optimized_l2"
  }
}

variable "enable_semantic_search" {
  description = "Enable semantic search"
  type        = bool
  default     = true
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

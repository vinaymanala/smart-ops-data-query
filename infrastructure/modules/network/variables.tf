variable "name" {
  description = "Name of the vnet name"
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

variable "subnet_name" {
  description = "Azure subnet name"
  type = string
}

variable "nsg_name" {
  description = "Azure nsg name"
  type = string
}

variable "address_space" {
  description = "subnet address prefixes"
  type = list(string)
}

variable "address_prefixes" {
  description = "subnet address prefixes"
  type = list(string)
}

variable "tags" {
  description = "Resource tags"
  type        = map(string)
  default     = {}
}
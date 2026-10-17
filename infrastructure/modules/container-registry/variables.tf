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

variable "uami" {
  description = "user assigned identity for the project"
  type = string
}


variable "tags" {
  description = "Resource tags"
  type        = map(string)
  default     = {}
}

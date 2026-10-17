variable "name" {
  description = "Name of the webapp name"
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

variable "enable_private_endpoint" {
  description = "for private endpoint for prod"
  type = bool
}
variable "tags" {
  description = "Resource tags"
  type        = map(string)
  default     = {}
}

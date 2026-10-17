variable "name" {
  description = "Name of the openai service"
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

variable "deployments" {
  description = "OpenAI model deployments"
  type = map(object({
    model_name    = string
    model_version = string
    scale_type    = string
    capacity      = number
  }))
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

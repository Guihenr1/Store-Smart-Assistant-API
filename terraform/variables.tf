variable "project_name" {
  description = "Project name used for resource naming"
  type        = string
  default     = "storesmart"
}

variable "location" {
  description = "Azure region"
  type        = string
  default     = "SwedenCentral"
}

variable "resource_group_name" {
  description = "Resource Group name"
  type        = string
}

variable "app_service_sku" {
  description = "App Service Plan SKU"
  type        = string
  default     = "B1"   # B1 = Basic, P1v2 = better performance
}

variable "postgres_admin_password" {
  description = "PostgreSQL admin password"
  type        = string
  sensitive   = true
}

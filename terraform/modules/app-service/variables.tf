variable "resource_group_name" {}
variable "location" {}
variable "project_name" {}
variable "app_service_sku" {}

variable "openai_endpoint" {}
variable "openai_key" {}
variable "key_vault_uri" {
  default = ""
}

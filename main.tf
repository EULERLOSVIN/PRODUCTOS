# 1. Proveedor y Versiones Requeridas
terraform {
  required_providers {
    aws = {
      source  = "hashicorp/aws"
      version = "~> 5.0"
    }
  }
}

provider "aws" {
  region = "us-east-1"
}

# 2. Red (VPC): Configuración para EKS y conectividad a Docker Hub
module "vpc" {
  source  = "terraform-aws-modules/vpc/aws"
  version = "~> 5.0"

  name = "vpc-productos"
  cidr = "10.0.0.0/16"

  azs             = ["us-east-1a", "us-east-1b"]
  private_subnets = ["10.0.1.0/24", "10.0.2.0/24"]
  public_subnets  = ["10.0.101.0/24", "10.0.102.0/24"]

  enable_nat_gateway   = true
  single_nat_gateway   = true # Ahorro de costos para cuenta nueva
  enable_dns_hostnames = true

  # Etiquetas necesarias para que Kubernetes cree balanceadores públicos
  public_subnet_tags = {
    "kubernetes.io/role/elb" = 1
  }
}

# 3. Clúster Kubernetes (EKS): El orquestador de tus microservicios
module "eks" {
  source  = "terraform-aws-modules/eks/aws"
  version = "19.15.3" # Versión estable del módulo

  cluster_name    = "microservicios-cluster" # Coincide con desplegar.yml
  
  # CORRECCIÓN: Actualizado de 1.27 a 1.31 para cumplir con los requisitos de AWS
  cluster_version = "1.31" 

  vpc_id                         = module.vpc.vpc_id
  subnet_ids                     = module.vpc.private_subnets
  cluster_endpoint_public_access = true

  # Nodos Gestionados: AWS se encarga de la configuración interna
  eks_managed_node_groups = {
    nodos_productos = {
      instance_types = ["t3.medium"] # Capacidad ideal para .NET 9
      min_size     = 1
      max_size     = 2
      desired_size = 1
    }
  }
}

# Salida para verificar que se creó correctamente
output "cluster_name" {
  value = module.eks.cluster_name
}
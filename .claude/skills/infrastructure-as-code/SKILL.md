---
name: "Infrastructure as Code"
description: "Define and manage infrastructure using code with tools like Terraform, CloudFormation, and Docker"
category: "devops"
required_tools: ["Read", "Write", "Bash"]
---

## Purpose
Define infrastructure as version-controlled code, enabling reproducible environments and automated provisioning.

## When to Use
- Provisioning cloud resources
- Creating development environments
- Managing infrastructure changes
- Disaster recovery planning

## Key Capabilities
1. **Declarative Infrastructure** - Define desired state in code
2. **Container Configuration** - Create Docker images and configurations
3. **State Management** - Track infrastructure state and changes

## Approach
1. Choose appropriate tool (Terraform, Docker, K8s)
2. Define resources declaratively
3. Use modules for reusability
4. Version control infrastructure code
5. Apply infrastructure changes safely

## Example
```hcl
# Terraform example
resource "aws_instance" "web" {
  ami           = "ami-0c55b159cbfafe1f0"
  instance_type = "t3.medium"
  
  tags = {
    Name = "web-server"
    Environment = "production"
  }
}

# Docker example
FROM node:18-alpine
WORKDIR /app
COPY package*.json ./
RUN npm ci --only=production
COPY . .
EXPOSE 3000
CMD ["node", "server.js"]
```

## Best Practices
- ✅ Version control all infrastructure code
- ✅ Use modules for reusability
- ✅ Implement least privilege access
- ✅ Tag resources for cost tracking
- ❌ Avoid: Manual infrastructure changes
- ❌ Avoid: Hardcoding credentials

---

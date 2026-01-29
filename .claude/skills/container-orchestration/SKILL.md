---
name: "Container Orchestration"
description: "Design Kubernetes deployment patterns, configure service mesh, and implement container best practices"
category: "devops"
required_tools: ["Read", "Write", "Bash"]
---

# Container Orchestration

## Purpose
Design and manage containerized applications using Kubernetes and container orchestration platforms, implementing deployment patterns, scaling strategies, and service management.

## When to Use
- Deploying containerized applications
- Managing microservices architectures
- Implementing auto-scaling
- Configuring service discovery and load balancing
- Managing application secrets and configs
- Setting up multi-environment deployments

## Key Capabilities

1. **Kubernetes Deployment** - Design and implement K8s resources (Deployments, Services, Ingress)
2. **Scaling Strategies** - Implement horizontal pod autoscaling and cluster autoscaling
3. **Service Management** - Configure service discovery, load balancing, and networking

## Approach

1. **Design Container Strategy**
   - Determine container boundaries (one process per container)
   - Define resource limits (CPU, memory)
   - Plan persistent storage needs
   - Design health check endpoints

2. **Create Kubernetes Manifests**
   - Deployments for stateless apps
   - StatefulSets for stateful apps
   - Services for networking
   - ConfigMaps and Secrets for configuration
   - Ingress for external access

3. **Implement Scaling**
   - Horizontal Pod Autoscaler (HPA) based on CPU/memory/custom metrics
   - Vertical Pod Autoscaler (VPA) for resource optimization
   - Cluster Autoscaler for node scaling

4. **Configure Networking**
   - Service types (ClusterIP, NodePort, LoadBalancer)
   - Ingress controllers (nginx, traefik)
   - Network policies for security
   - Service mesh (Istio, Linkerd) for advanced routing

5. **Manage Configuration**
   - ConfigMaps for non-sensitive config
   - Secrets for sensitive data
   - External secrets operators for vault integration
   - Environment-specific configurations

## Example

**Context**: Deploying a web application with database on Kubernetes

**Deployment Manifest**:
```yaml
# web-deployment.yaml
apiVersion: apps/v1
kind: Deployment
metadata:
  name: web-app
  labels:
    app: web
spec:
  replicas: 3
  selector:
    matchLabels:
      app: web
  template:
    metadata:
      labels:
        app: web
    spec:
      containers:
      - name: web
        image: myapp:v1.2.3
        ports:
        - containerPort: 8080
        env:
        - name: DATABASE_URL
          valueFrom:
            secretKeyRef:
              name: db-credentials
              key: url
        - name: REDIS_HOST
          valueFrom:
            configMapKeyRef:
              name: app-config
              key: redis-host
        resources:
          requests:
            memory: "256Mi"
            cpu: "250m"
          limits:
            memory: "512Mi"
            cpu: "500m"
        livenessProbe:
          httpGet:
            path: /health
            port: 8080
          initialDelaySeconds: 30
          periodSeconds: 10
        readinessProbe:
          httpGet:
            path: /ready
            port: 8080
          initialDelaySeconds: 5
          periodSeconds: 5
---
# web-service.yaml
apiVersion: v1
kind: Service
metadata:
  name: web-service
spec:
  selector:
    app: web
  ports:
  - protocol: TCP
    port: 80
    targetPort: 8080
  type: LoadBalancer
---
# web-hpa.yaml
apiVersion: autoscaling/v2
kind: HorizontalPodAutoscaler
metadata:
  name: web-hpa
spec:
  scaleTargetRef:
    apiVersion: apps/v1
    kind: Deployment
    name: web-app
  minReplicas: 3
  maxReplicas: 10
  metrics:
  - type: Resource
    resource:
      name: cpu
      target:
        type: Utilization
        averageUtilization: 70
  - type: Resource
    resource:
      name: memory
      target:
        type: Utilization
        averageUtilization: 80
---
# ingress.yaml
apiVersion: networking.k8s.io/v1
kind: Ingress
metadata:
  name: web-ingress
  annotations:
    cert-manager.io/cluster-issuer: "letsencrypt-prod"
    nginx.ingress.kubernetes.io/rate-limit: "100"
spec:
  tls:
  - hosts:
    - myapp.example.com
    secretName: web-tls
  rules:
  - host: myapp.example.com
    http:
      paths:
      - path: /
        pathType: Prefix
        backend:
          service:
            name: web-service
            port:
              number: 80
```

**ConfigMap**:
```yaml
apiVersion: v1
kind: ConfigMap
metadata:
  name: app-config
data:
  redis-host: "redis.default.svc.cluster.local"
  log-level: "info"
  feature-flag-new-ui: "true"
```

**Secret** (created via kubectl):
```bash
kubectl create secret generic db-credentials \
  --from-literal=url='postgresql://user:pass@db:5432/mydb'
```

**Deploy**:
```bash
# Apply all manifests
kubectl apply -f web-deployment.yaml

# Verify deployment
kubectl get deployments
kubectl get pods
kubectl get services
kubectl get hpa

# Check logs
kubectl logs -f deployment/web-app

# Scale manually if needed
kubectl scale deployment web-app --replicas=5
```

**Expected Result**:
- 3 replicas running initially
- Auto-scales between 3-10 based on CPU/memory
- External access via LoadBalancer or Ingress
- Health checks ensure only healthy pods receive traffic
- Zero-downtime rolling updates
- Configuration managed via ConfigMaps/Secrets

## Best Practices

- ✅ Always set resource requests and limits
- ✅ Implement health checks (liveness and readiness probes)
- ✅ Use namespaces to organize resources
- ✅ Tag images with specific versions, not `:latest`
- ✅ Use ConfigMaps/Secrets for configuration
- ✅ Implement network policies for security
- ✅ Use StatefulSets for stateful applications
- ✅ Enable Pod Security Standards
- ✅ Implement rolling update strategies
- ✅ Monitor resource usage with metrics server
- ✅ Use init containers for setup tasks
- ✅ Implement pod disruption budgets for high availability
- ❌ Avoid: Running containers as root
- ❌ Avoid: Hardcoding configuration in images
- ❌ Avoid: Using `:latest` tag in production
- ❌ Avoid: No resource limits (can starve other pods)
- ❌ Avoid: Storing secrets in ConfigMaps
- ❌ Avoid: Single replica for critical services
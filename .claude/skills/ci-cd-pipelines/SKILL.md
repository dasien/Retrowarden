---
name: "CI/CD Pipeline Design"
description: "Design and implement continuous integration and deployment pipelines with automated testing, builds, and deployments"
category: "devops"
required_tools: ["Read", "Write", "Bash", "WebSearch"]
---

## Purpose
Design robust CI/CD pipelines that automate building, testing, and deploying applications with quality gates and deployment strategies.

## When to Use
- Setting up new projects
- Automating deployment processes
- Implementing quality gates
- Configuring automated testing

## Key Capabilities
1. **Pipeline Design** - Structure multi-stage build/test/deploy workflows
2. **Quality Gates** - Implement automated testing and code quality checks
3. **Deployment Strategies** - Blue-green, canary, rolling deployments

## Approach
1. Define pipeline stages (build, test, deploy)
2. Configure triggers (push, PR, schedule)
3. Add quality gates (tests must pass, coverage >80%)
4. Implement deployment strategies
5. Add notifications and monitoring

## Example
```yaml
# .github/workflows/ci-cd.yml
name: CI/CD Pipeline

on:
  push:
    branches: [main, develop]
  pull_request:
    branches: [main]

jobs:
  build:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v3
      - name: Setup Node
        uses: actions/setup-node@v3
        with:
          node-version: '18'
      - run: npm ci
      - run: npm run build
      - run: npm test
      - name: Upload coverage
        uses: codecov/codecov-action@v3
  
  deploy:
    needs: build
    if: github.ref == 'refs/heads/main'
    runs-on: ubuntu-latest
    steps:
      - name: Deploy to production
        run: |
          ./deploy.sh production
```

## Best Practices
- ✅ Run tests on every commit
- ✅ Fail fast on test failures
- ✅ Use caching to speed up builds
- ✅ Separate build and deploy stages
- ✅ Require code review before merging
- ❌ Avoid: Skipping tests to deploy faster
- ❌ Avoid: Deploying without quality gates

---

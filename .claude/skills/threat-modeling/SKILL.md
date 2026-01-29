---
name: "Threat Modeling"
description: "Apply STRIDE methodology to identify security threats, analyze attack surfaces, and assess security risks in system architectures"
category: "security"
required_tools: ["Read", "Write", "WebSearch"]
---

# Threat Modeling

## Purpose
Systematically identify potential security threats in system designs using structured methodologies like STRIDE, analyze attack surfaces, and prioritize security risks for mitigation.

## When to Use
- Designing new systems or features
- Reviewing architectural changes
- Planning security controls
- Conducting security assessments
- Before building security-critical features
- Evaluating third-party integrations

## Key Capabilities

1. **STRIDE Analysis** - Apply structured threat identification framework
2. **Attack Surface Mapping** - Identify potential entry points for attackers
3. **Risk Assessment** - Prioritize threats by likelihood and impact

## Approach

1. **Decompose the System**
   - Identify components, data flows, trust boundaries
   - Map external dependencies and integrations
   - Document data stores and sensitive information
   - Identify user roles and privilege levels
   - Draw data flow diagrams (DFDs)

2. **Apply STRIDE Framework**
   - **S**poofing: Can attackers impersonate users/systems?
   - **T**ampering: Can data be modified without authorization?
   - **R**epudiation: Can actions be performed without audit trail?
   - **I**nformation Disclosure: Can sensitive data be exposed?
   - **D**enial of Service: Can the system be made unavailable?
   - **E**levation of Privilege: Can users gain unauthorized access?

3. **Identify Attack Vectors**
   - Network-based attacks (MITM, eavesdropping)
   - Application-level exploits (injection, XSS)
   - Social engineering vectors
   - Physical access threats
   - Supply chain risks
   - Insider threats

4. **Assess Risk**
   - **Likelihood**: How probable is this threat?
     - High: Easy to exploit, known attack patterns
     - Medium: Requires some skill or opportunity
     - Low: Difficult, requires significant resources
   - **Impact**: What's the damage if exploited?
     - Critical: Data breach, system compromise, financial loss
     - High: Service disruption, data corruption
     - Medium: Limited data exposure, temporary unavailability
     - Low: Minor inconvenience
   - **Priority**: Risk = Likelihood × Impact

5. **Define Mitigations**
   - Security controls to implement
   - Design changes to reduce risk
   - Monitoring and detection strategies
   - Incident response plans
   - Accept, mitigate, transfer, or avoid each risk

## Example

**Context**: Threat model for a web application with user authentication, file uploads, and database

### System Decomposition

**Components**:
- Web Frontend (React SPA)
- API Backend (Node.js/Express)
- Database (PostgreSQL)
- File Storage (S3)
- Authentication Service (OAuth2)
- Email Service (SendGrid)

**Data Flows**:
```
1. User → Frontend → API → Database
2. User → Frontend → API → File Storage
3. API → Auth Service → API
4. API → Email Service
5. Admin → API → Database (privileged operations)
```

**Trust Boundaries**:
```
Internet ↔ Frontend (HTTPS)
Frontend ↔ API (HTTPS, CORS)
API ↔ Database (private network)
API ↔ S3 (AWS IAM)
API ↔ Auth Service (OAuth2)
API ↔ Email Service (API key)
```

**Data Classification**:
- **Critical**: User passwords, payment info, SSNs
- **Sensitive**: Email addresses, names, personal data
- **Internal**: Session tokens, API keys
- **Public**: Product catalog, blog posts

### STRIDE Analysis

| Component | Threat | STRIDE | Likelihood | Impact | Risk | Mitigation |
|-----------|--------|--------|------------|--------|------|------------|
| API Login | Attacker steals session tokens by intercepting HTTPS | Spoofing | Low | High | **Medium** | Use HSTS, certificate pinning, short token lifetime |
| API Login | Brute force password attempts | Spoofing | High | High | **High** | Rate limiting, account lockout, CAPTCHA |
| Database | SQL injection modifies user data | Tampering | Medium | Critical | **High** | Parameterized queries, ORM, input validation |
| API | User denies making a purchase | Repudiation | High | Medium | **Medium** | Comprehensive audit logging, digital signatures |
| S3 Files | Public bucket exposes private files | Info Disclosure | Low | Critical | **High** | Private buckets, pre-signed URLs, access logging |
| S3 Files | Attacker uploads malicious file | Tampering | High | High | **High** | File type validation, virus scanning, size limits |
| API | DDoS overwhelms server | DoS | High | High | **High** | Rate limiting, CDN, auto-scaling, WAF |
| API Auth | Token forgery grants admin access | Elevation | Low | Critical | **High** | JWT signature verification, RBAC, token rotation |
| Frontend | XSS steals session tokens | Info Disclosure | Medium | High | **High** | CSP headers, output encoding, HTTPOnly cookies |
| API | CSRF performs unauthorized actions | Tampering | Medium | High | **High** | CSRF tokens, SameSite cookies |

### Attack Surface Map

```
External Attack Surface:
├── Web Frontend (exposed to internet)
│   ├── XSS via user inputs
│   ├── CSRF on state-changing operations
│   ├── Clickjacking via iframes
│   └── DOM-based attacks
├── API Endpoints (exposed to internet)
│   ├── Authentication bypass
│   ├── Injection attacks (SQL, NoSQL, Command)
│   ├── Broken authorization (IDOR)
│   ├── Rate limiting bypass
│   ├── Mass assignment
│   └── Insecure direct object references
├── File Upload (exposed to internet)
│   ├── Malicious file execution
│   ├── Path traversal
│   ├── Storage exhaustion
│   ├── Malware distribution
│   └── XXE attacks (XML uploads)
└── OAuth Callback (exposed to internet)
    ├── Authorization code interception
    ├── CSRF on OAuth flow
    └── Open redirect vulnerabilities

Internal Attack Surface:
├── Database (internal network)
│   ├── Credential compromise
│   ├── Privilege escalation within DB
│   └── Backup exposure
├── Auth Service (internal network)
│   ├── Token generation vulnerabilities
│   └── Session fixation
└── Email Service (external API)
    ├── Email injection
    ├── API key exposure
    └── Rate limit bypass
```

### Prioritized Threats

**Critical (Address Immediately)**:
1. **Token Forgery → Admin Access**
   - Risk: Critical
   - Mitigation: Implement JWT signature verification with RS256, rotate signing keys quarterly
   - Owner: Security team
   - Due: Before production launch

2. **Public S3 Bucket**
   - Risk: Critical
   - Mitigation: Configure all buckets as private, implement pre-signed URLs, enable access logging
   - Owner: DevOps team
   - Due: This week

3. **SQL Injection**
   - Risk: High
   - Mitigation: Use parameterized queries everywhere, add WAF rules, conduct code review
   - Owner: Dev team
   - Due: Next sprint

**High (Address Soon)**:
1. **Brute Force Login Attacks**
   - Risk: High
   - Mitigation: Implement rate limiting (5 attempts/minute), account lockout after 10 failures, CAPTCHA
   - Owner: Dev team
   - Due: Next sprint

2. **Malicious File Upload**
   - Risk: High
   - Mitigation: Validate file types (whitelist), scan with ClamAV, limit file sizes, store outside webroot
   - Owner: Dev team
   - Due: Next sprint

3. **DDoS Attack**
   - Risk: High
   - Mitigation: CloudFlare WAF, rate limiting (100 req/min per IP), auto-scaling
   - Owner: DevOps team
   - Due: Before marketing campaign

4. **XSS Vulnerabilities**
   - Risk: High
   - Mitigation: Implement CSP headers, use DOMPurify, encode all output, set HTTPOnly cookies
   - Owner: Dev team
   - Due: Next sprint

**Medium (Address in Backlog)**:
1. **Session Theft via HTTPS Interception**
   - Risk: Medium
   - Mitigation: HSTS with preload, short token lifetime (15 min), refresh tokens
   - Owner: Dev team
   - Due: Q2

2. **Audit Log Gaps (Repudiation)**
   - Risk: Medium
   - Mitigation: Comprehensive logging of all user actions, tamper-proof logs
   - Owner: Dev team
   - Due: Q2

### Threat Model Report

```markdown
# Threat Model Report - E-commerce Web Application
Date: 2025-01-12
Version: 1.0

## Executive Summary
Identified 10 security threats across 3 risk levels (Critical: 3, High: 4, Medium: 2).
Primary concerns: Token forgery, SQL injection, public file storage, brute force attacks.

## System Overview
Web application handling user authentication, file uploads, and payment processing.
External integrations: OAuth provider, S3 storage, SendGrid email.

## Critical Findings
1. Admin privilege escalation via token forgery (STRIDE: Elevation of Privilege)
2. Data breach via public S3 buckets (STRIDE: Information Disclosure)
3. Database compromise via SQL injection (STRIDE: Tampering)

## Recommended Actions
**Immediate (Before Launch)**:
- Implement JWT signature verification
- Configure S3 buckets as private
- Replace all string concatenation in SQL with parameterized queries

**Next Sprint**:
- Add rate limiting and account lockout
- Implement file upload validation and scanning
- Deploy CloudFlare WAF

**Ongoing**:
- Regular penetration testing
- Security code reviews for all PRs
- Quarterly threat model reviews

## Residual Risk
After mitigations: Medium
- Some risk remains from sophisticated targeted attacks
- Recommend: Bug bounty program, red team exercises

## Next Review
Scheduled: Q2 2025 or upon significant architecture changes
```

## Best Practices

- ✅ Involve security experts and developers in threat modeling sessions
- ✅ Model threats early in design phase (before implementation)
- ✅ Focus on high-value assets and critical functions
- ✅ Document assumptions and trust boundaries clearly
- ✅ Update threat models as system evolves
- ✅ Validate mitigations are actually implemented
- ✅ Use diagrams to visualize data flows and trust boundaries
- ✅ Consider both internal and external threats
- ✅ Think like an attacker (what would you exploit?)
- ✅ Link threats to business impact (not just technical risk)
- ✅ Assign owners and due dates for each mitigation
- ❌ Avoid: Threat modeling only after implementation
- ❌ Avoid: Ignoring low-likelihood but high-impact threats
- ❌ Avoid: Generic threats without specific context
- ❌ Avoid: Threat models that gather dust on a shelf
- ❌ Avoid: Not validating that mitigations were implemented
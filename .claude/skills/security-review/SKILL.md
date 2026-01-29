---
name: "Security Code Review"
description: "Identify security vulnerabilities including OWASP Top 10 issues, implement secure coding practices, and validate authentication/authorization implementations"
category: "security"
required_tools: ["Read", "Grep", "Glob", "WebSearch"]
---

# Security Code Review

## Purpose
Systematically review code for security vulnerabilities, identify potential attack vectors, and ensure implementation follows secure coding practices to protect against common exploits.

## When to Use
- Reviewing code before deployment
- Analyzing security-sensitive features (authentication, payments, data handling)
- Conducting security audits
- Investigating potential security incidents
- Validating third-party integrations

## Key Capabilities

1. **OWASP Top 10 Detection** - Identify common web application vulnerabilities
2. **Secure Coding Validation** - Verify adherence to security best practices
3. **Authentication/Authorization Review** - Validate access control implementations

## Approach

1. **Identify Security-Critical Areas**
   - Authentication and session management
   - Authorization and access control
   - Input validation and sanitization
   - Data encryption and storage
   - External API interactions

2. **Check for Common Vulnerabilities**
   - Injection flaws (SQL, command, LDAP)
   - Broken authentication
   - Sensitive data exposure
   - XML external entities (XXE)
   - Broken access control
   - Security misconfiguration
   - Cross-site scripting (XSS)
   - Insecure deserialization
   - Using components with known vulnerabilities
   - Insufficient logging and monitoring

3. **Validate Security Controls**
   - Input validation on all user inputs
   - Output encoding to prevent XSS
   - Parameterized queries to prevent SQL injection
   - Proper session management
   - Secure password storage (bcrypt, argon2)
   - HTTPS enforcement
   - CSRF protection
   - Rate limiting on sensitive endpoints

4. **Review Cryptographic Implementations**
   - Use of strong algorithms (AES-256, RSA-2048+)
   - Proper key management
   - Secure random number generation
   - Certificate validation

5. **Check Configuration Security**
   - No hardcoded credentials
   - Secrets in environment variables or vaults
   - Secure headers (CSP, HSTS, X-Frame-Options)
   - Minimal exposed services
   - Proper error handling (no stack traces to users)

## Example

**Context**: Reviewing authentication endpoint

**Code Under Review**:
```python
@app.route('/login', methods=['POST'])
def login():
    username = request.form['username']
    password = request.form['password']
    
    user = db.execute(
        f"SELECT * FROM users WHERE username='{username}' AND password='{password}'"
    ).fetchone()
    
    if user:
        session['user_id'] = user['id']
        return redirect('/dashboard')
    return "Invalid credentials"
```

**Security Issues Identified**:
1. **SQL Injection (Critical)**: String concatenation in query
2. **Plaintext Passwords (Critical)**: No password hashing
3. **Missing CSRF Protection (High)**: No token validation
4. **Information Disclosure (Medium)**: Generic error message is good, but could add rate limiting
5. **No Account Lockout (Medium)**: Vulnerable to brute force
6. **Session Fixation Risk (Medium)**: Should regenerate session ID after login

**Secure Implementation**:
```python
from werkzeug.security import check_password_hash
from flask import session, request, redirect
from flask_wtf.csrf import CSRFProtect
import secrets

@app.route('/login', methods=['POST'])
@limiter.limit("5 per minute")  # Rate limiting
def login():
    # CSRF protection (via flask_wtf)
    if not validate_csrf(request.form.get('csrf_token')):
        return "Invalid request", 403
    
    username = request.form.get('username', '').strip()
    password = request.form.get('password', '')
    
    # Input validation
    if not username or not password:
        return "Invalid credentials", 401
    
    # Parameterized query prevents SQL injection
    user = db.execute(
        "SELECT id, password_hash, failed_attempts, locked_until "
        "FROM users WHERE username = ?",
        (username,)
    ).fetchone()
    
    # Check account lockout
    if user and user['locked_until'] and user['locked_until'] > datetime.now():
        return "Account temporarily locked", 429
    
    # Check password with timing-safe comparison
    if user and check_password_hash(user['password_hash'], password):
        # Reset failed attempts
        db.execute("UPDATE users SET failed_attempts = 0 WHERE id = ?", (user['id'],))
        
        # Regenerate session to prevent fixation
        session.clear()
        session['user_id'] = user['id']
        session['csrf_token'] = secrets.token_hex(32)
        
        # Log successful login
        log_security_event('login_success', user_id=user['id'], ip=request.remote_addr)
        
        return redirect('/dashboard')
    
    # Track failed attempts
    if user:
        attempts = user['failed_attempts'] + 1
        if attempts >= 5:
            locked_until = datetime.now() + timedelta(minutes=15)
            db.execute(
                "UPDATE users SET failed_attempts = ?, locked_until = ? WHERE id = ?",
                (attempts, locked_until, user['id'])
            )
        else:
            db.execute("UPDATE users SET failed_attempts = ? WHERE id = ?", (attempts, user['id']))
    
    # Log failed attempt
    log_security_event('login_failure', username=username, ip=request.remote_addr)
    
    return "Invalid credentials", 401
```

**Expected Result**: Secure authentication with:
- SQL injection prevention
- Secure password handling
- CSRF protection
- Rate limiting
- Account lockout
- Session security
- Audit logging

## Best Practices

- ✅ Use parameterized queries or ORMs to prevent injection
- ✅ Hash passwords with bcrypt/argon2, never store plaintext
- ✅ Validate and sanitize all user inputs
- ✅ Use HTTPS for all communications
- ✅ Implement proper session management
- ✅ Apply principle of least privilege
- ✅ Keep dependencies updated and scan for vulnerabilities
- ✅ Log security events for monitoring
- ✅ Use security headers (CSP, HSTS, X-Frame-Options)
- ✅ Implement rate limiting on sensitive endpoints
- ✅ Store secrets in environment variables or vaults
- ❌ Avoid: Trusting user input without validation
- ❌ Avoid: Rolling your own cryptography
- ❌ Avoid: Exposing detailed error messages to users
- ❌ Avoid: Hardcoding credentials or API keys
- ❌ Avoid: Using deprecated or weak cryptographic algorithms
---
name: "Technical Debt Assessment"
description: "Identify code smells, prioritize refactoring opportunities, and assess technical debt impact"
category: "implementation"
required_tools: ["Read", "Grep", "Glob"]
---

# Technical Debt Assessment

## Purpose
Systematically identify, quantify, and prioritize technical debt to make informed decisions about when and what to refactor.

## When to Use
- Planning refactoring initiatives
- Evaluating code quality
- Making build vs buy decisions
- Estimating maintenance costs
- Sprint planning (allocating debt reduction time)
- Before major feature work

## Key Capabilities

1. **Debt Identification** - Find code smells and anti-patterns
2. **Impact Assessment** - Quantify cost and risk of debt
3. **Prioritization** - Rank debt by impact/effort ratio

## Approach

1. **Scan Codebase for Quality Metrics**
   - Cyclomatic complexity (>10 is problematic)
   - Code duplication (>5% is high)
   - Test coverage (<80% needs improvement)
   - Function length (>50 lines needs review)
   - Class size (>500 lines may need splitting)
   - Comment density (too many or too few)

2. **Identify Specific Debt Items**
   - Code smells (long methods, god classes, shotgun surgery)
   - Anti-patterns (spaghetti code, lava flow, golden hammer)
   - Missing tests
   - Outdated dependencies
   - TODO/FIXME comments
   - Dead code
   - Magic numbers and strings
   - Lack of error handling

3. **Assess Impact**
   - **High Impact**: Causes bugs, blocks features, slows development
   - **Medium Impact**: Makes code harder to understand/modify
   - **Low Impact**: Minor annoyance, cosmetic issues

4. **Estimate Effort to Fix**
   - **Low Effort**: <4 hours (quick wins)
   - **Medium Effort**: 1-3 days
   - **High Effort**: >3 days (may need to break down)

5. **Prioritize by Impact/Effort Ratio**
   - **P0 Critical**: High impact, any effort - fix now
   - **P1 High**: High impact, low-medium effort - next sprint
   - **P2 Medium**: Medium impact, low effort - backlog
   - **P3 Low**: Low impact or high effort - accept or defer

## Example

**Context**: Assessing technical debt in a payments module

```python
# Technical Debt Assessment Report

## Metrics Summary

**Overall Code Quality**: C (60/100)

| Metric | Value | Target | Status |
|--------|-------|--------|--------|
| Cyclomatic Complexity (avg) | 15.2 | <10 | ⚠️ High |
| Code Duplication | 18% | <5% | ❌ Critical |
| Test Coverage | 52% | >80% | ❌ Critical |
| Lines of Code | 12,500 | - | - |
| TODO/FIXME Comments | 47 | 0 | ⚠️ High |
| Outdated Dependencies | 8 | 0 | ⚠️ High |

## Identified Debt Items

### Critical Priority (P0) - Fix Immediately

**1. Missing Tests in Payment Processing**
- **Location**: `payments/processor.py`, `payments/refunds.py`
- **Type**: Missing test coverage
- **Impact**: Critical - Payment bugs reach production, financial risk
- **Current Coverage**: 35% (target: 80%)
- **Effort**: High (5 days for comprehensive coverage)
- **Risk**: High - No safety net for changes
- **Cost**: Estimated $50k per production payment bug
- **Priority**: P0 - Block releases until fixed
- **Recommendation**: Write tests immediately, starting with critical paths

**2. SQL Injection Vulnerability**
- **Location**: `payments/reports.py:145-167`
- **Type**: Security vulnerability
- **Impact**: Critical - Potential data breach
- **Code**:
  ```python
  # VULNERABLE
  query = f"SELECT * FROM payments WHERE user_id = {user_id}"
  ```
- **Effort**: Low (2 hours)
- **Risk**: Critical security risk
- **Priority**: P0 - Fix before next release
- **Recommendation**: Use parameterized queries immediately

### High Priority (P1) - Fix in Next Sprint

**3. Duplicated Payment Validation Logic**
- **Location**: 5 files in `payments/` module
- **Type**: Code duplication (DRY violation)
- **Impact**: High - Bug fixes must be applied in 5 places, leading to inconsistency
- **Duplication**: 250 lines duplicated across files
- **Effort**: Medium (2 days)
- **Bugs Caused**: 3 incidents where fix was missed in some files
- **Priority**: P1
- **Recommendation**: Extract to shared `PaymentValidator` class

**4. God Class: PaymentProcessor**
- **Location**: `payments/processor.py` (1,200 lines)
- **Type**: God class anti-pattern
- **Impact**: High - Hard to understand, test, and modify
- **Complexity**: Average cyclomatic complexity: 22 (target: <10)
- **Effort**: High (4 days)
- **Priority**: P1
- **Recommendation**: Split into `PaymentProcessor`, `RefundHandler`, `PaymentValidator`

**5. No Error Handling in Stripe Integration**
- **Location**: `payments/stripe_client.py`
- **Type**: Missing error handling
- **Impact**: High - App crashes on API failures
- **Errors**: 47 production errors in last month
- **Effort**: Medium (1 day)
- **Priority**: P1
- **Recommendation**: Add try-catch, retry logic, circuit breaker

### Medium Priority (P2) - Next Quarter

**6. Outdated Dependencies**
- **Location**: `requirements.txt`
- **Type**: Dependency debt
- **Impact**: Medium - Security vulnerabilities, missing features
- **Outdated**: 8 packages (2 with known CVEs)
- **Effort**: Medium (2 days with testing)
- **Priority**: P2
- **Recommendation**: Update and test, especially packages with CVEs

**7. 47 TODO Comments**
- **Location**: Throughout codebase
- **Type**: Incomplete work markers
- **Impact**: Medium - Uncertain about what needs completion
- **Oldest**: 2 years old
- **Effort**: Varies (1 hour to 1 day each)
- **Priority**: P2
- **Recommendation**: Create tickets for each, complete or remove

### Low Priority (P3) - Accept or Defer

**8. Inconsistent Naming Conventions**
- **Location**: Various files
- **Type**: Style inconsistency
- **Impact**: Low - Slightly harder to read, no functional impact
- **Effort**: Medium (1 day)
- **Priority**: P3 - Accept
- **Recommendation**: Fix incrementally as files are modified

## Debt Trends

**Last 6 Months**:
- Code Duplication: 12% → 18% (⬆️ Worsening)
- Test Coverage: 65% → 52% (⬇️ Worsening)
- Cyclomatic Complexity: 12 → 15.2 (⬆️ Worsening)
- TODO Comments: 32 → 47 (⬆️ Worsening)

**Trend Analysis**: Technical debt is accumulating faster than it's being paid down. Recommend allocating 20% of sprint capacity to debt reduction.

## Cost Analysis

**Estimated Cost of Current Debt**:
- Developer productivity loss: 15% (1.5 days per sprint)
- Increased bug rate: 2x normal rate
- Slower feature velocity: 25% reduction
- Estimated annual cost: $120,000

**Cost of Fixing**:
- Critical items (P0): 7 days ($8,400)
- High priority (P1): 8 days ($9,600)
- Total: $18,000

**ROI**: Fixing P0 and P1 debt saves ~$100,000/year in reduced bugs and faster development.

## Recommendations

1. **Immediate Actions** (This Week):
   - Fix SQL injection vulnerability (2 hours)
   - Start writing tests for payment processing (block releases)

2. **Next Sprint**:
   - Complete payment processing tests
   - Consolidate duplicated validation logic
   - Add error handling to Stripe integration

3. **Ongoing**:
   - Allocate 20% of sprint capacity to debt reduction
   - No new features in `PaymentProcessor` until refactored
   - Add pre-commit hooks to prevent new debt
   - Track debt metrics in dashboards

4. **Process Changes**:
   - Require tests for all new code (no exceptions)
   - Code review checklist includes debt checks
   - Monthly debt review meetings
   - Definition of Done includes "no new TODO comments"
```

**Tools for Debt Assessment**:
```bash
# Code complexity
radon cc src/ -a -nb

# Code duplication
pylint --disable=all --enable=duplicate-code src/

# Test coverage
pytest --cov=src --cov-report=html

# Security issues
bandit -r src/

# Outdated dependencies
pip list --outdated

# Find TODOs
grep -rn "TODO\|FIXME" src/
```

## Best Practices

- ✅ Track debt metrics over time (trends matter)
- ✅ Quantify debt in time and cost terms
- ✅ Fix high-impact, low-effort items first (quick wins)
- ✅ Allocate 10-20% of sprint time to debt reduction
- ✅ Prevent new debt (quality gates in CI/CD)
- ✅ Make debt visible (dashboards, reports)
- ✅ Link debt to business impact (bugs, velocity)
- ✅ Review debt regularly (monthly or quarterly)
- ❌ Avoid: Ignoring debt until it becomes a crisis
- ❌ Avoid: Refactoring everything at once
- ❌ Avoid: Only fixing debt when forced to
- ❌ Avoid: Treating all debt as equal priority
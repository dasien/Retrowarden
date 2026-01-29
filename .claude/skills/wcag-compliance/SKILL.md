---
name: "WCAG Compliance"
description: "Validate WCAG 2.1 AA/AAA standards, use accessibility testing tools, and implement remediation strategies"
category: "accessibility"
required_tools: ["Read", "Write", "WebSearch"]
---

# WCAG Compliance

## Purpose
Ensure web applications meet WCAG 2.1 Level AA accessibility standards, making them usable by people with disabilities and compliant with legal requirements (ADA, Section 508).

## When to Use
- Building user interfaces
- Accessibility audits
- Before production deployment
- Fixing accessibility issues
- Legal compliance validation

## Key Capabilities

1. **WCAG Validation** - Check compliance with WCAG 2.1 Level AA/AAA
2. **Automated Testing** - Use tools like axe, Lighthouse, WAVE
3. **Manual Testing** - Verify with assistive technologies

## Approach

1. **Understand WCAG Principles (POUR)**
   - **Perceivable**: Information and UI must be presentable to users
   - **Operable**: UI components must be operable by all users
   - **Understandable**: Information and UI must be understandable
   - **Robust**: Content must work with current and future technologies

2. **Run Automated Tests**
   - Lighthouse accessibility audit
   - axe DevTools browser extension
   - WAVE (Web Accessibility Evaluation Tool)
   - pa11y automated testing

3. **Manual Checks**
   - Keyboard navigation (no mouse)
   - Screen reader testing (NVDA, JAWS, VoiceOver)
   - Color contrast verification
   - Text resize to 200%
   - Captions/transcripts for media

4. **Prioritize Fixes**
   - Level A: Critical (must fix)
   - Level AA: Important (should fix)
   - Level AAA: Enhanced (nice to have)

5. **Document Compliance**
   - VPAT (Voluntary Product Accessibility Template)
   - Accessibility statement
   - Known issues and workarounds

## Example

**Context**: Auditing a web form for WCAG compliance

```html
<!-- BAD: Multiple WCAG violations -->
<div class="form">
    <div>Name</div>
    <input type="text" placeholder="Enter your name">
    
    <div style="color: #999;">Email</div>
    <input type="text">
    
    <div onclick="submitForm()">Submit</div>
</div>

<!-- Issues:
1. No label association (WCAG 3.3.2)
2. Poor color contrast (WCAG 1.4.3)
3. Div used as button (WCAG 4.1.2)
4. No keyboard support (WCAG 2.1.1)
5. Missing input type (WCAG 1.3.5)
-->

<!-- GOOD: WCAG 2.1 AA compliant -->
<form aria-label="Contact Form">
    <!-- Proper label association -->
    <label for="name">
        Name
        <span aria-label="required">*</span>
    </label>
    <input 
        id="name"
        type="text"
        name="name"
        required
        aria-required="true"
        aria-describedby="name-help"
    >
    <div id="name-help" class="help-text">
        Enter your full name
    </div>
    
    <!-- Proper contrast (4.5:1 minimum) -->
    <label for="email" style="color: #333;">
        Email
        <span aria-label="required">*</span>
    </label>
    <input 
        id="email"
        type="email"
        name="email"
        required
        aria-required="true"
        aria-invalid="false"
        aria-describedby="email-help email-error"
    >
    <div id="email-help" class="help-text">
        We'll never share your email
    </div>
    <div id="email-error" class="error" role="alert" aria-live="polite">
        <!-- Error message appears here -->
    </div>
    
    <!-- Semantic button element -->
    <button type="submit">
        Submit Form
    </button>
</form>

<style>
    /* Visible focus indicator (WCAG 2.4.7) */
    input:focus, button:focus {
        outline: 3px solid #0066cc;
        outline-offset: 2px;
    }
    
    /* Sufficient color contrast */
    label {
        color: #333; /* 12.6:1 contrast with white background */
    }
    
    .help-text {
        color: #666; /* 5.7:1 contrast */
        font-size: 0.9em;
    }
    
    .error {
        color: #c00; /* 5.9:1 contrast */
        font-weight: bold;
    }
    
    /* Required indicator */
    [aria-label="required"] {
        color: #c00;
    }
</style>
```

**WCAG 2.1 Level AA Checklist**:

```markdown
## 1. Perceivable

### 1.1 Text Alternatives
- [ ] Images have alt text
- [ ] Decorative images have empty alt (alt="")
- [ ] Icons have aria-label
- [ ] Form buttons have accessible names

### 1.2 Time-based Media
- [ ] Videos have captions
- [ ] Audio has transcripts
- [ ] Media players are keyboard accessible

### 1.3 Adaptable
- [ ] Logical heading structure (h1, h2, h3)
- [ ] Semantic HTML (nav, main, article, aside)
- [ ] Form inputs have labels
- [ ] Tables have proper headers

### 1.4 Distinguishable
- [ ] Text contrast ≥ 4.5:1 (AA) or 7:1 (AAA)
- [ ] Large text contrast ≥ 3:1
- [ ] Color not sole indicator
- [ ] Text resizable to 200%
- [ ] No images of text

## 2. Operable

### 2.1 Keyboard Accessible
- [ ] All functionality keyboard accessible
- [ ] No keyboard traps
- [ ] Visible focus indicators
- [ ] Logical tab order

### 2.2 Enough Time
- [ ] Adjustable time limits
- [ ] Pause/stop for moving content
- [ ] No auto-refresh without warning

### 2.3 Seizures
- [ ] No flashing content >3 per second
- [ ] No red flashing

### 2.4 Navigable
- [ ] Skip navigation link
- [ ] Descriptive page titles
- [ ] Logical focus order
- [ ] Link purpose clear
- [ ] Multiple navigation methods
- [ ] Descriptive headings and labels

## 3. Understandable

### 3.1 Readable
- [ ] Page language identified (lang="en")
- [ ] Language changes marked
- [ ] Unusual words defined

### 3.2 Predictable
- [ ] Consistent navigation
- [ ] Consistent identification
- [ ] No unexpected context changes
- [ ] Explicit form submission

### 3.3 Input Assistance
- [ ] Error identification
- [ ] Error suggestions provided
- [ ] Error prevention for legal/financial
- [ ] Labels and instructions provided
- [ ] Required fields indicated

## 4. Robust

### 4.1 Compatible
- [ ] Valid HTML (no parsing errors)
- [ ] Unique IDs
- [ ] Proper ARIA attributes
- [ ] Name, role, value for custom controls
```

**Automated Testing with axe-core**:
```javascript
// Using axe-core in JavaScript
const { axe } = require('axe-core');

async function runAccessibilityTests() {
    const results = await axe.run(document);
    
    console.log(`Found ${results.violations.length} violations`);
    
    results.violations.forEach(violation => {
        console.log(`
[${violation.impact.toUpperCase()}] ${violation.help}
    WCAG: ${violation.tags.join(', ')}
    Affected elements: ${violation.nodes.length}
        `);
        
        violation.nodes.forEach(node => {
            console.log(`  - ${node.html}`);
            console.log(`    Fix: ${node.failureSummary}`);
        });
    });
    
    return results.violations.length === 0;
}
```

**Python/Selenium Testing**:
```python
from axe_selenium_python import Axe

def test_accessibility(selenium_driver):
    """Test page accessibility with axe"""
    driver.get('https://example.com')
    
    axe = Axe(driver)
    axe.inject()
    results = axe.run()
    
    # Check for violations
    violations = results['violations']
    
    assert len(violations) == 0, f"Found {len(violations)} accessibility violations"
    
    # Report violations
    for violation in violations:
        print(f"[{violation['impact']}] {violation['help']}")
        print(f"  Description: {violation['description']}")
        print(f"  WCAG: {', '.join(violation['tags'])}")
        
        for node in violation['nodes']:
            print(f"  Element: {node['html']}")
            print(f"  Fix: {node['failureSummary']}")
```

**Color Contrast Checking**:
```python
from colour import Color

def check_contrast_ratio(foreground: str, background: str) -> float:
    """
    Calculate WCAG contrast ratio
    Minimum ratios:
    - Normal text: 4.5:1 (AA), 7:1 (AAA)
    - Large text (18pt+): 3:1 (AA), 4.5:1 (AAA)
    """
    fg = Color(foreground)
    bg = Color(background)
    
    # Convert to relative luminance
    def luminance(color):
        rgb = [color.red, color.green, color.blue]
        rgb = [
            c / 12.92 if c <= 0.03928 else ((c + 0.055) / 1.055) ** 2.4
            for c in rgb
        ]
        return 0.2126 * rgb[0] + 0.7152 * rgb[1] + 0.0722 * rgb[2]
    
    l1 = luminance(fg)
    l2 = luminance(bg)
    
    lighter = max(l1, l2)
    darker = min(l1, l2)
    
    ratio = (lighter + 0.05) / (darker + 0.05)
    return ratio

# Example
ratio = check_contrast_ratio('#333333', '#ffffff')
print(f"Contrast ratio: {ratio:.2f}:1")
print(f"AA Normal text: {'✓' if ratio >= 4.5 else '✗'}")
print(f"AA Large text: {'✓' if ratio >= 3.0 else '✗'}")
print(f"AAA Normal text: {'✓' if ratio >= 7.0 else '✗'}")
```

## Best Practices

- ✅ 4.5:1 contrast for normal text, 3:1 for large text (18pt+)
- ✅ Use semantic HTML (nav, main, button, not div)
- ✅ Alt text for images (empty alt="" for decorative)
- ✅ Keyboard accessible (Tab, Enter, Space, Arrow keys)
- ✅ ARIA labels where semantic HTML insufficient
- ✅ Visible focus indicators (outline, ring)
- ✅ Logical heading structure (h1 → h2 → h3)
- ✅ Form labels properly associated with inputs
- ✅ Error messages announced to screen readers
- ✅ Responsive text sizing (rem, em units)
- ✅ Test with actual assistive technologies
- ❌ Avoid: Color as only indicator (use icons, text)
- ❌ Avoid: Div/span as buttons (use <button>)
- ❌ Avoid: Placeholder as label
- ❌ Avoid: Removing focus outlines (outline: none)
- ❌ Avoid: Auto-playing audio/video
- ❌ Avoid: Time limits without ability to extend
---
name: "Screen Reader Testing"
description: "Test with NVDA, JAWS, VoiceOver, validate ARIA attributes, and ensure semantic HTML"
category: "accessibility"
required_tools: ["Read", "Bash", "WebSearch"]
---

# Screen Reader Testing

## Purpose
Validate that applications work correctly with screen readers used by blind and visually impaired users, ensuring all content and functionality is accessible.

## When to Use
- Testing web applications
- Validating ARIA implementations
- Accessibility compliance checks
- After UI changes
- Before production deployment

## Key Capabilities

1. **Screen Reader Testing** - Test with NVDA, JAWS, VoiceOver
2. **ARIA Validation** - Ensure proper ARIA usage
3. **Announcement Testing** - Verify meaningful announcements

## Approach

1. **Select Screen Readers to Test**
   - **NVDA** (Windows, free): Most common for testing
   - **JAWS** (Windows, paid): Industry standard, widely used
   - **VoiceOver** (Mac/iOS, built-in): Apple ecosystem
   - **TalkBack** (Android, built-in): Mobile testing
   - **Narrator** (Windows, built-in): Basic testing

2. **Test Navigation Patterns**
   - Linear reading (browse mode)
   - Heading navigation (H key)
   - Link navigation (Tab, K key)
   - Form field navigation (F key)
   - Landmark navigation (D key)
   - Table navigation (T key, Ctrl+Alt+arrows)

3. **Verify Announcements**
   - All content is announced
   - Interactive elements have clear labels
   - Dynamic content changes are announced
   - Form errors are announced
   - Loading states are announced

4. **Check ARIA Implementation**
   - Roles are appropriate
   - States and properties are accurate
   - Live regions work correctly
   - Hidden content is properly hidden

5. **Test Common Scenarios**
   - Page load and initial focus
   - Form submission
   - Modal dialogs
   - Dropdown menus
   - Dynamic content updates
   - Error messages

## Example

**Context**: Testing a form with screen reader

```html
<!-- Good: Screen reader friendly form -->
<form role="search" aria-label="Product Search">
    <!-- Proper label association -->
    <label for="search-input">
        Search products
    </label>
    <input 
        id="search-input"
        type="search"
        name="q"
        aria-label="Search products"
        aria-describedby="search-help"
        autocomplete="off"
        role="searchbox"
    >
    <div id="search-help" class="sr-only">
        Enter product name or SKU. Use arrow keys to navigate suggestions.
    </div>
    
    <!-- Live region for results -->
    <div 
        role="status" 
        aria-live="polite" 
        aria-atomic="true"
        class="sr-only"
    >
        <span id="results-count"></span>
    </div>
    
    <!-- Autocomplete results -->
    <ul 
        id="search-results"
        role="listbox"
        aria-label="Search suggestions"
    >
        <!-- Populated dynamically -->
    </ul>
    
    <button type="submit" aria-label="Search">
        <svg aria-hidden="true" focusable="false">
            <!-- Search icon -->
        </svg>
        Search
    </button>
</form>

<script>
// Update results count for screen readers
function updateResultsCount(count) {
    const announcement = document.getElementById('results-count');
    announcement.textContent = `${count} results found`;
}

// Manage focus in autocomplete
function handleSearchInput(input) {
    const results = document.getElementById('search-results');
    
    // Update ARIA attributes
    input.setAttribute('aria-expanded', 'true');
    input.setAttribute('aria-activedescendant', '');
    input.setAttribute('aria-controls', 'search-results');
    
    // Announce results
    updateResultsCount(5);
}
</script>
```

**Testing with NVDA (Windows)**:
```
Test Steps:
1. Start NVDA (Ctrl + Alt + N)
2. Navigate to page
3. Press H to jump between headings
4. Press F to move to form fields
5. Press Tab to move through interactive elements
6. Type in search box, verify autocomplete announcements
7. Press Escape to close autocomplete
8. Press Enter to submit

Expected Announcements:
- "Search products, edit text" (focus on input)
- "5 results found" (after typing)
- "Product name, list, 1 of 5" (arrow down to first result)
- "Search, button" (Tab to submit button)
```

**Testing with VoiceOver (Mac)**:
```
Test Steps:
1. Enable VoiceOver (Cmd + F5)
2. Navigate page (VO + Right Arrow)
3. Jump to headings (VO + Cmd + H)
4. Open rotor (VO + U) to see landmarks, headings, links
5. Navigate form (VO + Cmd + J)
6. Interact with form (VO + Space to enter form)
7. Type in field, verify announcements
8. Exit form (VO + Shift + Up Arrow)

Expected Announcements:
- "Search products, search text field"
- "You are currently on a text field, to enter text in this field, type"
- "5 results found" (status message)
- "Search, button"
```

**Common Screen Reader Issues**:

```html
<!-- BAD: No label -->
<input type="text" placeholder="Email">
<!-- Screen reader: "Edit text" (no context) -->

<!-- GOOD: Proper label -->
<label for="email">Email address</label>
<input id="email" type="email">
<!-- Screen reader: "Email address, edit text" -->

---

<!-- BAD: Div as button -->
<div onclick="submit()">Submit</div>
<!-- Screen reader: "Submit" (not announced as interactive) -->

<!-- GOOD: Semantic button -->
<button type="submit">Submit</button>
<!-- Screen reader: "Submit, button" -->

---

<!-- BAD: Icon without text -->
<button><i class="icon-search"></i></button>
<!-- Screen reader: "Button" (no purpose) -->

<!-- GOOD: Icon with label -->
<button aria-label="Search">
    <svg aria-hidden="true" focusable="false">
        <!-- Icon SVG -->
    </svg>
    <span class="sr-only">Search</span>
</button>
<!-- Screen reader: "Search, button" -->

---

<!-- BAD: Dynamic content not announced -->
<div id="status">Loading...</div>
<script>
    setTimeout(() => {
        document.getElementById('status').textContent = 'Loaded';
    }, 2000);
</script>
<!-- Screen reader: No announcement of change -->

<!-- GOOD: Live region -->
<div role="status" aria-live="polite" aria-atomic="true">
    <span id="status">Loading...</span>
</div>
<script>
    setTimeout(() => {
        document.getElementById('status').textContent = 'Content loaded';
    }, 2000);
</script>
<!-- Screen reader: "Content loaded" -->

---

<!-- BAD: Table without headers -->
<table>
    <tr><td>Name</td><td>Age</td></tr>
    <tr><td>John</td><td>30</td></tr>
</table>
<!-- Screen reader: Announces cells but no context -->

<!-- GOOD: Table with proper headers -->
<table>
    <thead>
        <tr>
            <th scope="col">Name</th>
            <th scope="col">Age</th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <td>John</td>
            <td>30</td>
        </tr>
    </tbody>
</table>
<!-- Screen reader: "Name column header, John. Age column header, 30" -->
```

**ARIA Live Regions**:
```html
<!-- Polite: Announces when screen reader is idle -->
<div role="status" aria-live="polite">
    Form saved successfully
</div>

<!-- Assertive: Announces immediately (use sparingly) -->
<div role="alert" aria-live="assertive">
    Error: Invalid email address
</div>

<!-- Off: Never announces (default) -->
<div aria-live="off">
    This won't be announced
</div>

<!-- Atomic: Announce entire region vs just changes -->
<div aria-live="polite" aria-atomic="true">
    <span id="count">5</span> items in cart
</div>
<!-- Announces: "5 items in cart" (entire message) -->

<div aria-live="polite" aria-atomic="false">
    <span id="count">5</span> items in cart
</div>
<!-- Announces: "5" (just the changed part) -->
```

**Screen Reader Only Text**:
```css
/* Visually hidden but announced by screen readers */
.sr-only {
    position: absolute;
    width: 1px;
    height: 1px;
    padding: 0;
    margin: -1px;
    overflow: hidden;
    clip: rect(0, 0, 0, 0);
    white-space: nowrap;
    border-width: 0;
}
```

**Testing Checklist**:
```markdown
## Navigation
- [ ] Can navigate entire page with screen reader
- [ ] Headings provide logical structure (h1, h2, h3)
- [ ] Landmarks are properly labeled (nav, main, aside)
- [ ] Skip navigation link works
- [ ] Tab order is logical

## Forms
- [ ] All form fields have labels
- [ ] Required fields are announced
- [ ] Error messages are associated with fields
- [ ] Errors announced when they occur
- [ ] Success messages announced

## Interactive Elements
- [ ] Buttons announced as "button"
- [ ] Links announced as "link" with purpose
- [ ] Current page/tab indicated
- [ ] Expanded/collapsed state announced
- [ ] Loading/busy states announced

## Images and Media
- [ ] Images have alt text
- [ ] Decorative images hidden (alt="", aria-hidden="true")
- [ ] Complex images have extended descriptions
- [ ] Videos have captions
- [ ] Audio has transcripts

## Dynamic Content
- [ ] Dynamic changes announced (aria-live)
- [ ] Loading spinners have labels
- [ ] Modals trap and restore focus
- [ ] Toast notifications announced
- [ ] Auto-updates announced

## Tables
- [ ] Headers properly marked (<th>)
- [ ] Row/column headers associated
- [ ] Complex tables have summary
- [ ] Navigation between cells works

## Custom Controls
- [ ] Role attributes appropriate
- [ ] States and properties accurate
- [ ] Keyboard interactions work
- [ ] Focus management correct
```

## Best Practices

- ✅ Use semantic HTML first (button, nav, main)
- ✅ ARIA only when semantic HTML insufficient
- ✅ Test with multiple screen readers
- ✅ Provide text alternatives for all content
- ✅ Announce dynamic changes (aria-live)
- ✅ Associate labels with form controls
- ✅ Provide skip links for navigation
- ✅ Use headings for document structure
- ✅ Test keyboard and screen reader together
- ✅ Hide decorative content (aria-hidden="true")
- ❌ Avoid: Overusing ARIA (semantic HTML better)
- ❌ Avoid: Unlabeled form controls
- ❌ Avoid: Empty links or buttons
- ❌ Avoid: Not announcing dynamic content
- ❌ Avoid: Breaking document structure with headings
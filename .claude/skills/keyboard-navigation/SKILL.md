---
name: "Keyboard Navigation"
description: "Implement focus management, optimize tab order, and validate keyboard shortcuts"
category: "accessibility"
required_tools: ["Read", "Bash"]
---

# Keyboard Navigation

## Purpose
Ensure all functionality is accessible via keyboard for users who cannot or do not use a mouse, including those with motor disabilities and power users.

## When to Use
- Building interactive UIs
- Implementing modals, dropdowns, menus
- Creating custom controls
- Accessibility testing
- Keyboard shortcut implementation

## Key Capabilities

1. **Focus Management** - Ensure logical focus order and visible indicators
2. **Keyboard Shortcuts** - Implement accessible shortcuts
3. **Focus Trapping** - Manage focus in modals and dialogs

## Approach

1. **Enable Keyboard Access**
   - All interactive elements focusable
   - Logical tab order (left-to-right, top-to-bottom)
   - No keyboard traps (can escape any control)
   - Custom controls keyboard accessible

2. **Implement Standard Keys**
   - **Tab**: Move forward through interactive elements
   - **Shift+Tab**: Move backward
   - **Enter**: Activate buttons, submit forms, follow links
   - **Space**: Activate buttons, checkboxes, toggle controls
   - **Escape**: Close modals, cancel operations, clear selections
   - **Arrow keys**: Navigate within components (menus, tabs, lists)
   - **Home/End**: Jump to start/end of content

3. **Provide Visible Focus Indicators**
   - Clear outline or ring
   - Sufficient contrast (3:1 minimum)
   - Consistent across site
   - Never remove without replacement

4. **Manage Focus**
   - Set focus to modals when opened
   - Restore focus when closed
   - Trap focus in modals
   - Skip navigation links

5. **Test Keyboard Navigation**
   - Unplug mouse
   - Navigate entire site with keyboard only
   - Verify all functionality accessible
   - Check focus indicators visible

## Example

**Context**: Accessible modal dialog with focus trap

```javascript
class AccessibleModal {
    constructor(modalElement) {
        this.modal = modalElement;
        this.focusableElements = null;
        this.firstFocusable = null;
        this.lastFocusable = null;
        this.previousFocus = null;
        
        // Bind event handlers
        this.handleKeyDown = this.handleKeyDown.bind(this);
    }
    
    open() {
        // Store current focus to restore later
        this.previousFocus = document.activeElement;
        
        // Show modal
        this.modal.style.display = 'block';
        this.modal.setAttribute('aria-hidden', 'false');
        
        // Get all focusable elements
        this.focusableElements = this.modal.querySelectorAll(
            'button, [href], input, select, textarea, [tabindex]:not([tabindex="-1"])'
        );
        
        this.firstFocusable = this.focusableElements[0];
        this.lastFocusable = this.focusableElements[this.focusableElements.length - 1];
        
        // Focus first element or modal itself
        if (this.firstFocusable) {
            this.firstFocusable.focus();
        } else {
            this.modal.focus();
        }
        
        // Trap focus
        this.modal.addEventListener('keydown', this.handleKeyDown);
        
        // Prevent body scroll
        document.body.style.overflow = 'hidden';
    }
    
    close() {
        // Hide modal
        this.modal.style.display = 'none';
        this.modal.setAttribute('aria-hidden', 'true');
        
        // Remove event listener
        this.modal.removeEventListener('keydown', this.handleKeyDown);
        
        // Restore focus to previous element
        if (this.previousFocus) {
            this.previousFocus.focus();
        }
        
        // Restore body scroll
        document.body.style.overflow = '';
    }
    
    handleKeyDown(e) {
        // Trap focus within modal
        if (e.key === 'Tab') {
            if (e.shiftKey) {
                // Shift+Tab: backward
                if (document.activeElement === this.firstFocusable) {
                    e.preventDefault();
                    this.lastFocusable.focus();
                }
            } else {
                // Tab: forward
                if (document.activeElement === this.lastFocusable) {
                    e.preventDefault();
                    this.firstFocusable.focus();
                }
            }
        }
        
        // Close on Escape
        if (e.key === 'Escape') {
            this.close();
        }
    }
}

// Usage
const modal = new AccessibleModal(document.getElementById('myModal'));

document.getElementById('openModal').addEventListener('click', () => {
    modal.open();
});

document.getElementById('closeModal').addEventListener('click', () => {
    modal.close();
});
```

**HTML for Accessible Modal**:
```html
<!-- Modal trigger -->
<button 
    id="openModal"
    aria-haspopup="dialog"
    aria-expanded="false"
>
    Open Dialog
</button>

<!-- Modal -->
<div 
    id="myModal"
    role="dialog"
    aria-modal="true"
    aria-labelledby="modal-title"
    aria-describedby="modal-description"
    aria-hidden="true"
    tabindex="-1"
>
    <div class="modal-content">
        <h2 id="modal-title">Confirm Action</h2>
        <p id="modal-description">
            Are you sure you want to delete this item?
        </p>
        
        <div class="modal-actions">
            <button id="confirmButton">
                Confirm
            </button>
            <button id="closeModal">
                Cancel
            </button>
        </div>
    </div>
</div>

<style>
    /* Visible focus indicators */
    button:focus,
    a:focus,
    input:focus {
        outline: 3px solid #0066cc;
        outline-offset: 2px;
    }
    
    /* Focus within modal */
    .modal-content *:focus {
        outline-color: #0066cc;
    }
</style>
```

**Accessible Dropdown Menu**:
```javascript
class AccessibleDropdown {
    constructor(button, menu) {
        this.button = button;
        this.menu = menu;
        this.menuItems = menu.querySelectorAll('[role="menuitem"]');
        this.currentIndex = -1;
        
        this.button.addEventListener('click', () => this.toggle());
        this.button.addEventListener('keydown', (e) => this.handleButtonKey(e));
        this.menu.addEventListener('keydown', (e) => this.handleMenuKey(e));
        
        // Close on outside click
        document.addEventListener('click', (e) => {
            if (!this.button.contains(e.target) && !this.menu.contains(e.target)) {
                this.close();
            }
        });
    }
    
    toggle() {
        if (this.menu.style.display === 'block') {
            this.close();
        } else {
            this.open();
        }
    }
    
    open() {
        this.menu.style.display = 'block';
        this.button.setAttribute('aria-expanded', 'true');
        this.currentIndex = 0;
        this.menuItems[0].focus();
    }
    
    close() {
        this.menu.style.display = 'none';
        this.button.setAttribute('aria-expanded', 'false');
        this.button.focus();
        this.currentIndex = -1;
    }
    
    handleButtonKey(e) {
        // Open on Enter, Space, or Down Arrow
        if (e.key === 'Enter' || e.key === ' ' || e.key === 'ArrowDown') {
            e.preventDefault();
            this.open();
        }
    }
    
    handleMenuKey(e) {
        switch (e.key) {
            case 'ArrowDown':
                e.preventDefault();
                this.currentIndex = (this.currentIndex + 1) % this.menuItems.length;
                this.menuItems[this.currentIndex].focus();
                break;
            
            case 'ArrowUp':
                e.preventDefault();
                this.currentIndex = this.currentIndex - 1;
                if (this.currentIndex < 0) {
                    this.currentIndex = this.menuItems.length - 1;
                }
                this.menuItems[this.currentIndex].focus();
                break;
            
            case 'Home':
                e.preventDefault();
                this.currentIndex = 0;
                this.menuItems[0].focus();
                break;
            
            case 'End':
                e.preventDefault();
                this.currentIndex = this.menuItems.length - 1;
                this.menuItems[this.currentIndex].focus();
                break;
            
            case 'Escape':
                e.preventDefault();
                this.close();
                break;
            
            case 'Enter':
            case ' ':
                e.preventDefault();
                this.menuItems[this.currentIndex].click();
                this.close();
                break;
        }
    }
}
```

**Skip Navigation Link**:
```html
<!-- Skip link (first focusable element) -->
<a href="#main-content" class="skip-link">
    Skip to main content
</a>

<!-- Navigation -->
<nav>
    <!-- Many navigation links -->
</nav>

<!-- Main content -->
<main id="main-content" tabindex="-1">
    <!-- Page content -->
</main>

<style>
    /* Hidden by default, visible on focus */
    .skip-link {
        position: absolute;
        top: -40px;
        left: 0;
        background: #000;
        color: #fff;
        padding: 8px;
        text-decoration: none;
        z-index: 100;
    }
    
    .skip-link:focus {
        top: 0;
    }
</style>
```

**Keyboard Shortcuts**:
```javascript
// Global keyboard shortcuts
document.addEventListener('keydown', (e) => {
    // Ignore if typing in input
    if (e.target.matches('input, textarea')) {
        return;
    }
    
    // Ctrl/Cmd + K: Search
    if ((e.ctrlKey || e.metaKey) && e.key === 'k') {
        e.preventDefault();
        document.getElementById('search-input').focus();
    }
    
    // ? : Show keyboard shortcuts
    if (e.key === '?') {
        e.preventDefault();
        showKeyboardShortcuts();
    }
    
    // Esc: Close any open modals
    if (e.key === 'Escape') {
        closeAllModals();
    }
});

// Show keyboard shortcuts dialog
function showKeyboardShortcuts() {
    const shortcuts = [
        { keys: 'Ctrl+K', action: 'Open search' },
        { keys: '?', action: 'Show keyboard shortcuts' },
        { keys: 'Esc', action: 'Close dialog' },
        { keys: 'Tab', action: 'Next element' },
        { keys: 'Shift+Tab', action: 'Previous element' },
    ];
    
    // Display shortcuts in accessible modal
    showModal({
        title: 'Keyboard Shortcuts',
        content: renderShortcuts(shortcuts)
    });
}
```

**Testing Keyboard Navigation**:
```markdown
## Testing Checklist

### Basic Navigation
- [ ] Tab moves forward through all interactive elements
- [ ] Shift+Tab moves backward
- [ ] Focus indicator always visible
- [ ] Tab order is logical (reading order)
- [ ] No keyboard traps (can escape every control)

### Interactive Elements
- [ ] Enter activates buttons and links
- [ ] Space activates buttons and checkboxes
- [ ] Arrow keys navigate dropdowns/menus
- [ ] Escape closes modals and menus
- [ ] Custom controls have keyboard support

### Forms
- [ ] Tab moves between form fields
- [ ] Arrow keys work in radio groups
- [ ] Space toggles checkboxes
- [ ] Enter submits form
- [ ] Error messages reachable via keyboard

### Modals
- [ ] Focus moves to modal when opened
- [ ] Tab cycles within modal (focus trap)
- [ ] Escape closes modal
- [ ] Focus returns to trigger on close

### Navigation
- [ ] Skip link works
- [ ] All navigation items reachable
- [ ] Current page indicated
- [ ] Submenus keyboard accessible

### Custom Controls
- [ ] Appropriate ARIA roles
- [ ] Keyboard interactions documented
- [ ] All states keyboard accessible
- [ ] Focus management correct
```

## Best Practices

- ✅ All interactive elements keyboard accessible
- ✅ Visible focus indicators (3px outline minimum)
- ✅ Logical tab order (no tabindex > 0)
- ✅ Escape closes modals/menus
- ✅ Arrow keys for menus/lists/tabs
- ✅ Home/End for start/end navigation
- ✅ Skip navigation links
- ✅ Focus trapping in modals
- ✅ Restore focus after closing modals
- ✅ Document keyboard shortcuts
- ❌ Avoid: Keyboard traps (can't escape)
- ❌ Avoid: Removing focus outline without replacement
- ❌ Avoid: Using tabindex > 0 (breaks natural order)
- ❌ Avoid: Non-standard keyboard interactions
---
name: "Design Pattern Application"
description: "Apply Gang of Four and architectural patterns appropriately to solve common software design problems"
category: "architecture"
required_tools: ["Read", "Write"]
---

## Purpose
Recognize when to apply established design patterns to create flexible, maintainable, and reusable code structures.

## When to Use
- Designing class structures
- Solving recurring design problems
- Refactoring complex code
- Planning extensible architectures

## Key Capabilities
1. **Pattern Recognition** - Identify when patterns apply
2. **Pattern Implementation** - Correctly implement chosen patterns
3. **Trade-off Analysis** - Understand pattern costs and benefits

## Common Patterns
- **Creational**: Factory, Builder, Singleton
- **Structural**: Adapter, Decorator, Facade
- **Behavioral**: Observer, Strategy, Command

## Example - Strategy Pattern
```python
# Problem: Multiple payment methods with different processing logic

class PaymentStrategy:
    def process(self, amount): pass

class CreditCardPayment(PaymentStrategy):
    def process(self, amount):
        # Credit card processing logic
        print(f"Processing ${amount} via credit card")

class PayPalPayment(PaymentStrategy):
    def process(self, amount):
        # PayPal processing logic
        print(f"Processing ${amount} via PayPal")

class Order:
    def __init__(self, payment_strategy: PaymentStrategy):
        self.payment = payment_strategy
    
    def checkout(self, amount):
        self.payment.process(amount)

# Usage
order1 = Order(CreditCardPayment())
order1.checkout(100)

order2 = Order(PayPalPayment())
order2.checkout(50)
```

## Best Practices
- ✅ Use patterns to solve actual problems, not for their own sake
- ✅ Understand the problem before applying a pattern
- ✅ Keep it simple - don't over-engineer
- ❌ Avoid: Using patterns you don't fully understand
- ❌ Avoid: Forcing patterns where they don't fit

---
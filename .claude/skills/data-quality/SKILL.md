---
name: "Data Quality"
description: "Implement data validation rules, quality metrics, and data cleansing strategies"
category: "database"
required_tools: ["Read", "Write", "Bash"]
---

# Data Quality

## Purpose
Ensure data accuracy, completeness, and consistency through systematic validation, monitoring, and cleansing processes.

## When to Use
- Building data pipelines
- Data migration projects
- Implementing data governance
- Setting up data monitoring
- Resolving data quality issues

## Key Capabilities

1. **Validation Rules** - Define and enforce data quality constraints
2. **Quality Monitoring** - Track metrics and detect anomalies
3. **Data Cleansing** - Fix invalid, duplicate, or inconsistent data

## Approach

1. **Define Quality Dimensions**
   - Accuracy: Data is correct and reliable
   - Completeness: No missing required values
   - Consistency: Data is uniform across systems
   - Timeliness: Data is current and available when needed
   - Validity: Data conforms to defined formats and rules
   - Uniqueness: No unwanted duplicates

2. **Implement Validation Rules**
   - Type checks (correct data types)
   - Range checks (values within bounds)
   - Format checks (email, phone, date patterns)
   - Referential integrity (foreign keys exist)
   - Business rules (quantity > 0, dates in order)

3. **Monitor Quality Metrics**
   - Track error rates over time
   - Measure completeness percentages
   - Count null/missing values
   - Identify duplicate records
   - Detect anomalies and outliers

4. **Cleanse Problematic Data**
   - Remove duplicates
   - Fill missing values (default, interpolation)
   - Standardize formats
   - Correct known errors
   - Flag unresolvable issues

5. **Report and Alert**
   - Dashboard with quality metrics
   - Alerts when thresholds breached
   - Quality reports for stakeholders
   - Trend analysis

## Example

**Context**: Sales data quality validation

```python
import pandas as pd
import numpy as np
from datetime import datetime, timedelta

class DataQualityChecker:
    def __init__(self, df):
        self.df = df
        self.issues = []
        self.metrics = {}
    
    def check_completeness(self):
        """Check for missing required fields"""
        required_fields = ['customer_id', 'product_id', 'sale_date', 
                          'quantity', 'unit_price', 'total_amount']
        
        for field in required_fields:
            null_count = self.df[field].isna().sum()
            null_pct = (null_count / len(self.df)) * 100
            
            self.metrics[f'{field}_completeness'] = 100 - null_pct
            
            if null_count > 0:
                self.issues.append({
                    'dimension': 'Completeness',
                    'severity': 'High' if null_pct > 5 else 'Medium',
                    'field': field,
                    'description': f'{null_count} missing values ({null_pct:.1f}%)',
                    'impact': f'{null_count} records cannot be processed'
                })
    
    def check_accuracy(self):
        """Validate data accuracy"""
        # Quantity must be positive
        invalid_qty = (self.df['quantity'] <= 0).sum()
        if invalid_qty > 0:
            self.issues.append({
                'dimension': 'Accuracy',
                'severity': 'High',
                'field': 'quantity',
                'description': f'{invalid_qty} records with quantity <= 0',
                'impact': 'Invalid sales records'
            })
        
        # Price must be positive
        invalid_price = (self.df['unit_price'] <= 0).sum()
        if invalid_price > 0:
            self.issues.append({
                'dimension': 'Accuracy',
                'severity': 'High',
                'field': 'unit_price',
                'description': f'{invalid_price} records with price <= 0',
                'impact': 'Revenue calculations incorrect'
            })
        
        # Total amount should match quantity * price
        calculated_total = self.df['quantity'] * self.df['unit_price']
        mismatch = ~np.isclose(calculated_total, self.df['total_amount'], rtol=0.01)
        mismatch_count = mismatch.sum()
        
        if mismatch_count > 0:
            self.issues.append({
                'dimension': 'Accuracy',
                'severity': 'High',
                'field': 'total_amount',
                'description': f'{mismatch_count} records where total != qty * price',
                'impact': 'Financial reports will be incorrect'
            })
    
    def check_consistency(self):
        """Check data consistency"""
        # Sale date should not be in the future
        future_dates = (self.df['sale_date'] > datetime.now()).sum()
        if future_dates > 0:
            self.issues.append({
                'dimension': 'Consistency',
                'severity': 'High',
                'field': 'sale_date',
                'description': f'{future_dates} records with future dates',
                'impact': 'Time series analysis will be incorrect'
            })
        
        # Check for duplicate sale IDs
        duplicates = self.df['sale_id'].duplicated().sum()
        if duplicates > 0:
            self.issues.append({
                'dimension': 'Consistency',
                'severity': 'Critical',
                'field': 'sale_id',
                'description': f'{duplicates} duplicate sale IDs',
                'impact': 'Double counting revenue'
            })
    
    def check_timeliness(self):
        """Check data freshness"""
        if 'sale_date' in self.df.columns:
            max_date = self.df['sale_date'].max()
            age_days = (datetime.now() - max_date).days
            
            self.metrics['data_age_days'] = age_days
            
            if age_days > 7:
                self.issues.append({
                    'dimension': 'Timeliness',
                    'severity': 'Medium',
                    'field': 'sale_date',
                    'description': f'Data is {age_days} days old',
                    'impact': 'Reports may not reflect current state'
                })
    
    def check_validity(self):
        """Check format validity"""
        # Email format check (if email field exists)
        if 'email' in self.df.columns:
            email_pattern = r'^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$'
            invalid_emails = ~self.df['email'].str.match(email_pattern, na=False)
            invalid_count = invalid_emails.sum()
            
            if invalid_count > 0:
                self.issues.append({
                    'dimension': 'Validity',
                    'severity': 'Medium',
                    'field': 'email',
                    'description': f'{invalid_count} invalid email formats',
                    'impact': 'Cannot send notifications to customers'
                })
    
    def check_referential_integrity(self, customer_df, product_df):
        """Check foreign key references"""
        # Customers exist
        invalid_customers = ~self.df['customer_id'].isin(customer_df['customer_id'])
        invalid_count = invalid_customers.sum()
        
        if invalid_count > 0:
            self.issues.append({
                'dimension': 'Referential Integrity',
                'severity': 'Critical',
                'field': 'customer_id',
                'description': f'{invalid_count} sales with non-existent customers',
                'impact': 'Cannot join with customer data'
            })
        
        # Products exist
        invalid_products = ~self.df['product_id'].isin(product_df['product_id'])
        invalid_count = invalid_products.sum()
        
        if invalid_count > 0:
            self.issues.append({
                'dimension': 'Referential Integrity',
                'severity': 'Critical',
                'field': 'product_id',
                'description': f'{invalid_count} sales with non-existent products',
                'impact': 'Cannot join with product data'
            })
    
    def run_all_checks(self, customer_df=None, product_df=None):
        """Run all quality checks"""
        self.check_completeness()
        self.check_accuracy()
        self.check_consistency()
        self.check_timeliness()
        self.check_validity()
        
        if customer_df is not None and product_df is not None:
            self.check_referential_integrity(customer_df, product_df)
        
        return self.generate_report()
    
    def generate_report(self):
        """Generate quality report"""
        report = {
            'total_records': len(self.df),
            'total_issues': len(self.issues),
            'critical_issues': len([i for i in self.issues if i['severity'] == 'Critical']),
            'high_issues': len([i for i in self.issues if i['severity'] == 'High']),
            'medium_issues': len([i for i in self.issues if i['severity'] == 'Medium']),
            'metrics': self.metrics,
            'issues': self.issues,
            'passed': len(self.issues) == 0
        }
        
        return report

# Usage example
df = pd.read_csv('sales_data.csv')
checker = DataQualityChecker(df)

customers = pd.read_csv('customers.csv')
products = pd.read_csv('products.csv')

report = checker.run_all_checks(customers, products)

print(f"Data Quality Report")
print(f"Total Records: {report['total_records']}")
print(f"Total Issues: {report['total_issues']}")
print(f"  Critical: {report['critical_issues']}")
print(f"  High: {report['high_issues']}")
print(f"  Medium: {report['medium_issues']}")

if not report['passed']:
    print("\nIssues Found:")
    for issue in report['issues']:
        print(f"  [{issue['severity']}] {issue['field']}: {issue['description']}")
```

**Data Cleansing Example**:
```python
def cleanse_sales_data(df):
    """Clean and standardize sales data"""
    df_clean = df.copy()
    
    # Remove exact duplicates
    df_clean = df_clean.drop_duplicates()
    
    # Remove records with invalid quantities
    df_clean = df_clean[df_clean['quantity'] > 0]
    
    # Fill missing discounts with 0
    df_clean['discount'] = df_clean['discount'].fillna(0)
    
    # Standardize date format
    df_clean['sale_date'] = pd.to_datetime(df_clean['sale_date'], errors='coerce')
    
    # Remove records with future dates
    df_clean = df_clean[df_clean['sale_date'] <= datetime.now()]
    
    # Recalculate total amount for consistency
    df_clean['total_amount'] = df_clean['quantity'] * df_clean['unit_price'] - df_clean['discount']
    
    # Flag records that couldn't be cleaned
    df_clean['data_quality_flag'] = 'clean'
    
    return df_clean
```

## Best Practices

- ✅ Validate data at source and target systems
- ✅ Define quality metrics and acceptable thresholds
- ✅ Monitor quality trends over time, not just point checks
- ✅ Automate quality checks in ETL pipelines
- ✅ Alert on quality degradation immediately
- ✅ Document all quality rules and their business rationale
- ✅ Quarantine bad data rather than rejecting it
- ✅ Track quality metrics in dashboards
- ✅ Implement data quality SLAs
- ✅ Log all data quality issues for analysis
- ❌ Avoid: Silently fixing data without logging
- ❌ Avoid: Loading invalid data to production systems
- ❌ Avoid: Ignoring data quality until reports break
- ❌ Avoid: Manual data quality checks (automate them)
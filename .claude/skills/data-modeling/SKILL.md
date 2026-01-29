---
name: "Data Modeling"
description: "Design dimensional models, star/snowflake schemas, and data warehouse structures for analytics and reporting"
category: "database"
required_tools: ["Read", "Write", "WebSearch"]
---

# Data Modeling

## Purpose
Design effective data models for databases and data warehouses, using normalization for transactional systems and dimensional modeling for analytics systems.

## When to Use
- Designing new database schemas
- Building data warehouses or data marts
- Creating analytics platforms
- Planning data migration projects
- Optimizing existing data structures
- Designing for reporting and BI tools

## Key Capabilities

1. **Dimensional Modeling** - Design star and snowflake schemas for analytics
2. **Normalization** - Apply normal forms for transactional databases
3. **Entity-Relationship Design** - Model entities, attributes, and relationships

## Approach

1. **Identify Business Processes**
   - Understand business questions to be answered
   - Identify key metrics and KPIs
   - Determine grain (level of detail) for facts

2. **For Transactional Systems (OLTP)**
   - Use normalized design (3NF typically)
   - Define entities and relationships
   - Identify primary and foreign keys
   - Apply normalization rules to reduce redundancy

3. **For Analytics Systems (OLAP)**
   - Use dimensional modeling (star or snowflake)
   - Define fact tables (measurements)
   - Define dimension tables (context)
   - Create slowly changing dimensions (SCD) strategy
   - Design aggregate tables for performance

4. **Design Patterns**
   - **Star Schema**: Central fact table with denormalized dimensions
   - **Snowflake Schema**: Normalized dimension tables
   - **Fact Tables**: Measurements, metrics, additive values
   - **Dimension Tables**: Descriptive attributes, hierarchies
   - **Bridge Tables**: Many-to-many relationships

5. **Validate Design**
   - Can answer all business questions
   - Appropriate grain for analysis
   - Efficient query performance
   - Scalable for data growth

## Example

**Context**: E-commerce sales analytics data warehouse

**Star Schema Design**:

```sql
-- Fact Table: Sales
CREATE TABLE fact_sales (
    sale_id BIGINT PRIMARY KEY,
    date_key INT NOT NULL,
    product_key INT NOT NULL,
    customer_key INT NOT NULL,
    store_key INT NOT NULL,
    
    -- Measures (additive)
    quantity INT NOT NULL,
    unit_price DECIMAL(10,2) NOT NULL,
    discount_amount DECIMAL(10,2) DEFAULT 0,
    tax_amount DECIMAL(10,2) NOT NULL,
    total_amount DECIMAL(10,2) NOT NULL,
    cost_amount DECIMAL(10,2) NOT NULL,
    
    -- Calculated at query time:
    -- profit = total_amount - cost_amount
    -- revenue = total_amount - discount_amount
    
    FOREIGN KEY (date_key) REFERENCES dim_date(date_key),
    FOREIGN KEY (product_key) REFERENCES dim_product(product_key),
    FOREIGN KEY (customer_key) REFERENCES dim_customer(customer_key),
    FOREIGN KEY (store_key) REFERENCES dim_store(store_key)
);

-- Dimension: Date (date dimension is special - no surrogate key changes)
CREATE TABLE dim_date (
    date_key INT PRIMARY KEY,  -- YYYYMMDD format
    full_date DATE NOT NULL,
    year INT NOT NULL,
    quarter INT NOT NULL,
    month INT NOT NULL,
    month_name VARCHAR(20),
    week INT NOT NULL,
    day_of_month INT NOT NULL,
    day_of_week INT NOT NULL,
    day_name VARCHAR(20),
    is_weekend BOOLEAN,
    is_holiday BOOLEAN,
    holiday_name VARCHAR(100),
    fiscal_year INT,
    fiscal_quarter INT
);

-- Dimension: Product (SCD Type 2 - track history)
CREATE TABLE dim_product (
    product_key INT PRIMARY KEY,  -- Surrogate key
    product_id VARCHAR(50) NOT NULL,  -- Business key
    product_name VARCHAR(200) NOT NULL,
    category VARCHAR(100),
    subcategory VARCHAR(100),
    brand VARCHAR(100),
    supplier_name VARCHAR(200),
    
    -- SCD Type 2 fields
    effective_date DATE NOT NULL,
    end_date DATE,  -- NULL for current record
    is_current BOOLEAN DEFAULT TRUE,
    
    INDEX idx_product_id (product_id),
    INDEX idx_current (product_id, is_current)
);

-- Dimension: Customer (SCD Type 2)
CREATE TABLE dim_customer (
    customer_key INT PRIMARY KEY,
    customer_id VARCHAR(50) NOT NULL,
    customer_name VARCHAR(200),
    email VARCHAR(200),
    customer_segment VARCHAR(50),  -- Platinum, Gold, Silver, Bronze
    
    -- Geographic hierarchy
    city VARCHAR(100),
    state VARCHAR(50),
    country VARCHAR(50),
    region VARCHAR(50),
    
    -- Demographics
    age_group VARCHAR(20),
    gender VARCHAR(20),
    
    -- SCD Type 2 fields
    effective_date DATE NOT NULL,
    end_date DATE,
    is_current BOOLEAN DEFAULT TRUE,
    
    INDEX idx_customer_id (customer_id),
    INDEX idx_current (customer_id, is_current)
);

-- Dimension: Store
CREATE TABLE dim_store (
    store_key INT PRIMARY KEY,
    store_id VARCHAR(50) NOT NULL,
    store_name VARCHAR(200),
    store_type VARCHAR(50),  -- Mall, Street, Online
    
    -- Location
    address VARCHAR(500),
    city VARCHAR(100),
    state VARCHAR(50),
    country VARCHAR(50),
    region VARCHAR(50),
    
    -- Store attributes
    size_sqft INT,
    opening_date DATE,
    manager_name VARCHAR(200),
    
    effective_date DATE NOT NULL,
    end_date DATE,
    is_current BOOLEAN DEFAULT TRUE
);

-- Aggregate Table (for performance)
CREATE TABLE fact_sales_daily_summary (
    date_key INT NOT NULL,
    product_key INT NOT NULL,
    store_key INT NOT NULL,
    
    total_quantity INT,
    total_sales DECIMAL(12,2),
    total_cost DECIMAL(12,2),
    total_profit DECIMAL(12,2),
    transaction_count INT,
    
    PRIMARY KEY (date_key, product_key, store_key),
    FOREIGN KEY (date_key) REFERENCES dim_date(date_key),
    FOREIGN KEY (product_key) REFERENCES dim_product(product_key),
    FOREIGN KEY (store_key) REFERENCES dim_store(store_key)
);
```

**Example Queries**:

```sql
-- Total sales by product category for Q4 2024
SELECT 
    p.category,
    SUM(s.total_amount) as total_sales,
    SUM(s.quantity) as total_units,
    COUNT(DISTINCT s.customer_key) as unique_customers
FROM fact_sales s
JOIN dim_product p ON s.product_key = p.product_key
JOIN dim_date d ON s.date_key = d.date_key
WHERE d.year = 2024 
  AND d.quarter = 4
  AND p.is_current = TRUE
GROUP BY p.category
ORDER BY total_sales DESC;

-- Sales trend by month for specific customer segment
SELECT 
    d.year,
    d.month,
    d.month_name,
    c.customer_segment,
    SUM(s.total_amount) as monthly_sales
FROM fact_sales s
JOIN dim_customer c ON s.customer_key = c.customer_key
JOIN dim_date d ON s.date_key = d.date_key
WHERE c.is_current = TRUE
GROUP BY d.year, d.month, d.month_name, c.customer_segment
ORDER BY d.year, d.month, c.customer_segment;
```

**Design Rationale**:
- **Star schema** for simple queries and good performance
- **Surrogate keys** (product_key, customer_key) for dimension tables
- **Business keys** (product_id, customer_id) preserved for lookups
- **SCD Type 2** for products and customers to track history
- **Date dimension** pre-populated with all dates for easy time-series analysis
- **Additive measures** in fact table (can be summed across any dimension)
- **Aggregate table** for commonly-run reports (daily summaries)
- **Hierarchies** in dimensions (City → State → Country → Region)

## Best Practices

### For Transactional Systems (OLTP)
- ✅ Normalize to 3NF to reduce redundancy
- ✅ Use meaningful primary keys
- ✅ Define foreign key constraints
- ✅ Index foreign key columns
- ✅ Use appropriate data types (don't over-provision)

### For Analytics Systems (OLAP)
- ✅ Use star schema for simplicity (snowflake if space critical)
- ✅ Use surrogate keys for dimensions
- ✅ Implement slowly changing dimensions (SCD)
- ✅ Pre-build date dimension with all dates
- ✅ Keep fact tables lean (only measurements)
- ✅ Denormalize dimensions for query performance
- ✅ Create aggregate tables for common queries
- ✅ Partition large fact tables by date
- ✅ Use columnar storage for analytics workloads

### General
- ✅ Document business definitions of fields
- ✅ Use consistent naming conventions
- ✅ Define clear grain for fact tables
- ✅ Model hierarchies explicitly in dimensions
- ✅ Plan for data growth and archival strategy
- ❌ Avoid: Non-additive facts (use semi-additive or non-additive carefully)
- ❌ Avoid: Storing derived values in OLTP (calculate at query time)
- ❌ Avoid: Over-normalizing analytics systems (hurts query performance)
- ❌ Avoid: Mixing OLTP and OLAP patterns in same database
---
name: "ETL Design"
description: "Design extract-transform-load patterns, data pipeline orchestration, and incremental loading strategies"
category: "database"
required_tools: ["Read", "Write", "Bash"]
---

# ETL Design

## Purpose
Design robust ETL (Extract-Transform-Load) pipelines that move data from source systems to target systems with appropriate transformations, error handling, and performance optimization.

## When to Use
- Building data warehouses or data lakes
- Migrating data between systems
- Integrating multiple data sources
- Creating analytics pipelines
- Implementing data synchronization
- Building real-time or batch data flows

## Key Capabilities

1. **Pipeline Architecture** - Design scalable, maintainable ETL workflows
2. **Incremental Loading** - Implement efficient delta/change detection strategies
3. **Data Transformation** - Clean, enrich, and transform data between systems

## Approach

1. **Extraction Strategy**
   - **Full Extract**: Pull entire dataset (initial load or small tables)
   - **Incremental Extract**: Pull only changes since last run
   - **Change Data Capture (CDC)**: Capture changes from transaction logs
   - **API-based**: Extract via REST APIs with pagination
   - **File-based**: Process CSV, JSON, Parquet files

2. **Transformation Design**
   - **Cleansing**: Handle nulls, duplicates, invalid data
   - **Standardization**: Consistent formats, codes, naming
   - **Enrichment**: Add calculated fields, lookups, joins
   - **Aggregation**: Summarize data for reporting
   - **Normalization/Denormalization**: Match target schema

3. **Loading Strategy**
   - **Full Load**: Truncate and reload (simple but slow)
   - **Incremental Load**: Insert new, update changed (efficient)
   - **Upsert/Merge**: Insert or update based on key
   - **SCD Type 2**: Insert new version, expire old
   - **Append-only**: Insert new records only

4. **Orchestration**
   - Schedule pipelines (cron, Airflow, Luigi)
   - Handle dependencies between jobs
   - Implement error handling and retries
   - Monitor pipeline health
   - Alert on failures

5. **Performance Optimization**
   - Parallel processing where possible
   - Batch operations (bulk insert)
   - Partition large datasets
   - Use staging tables
   - Optimize transformations

## Example

**Context**: Daily ETL from operational database to data warehouse

**Pipeline Design**:

```python
# etl_sales_pipeline.py
import pandas as pd
from datetime import datetime, timedelta
import logging

class SalesETL:
    def __init__(self, source_conn, target_conn):
        self.source = source_conn
        self.target = target_conn
        self.logger = logging.getLogger(__name__)
        
    def extract_incremental(self, last_run_date):
        """Extract new/modified sales records since last run"""
        query = """
            SELECT 
                sale_id,
                sale_date,
                customer_id,
                product_id,
                store_id,
                quantity,
                unit_price,
                discount,
                total_amount,
                updated_at
            FROM sales
            WHERE updated_at > %s
            ORDER BY updated_at
        """
        
        self.logger.info(f"Extracting sales since {last_run_date}")
        df = pd.read_sql(query, self.source, params=[last_run_date])
        self.logger.info(f"Extracted {len(df)} records")
        
        return df
    
    def transform(self, df):
        """Transform and cleanse data"""
        self.logger.info("Starting transformation")
        
        # 1. Data cleansing
        # Remove duplicates
        df = df.drop_duplicates(subset=['sale_id'], keep='last')
        
        # Handle nulls
        df['discount'] = df['discount'].fillna(0)
        
        # Validate data
        df = df[df['quantity'] > 0]  # Remove invalid quantities
        df = df[df['total_amount'] >= 0]  # Remove negative amounts
        
        # 2. Data standardization
        # Convert date formats
        df['sale_date'] = pd.to_datetime(df['sale_date'])
        df['date_key'] = df['sale_date'].dt.strftime('%Y%m%d').astype(int)
        
        # 3. Enrichment - lookup dimension keys
        df = self._lookup_dimension_keys(df)
        
        # 4. Calculate derived fields
        df['tax_amount'] = df['total_amount'] * 0.08  # 8% tax
        df['cost_amount'] = self._lookup_product_cost(df['product_id'])
        df['profit'] = df['total_amount'] - df['cost_amount']
        
        # 5. Select final columns for target
        target_columns = [
            'sale_id', 'date_key', 'customer_key', 'product_key', 
            'store_key', 'quantity', 'unit_price', 'discount',
            'tax_amount', 'total_amount', 'cost_amount'
        ]
        df = df[target_columns]
        
        self.logger.info(f"Transformation complete: {len(df)} records")
        return df
    
    def _lookup_dimension_keys(self, df):
        """Join with dimension tables to get surrogate keys"""
        # Get current dimension keys
        customers = pd.read_sql(
            "SELECT customer_id, customer_key FROM dim_customer WHERE is_current = TRUE",
            self.target
        )
        products = pd.read_sql(
            "SELECT product_id, product_key FROM dim_product WHERE is_current = TRUE",
            self.target
        )
        stores = pd.read_sql(
            "SELECT store_id, store_key FROM dim_store WHERE is_current = TRUE",
            self.target
        )
        
        # Join to get dimension keys
        df = df.merge(customers, on='customer_id', how='left')
        df = df.merge(products, on='product_id', how='left')
        df = df.merge(stores, on='store_id', how='left')
        
        # Handle missing keys (new customers/products/stores)
        missing_customers = df[df['customer_key'].isna()]['customer_id'].unique()
        if len(missing_customers) > 0:
            self.logger.warning(f"Missing customer keys: {missing_customers}")
            # In production: create new dimension records
        
        return df
    
    def load_upsert(self, df):
        """Load data using UPSERT strategy"""
        self.logger.info(f"Loading {len(df)} records")
        
        # Stage data first
        self._load_to_staging(df)
        
        # Merge from staging to target
        merge_query = """
            MERGE INTO fact_sales AS target
            USING staging_sales AS source
            ON target.sale_id = source.sale_id
            WHEN MATCHED THEN
                UPDATE SET
                    date_key = source.date_key,
                    customer_key = source.customer_key,
                    product_key = source.product_key,
                    store_key = source.store_key,
                    quantity = source.quantity,
                    unit_price = source.unit_price,
                    discount = source.discount,
                    tax_amount = source.tax_amount,
                    total_amount = source.total_amount,
                    cost_amount = source.cost_amount
            WHEN NOT MATCHED THEN
                INSERT (sale_id, date_key, customer_key, product_key, store_key,
                        quantity, unit_price, discount, tax_amount, 
                        total_amount, cost_amount)
                VALUES (source.sale_id, source.date_key, source.customer_key,
                        source.product_key, source.store_key, source.quantity,
                        source.unit_price, source.discount, source.tax_amount,
                        source.total_amount, source.cost_amount);
        """
        
        cursor = self.target.cursor()
        cursor.execute(merge_query)
        self.target.commit()
        
        rows_affected = cursor.rowcount
        self.logger.info(f"Loaded {rows_affected} records")
        
        # Clean up staging
        cursor.execute("TRUNCATE TABLE staging_sales")
        self.target.commit()
        
        return rows_affected
    
    def _load_to_staging(self, df):
        """Load data to staging table"""
        df.to_sql('staging_sales', self.target, if_exists='replace', 
                  index=False, method='multi', chunksize=1000)
    
    def get_last_run_date(self):
        """Get last successful run timestamp"""
        query = """
            SELECT MAX(last_run_date) as last_run
            FROM etl_run_log
            WHERE pipeline_name = 'sales_etl'
              AND status = 'SUCCESS'
        """
        result = pd.read_sql(query, self.target)
        
        if result['last_run'].iloc[0] is None:
            # First run - go back 30 days
            return datetime.now() - timedelta(days=30)
        
        return result['last_run'].iloc[0]
    
    def log_run(self, status, records_processed, error_msg=None):
        """Log pipeline execution"""
        log_query = """
            INSERT INTO etl_run_log 
            (pipeline_name, run_date, last_run_date, status, 
             records_processed, error_message)
            VALUES (%s, %s, %s, %s, %s, %s)
        """
        
        cursor = self.target.cursor()
        cursor.execute(log_query, (
            'sales_etl',
            datetime.now(),
            self.get_last_run_date(),
            status,
            records_processed,
            error_msg
        ))
        self.target.commit()
    
    def run(self):
        """Execute full ETL pipeline"""
        try:
            # Get last successful run
            last_run = self.get_last_run_date()
            
            # Extract
            df = self.extract_incremental(last_run)
            
            if len(df) == 0:
                self.logger.info("No new records to process")
                return
            
            # Transform
            df = self.transform(df)
            
            # Load
            rows_loaded = self.load_upsert(df)
            
            # Log success
            self.log_run('SUCCESS', rows_loaded)
            
            self.logger.info("ETL pipeline completed successfully")
            
        except Exception as e:
            self.logger.error(f"ETL pipeline failed: {e}", exc_info=True)
            self.log_run('FAILED', 0, str(e))
            raise

# Orchestration with Airflow
from airflow import DAG
from airflow.operators.python import PythonOperator
from datetime import datetime

def run_sales_etl():
    # Connection setup
    source_conn = get_source_connection()
    target_conn = get_target_connection()
    
    # Run pipeline
    etl = SalesETL(source_conn, target_conn)
    etl.run()

dag = DAG(
    'sales_etl_pipeline',
    schedule_interval='0 2 * * *',  # Run daily at 2 AM
    start_date=datetime(2024, 1, 1),
    catchup=False,
    tags=['etl', 'sales']
)

run_etl = PythonOperator(
    task_id='run_sales_etl',
    python_callable=run_sales_etl,
    dag=dag
)
```

**ETL Run Log Table**:
```sql
CREATE TABLE etl_run_log (
    run_id SERIAL PRIMARY KEY,
    pipeline_name VARCHAR(100) NOT NULL,
    run_date TIMESTAMP NOT NULL,
    last_run_date TIMESTAMP,
    status VARCHAR(20) NOT NULL,
    records_processed INT,
    error_message TEXT,
    duration_seconds INT
);
```

## Best Practices

- ✅ Use incremental loading where possible (not full refresh)
- ✅ Implement idempotent pipelines (can re-run safely)
- ✅ Use staging tables for complex transformations
- ✅ Log all pipeline runs with status and metrics
- ✅ Implement error handling and retries
- ✅ Validate data quality after load
- ✅ Use bulk operations for performance
- ✅ Monitor pipeline execution time trends
- ✅ Implement data quality checks
- ✅ Version control ETL code
- ✅ Document data lineage
- ✅ Use transaction boundaries appropriately
- ❌ Avoid: Transforming data in the database (do in memory/pipeline)
- ❌ Avoid: Loading one record at a time (use batches)
- ❌ Avoid: No error handling (pipelines will fail)
- ❌ Avoid: Overwriting good data with bad data
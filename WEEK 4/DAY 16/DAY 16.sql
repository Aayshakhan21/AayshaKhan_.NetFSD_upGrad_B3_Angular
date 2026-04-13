CREATE DATABASE store_db;
USE store_db;

CREATE TABLE customers
(
customer_id INT PRIMARY KEY IDENTITY(1,1),
first_name VARCHAR(50),
last_name VARCHAR(50)
);

CREATE TABLE stores
(
store_id INT PRIMARY KEY IDENTITY(1,1),
store_name VARCHAR(100)
);

CREATE TABLE products
(
product_id INT PRIMARY KEY IDENTITY(1,1),
product_name VARCHAR(100),
price DECIMAL(10,2)
);

CREATE TABLE orders
(
order_id INT PRIMARY KEY IDENTITY(1,1),
customer_id INT,
store_id INT,
order_date DATE,
order_status INT,
shipped_date DATE,

FOREIGN KEY (customer_id) REFERENCES customers(customer_id),
FOREIGN KEY (store_id) REFERENCES stores(store_id)
);

CREATE TABLE order_items
(
order_item_id INT PRIMARY KEY IDENTITY(1,1),
order_id INT,
product_id INT,
quantity INT,
price DECIMAL(10,2),

FOREIGN KEY (order_id) REFERENCES orders(order_id),
FOREIGN KEY (product_id) REFERENCES products(product_id)
);

-- Insert Data

INSERT INTO customers(first_name,last_name) VALUES
('Aaysha','Khan'),
('Ravi','Sharma'),
('Sita','Patel'),
('John','Smith');

INSERT INTO stores(store_name) VALUES
('Store A'),
('Store B'),
('Store C');

INSERT INTO products(product_name,price) VALUES
('Laptop',50000),
('Phone',20000),
('Tablet',15000),
('Headphones',2000),
('Mouse',800);

INSERT INTO orders(customer_id,store_id,order_date,order_status,shipped_date) VALUES
(1,1,'2024-02-01',1,NULL),
(2,2,'2024-02-03',4,'2024-02-04'),
(3,1,'2024-02-05',2,NULL),
(4,3,'2024-02-06',1,NULL),
(2,1,'2024-02-08',4,'2024-02-09');

INSERT INTO order_items(order_id,product_id,quantity,price) VALUES
(1,1,1,50000),
(1,4,2,2000),
(2,2,1,20000),
(3,3,1,15000),
(4,5,3,800),
(5,2,1,20000);

-- Problem 1

-- Create a stored procedure to generate total sales amount per store.

CREATE PROCEDURE usp_total_sales_per_store
AS
BEGIN

SELECT 
s.store_name,
COALESCE(SUM(oi.quantity * oi.price),0) AS total_sales
FROM stores s
LEFT JOIN orders o
ON s.store_id = o.store_id
LEFT JOIN order_items oi
ON o.order_id = oi.order_id
GROUP BY s.store_name;

END;

EXEC usp_total_sales_per_store;

-- Create a stored procedure to retrieve orders by date range.

CREATE PROCEDURE usp_orders_by_date_range
@start_date DATE,
@end_date DATE
AS
BEGIN

SELECT 
o.order_id,
c.first_name,
c.last_name,
s.store_name,
o.order_date
FROM orders o
JOIN customers c
ON o.customer_id = c.customer_id
JOIN stores s
ON o.store_id = s.store_id
WHERE o.order_date BETWEEN @start_date AND @end_date;

END;

EXEC usp_orders_by_date_range 
'2024-02-01','2024-02-10';

-- Create a scalar function to calculate total price after discount.

CREATE FUNCTION fn_price_after_discount
(
@price DECIMAL(10,2),
@discount_percent INT
)
RETURNS DECIMAL(10,2)
AS
BEGIN

RETURN @price - (@price * @discount_percent / 100);

END;

SELECT 
product_name,
price,
dbo.fn_price_after_discount(price,10) AS discounted_price
FROM products;

-- Create a table-valued function to return top 5 selling products.

CREATE FUNCTION fn_top_5_selling_products()
RETURNS TABLE
AS
RETURN
(
SELECT TOP 5
p.product_name,
SUM(oi.quantity) AS total_sold
FROM products p
JOIN order_items oi
ON p.product_id = oi.product_id
GROUP BY p.product_name
ORDER BY total_sold DESC
);

SELECT * 
FROM dbo.fn_top_5_selling_products();


--------------- PROBLEM 2 ------------------

CREATE TABLE stocks
(
product_id INT PRIMARY KEY,
quantity INT,

FOREIGN KEY (product_id) REFERENCES products(product_id)
);

INSERT INTO stocks(product_id, quantity) VALUES
(1,10),
(2,15),
(3,12),
(4,20),
(5,25);

SELECT * FROM stocks;

CREATE TRIGGER trg_update_stock
ON order_items
AFTER INSERT
AS
BEGIN
    BEGIN TRY
        IF EXISTS
        (
            SELECT 1
            FROM inserted i
            JOIN stocks s
            ON i.product_id = s.product_id
            WHERE s.quantity < i.quantity
        )
        BEGIN
            THROW 50001, 'Insufficient stock available.', 1;
        END

        UPDATE s
        SET s.quantity = s.quantity - i.quantity
        FROM stocks s
        JOIN inserted i
        ON s.product_id = i.product_id;

    END TRY

    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;

-- Reduce the corresponding quantity in stocks table

INSERT INTO order_items(order_id, product_id, quantity, price)
VALUES (1,1,2,50000);

-- Rollback transaction with custom error message

INSERT INTO order_items(order_id, product_id, quantity, price)
VALUES (1,1,50,50000);

SELECT * FROM stocks;

------------------ PROBLEM 3 ------------------

CREATE TRIGGER trg_validate_order_status
ON orders
AFTER UPDATE
AS
BEGIN
    BEGIN TRY

        IF EXISTS
        (
            SELECT 1
            FROM inserted
            WHERE order_status = 4
            AND shipped_date IS NULL
        )
        BEGIN
            THROW 50002, 'Cannot mark order as Completed without shipped_date.',1;
        END

    END TRY

    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;

-- Update triggers on orders

UPDATE orders
SET shipped_date = '2024-02-10',
    order_status = 4
WHERE order_id = 1;

--------------------- Problem 4 -------------------

BEGIN TRY

BEGIN TRANSACTION;

DECLARE @order_id INT;
DECLARE @store_id INT;
DECLARE @revenue DECIMAL(10,2);

CREATE TABLE #temp_revenue
(
store_id INT,
order_id INT,
revenue DECIMAL(10,2)
);

DECLARE order_cursor CURSOR
FOR
SELECT order_id, store_id
FROM orders
WHERE order_status = 4;

OPEN order_cursor;

FETCH NEXT FROM order_cursor INTO @order_id, @store_id;

WHILE @@FETCH_STATUS = 0
BEGIN

    SELECT @revenue = SUM(quantity * price)
    FROM order_items
    WHERE order_id = @order_id;

    INSERT INTO #temp_revenue
    VALUES(@store_id,@order_id,@revenue);

    FETCH NEXT FROM order_cursor INTO @order_id, @store_id;

END

CLOSE order_cursor;
DEALLOCATE order_cursor;

SELECT 
s.store_name,
SUM(t.revenue) AS total_store_revenue
FROM #temp_revenue t
JOIN stores s
ON t.store_id = s.store_id
GROUP BY s.store_name;

COMMIT TRANSACTION;

END TRY

BEGIN CATCH
    ROLLBACK TRANSACTION;
    PRINT ERROR_MESSAGE();
END CATCH;
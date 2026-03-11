

CREATE DATABASE ProductStoreDB;
USE ProductStoreDB;

CREATE TABLE categories
(
categoryId INT PRIMARY KEY IDENTITY(1,1),
categoryName VARCHAR(50)
);

CREATE TABLE brands
(
brandId INT PRIMARY KEY IDENTITY(1,1),
brandName VARCHAR(50)
);

CREATE TABLE products
(
productId INT PRIMARY KEY IDENTITY(1,1),
productName VARCHAR(100),
brandId INT,
categoryId INT,
modelYear INT,
listPrice DECIMAL(10,2),
FOREIGN KEY (brandId) REFERENCES brands(brandId),
FOREIGN KEY (categoryId) REFERENCES categories(categoryId)
);

INSERT INTO categories (categoryName) VALUES
('Category A'),
('Category B'),
('Category C');

INSERT INTO brands (brandName) VALUES
('Brand A'),
('Brand B'),
('Brand C');

INSERT INTO products (productName, brandId, categoryId, modelYear, listPrice) VALUES
('Product A',1,1,2022,500),
('Product B',2,1,2023,700),
('Product C',3,2,2022,800),
('Product D',1,2,2023,600),
('Product E',2,3,2024,900),
('Product F',3,3,2023,750);

SELECT * FROM categories;
SELECT * FROM brands;
SELECT * FROM products;

----- PROBLEM 1ST -----

SELECT 
    CONCAT(p.productName,' (',p.modelYear,')') AS ProductInfo,
    c.categoryName,
    p.listPrice,
    ROUND(p.listPrice - (
        SELECT AVG(listPrice)
        FROM products p2
        WHERE p2.categoryId = p.categoryId
    ),2) AS PriceDifference
FROM products p
JOIN categories c 
ON p.categoryId = c.categoryId
WHERE p.listPrice > (
        SELECT AVG(listPrice)
        FROM products p2
        WHERE p2.categoryId = p.categoryId
);


-----	  PROBLEM 2   ----

CREATE DATABASE StoreDb;
USE StoreDb;

CREATE TABLE Customers (
  customerId INT PRIMARY KEY IDENTITY(1,1),
  firstName VARCHAR(50),
  lastName VARCHAR(50)
);

CREATE TABLE Orders(
  orderId INT PRIMARY KEY IDENTITY(1,1),
  customerId INT,
  orderDate DATE,
  orderStatus INT,
  FOREIGN KEY (customerId) REFERENCES Customers(customerId)
);

INSERT INTO Customers (firstName, lastName) VALUES
('Aaysha','Khan'),
('Ravi','Sharma'),
('Sita','Patel'),
('John','Smith');

INSERT INTO Orders (customerId, orderDate, orderStatus) VALUES
(1,'2024-02-01',1),
(2,'2024-02-03',4),
(3,'2024-02-05',2),
(4,'2024-02-06',1),
(2,'2024-02-08',4);
	
SELECT * FROM Customers;
SELECT * FROM Orders;


SELECT 
    CONCAT(c.firstName,' ',c.lastName) AS FullName,
    (SELECT SUM(o.orderValue)
     FROM orders o
     WHERE o.customerId = c.customerId) AS TotalOrderValue,
    CASE
        WHEN (SELECT SUM(o.orderValue)
              FROM orders o
              WHERE o.customerId = c.customerId) > 10000
             THEN 'Premium'
        WHEN (SELECT SUM(o.orderValue)
              FROM orders o
              WHERE o.customerId = c.customerId) BETWEEN 5000 AND 10000
             THEN 'Regular'
        WHEN (SELECT SUM(o.orderValue)
              FROM orders o
              WHERE o.customerId = c.customerId) < 5000
             THEN 'Basic'
    END AS CustomerType
FROM customers c
JOIN orders o
ON c.customerId = o.customerId
GROUP BY c.customerId, c.firstName, c.lastName

UNION

SELECT 
    CONCAT(firstName,' ',lastName) AS FullName,
    0 AS TotalOrderValue,
    'No Orders' AS CustomerType
FROM customers
WHERE customerId NOT IN
(
SELECT customerId FROM orders
);

-------------------- PROBLEM 3 --------------------------

USE ProductStoreDB;

CREATE TABLE stores(
storeId INT PRIMARY KEY IDENTITY(1,1),
storeName VARCHAR(50)
);


CREATE TABLE orders(
orderId INT PRIMARY KEY IDENTITY(1,1),
storeId INT,
orderDate DATE,
FOREIGN KEY (storeId) REFERENCES stores(storeId)
);


CREATE TABLE order_items(
orderItemId INT PRIMARY KEY IDENTITY(1,1),
orderId INT,
productId INT,
quantity INT,
list_price DECIMAL(10,2),
discount DECIMAL(10,2),
FOREIGN KEY (orderId) REFERENCES orders(orderId),
FOREIGN KEY (productId) REFERENCES products(productId)
);

CREATE TABLE stocks(
storeId INT,
productId INT,
quantity INT,
PRIMARY KEY(storeId, productId),
FOREIGN KEY (storeId) REFERENCES stores(storeId),
FOREIGN KEY (productId) REFERENCES products(productId)
);


INSERT INTO stores(storeName) VALUES
('Store A'),
('Store B');

INSERT INTO orders(storeId,orderDate) VALUES
(1,'2024-02-01'),
(1,'2024-02-02'),
(2,'2024-02-03');

INSERT INTO order_items(orderId,productId,quantity,list_price,discount) VALUES
(1,1,5,500,50),
(1,2,2,700,30),
(2,3,3,800,20),
(3,4,4,600,10);

INSERT INTO stocks(storeId,productId,quantity) VALUES
(1,1,0),
(1,2,5),
(1,3,0),
(2,4,10);

SELECT * FROM stores;
SELECT * FROM orders;
SELECT * FROM order_items;
SELECT * FROM stocks;

-- Identify products sold in each store using nested queries.

SELECT 
    s.storeName,
    p.productName,
    sales.total_quantity_sold,
    sales.total_revenue
FROM
(
    SELECT 
        o.storeId,
        oi.productId,
        SUM(oi.quantity) AS total_quantity_sold,
        SUM((oi.quantity * oi.list_price) - oi.discount) AS total_revenue
    FROM orders o
    JOIN order_items oi
        ON o.orderId = oi.orderId
    GROUP BY o.storeId, oi.productId
) AS sales
JOIN stores s 
    ON sales.storeId = s.storeId
JOIN products p 
    ON sales.productId = p.productId;

-- Compare sold products with current stock using INTERSECT and EXCEPT operators.

SELECT productId FROM order_items
INTERSECT
SELECT productId FROM stocks WHERE quantity = 0;

SELECT productId FROM order_items
EXCEPT
SELECT productId FROM stocks WHERE quantity = 0;

-- Display store_name, product_name, total quantity sold.

SELECT 
    s.storeName,
    p.productName,
    SUM(oi.quantity) AS total_quantity_sold
FROM stores s
JOIN orders o 
    ON s.storeId = o.storeId
JOIN order_items oi 
    ON o.orderId = oi.orderId
JOIN products p 
    ON oi.productId = p.productId
WHERE oi.productId IN
(
    SELECT productId FROM order_items
    INTERSECT
    SELECT productId FROM stocks WHERE quantity = 0
)
GROUP BY s.storeName, p.productName;

-- Calculate total revenue per product (quantity × list_price – discount).

SELECT 
    s.storeName,
    p.productName,
    SUM(oi.quantity) AS total_quantity_sold,
    SUM((oi.quantity * oi.list_price) - oi.discount) AS total_revenue
FROM stores s
JOIN orders o
    ON s.storeId = o.storeId
JOIN order_items oi
    ON o.orderId = oi.orderId
JOIN products p
    ON oi.productId = p.productId
GROUP BY s.storeName, p.productName;

-- Update stock quantity to 0 for discontinued products (simulation).

UPDATE stocks
SET quantity = 0
WHERE productId IN
(
SELECT productId FROM order_items
EXCEPT
SELECT productId FROM stocks WHERE quantity = 0
);


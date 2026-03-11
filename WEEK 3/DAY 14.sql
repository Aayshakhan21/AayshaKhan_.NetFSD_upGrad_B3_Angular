

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


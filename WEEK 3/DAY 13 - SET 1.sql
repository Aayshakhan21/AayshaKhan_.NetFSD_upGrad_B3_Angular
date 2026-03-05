
-----	  PROBLEM 1   ----

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

SELECT c.firstName, c.lastName, o.orderId, o.orderDate, o.orderStatus
FROM Customers AS c INNER JOIN Orders AS o
ON c.customerId = o.orderId
WHERE orderStatus IN (1,4)
ORDER BY orderDate DESC;

------ PROBLEM 2 -----


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
('Mountain Bikes'),
('Road Bikes'),
('Electric Bikes'),
('Accessories'),
('Kids Bikes');

INSERT INTO brands (brandName) VALUES
('Trek'),
('Giant'),
('Specialized'),
('Cannondale'),
('Scott');

INSERT INTO products (productName, brandId, categoryId, modelYear, listPrice) VALUES
('Trek Marlin 7',1,1,2023,850.00),
('Giant Talon 3',2,1,2022,600.00),
('Specialized Allez',3,2,2023,1200.00),
('Cannondale Quick 4',4,2,2022,700.00),
('Scott Spark 960',5,1,2024,1500.00),
('Trek Powerfly',1,3,2024,3200.00),
('Giant Explore E+',2,3,2023,2800.00),
('Specialized Turbo Vado',3,3,2024,3500.00),
('Bike Helmet',4,4,2023,120.00),
('Kids Trek Bike',1,5,2022,450.00);

SELECT * FROM categories;
SELECT * FROM brands;
SELECT * FROM products;

SELECT p.productName,p.modelYear, p.listPrice, b.brandName, c.categoryName
From products p INNER JOIN brands b ON p.brandId = b.brandId 
INNER JOIN  categories c ON P.categoryId = c.categoryId
WHERE listPrice > 500
ORDER BY listPrice;



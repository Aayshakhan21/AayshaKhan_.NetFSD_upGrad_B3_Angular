

CREATE DATABASE SalesDB;
USE SalesDB;

CREATE TABLE stores
(
store_id INT PRIMARY KEY IDENTITY(1,1),
store_name VARCHAR(50)
);

CREATE TABLE orders_
(
order_id INT PRIMARY KEY IDENTITY(1,1),
store_id INT,
order_status INT,
FOREIGN KEY (store_id) REFERENCES stores(store_id)
);

CREATE TABLE order_items
(
item_id INT PRIMARY KEY IDENTITY(1,1),
order_id INT,
quantity INT,
list_price DECIMAL(10,2),
discount DECIMAL(4,2),
FOREIGN KEY (order_id) REFERENCES orders_(order_id)
);


INSERT INTO orders_ (store_id, order_status) VALUES
(1,4),
(2,4),
(3,1),
(1,4),
(2,2);

INSERT INTO order_items (order_id, quantity, list_price, discount) VALUES
(1,2,1000,0.10),
(1,1,500,0.05),
(2,3,800,0.15),
(3,2,700,0.05),
(4,4,600,0.10);

INSERT INTO stores (store_name) VALUES
(' Store A'),
(' Store B'),
(' Store C');


SELECT * FROM stores;
SELECT * FROM orders_;
SELECT * FROM order_items;

SELECT s.store_name, SUM(oi.quantity * oi.list_price * (1 - oi.discount)) AS total_sales
FROM stores s INNER JOIN orders_ o
ON s.store_id = o.store_id
INNER JOIN order_items oi
ON o.order_id = oi.order_id
WHERE o.order_status = 4
GROUP BY s.store_name
ORDER BY total_sales DESC;


----- problem 2 ---

CREATE DATABASE StockDb;
USE StockDb;


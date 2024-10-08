-- DATABASE DESIGN:
-- Entity Relationship Diagram

CREATE DATABASE [Ecommerce Application];
USE [Ecommerce Application];

-- 1. CUSTOMERS TABLE:
CREATE TABLE customers (
customer_id INT PRIMARY KEY IDENTITY(1,1),
name NVARCHAR(170),
email NVARCHAR(170) UNIQUE,
password NVARCHAR(170)
);

-- 2. PRODUCTS TABLE:
CREATE TABLE products (
product_id INT PRIMARY KEY IDENTITY,
name NVARCHAR(200),
price DECIMAL(10, 2),
description TEXT,
stockQuantity INT
);

-- 3. CART TABLE:
CREATE TABLE cart (
cart_id INT PRIMARY KEY IDENTITY,
customer_id INT,
product_id INT,
quantity INT,
FOREIGN KEY (customer_id) REFERENCES customers(customer_id) ON DELETE CASCADE ON UPDATE CASCADE,
FOREIGN KEY (product_id) REFERENCES products(product_id) ON DELETE CASCADE ON UPDATE CASCADE
);

-- 4. ORDERS TABLE:
CREATE TABLE orders (
order_id INT PRIMARY KEY IDENTITY,
customer_id INT,
order_date DATE,
total_price DECIMAL(10, 2),
shipping_address NVARCHAR(255),
FOREIGN KEY (customer_id) REFERENCES customers(customer_id) ON DELETE CASCADE ON UPDATE CASCADE
);

-- 5. ORDER_ITEMS TABLE:
CREATE TABLE order_items (
order_item_id INT PRIMARY KEY IDENTITY(1,1),
order_id INT,
product_id INT,
quantity INT,
FOREIGN KEY (order_id) REFERENCES orders(order_id) ON DELETE CASCADE ON UPDATE CASCADE,
FOREIGN KEY (product_id) REFERENCES products(product_id) ON DELETE CASCADE ON UPDATE CASCADE
);


INSERT INTO customers (name, email, password)
VALUES ('Ravi', 'ravi@example.com', 'Ravikumar@123'),
('Priya', 'priya@example.com', 'Priya@@456'),
('Amit', 'amit@example.com', 'Amit@@789'),
('ram Kumar', 'ram.kumar@example.com', 'Ramkumar@123'),
('Vikram', 'vikram@example.com', 'Vikram@456'),
('thiyanesh','thiyanesh@gmail.com','Thiyanesh@123');

INSERT INTO products (name, price, description, stockQuantity)
VALUES ('Smartphone', 19999.99, 'Latest Android smartphone', 50),
('Laptop', 49999.99, 'High-performance laptop', 30),
('Headphones', 2999.99, 'Noise-cancelling over-ear headphones', 100),
('Smartwatch', 9999.99, 'Wearable smartwatch with fitness tracker', 75),
('Tablet', 15999.99, '10-inch Android tablet', 40);

INSERT INTO cart (customer_id, product_id, quantity)
VALUES (1, 1, 1),  
(2, 3, 2),  
(3, 2, 1),  
(4, 4, 1), 
(5, 5, 1);  

INSERT INTO orders (customer_id, order_date, total_price, shipping_address)
VALUES 
(1, '2024-09-25', 19999.99, '123 MG Road, Delhi'),
(2, '2024-09-26', 5999.98, '45 Gandhi Nagar, Mumbai'),
(3, '2024-09-27', 49999.99, '78 Nehru Street, Ahmedabad'),
(4, '2024-09-28', 9999.99, '67 Rajpath, Bangalore'),
(5, '2024-09-29', 15999.99, '89 Marine Drive, Chennai');

INSERT INTO order_items (order_id, product_id, quantity)
VALUES(1, 1, 1),  
(2, 3, 2),  
(3, 2, 1),  
(4, 4, 1),  
(5, 5, 1);  

select * from customers;
select * from products;
select * from cart;
select * from orders;
select * from order_items;




-- ==============================================
-- ECommerce Database Schema for MySQL
-- Run this script in MySQL Workbench to create
-- the database and tables
-- ==============================================

-- Create the database
CREATE DATABASE IF NOT EXISTS ecommerce_db;
USE ecommerce_db;

-- ==============================================
-- Table: categories
-- ==============================================
DROP TABLE IF EXISTS order_items;
DROP TABLE IF EXISTS cart_items;
DROP TABLE IF EXISTS orders;
DROP TABLE IF EXISTS products;
DROP TABLE IF EXISTS categories;
DROP TABLE IF EXISTS users;

CREATE TABLE categories (
    id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(100) NOT NULL,
    description TEXT,
    is_active TINYINT(1) DEFAULT 1,
    created_at DATETIME DEFAULT CURRENT_TIMESTAMP,
    updated_at DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ==============================================
-- Table: users
-- ==============================================
CREATE TABLE users (
    id INT AUTO_INCREMENT PRIMARY KEY,
    first_name VARCHAR(100) NOT NULL,
    last_name VARCHAR(100) NOT NULL,
    email VARCHAR(255) NOT NULL UNIQUE,
    password VARCHAR(255) NOT NULL,
    role ENUM('Customer', 'Admin') DEFAULT 'Customer',
    is_active TINYINT(1) DEFAULT 1,
    created_at DATETIME DEFAULT CURRENT_TIMESTAMP,
    updated_at DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    INDEX idx_email (email),
    INDEX idx_role (role)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ==============================================
-- Table: products
-- ==============================================
CREATE TABLE products (
    id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(255) NOT NULL,
    description TEXT,
    price DECIMAL(10, 2) NOT NULL,
    image VARCHAR(500),
    type VARCHAR(100),
    stock_quantity INT DEFAULT 0,
    is_favorite TINYINT(1) DEFAULT 0,
    is_active TINYINT(1) DEFAULT 1,
    category_id INT,
    created_at DATETIME DEFAULT CURRENT_TIMESTAMP,
    updated_at DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    FOREIGN KEY (category_id) REFERENCES categories(id) ON DELETE SET NULL,
    INDEX idx_type (type),
    INDEX idx_is_active (is_active)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ==============================================
-- Table: orders
-- ==============================================
CREATE TABLE orders (
    id VARCHAR(50) PRIMARY KEY,
    user_id INT,
    customer_name VARCHAR(200) NOT NULL,
    customer_email VARCHAR(255) NOT NULL,
    shipping_address TEXT NOT NULL,
    city VARCHAR(100) NOT NULL,
    state VARCHAR(100) NOT NULL,
    zip_code VARCHAR(20) NOT NULL,
    subtotal DECIMAL(10, 2) NOT NULL,
    tax DECIMAL(10, 2) NOT NULL,
    total DECIMAL(10, 2) NOT NULL,
    status ENUM('Pending', 'Processing', 'Shipped', 'Delivered', 'Cancelled') DEFAULT 'Pending',
    payment_method VARCHAR(50),
    payment_status ENUM('Pending', 'Paid', 'Failed', 'Refunded') DEFAULT 'Pending',
    notes TEXT,
    order_date DATETIME DEFAULT CURRENT_TIMESTAMP,
    updated_at DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    FOREIGN KEY (user_id) REFERENCES users(id) ON DELETE SET NULL,
    INDEX idx_user_id (user_id),
    INDEX idx_status (status),
    INDEX idx_order_date (order_date)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ==============================================
-- Table: order_items
-- ==============================================
CREATE TABLE order_items (
    id INT AUTO_INCREMENT PRIMARY KEY,
    order_id VARCHAR(50) NOT NULL,
    product_id INT,
    product_name VARCHAR(255) NOT NULL,
    product_image VARCHAR(500),
    price DECIMAL(10, 2) NOT NULL,
    quantity INT NOT NULL,
    total_price DECIMAL(10, 2) NOT NULL,
    type VARCHAR(100),
    created_at DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (order_id) REFERENCES orders(id) ON DELETE CASCADE,
    FOREIGN KEY (product_id) REFERENCES products(id) ON DELETE SET NULL,
    INDEX idx_order_id (order_id)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ==============================================
-- Table: cart_items (for persistent cart storage)
-- ==============================================
CREATE TABLE cart_items (
    id INT AUTO_INCREMENT PRIMARY KEY,
    session_id VARCHAR(255) NOT NULL,
    user_id INT,
    product_id INT NOT NULL,
    quantity INT NOT NULL DEFAULT 1,
    created_at DATETIME DEFAULT CURRENT_TIMESTAMP,
    updated_at DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    FOREIGN KEY (user_id) REFERENCES users(id) ON DELETE CASCADE,
    FOREIGN KEY (product_id) REFERENCES products(id) ON DELETE CASCADE,
    INDEX idx_session_id (session_id),
    INDEX idx_user_id (user_id)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ==============================================
-- Insert Sample Data
-- ==============================================

-- Insert default admin user (password: Admin123!)
INSERT INTO users (first_name, last_name, email, password, role) VALUES
('Admin', 'User', 'admin@ecommerce.com', 'Admin123!', 'Admin');

-- Insert sample customers
INSERT INTO users (first_name, last_name, email, password, role) VALUES
('John', 'Doe', 'john.doe@email.com', 'Customer123!', 'Customer'),
('Jane', 'Smith', 'jane.smith@email.com', 'Customer123!', 'Customer');

-- Insert sample categories
INSERT INTO categories (name, description) VALUES
('Electronics', 'Electronic devices and accessories'),
('Clothing', 'Apparel and fashion items'),
('Shoes', 'Footwear for all occasions'),
('Accessories', 'Bags, watches, and more');

-- Insert sample products
INSERT INTO products (name, description, price, image, type, stock_quantity, is_favorite, category_id) VALUES
('Wireless Headphones', 'Premium noise-cancelling wireless headphones', 199.99, '/Content/images/products/headphones.jpg', 'Electronics', 50, 1, 1),
('Smart Watch', 'Feature-rich smartwatch with health tracking', 299.99, '/Content/images/products/smartwatch.jpg', 'Electronics', 30, 1, 1),
('Laptop Bag', 'Professional laptop bag with multiple compartments', 79.99, '/Content/images/products/laptopbag.jpg', 'Accessories', 100, 0, 4),
('Running Shoes', 'Comfortable running shoes for athletes', 129.99, '/Content/images/products/runningshoes.jpg', 'Shoes', 75, 1, 3),
('Classic T-Shirt', 'Premium cotton classic fit t-shirt', 29.99, '/Content/images/products/tshirt.jpg', 'Clothing', 200, 0, 2),
('Denim Jeans', 'Slim fit denim jeans', 59.99, '/Content/images/products/jeans.jpg', 'Clothing', 150, 0, 2),
('Leather Wallet', 'Genuine leather bi-fold wallet', 49.99, '/Content/images/products/wallet.jpg', 'Accessories', 80, 1, 4),
('Bluetooth Speaker', 'Portable waterproof bluetooth speaker', 89.99, '/Content/images/products/speaker.jpg', 'Electronics', 60, 0, 1);

-- Insert a sample order
INSERT INTO orders (id, user_id, customer_name, customer_email, shipping_address, city, state, zip_code, subtotal, tax, total, status, payment_method, payment_status) VALUES
('ORD-20231201-000001', 2, 'John Doe', 'john.doe@email.com', '123 Main Street', 'New York', 'NY', '10001', 229.98, 18.40, 248.38, 'Delivered', 'Credit Card', 'Paid');

-- Insert sample order items
INSERT INTO order_items (order_id, product_id, product_name, product_image, price, quantity, total_price, type) VALUES
('ORD-20231201-000001', 1, 'Wireless Headphones', '/Content/images/products/headphones.jpg', 199.99, 1, 199.99, 'Electronics'),
('ORD-20231201-000001', 5, 'Classic T-Shirt', '/Content/images/products/tshirt.jpg', 29.99, 1, 29.99, 'Clothing');

-- ==============================================
-- Verification Queries (optional - for testing)
-- ==============================================
-- SELECT * FROM users;
-- SELECT * FROM categories;
-- SELECT * FROM products;
-- SELECT * FROM orders;
-- SELECT * FROM order_items;

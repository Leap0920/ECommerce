-- ==============================================
-- Sample Users for FreshMarket E-commerce
-- ==============================================
-- 
-- IMPORTANT: Since BCrypt hashes are complex to generate manually,
-- the EASIEST approach is to:
--   1. Clear existing users
--   2. Register new accounts through the website
-- 
-- However, if you want sample data immediately, these users
-- use PLAIN TEXT passwords that work with the fallback login.
-- ==============================================

USE ecommerce_db;

-- Clear existing sample users (be careful with foreign key constraints)
SET FOREIGN_KEY_CHECKS = 0;
DELETE FROM cart_items WHERE user_id IN (SELECT id FROM users);
DELETE FROM order_items WHERE order_id IN (SELECT id FROM orders WHERE user_id IN (SELECT id FROM users));  
DELETE FROM orders WHERE user_id IN (SELECT id FROM users);
DELETE FROM users;
SET FOREIGN_KEY_CHECKS = 1;

-- ==============================================
-- Insert Sample Users 
-- These use plain text passwords for immediate testing
-- The login system has fallback support for plain text
-- ==============================================

-- ADMIN ACCOUNT
-- Email: admin@freshmarket.com
-- Password: admin123
INSERT INTO users (first_name, last_name, email, password, role, is_active, created_at) VALUES
('Admin', 'User', 'admin@freshmarket.com', 'admin123', 'Admin', 1, NOW());

-- CUSTOMER ACCOUNT
-- Email: customer@freshmarket.com  
-- Password: customer123
INSERT INTO users (first_name, last_name, email, password, role, is_active, created_at) VALUES
('Juan', 'Dela Cruz', 'customer@freshmarket.com', 'customer123', 'Customer', 1, NOW());

-- TEST ACCOUNT (for quick testing)
-- Email: test@test.com
-- Password: test123
INSERT INTO users (first_name, last_name, email, password, role, is_active, created_at) VALUES
('Test', 'User', 'test@test.com', 'test123', 'Customer', 1, NOW());

-- ==============================================
-- LOGIN CREDENTIALS
-- ==============================================
-- 
-- ADMIN ACCESS:
--   Email: admin@freshmarket.com
--   Password: admin123
--
-- CUSTOMER ACCESS:
--   Email: customer@freshmarket.com
--   Password: customer123
--   
--   Email: test@test.com  
--   Password: test123
--
-- NOTE: For NEW accounts registered through the website,
-- passwords will be properly BCrypt hashed automatically.
-- 
-- ==============================================

-- Verify users were created
SELECT id, first_name, last_name, email, role, is_active FROM users;

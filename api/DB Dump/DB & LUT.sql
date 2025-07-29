-- Create the database
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'db')
    CREATE DATABASE db;
GO

-- Use the database
USE db;
GO

-- Create Roles table
CREATE TABLE Roles (
    id INT IDENTITY(1,1) PRIMARY KEY,
    name VARCHAR(50) UNIQUE NOT NULL
);

-- Create Categories table
CREATE TABLE Categories (
    id INT IDENTITY(1,1) PRIMARY KEY,
    name VARCHAR(100) UNIQUE NOT NULL
);

-- Create OrderStatus table
CREATE TABLE OrderStatus (
    id INT IDENTITY(1,1) PRIMARY KEY,
    name VARCHAR(50) UNIQUE NOT NULL
);

-- Insert Roles
INSERT INTO Roles (name) VALUES 
('Customer'),
('Employee'),
('Manager');

-- Insert Categories
INSERT INTO Categories (name) VALUES 
('Fruits & Vegetables'),
('Eggs, Meat & Fish'),
('Snacks & Branded Foods'),
('Baby Care'),
('Bakery, Cakes & Diary'),
('Beverages');

-- Insert Order Statuses
INSERT INTO OrderStatus (name) VALUES 
('ordered'),
('accepted'),
('packed');

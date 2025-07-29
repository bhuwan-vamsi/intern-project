-- Use the database
USE db;
GO

-- Drop tables if they already exist (optional cleanup order to avoid FK conflicts)
IF OBJECT_ID('OrderItems', 'U') IS NOT NULL DROP TABLE OrderItems;
IF OBJECT_ID('Orders', 'U') IS NOT NULL DROP TABLE Orders;
IF OBJECT_ID('Users', 'U') IS NOT NULL DROP TABLE Users;
GO

-- Create Users table
CREATE TABLE Users (
    id INT IDENTITY(1,1) PRIMARY KEY,
    name VARCHAR(255) NOT NULL,
    email VARCHAR(255) UNIQUE NOT NULL,
    phone VARCHAR(20) UNIQUE NOT NULL,
    password VARCHAR(255) NOT NULL,
    address TEXT,
    role_id INT NOT NULL,
    FOREIGN KEY (role_id) REFERENCES Roles(id)
);
GO

-- Create Orders table
CREATE TABLE Orders (
    id INT IDENTITY(1,1) PRIMARY KEY,
    client_id INT NOT NULL,
    employee_id INT NULL,
    amount DECIMAL(10,2) NOT NULL,
    status_id INT NOT NULL,
    created_at DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (client_id) REFERENCES Users(id),
    FOREIGN KEY (employee_id) REFERENCES Users(id),
    FOREIGN KEY (status_id) REFERENCES OrderStatus(id)
);
GO

-- Create OrderItems table
CREATE TABLE OrderItems (
    id INT IDENTITY(1,1) PRIMARY KEY,
    product_id INT NOT NULL,
    order_id INT NOT NULL,
    quantity INT NOT NULL,
    unit_price DECIMAL(10,2) NOT NULL,
    FOREIGN KEY (product_id) REFERENCES Products(id),
    FOREIGN KEY (order_id) REFERENCES Orders(id)
);
GO

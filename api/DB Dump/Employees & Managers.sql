-- Use the database
USE db;
GO

-- Insert Employees
INSERT INTO Users (name, email, phone, password, address, role_id)
VALUES 
('Alice Sharma', 'alice@gmail.com', '9876543210', '123456', NULL, 2),
('Ravi Kumar', 'ravi@gmail.com', '9876543211', '123456', NULL, 2),
('Sara Iyer', 'sara@gmail.com', '9876543212', '123456', NULL, 2);

-- Insert Manager
INSERT INTO Users (name, email, phone, password, address, role_id)
VALUES 
('Meera Singh', 'meera@gmail.com', '9876543213', '654321', NULL, 3);
GO

SELECT * FROM Users
-- Use the database
USE db;
GO

-- Drop Products table if it exists
IF OBJECT_ID('Products', 'U') IS NOT NULL
    DROP TABLE Products;
GO

-- Create Products table
CREATE TABLE Products (
    id INT IDENTITY(1,1) PRIMARY KEY,
    name VARCHAR(255) NOT NULL,
    price DECIMAL(10,2) NOT NULL,
    units VARCHAR(50),
    quantity INT DEFAULT 100,
    threshold INT DEFAULT 20,
    image_url VARCHAR(500),
    category_id INT,
    is_active BIT DEFAULT 1,
    FOREIGN KEY (category_id) REFERENCES Categories(id)
);
GO

-- Insert data into Products from items table
  INSERT INTO Products (name, price, units, quantity, threshold, ImageUrl,IsActive, CategoryId)
SELECT 
    i.name,
    i.price,
    i.units,
    100,              -- Default quantity
    20,               -- Default threshold
    i.image_url,
	1,
    c.id              -- Foreign key from Categories
FROM items i
JOIN Categories c ON i.category = c.name;
GO

-- Drop items table after import
IF OBJECT_ID('items', 'U') IS NOT NULL
    DROP TABLE items;
GO
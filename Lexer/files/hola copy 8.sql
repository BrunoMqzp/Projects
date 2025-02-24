CREATE TABLE Categories (
    id INT AUTO_INCREMENT PRIMARY KEY,
    category_name VARCHAR(255)
);

CREATE TABLE Products (
    id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(255),
    category_id INT,
    price DECIMAL(10, 2),
    stock INT,
    FOREIGN KEY (category_id) REFERENCES Categories(id)
);

CREATE TABLE Discounts (
    id INT AUTO_INCREMENT PRIMARY KEY,
    product_id INT,
    discount_percentage DECIMAL(5, 2),
    start_date DATE,
    end_date DATE,
    FOREIGN KEY (product_id) REFERENCES Products(id)
);

INSERT INTO Categories (category_name) VALUES
("Electronics"),
("Clothing"),
("Home Appliances"),
("Toys"),
("Books");

INSERT INTO Products (name, category_id, price, stock) VALUES
("Laptop", 1, 999.99, 50),
("T-shirt", 2, 19.99, 150),
("Refrigerator", 3, 499.99, 20),
("Action Figure", 4, 29.99, 100),
("Novel", 5, 9.99, 200);

INSERT INTO Discounts (product_id, discount_percentage, start_date, end_date) VALUES
(1, 10.00, "2024-11-01", "2024-12-31"),
(2, 5.00, "2024-11-01", "2024-11-30"),
(3, 15.00, "2024-11-01", "2024-11-15"),
(4, 20.00, "2024-11-01", "2024-11-30");

SELECT p.name, p.price, d.discount_percentage
FROM Products p
JOIN Discounts d ON p.id = d.product_id
WHERE d.start_date <= CURDATE() AND d.end_date >= CURDATE();

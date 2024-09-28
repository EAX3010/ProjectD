-- Insert sample products
INSERT INTO Products (Name, Price, Description, Image, StockQuantity, Featured, CategoryId)
VALUES 
('Smartphone', 599.99, 'Latest model smartphone with advanced features.', 'smartphone.jpg', 100, 1, 1),
('Laptop', 999.99, 'High-performance laptop for work and gaming.', 'laptop.jpg', 50, 1, 1),
('T-Shirt', 19.99, 'Comfortable cotton t-shirt.', 'tshirt.jpg', 200, 0, 2),
('Jeans', 49.99, 'Classic blue jeans.', 'jeans.jpg', 150, 0, 2),
('Programming Basics', 39.99, 'Introductory book to programming.', 'programming_book.jpg', 50, 1, 3),
('Garden Tools Set', 79.99, 'Complete set of essential garden tools.', 'garden_tools.jpg', 30, 0, 4),
('Tennis Racket', 89.99, 'Professional-grade tennis racket.', 'tennis_racket.jpg', 40, 1, 5);

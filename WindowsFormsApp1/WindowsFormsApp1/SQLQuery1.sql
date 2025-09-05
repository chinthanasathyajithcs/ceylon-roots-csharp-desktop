CREATE TABLE products
(
    id INT PRIMARY KEY IDENTITY(1,1),
    productid VARCHAR(MAX) NULL,
    productname VARCHAR(MAX) NULL,
    category VARCHAR(MAX) NULL,
    stock INT NULL,
    price FLOAT,
    status VARCHAR(MAX) NULL,
    image VARCHAR (MAX) NULL,
    date_import DATE NULL,
    date_update DATE NULL


)

SELECT * FROM products
DELETE FROM products;

CREATE TABLE categories
(
    id INT PRIMARY KEY IDENTITY(1,1),
    category VARCHAR(MAX) NULL,
    status VARCHAR(MAX) NULL,
    date_insert DATE NULL,

)

SELECT * FROM categories
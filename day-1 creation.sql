create database EcommerceDB

use ecommercedb


--1.customers table
create table customers (
    customer_id int identity(1,1) primary key,
    name varchar(30) not null,
    email varchar(40) unique not null,
    password varchar(15) not null
);

--2.products table
create table products (
    product_id int identity(1,1) primary key,
    name varchar(70) not null,
    price decimal(10,2) not null,
    description varchar(255),
    stock_quantity int not null
);

--3.cart table
create table cart (
    cart_id int identity(1,1) primary key,
    customer_id int not null,
    product_id int not null,
    quantity int not null,
    foreign key (customer_id) references customers(customer_id),
    foreign key (product_id) references products(product_id)
);

--4.orders table
create table orders (
    order_id int identity(1,1) primary key,
    customer_id int not null,
    order_date datetime default getdate(),
    total_price decimal(10,2) not null,
    shipping_address nvarchar(255) not null,
    foreign key (customer_id) references customers(customer_id)
);

--5.order_items table(stores order details)
create table order_items (
    order_item_id int identity(1,1) primary key,
    order_id int not null,
    product_id int not null,
    quantity int not null,
    foreign key (order_id) references orders(order_id),
    foreign key (product_id) references products(product_id)
);

--inserting datas in customer table
insert into customers (name, email, password) 
values ('jonsnow', 'jonsnow@example.com', 'abcxyz');
insert into customers (name, email, password) 
values ('lewis', 'lewsi@example.com', 'a1b2c3');

select * from customers
--delete from customers where customer_id = 1;

-- Insert sample products
insert into products (name, price, description, stock_quantity) 
values ('laptop',75000,'dell inspiron 15',10),
       ('smartphone',25000,'samsung galaxy s21',15);

-- Check inserted data
select * from customers;
select * from products;
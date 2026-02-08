
create table [Role] (
IdRole int primary key identity(1,1),
RoleName nvarchar(50) not null 
)
create table [Country](
IdCountry int primary key identity(1,1),
[Name] nvarchar(70) not null,
)

create table ProductCategory(
IdCategory int primary key identity(1,1),
[Name] nvarchar(50) not null
)


create table [Product] (
IdProduct int primary key identity(1,1),
[Name] nvarchar(100) not null,
ProductDescription nvarchar(4000) not null,
Price decimal(10,2) not null,
Quantity int not null,
ImageUrl nvarchar(max) null,
CategoryId int null,
foreign key (CategoryId) references [ProductCategory] (IdCategory)
)

create table CountryProduct (
IdCountryProduct int primary key identity(1,1),
CountryId int not null,
ProductId int not null,
foreign key (CountryId) references [Country] (IdCountry),
foreign key (ProductId) references [Product] (IdProduct)
)


create table [User](
IdUser int primary key identity(1,1),
Username nvarchar (100) not null,
[Name] nvarchar(50) not null,
LastName nvarchar(50) not null,
Email nvarchar(100) unique not null,
PwdHash nvarchar(256) not null,
PwdSalt nvarchar(256) not null,
Phone nvarchar(25),
RoleId int not null,
foreign key (RoleId) references [Role] (IdRole),
)

create table [Order] (
IdOrder int primary key identity(1,1),
UserId int not null, 
OrderDate datetime not null,
TotalAmount decimal(10,2) not null,
foreign key (UserId) references [User] (IdUser)

)
create table OrderItem(
IdOrderItem int primary key identity(1,1),
OrderId int not null,
ProductId int not null,
Quantity int not null,
Price decimal(10,2) not null,
TotalCost decimal(10,2) not null,
foreign key (OrderId) references [Order] (IdOrder),
foreign key (ProductId) references [Product] (IdProduct)
)

CREATE TABLE Logs
(
	[Id] [int] PRIMARY KEY IDENTITY(1,1) NOT NULL,
	[Message] [nvarchar](max) NOT NULL,
	[Level] [int] NOT NULL,
	[Timestamp] [datetime] NOT NULL
)


-- Uloge (Role)
INSERT INTO [Role] (RoleName) VALUES ('Admin');
INSERT INTO [Role] (RoleName) VALUES ('User');

-- Države (Country)
INSERT INTO [Country] ([Name]) VALUES ('Hrvatska');
INSERT INTO [Country] ([Name]) VALUES ('Bosna i Hercegovina');

-- Kategorija proizvoda (ProductCategory)
INSERT INTO ProductCategory ([Name]) VALUES ('Kratke majice');
INSERT INTO ProductCategory ([Name]) VALUES ('Duge majice');

-- Proizvodi (Product) 
INSERT INTO [Product] ([Name], ProductDescription, Price, Quantity, ImageUrl, CategoryId) VALUES
('Majica Crna', 'Crna pamučna majica, 100% pamuk.', 16, 100, null, 1),
('Majica Bijela', 'Bijela majica s printom, moderan kroj.', 18, 80, null, 2),
('Majica Plava', 'Plava sportska majica, prozračna.', 19, 60, null, 1);

-- Veza proizvoda i država (CountryProduct)
INSERT INTO CountryProduct (CountryId, ProductId) VALUES (1, 1); -- Hrvatska - Majica Crna
INSERT INTO CountryProduct (CountryId, ProductId) VALUES (2, 2); -- Hrvatska - Majica Bijela

-- Logovi (Logs)
INSERT INTO Logs ([Message], [Level], [Timestamp]) VALUES ('Korisnik Ana napravio narudžbu', 1, GETDATE());
INSERT INTO Logs ([Message], [Level], [Timestamp]) VALUES ('Korisnik Luka napravio narudžbu', 1, GETDATE());
INSERT INTO Logs ([Message], [Level], [Timestamp]) VALUES ('Admin prijavljen', 2, GETDATE());
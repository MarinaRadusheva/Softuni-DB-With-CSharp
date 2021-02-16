CREATE DATABASE CarRental
USE CarRental
CREATE TABLE Categories (
	Id INT IDENTITY PRIMARY KEY NOT NULL,
	CategoryName VARCHAR(50) NOT NULL,
	DailyRate DECIMAL(20,2) NOT NULL,
	WeeklyRate DECIMAL(20,2) NOT NULL,
	MonthlyRate DECIMAL(20,2) NOT NULL,
	WeekendRate DECIMAL(20,2) NOT NULL
	)

INSERT INTO Categories VALUES
	('Comby', 5.20, 20.99, 100, 15.50),
	('Sports', 7.20, 22.99, 120, 15.50),
	('4x4', 8.20, 25.99, 150, 15.50)

CREATE TABLE Cars (
	Id INT IDENTITY PRIMARY KEY NOT NULL,
	PlateNumber VARCHAR(10) NOT NULL,
	Manufacturer VARCHAR(20),
	Model VARCHAR(20),
	CarYear SMALLINT,
	CategoryId INT NOT NULL,
	Doors TINYINT,
	Picture VARCHAR(MAX),
	Condition VARCHAR(100),
	Available BIT NOT NULL
	)
INSERT INTO Cars VALUES
	('B6867PH', 'Mazda', '323f', 1995, 2, 5, 'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSyPF98UAuAzVVXPev7RRenNIO0euUNaOvoFg&usqp=CAU', 'good', 1),
	('klagtm', 'Renault', 'Megane', 2000, 3, 5, NULL, 'good', 1),
	('B6867PH', 'Audi', 'a3', 2010, 1, 2, NULL, 'good', 0)

CREATE TABLE Employees (
	Id INT IDENTITY PRIMARY KEY NOT NULL,
	FirstName VARCHAR(20) NOT NULL,
	LastName VARCHAR(20) NOT NULL,
	Title VARCHAR(30) NOT NULL,
	Notes VARCHAR(MAX)
)
INSERT INTO Employees VALUES
	('Pesho', 'Peshev', 'junior', NULL),
	('Pesho', 'Peshev', 'mid', NULL),
	('Pesho', 'Peshev', 'ceo', NULL)

CREATE TABLE Customers (
	Id INT IDENTITY PRIMARY KEY NOT NULL,
	DriverLicenceNumber VARCHAR(30) NOT NULL,
	FullName VARCHAR(50) NOT NULL,
	[Address] VARCHAR(100),
	City VARCHAR(30),
	ZIPCode VARCHAR(10),
	Notes VARCHAR(MAX)
	)
INSERT INTO Customers VALUES
	('165113845', 'Petko', NULL, NUll, NULL, 'some guy'),
	('15865', 'Goshko', 'kv.Levski', 'Sopot', '500', 'some guy'),
	('4468534', 'Nasko', NULL, NUll, NULL, 'some guy')

CREATE TABLE RentalOrders (
	Id INT IDENTITY PRIMARY KEY NOT NULL,
	EmployeeId INT NOT NULL,
	CustomerId INT NOT NULL,
	CarId INT NOT NULL,
	TankLevel REAL,
	KilometrageStart INT NOT NULL,
	KilometrageEnd INT NOT NULL,
	TotalKilometrage INT NOT NULL,
	StartDate DATETIME NOT NULL,
	EndDate DATETIME NOT NULL,
	TotalDays TINYINT NOT NULL,
	RateApplied DECIMAL(10,2),
	TaxRate DECIMAL(4,2),
	OrderStatus VARCHAR(20),
	Notes VARCHAR(MAX)
	)
INSERT INTO RentalOrders VALUES
	(1, 1, 1, 50.8, 50123, 50800, 677, '12/05/2020', '12/10/2020', 5, 5.20, 1.2, 'FINISHED', 'FVDSHIFKJB'),
	(1, 1, 1, 50.8, 50123, 50800, 677, '12/05/2020', '12/10/2020', 5, 5.20, 1.2, 'FINISHED', 'FVDSHIFKJB'),
	(1, 1, 1, 50.8, 50123, 50800, 677, '12/05/2020', '12/10/2020', 5, 5.20, 1.2, 'FINISHED', 'FVDSHIFKJB')
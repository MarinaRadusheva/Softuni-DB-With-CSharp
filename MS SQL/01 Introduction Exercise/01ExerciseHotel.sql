--CREATE DATABASE Hotel
--USE Hotel

CREATE TABLE Employees (
	Id INT IDENTITY PRIMARY KEY NOT NULL,
	FirstName VARCHAR(30) NOT NULL,
	LastName VARCHAR(30) NOT NULL,
	Title VARCHAR (30) NOT NULL,
	Notes VARCHAR(MAX)
	)
INSERT INTO Employees VALUES 
	('Ivo', 'Peshev', 'CEO', NULL),
	('Rayko', 'Peshev', 'Manager', NULL),
	('Toni', 'Peshev', 'Front desk', NULL)

CREATE TABLE Customers (
	AccountNumber INT IDENTITY PRIMARY KEY NOT NULL,
	FirstName VARCHAR(30) NOT NULL, 
	LastName VARCHAR(30) NOT NULL,
	PhoneNumber VARCHAR(10),
	EmergencyName VARCHAR(30) NOT NULL,
	EmergencyNumber VARCHAR(10) NOT NULL,
	Notes VARCHAR(MAX)
	)
INSERT INTO Customers VALUES
	('Traicho', 'Traikov', '879546','Mama', '486566', 'vip'),
	('Bobi', 'Bobev', '879546','Iva', '185484', 'ordinary'),
	('Marko', 'Markov', '879546','Petya', '189', 'vip')

CREATE TABLE RoomStatus (
	RoomStatus VARCHAR(30) PRIMARY KEY NOT NULL,
	Notes VARCHAR(MAX)
	)
INSERT INTO RoomStatus VALUES
	('Occupied', 'there are people in'),
	('Vacant', 'it  is ready for guests'),
	('Pending', 'it is being prepaired')
	
CREATE TABLE RoomTypes (
	RoomType VARCHAR(30) PRIMARY KEY NOT NULL, 
	Notes VARCHAR(MAX)
	)
INSERT INTO RoomTypes VALUES 
	('Suite', 'luxury'),
	('Double', NULL),
	('Single', NULL)

CREATE TABLE BedTypes (
	BedType VARCHAR(20) PRIMARY KEY NOT NULL,
	Notes VARCHAR(MAX)
	)
INSERT INTO BedTypes VALUES
	('Single', NULL),
	('Queen', NULL),
	('King', NULl)

CREATE TABLE Rooms (
	RoomNumber TINYINT PRIMARY KEY IDENTITY NOT NULL,
	RoomType VARCHAR(30) NOT NULL, 
	BedType VARCHAR(20) NOT NULL,
	Rate DECIMAL(6,2) NOT NULL,
	RoomStatus VARCHAR(30) NOT NULL,
	Notes VARCHAR(MAX)
	)
INSERT INTO Rooms VALUES
	('Double', 'King', 25.40, 'Vacant', NULL),
	('Single', 'Queen', 55.40, 'Vacant', NULL),
	('Suite', 'King', 85.40, 'Vacant', NULL)

CREATE TABLE Payments (
	Id INT IDENTITY PRIMARY KEY NOT NULL,
	EmployeeId INT NOT NULL,
	PaymentDate DATETIME NOT NULL,
	AccountNumber INT NOT NULL,
	FirstDateOccupied DATETIME NOT NULL,
	LastDateOccupied DATETIME NOT NULL,
	TotalDays INT NOT NULL,
	AmountCharged DECIMAL(10,2) NOT NULL,
	TaxRate DECIMAL(4,2) NOT NULL,
	TaxAmount DECIMAL(10,2) NOT NULL,
	PaymentTotal DECIMAL(10,2) NOT NULL,
	Notes VARCHAR(MAX)
	)
INSERT INTO Payments VALUES
	(1, GETDATE(), 2, '08/05/2020', '08/15/2020', 10, 508.90, 5.2, 26.46, 535.36, 'Paid'),
	(2, GETDATE(), 2, '08/05/2020', '08/15/2020', 10, 508.90, 5.2, 26.46, 535.36, 'Paid'),
	(1, GETDATE(), 2, '08/05/2020', '08/15/2020', 10, 508.90, 5.2, 26.46, 535.36, 'Paid')


CREATE TABLE Occupancies (
	Id INT IDENTITY PRIMARY KEY NOT NULL,
	EmployeeId INT NOT NULL,
	DateOccupied DATETIME NOT NULL,
	AccountNumber INT NOT NULL,
	RoomNumber TINYINT NOT NULL,
	RateApplied DECIMAL(10,2) NOT NULL,
	PhoneCharge DECIMAL (10,2),
	Notes VARCHAR(MAX)
	)
INSERT INTO Occupancies VALUES
	(2, '05/16/2020', 2, 1, 20.30, 7.80, 'LEAVING'),
	(1, '10/04/2020', 1, 3, 15.90, NULL, NULL),
	(3, '06/05/2020', 3, 2, 67.85, 15.30, NULL)

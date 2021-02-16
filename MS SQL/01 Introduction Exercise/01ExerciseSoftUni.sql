USE SoftUni
INSERT INTO Towns VALUES
	('Sofia'),
	('Plovdiv'),
	('Varna'), 
	('Burgas')
INSERT INTO Departments VALUES
	('Engineering'), 
	('Sales'),
	('Marketing'),
	('Software Development'),
	('Quality Assurance')

INSERT INTO Employees VALUES
	('Ivan', 'Ivanov', 'Ivanov', '.NET Developer',	4,	'01/02/2013', 3500.00, NULL),
	('Petar', 'Petrov', 'Petrov',	'Senior Engineer',	1,	'02/03/2004',	4000.00, NULL),
	('Maria', 'Petrova', 'Ivanova',	'Intern', 5,	'08/28/2016',	525.25, NULL),
	('Georgi', 'Teziev', 'Ivanov',	'CEO',	2,	'09/12/2007',	3000.00, NULL),
	('Peter', 'Pan', 'Pan',	'Intern', 3,	'08/28/2016',	599.88, NULL)


ALTER TABLE Employees
ALTER COLUMN AddressId INT NULL
SELECT* FROM Towns
SELECT* FROM Departments
SELECT * FROM Employees

SELECT* FROM Towns ORDER BY [Name] ASC
SELECT* FROM Departments ORDER BY [NAME] ASC
SELECT * FROM Employees ORDER BY Salary DESC

SELECT [Name] FROM Towns ORDER BY [Name] ASC
SELECT [Name] FROM Departments ORDER BY [NAME] ASC
SELECT FirstName, LastName, JobTitle, Salary FROM Employees ORDER BY Salary DESC

UPDATE Employees
SET Salary *=1.1
SELECT Salary FROM Employees

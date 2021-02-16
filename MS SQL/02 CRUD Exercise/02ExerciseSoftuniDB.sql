SELECT * FROM Departments
SELECT [Name] FROM Departments
SELECT FirstName, LastName, Salary FROM Employees
SELECT FirstName, MiddleName, LastName FROM Employees
SELECT FirstName + '.' + LastName + '@softuni.bg' AS [Full Email Address] FROM Employees
SELECT DISTINCT Salary FROM Employees

SELECT * FROM Employees
	WHERE JobTitle = 'Sales Representative';

SELECT FirstName, LastName, JobTitle
	FROM Employees
	WHERE Salary BETWEEN 20000 AND 30000;

SELECT FirstName + ' ' + MiddleName + ' ' +LastName AS [Full Name] 
	FROM Employees
	WHERE Salary IN (25000, 14000, 12500, 23600);

SELECT FirstName, LastName
	FROM Employees
	WHERE ManagerID IS NULL

SELECT FirstName, LastName, Salary
	FROM Employees
	WHERE Salary>50000
	ORDER BY Salary DESC;

SELECT TOP(5) FirstName, LastName
	FROM Employees
	ORDER BY Salary DESC;

SELECT FirstName, LastName
	FROM Employees
	WHERE DepartmentID !=4;

SELECT * FROM Employees
	ORDER BY Salary DESC, FirstName, LastName DESC, MiddleName;

SELECT DISTINCT JobTitle FROM Employees

SELECT TOP(10) * FROM Projects
	ORDER BY StartDate, [Name]

--Write a SQL query to find last 7 hired employees. Select their first, last name and their hire date.
SELECT TOP(7) FirstName, LastName, HireDate
	FROM Employees
	ORDER BY HireDate DESC

--Write a SQL query to increase salaries of all employees that are in the Engineering, Tool Design, Marketing or Information Services department by 12%. Then select Salaries column from the Employees table	
UPDATE Employees
SET Salary = Salary*1.12
	WHERE DepartmentID IN (1,2,4,11)
SELECT Salary FROM Employees
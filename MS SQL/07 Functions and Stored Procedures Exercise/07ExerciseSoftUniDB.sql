--Create stored procedure usp_GetEmployeesSalaryAbove35000 that returns all employees’ first and last names for whose salary is above 35000. 

CREATE PROCEDURE dbo.usp_GetEmployeesSalaryAbove35000
AS
	SELECT FirstName, LastName FROM Employees
	WHERE Salary>35000

GO
EXEC DBO.usp_GetEmployeesSalaryAbove35000
GO
--Create stored procedure usp_GetEmployeesSalaryAboveNumber that accept a number (of type DECIMAL(18,4)) as parameter and returns all employees’ first and last names whose salary is above or equal to the given number. 
CREATE PROC usp_GetEmployeesSalaryAboveNumber (@minSalary DECIMAL(18,4))
 AS
	SELECT FirstName, LastName FROM Employees 
	WHERE Salary>=@minSalary
GO
EXEC DBO.usp_GetEmployeesSalaryAboveNumber 48100
GO

--Write a stored procedure usp_GetTownsStartingWith that accept string as parameter and returns all town names starting with that string.
CREATE PROC usp_GetTownsStartingWith (@townName VARCHAR(50))
AS
	SELECT [Name] FROM Towns
	WHERE LEFT([Name], LEN(@TOWNNAME)) = @townName
GO
EXEC usp_GetTownsStartingWith 'B'
GO

--Write a stored procedure usp_GetEmployeesFromTown that accepts town name as parameter and return the employees’ first and last name that live in the given town. 

CREATE PROC usp_GetEmployeesFromTown (@townName VARCHAR(50))
AS
	SELECT FirstName, LastName FROM Employees E
	JOIN Addresses A ON E.AddressID = A.AddressID
	JOIN Towns T ON T.TownID = A.TownID
	WHERE T.Name = @townName
GO
EXEC usp_GetEmployeesFromTown 'SOFIA'
GO

/*Write a function ufn_GetSalaryLevel(@salary DECIMAL(18,4)) that receives salary of an employee and returns the level of the salary.
If salary is < 30000 return "Low"
If salary is between 30000 and 50000 (inclusive) return "Average"
If salary is > 50000 return "High" */

CREATE FUNCTION ufn_GetSalaryLevel(@salary DECIMAL(18,4)) RETURNS VARCHAR(10)
AS
BEGIN
	DECLARE @result VARCHAR(10)
	IF @salary<30000
	SET @result = 'Low'
	ELSE IF @salary<=50000
	SET @result = 'Average'
	ELSE
	SET @result ='High'
	RETURN @RESULT
END
GO
SELECT Salary, DBO.ufn_GetSalaryLevel(Salary) AS [Salary Level] FROM Employees
GO
--Write a stored procedure usp_EmployeesBySalaryLevel that receive as parameter level of salary (low, average or high) and print the names of all employees that have given level of salary. You should use the function - "dbo.ufn_GetSalaryLevel(@Salary) ", which was part of the previous task, inside your "CREATE PROCEDURE …" query.

CREATE PROC usp_EmployeesBySalaryLevel (@salaryLevel VARCHAR(10))
AS
	SELECT FirstName, LastName FROM Employees
	WHERE dbo.ufn_GetSalaryLevel(Salary) = @salaryLevel
GO
EXEC usp_EmployeesBySalaryLevel 'High'
GO

--Define a function ufn_IsWordComprised(@setOfLetters, @word) that returns true or false depending on that if the word is a comprised of the given set of letters. 
CREATE FUNCTION ufn_IsWordComprised(@setOfLetters VARCHAR(MAX), @word VARCHAR(MAX))
RETURNS BIT
AS
BEGIN
	DECLARE @result BIT = 1
	DECLARE @current CHAR(1)
	WHILE (LEN(@word)>0)
	BEGIN 
		SET @current = LEFT(@word, 1)
		IF @setOfLetters NOT LIKE '%'+@CURRENT+'%' 
			SET @result =0
		ELSE
		SET @word = RIGHT(@word, LEN(@WORD)-1)
		IF @result = 0
		BREAK
	END
RETURN @result
			
END

GO

SELECT 'oistmiahf'  , 'halves' , DBO.ufn_IsWordComprised('oistmiahf'  , 'halves')
GO

--Write a procedure with the name usp_DeleteEmployeesFromDepartment (@departmentId INT) which deletes all Employees from a given department. Delete these departments from the Departments table too. Finally SELECT the number of employees from the given department. If the delete statements are correct the select query should return 0.

CREATE PROC usp_DeleteEmployeesFromDepartment (@departmentId INT)
AS
	ALTER TABLE Departments
	ALTER COLUMN ManagerID INT NULL

	DELETE FROM EmployeesProjects WHERE EmployeeID IN (SELECT EmployeeID FROM Employees WHERE DepartmentID = @departmentId)

	UPDATE Employees
	SET ManagerID = NULL
	WHERE ManagerID IN (SELECT EmployeeID FROM Employees WHERE DepartmentID = @departmentId)

	UPDATE Departments
	SET ManagerID = NULL
	WHERE ManagerID IN (SELECT EmployeeID FROM Employees WHERE DepartmentID = @departmentId)

	DELETE FROM Employees
		WHERE DepartmentID = @departmentId
	DELETE FROM Departments
		WHERE DepartmentID = @departmentId

	SELECT COUNT(*) FROM Employees WHERE DepartmentID = @departmentId

GO

EXEC DBO.usp_DeleteEmployeesFromDepartment 2




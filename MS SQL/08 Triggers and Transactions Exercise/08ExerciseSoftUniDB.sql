--Create a procedure usp_AssignProject(@emloyeeId, @projectID) that assigns projects to employee. If the employee has more than 3 project throw exception and rollback the changes. The exception message must be: "The employee has too many projects!" with Severity = 16, State = 1.
CREATE PROC usp_AssignProject(@emloyeeId INT, @projectID INT)
AS
 BEGIN TRANSACTION
 
	IF ((SELECT COUNT(*) FROM EmployeesProjects WHERE EmployeeID=@emloyeeId)>=3)
	BEGIN
		ROLLBACK;
		THROW 50005, 'The employee has too many projects!', 1
	END
	INSERT INTO EmployeesProjects (EmployeeID, ProjectID) VALUES (@emloyeeId, @projectID);
COMMIT

EXEC DBO.usp_AssignProject 2,5
SELECT * FROM EmployeesProjects

--Create a table Deleted_Employees(EmployeeId PK, FirstName, LastName, MiddleName, JobTitle, DepartmentId, Salary) that will hold information about fired (deleted) employees from the Employees table. Add a trigger to Employees table that inserts the corresponding information about the deleted records in Deleted_Employees.

CREATE TABLE Deleted_Employees (
	EmployeeId INT IDENTITY PRIMARY KEY,
	FirstName VARCHAR(50) NOT NULL, 
	LastName VARCHAR(50) NOT NULL, 
	MiddleName VARCHAR(50),
	JobTitle VARCHAR(50) NOT NULL,
	DepartmentId INT NOT NULL, 
	Salary MONEY NOT NULL)

	SELECT * FROM Deleted_Employees
	GO
CREATE TRIGGER tr_StoreDeletedEmployees ON Employees FOR DELETE
AS
INSERT INTO Deleted_Employees (FirstName, LastName, MiddleName, JobTitle, DepartmentID, Salary) SELECT FirstName, LastName, MiddleName, JobTitle, DepartmentID, Salary FROM deleted;


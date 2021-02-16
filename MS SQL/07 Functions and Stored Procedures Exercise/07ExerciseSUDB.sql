/*Write a procedure with the name usp_DeleteEmployeesFromDepartment (@departmentId INT) which deletes all Employees from a given department. Delete these departments from the Departments table too. Finally SELECT the number of employees from the given department. If the delete statements are correct the select query should return 0.
After completing that exercise restore your database to revert all changes. */

--CREATE PROC usp_DeleteEmployeesFromDepartment (@departmentId INT)
SELECT * FROM Employees
WHERE DepartmentID = 2

DELETE FROM Employees
WHERE DepartmentID = 2
SELECT * FROM EmployeesProjects
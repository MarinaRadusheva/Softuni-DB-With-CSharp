--Write a query that selects: EmployeeId, JobTitle, AddressId,AddressText; Return the first 5 rows sorted by AddressId in ascending order.

SELECT TOP (5) E.EmployeeID, E.JobTitle, E.AddressID, A.AddressText
	FROM Employees E
	JOIN Addresses A ON E.AddressID = A.AddressID
	ORDER BY AddressID

--Write a query that selects: FirstName, LastName, Town, AddressText Sorted by FirstName in ascending order then by LastName. Select first 50 employees

SELECT TOP(50) FirstName, LastName, T.Name AS [Town], A.AddressText
	FROM Employees E
	JOIN Addresses A ON E.AddressID = A.AddressID
	JOIN Towns T ON A.TownID = T.TownID
	ORDER BY FirstName, LastName;

--Write a query that selects: EmployeeID, FirstName LastName, DepartmentName Sorted by EmployeeID in ascending order. Select only employees from "Sales" department

SELECT e.EmployeeID, e.FirstName, e.LastName, d.Name
	FROM Employees e
	JOIN Departments d ON e.DepartmentID = d.DepartmentID
	WHERE D.Name = 'Sales'
	ORDER BY EmployeeID;

--Write a query that selects: EmployeeID, FirstName, Salary, DepartmentName. Filter only employees with salary higher than 15000. Return the first 5 rows sorted by DepartmentID in ascending order.

SELECT TOP(5) E.EmployeeID, E.FirstName, E.Salary, D.Name AS DepartmentName
	FROM Employees E
	JOIN Departments D ON E.DepartmentID = D.DepartmentID
	WHERE Salary>15000
	ORDER BY D.DepartmentID

--Write a query that selects:, EmployeeID, FirstName. Filter only employees without a project. Return the first 3 rows sorted by EmployeeID in ascending order.
SELECT TOP (3) E.EmployeeID, E.FirstName
	FROM Employees  E  
	LEFT JOIN EmployeesProjects EP ON E.EmployeeID = EP.EmployeeID
	WHERE ProjectID IS NULL

--Write a query that selects: FirstName, LastName, HireDate, DeptName. Filter only employees hired after 1.1.1999 and are from either "Sales" or "Finance" departments, sorted by HireDate (ascending).

SELECT FirstName, LastName, HireDate, D.Name AS DeptName
	FROM Employees E
	JOIN Departments D ON E.DepartmentID = D.DepartmentID
	WHERE HireDate>'1-1-1999' AND (D.Name IN ('Sales' , 'Finance'))
	ORDER BY HireDate;
--Write a query that selects: EmployeeID, FirstName, ProjectName. Filter only employees with a project which has started after 13.08.2002 and it is still ongoing (no end date). Retur the first 5 rows sorted by EmployeeID in ascending order.

SELECT TOP(5) E.EmployeeID, E.FirstName, P.Name AS ProjectName
	FROM Employees E
	JOIN EmployeesProjects EP ON E.EmployeeID = EP.EmployeeID
	JOIN Projects P ON EP.ProjectID = P.ProjectID
	WHERE P.StartDate>'8-13-2002' AND P.EndDate IS NULL
	ORDER BY E.EmployeeID

--Write a query that selects: EmployeeID, FirstName, ProjectName. Filter all the projects of employee with Id 24. If the project has started during or after 2005 the returned value should be NULL.

SELECT E.EmployeeID, FirstName, CASE WHEN P.StartDate>'01-01-2005' THEN NULL ELSE P.Name END AS [ProjectName]
	FROM Employees E
	JOIN EmployeesProjects EP ON E.EmployeeID = EP.EmployeeID
	JOIN Projects P ON EP.ProjectID = P.ProjectID
	WHERE E.EmployeeID = 24

--Write a query that selects: EmployeeID, FirstName, ManagerID, ManagerName. Filter all employees with a manager who has ID equals to 3 or 7. Return all the rows, sorted by EmployeeID in ascending order.

SELECT E.EmployeeID, E.FirstName, E.ManagerID, M.FirstName
	FROM Employees E
	JOIN Employees M ON E.ManagerID = M.EmployeeID
	WHERE E.ManagerID IN (3, 7)
	ORDER BY E.EmployeeID

--Write a query that selects: EmployeeID, EmployeeName, ManagerName, DepartmentName. Show first 50 employees with their managers and the departments they are in (show the departments of the employees). Order by EmployeeID.

SELECT TOP(50) E.EmployeeID, E.FirstName+' '+E.LastName AS EmployeeName, M.FirstName + ' ' + M.LastName AS ManagerName, D.Name AS DepartmentName
	FROM Employees E
	LEFT JOIN Employees M ON E.ManagerID = M.EmployeeID
	LEFT JOIN Departments D ON E.DepartmentID = D.DepartmentID
	ORDER BY E.EmployeeID

--Write a query that returns the value of the lowest average salary of all departments.
SELECT MIN(AvS.AverageSalary) FROM (
SELECT E.DepartmentID, AVG(E.Salary) AS AverageSalary
	FROM Employees E
	GROUP BY E.DepartmentID) AS AvS



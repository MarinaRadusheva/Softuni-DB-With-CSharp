--You are given a table Orders(Id, ProductName, OrderDate) filled with data. Consider that the payment for that order must be accomplished within 3 days after the order date. Also the delivery date is up to 1 month. Write a query to show each product’s name, order date, pay and deliver due dates. 
use Store
CREATE TABLE Orders (
	Id INT IDENTITY PRIMARY KEY NOT NULL,
	ProductName VARCHAR(50) NOT NULL,
	OrderDate DATETIME NOT NULL
	)
INSERT INTO Orders VALUES 
	('Butter',	'2016-09-19 00:00:00.000'),
	('Milk',	'2016-09-30 00:00:00.000'),
	('Cheese',	'2016-09-04 00:00:00.000'),
	('Bread',	'2015-12-20 00:00:00.000'),
	('Tomatoes',	'2015-12-30 00:00:00.000')

	SELECT ProductName, OrderDate, DATEADD(DAY, 3, OrderDate) AS[Pay Due], DATEADD(MONTH, 1, OrderDate) AS [Deliver Due] 
	FROM Orders
--query to find TIME SPAN in years, months, days and minutes
SELECT OrderDate,
	   DATEDIFF(YEAR, OrderDate, GETDATE()) AS YEARS,
	   DATEDIFF(MONTH, OrderDate, GETDATE()) AS MONTHS,
	   DATEDIFF(DAY, OrderDate, GETDATE()) AS [DAYS],
	   DATEDIFF(MINUTE, OrderDate, GETDATE()) AS [MINUTES]
	FROM Orders
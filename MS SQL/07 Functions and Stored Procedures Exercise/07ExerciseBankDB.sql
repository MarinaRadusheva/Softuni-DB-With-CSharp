--You are given a database schema with tables AccountHolders(Id (PK), FirstName, LastName, SSN) and Accounts(Id (PK), AccountHolderId (FK), Balance).  Write a stored procedure usp_GetHoldersFullName that selects the full names of all people. 
CREATE PROCEDURE usp_GetHoldersFullName
AS
	SELECT FirstName + ' ' + LastName AS [Full Name] FROM AccountHolders
GO

EXEC DBO.usp_GetHoldersFullName
GO
-- Your task is to create a stored procedure usp_GetHoldersWithBalanceHigherThan that accepts a number as a parameter and returns all people who have more money in total of all their accounts than the supplied number. Order them by first name, then by last name
CREATE PROC usp_GetHoldersWithBalanceHigherThan (@amount DECIMAL(10,2))
AS
	SELECT  H.FirstName, H.LastName
	FROM AccountHolders H
 	JOIN Accounts A ON H.Id = A.AccountHolderId
	GROUP BY H.FirstName, H.LastName
	HAVING SUM(A.Balance)>@amount
	ORDER BY H.FirstName, H.LastName
GO
EXEC DBO.usp_GetHoldersWithBalanceHigherThan 150000.00
GO

/*Your task is to create a function ufn_CalculateFutureValue that accepts as parameters – sum (decimal), yearly interest rate (float) and number of years(int). It should calculate and return the future value of the initial sum rounded to the fourth digit after the decimal delimiter. Using the following formula:

I – Initial sum
R – Yearly interest rate
T – Number of years
*/

CREATE FUNCTION ufn_CalculateFutureValue(@amount decimal(18,2), @interestRate float, @years int)
RETURNS DECIMAL(18,4)
AS
BEGIN
  DECLARE @exactAmount DECIMAL(18,4) = @AMOUNT*POWER((1+@interestRate),@years)
		
			RETURN @exactAmount
END
GO
SELECT DBO.ufn_CalculateFutureValue(1000, 0.1, 5)
GO

/*Your task is to create a stored procedure usp_CalculateFutureValueForAccount that uses the function from the previous problem to give an interest to a person's account for 5 years, along with information about his/her account id, first name, last name and current balance as it is shown in the example below. It should take the AccountId and the interest rate as parameters. Again you are provided with “dbo.ufn_CalculateFutureValue” function which was part of the previous task.
Example*/

CREATE PROC usp_CalculateFutureValueForAccount (@accountId INT,  @interestRate FLOAT)
AS
	SELECT A.Id, H.FirstName, H.LastName, A.Balance, DBO.ufn_CalculateFutureValue(A.Balance, @interestRate,5) AS [Balance in 5 years]
	FROM Accounts A
	JOIN AccountHolders H ON A.AccountHolderId = H.Id
	WHERE A.Id = @accountId;

GO
EXEC DBO.usp_CalculateFutureValueForAccount 1, 0.1
GO
SELECT * FROM Accounts, AccountHolders
GO

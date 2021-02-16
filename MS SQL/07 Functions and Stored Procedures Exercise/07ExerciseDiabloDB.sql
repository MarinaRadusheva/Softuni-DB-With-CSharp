--Create a function ufn_CashInUsersGames that sums the cash of odd rows. Rows must be ordered by cash in descending order. The function should take a game name as a parameter and return the result as table. Submit only your function in.
CREATE FUNCTION ufn_CashInUsersGames (@gameName NVARCHAR(50))
RETURNS TABLE
AS	
 RETURN (SELECT SUM(Cash) AS [SumCash] FROM
	(SELECT ROW_NUMBER() OVER (ORDER BY U.Cash DESC) AS [RowN],
	Cash FROM Games G
	JOIN UsersGames U  ON G.Id = U.GameId
	WHERE G.Name = @gameName)  AS C 
	WHERE RowN%2=1 ) 
GO
SELECT * FROM DBO.ufn_CashInUsersGames ('Love in a mist')

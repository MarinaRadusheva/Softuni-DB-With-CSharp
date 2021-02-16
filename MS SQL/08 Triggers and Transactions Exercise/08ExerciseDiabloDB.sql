--User Stamat in Safflower game wants to buy some items. He likes all items from Level 11 to 12 as well as all items from Level 19 to 21. As it is a bulk operation you have to use transactions. 
--A transaction is the operation of taking out the cash from the user in the current game as well as adding up the items. 
--Write transactions for each level range. If anything goes wrong turn back the changes inside of the transaction.
--Extract all of Stamat’s item names in the given game sorted by name alphabetically

--SELECT* FROM Users U
--JOIN UsersGames UG ON U.Id = UG.UserId
--JOIN Games G ON G.Id = UG.GameId
--WHERE Username = 'Stamat' AND G.[Name] = 'Safflower'

--SELECT * FROM Items WHERE MinLevel IN (11,12,19,20,21)
--ORDER BY MinLevel
--SELECT * FROM Items IT 
--JOIN UserGameItems I ON IT.Id = I.ItemId 
--JOIN UsersGames U ON I.UserGameId = U.Id
--WHERE U.UserId = 9 AND GameId = 87

DECLARE @userGameId INT = (SELECT Id FROM UsersGames WHERE UserId = 9 AND GameId = 87)
DECLARE @totalCash MONEY = (SELECT Cash FROM UsersGames WHERE UserId = 9 AND GameId = 87)
DECLARE @totalSum MONEY = (SELECT SUM(Price) FROM Items WHERE MinLevel BETWEEN 11 AND 12)

IF (@totalCash>=@totalSum)
	BEGIN
		BEGIN TRANSACTION
		INSERT INTO UserGameItems (ItemId, UserGameId)
		(SELECT Id, @userGameId FROM Items WHERE MinLevel BETWEEN 11 AND 12)
		UPDATE UsersGames
		SET Cash -= @totalSum WHERE UserId = 9 AND GameId = 87
	COMMIT
	END
SET @totalCash = (SELECT Cash FROM UsersGames WHERE UserId = 9 AND GameId = 87)
SET @totalSum = (SELECT SUM(Price) FROM Items WHERE MinLevel BETWEEN 19 AND 21)
IF(@totalCash>=@totalSum)
	BEGIN
		BEGIN TRANSACTION
		INSERT INTO UserGameItems (ItemId, UserGameId)
		(SELECT Id, @userGameId FROM Items WHERE MinLevel BETWEEN 19 AND 21)
		UPDATE UsersGames
		SET Cash -= @totalSum WHERE UserId = 9 AND GameId = 87
	COMMIT
	END
SELECT I.[Name] FROM Items I
JOIN UserGameItems UG ON I.Id = UG.ItemId WHERE UserGameId=@userGameId ORDER BY I.[Name]
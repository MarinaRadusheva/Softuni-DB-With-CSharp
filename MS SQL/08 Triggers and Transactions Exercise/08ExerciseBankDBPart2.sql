--Add stored procedure usp_DepositMoney (AccountId, MoneyAmount) that deposits money to an existing account. Make sure to guarantee valid positive MoneyAmount with precision up to fourth sign after decimal point. The procedure should produce exact results working with the specified precision.

CREATE OR ALTER PROC usp_DepositMoney (@AccountId INT , @MoneyAmount MONEY)
AS
BEGIN TRANSACTION
  IF NOT EXISTS (SELECT * FROM Accounts WHERE Id = @AccountId)
  BEGIN
  ROLLBACK;
  THROW 50001, 'Invalid Aaccount!', 1
 END
	UPDATE Accounts
	SET Balance +=@MoneyAmount
	WHERE Id = @AccountId
COMMIT

EXEC usp_DepositMoney 2, 10
SELECT * FROM Accounts WHERE Id =2

--Add stored procedure usp_WithdrawMoney (AccountId, MoneyAmount) that withdraws money from an existing account. Make sure to guarantee valid positive MoneyAmount with precision up to fourth sign after decimal point. The procedure should produce exact results working with the specified precision.

CREATE PROC usp_WithdrawMoney (@AccountId INT, @MoneyAmount MONEY)
AS
BEGIN TRANSACTION
	IF NOT EXISTS (SELECT* FROM Accounts WHERE Id=@AccountId)
	BEGIN
		ROLLBACK;
		THROW 50003, 'Invalid Aaccount!', 1
	END
	IF ((SELECT Balance FROM Accounts WHERE Id = @AccountId)<@MoneyAmount)
	BEGIN
		ROLLBACK;
		THROW 50004, 'Insufficient funds', 1
	END
	UPDATE Accounts
	SET Balance -=@MoneyAmount WHERE Id=@AccountId
COMMIT

EXEC DBO.usp_WithdrawMoney 5, 25
SELECT * FROM Accounts WHERE Id=5

--Write stored procedure usp_TransferMoney(SenderId, ReceiverId, Amount) that transfers money from one account to another. Make sure to guarantee valid positive MoneyAmount with precision up to fourth sign after decimal point. Make sure that the whole procedure passes without errors and if error occurs make no change in the database. You can use both: "usp_DepositMoney", "usp_WithdrawMoney" (look at previous two problems about those procedures). 

CREATE PROC usp_TransferMoney(@SenderId INT, @ReceiverId INT, @Amount MONEY)
AS
BEGIN TRANSACTION
	
	EXEC DBO.usp_WithdrawMoney @SenderId, @Amount
	EXEC DBO.usp_DepositMoney @ReceiverId, @Amount
COMMIT
EXEC DBO.usp_TransferMoney 5, 1, 5000
SELECT * FROM Accounts WHERE Id IN (5,1)

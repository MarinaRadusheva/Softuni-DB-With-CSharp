--Create a table – Logs (LogId, AccountId, OldSum, NewSum). Add a trigger to the Accounts table that enters a new entry into the Logs table every time the sum on an account changes. Submit only the query that creates the trigger.

USE Bank
CREATE TABLE Logs (
	LogId INT IDENTITY PRIMARY KEY NOT NULL,
	AccountId INT NOT NULL,
	OldSum MONEY NOT NULL, 
	NewSum MONEY NOT NULL)
GO
CREATE TRIGGER tr_LogChangesToAccounts ON Accounts AFTER UPDATE
AS
	INSERT INTO Logs (AccountId, OldSum, NewSum)(SELECT I.Id , D.Balance, I.Balance FROM inserted AS I
	JOIN deleted  AS D ON I.Id = D.Id)
GO
/*Create another table – NotificationEmails(Id, Recipient, Subject, Body). Add a trigger to logs table and create new email whenever new record is inserted in logs table. The following data is required to be filled for each email:
Recipient – AccountId
Subject – "Balance change for account: {AccountId}"
Body - "On {date} your balance was changed from {old} to {new}."
Submit your query only for the trigger action.*/
CREATE TABLE NotificationEmails(
	Id INT IDENTITY PRIMARY KEY ,
	Recipient INT NOT NULL, 
	[Subject] VARCHAR(200) NOT NULL,
	Body VARCHAR(200) NOT NULL)
GO
CREATE TRIGGER tr_GenerateEmail ON Logs AFTER INSERT
AS
INSERT INTO NotificationEmails ( Recipient, [Subject], Body)
	SELECT AccountId, 
	'Balance change for account: ' + CAST(AccountId AS VARCHAR(50)), 
	CONCAT('On ',GETDATE(), ' your balance was changed from ',OldSum,' to ',NewSum, '.' )
	FROM inserted
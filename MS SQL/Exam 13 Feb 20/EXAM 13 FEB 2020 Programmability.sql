--Create a user defined function, named udf_AllUserCommits(@username) that receives a username.
--The function must return count of all commits for the user:

CREATE FUNCTION udf_AllUserCommits(@username VARCHAR(MAX))
RETURNS INT
AS
BEGIN
	DECLARE @result INT = 
		(SELECT COUNT(*) 
		FROM Commits C
		JOIN USERS U ON C.ContributorId = U.Id WHERE Username = @username)
	RETURN @result
END
SELECT dbo.udf_AllUserCommits('UnderSinduxrein')

--Create a user defined stored procedure, named usp_SearchForFiles(@fileExtension), that receives files extensions.
--The procedure must print the id, name and size of the file. Add "KB" in the end of the size. Order them by id (ascending), file name (ascending) and file size (descending)

CREATE PROC usp_SearchForFiles(@fileExtension VARCHAR(MAX))
AS
SELECT Id, [Name], CONCAT(Size, 'KB') AS FSize
FROM Files 
WHERE [Name] LIKE '%'+@fileExtension
ORDER BY Id ASC, [Name] ASC, FSize DESC

EXEC usp_SearchForFiles 'txt'


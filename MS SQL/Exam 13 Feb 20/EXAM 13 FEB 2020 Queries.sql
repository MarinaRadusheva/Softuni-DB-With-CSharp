--Select all commits from the database. Order them by id (ascending), message (ascending), repository id (ascending) and contributor id (ascending).

SELECT Id, [Message], RepositoryId, ContributorId FROM Commits
ORDER BY Id ASC, Message ASC, RepositoryId ASC, ContributorId ASC

--Select all of the files, which have size, greater than 1000, and a name containing "html". Order them by size (descending), id (ascending) and by file name (ascending).

SELECT Id, [Name], Size FROM Files WHERE Size > 1000 AND [Name] LIKE '%html%'
ORDER BY Size DESC, Id ASC, [Name] ASC

--Select all of the issues, and the users that are assigned to them, so that they end up in the following format: {username} : {issueTitle}. Order them by issue id (descending) and issue assignee (ascending).

SELECT I.Id, CONCAT(U.Username, ' : ', I.Title) FROM Issues I
JOIN Users U ON I.AssigneeId = U.Id
ORDER BY I.Id DESC, I.AssigneeId ASC

--Select all of the files, which are NOT a parent to any other file. Select their size of the file and add "KB" to the end of it. Order them file id (ascending), file name (ascending) and file size (descending).

SELECT C.Id, C.[Name], CONCAT(C.Size, 'KB') FROM Files C
LEFT JOIN Files P ON C.Id = P.ParentId
WHERE P.Id IS NULL
ORDER BY C.Id ASC, C.[Name] ASC, C.Size DESC

--Select the top 5 repositories in terms of count of commits. Order them by commits count (descending), repository id (ascending) then by repository name (ascending).

SELECT TOP(5) R.Id, R.[Name], COUNT(*) AS [Count] FROM 
RepositoriesContributors RC 
JOIN Repositories R ON RC.RepositoryId = R.Id
JOIN Commits C ON C.RepositoryId = R.Id
GROUP BY R.Id, R.[Name]
ORDER BY [Count] DESC, R.Id ASC, R.[Name] ASC

--Select all users which have commits. Select their username and average size of the file, which were uploaded by them. Order the results by average upload size (descending) and by username (ascending).
SELECT U.Username, AVG(F.Size) AS AVGSIZE FROM Users U 
JOIN Commits C ON U.Id = C.ContributorId
JOIN Files F ON F.CommitId = C.Id
GROUP BY U.Id, U.Username
ORDER BY AVGSIZE DESC, U.Username ASC



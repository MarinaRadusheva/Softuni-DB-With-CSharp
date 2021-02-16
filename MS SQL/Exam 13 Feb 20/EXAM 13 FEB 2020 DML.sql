INSERT INTO Files VALUES 
('Trade.idk',	2598.0,	1,	1),
('menu.net',	9238.31,	2,	2),
('Administrate.soshy',	1246.93,	3,	3),
('Controller.php',	7353.15,	4,	4),
('Find.java',	9957.86,	5,	5),
('Controller.json',	14034.87,	3,	6),
('Operate.xix',	7662.92, 7,	7)

INSERT INTO Issues VALUES
('Critical Problem with HomeController.cs file',	'open',	1,	4),
('Typo fix in Judge.html',	'open',	4,	3),
('Implement documentation for UsersService.cs',	'closed',	8,	2),
('Unreachable code in Index.cs',	'open',	9,	8)

--Make issue status 'closed' where Assignee Id is 6.
UPDATE Issues
SET IssueStatus = 'closed' WHERE AssigneeId = 6
SELECT * FROM Issues WHERE AssigneeId = 6

--Delete repository "Softuni-Teamwork" in repository contributors and issues.
SELECT * FROM Repositories WHERE Name = 'Softuni-Teamwork'
SELECT * FROM RepositoriesContributors WHERE RepositoryId = 3
SELECT * FROM Issues WHERE RepositoryId =3

DELETE FROM Issues WHERE RepositoryId = 3
DELETE FROM RepositoriesContributors WHERE RepositoryId =3
DELETE FROM Repositories WHERE Id = 3


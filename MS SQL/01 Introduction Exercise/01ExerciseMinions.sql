 SELECT * FROM Minions
-- Id – unique number for every person there will be no more than 231-1 people. (Auto incremented)
--Name – full name of the person will be no more than 200 Unicode characters. (Not null)
--Picture – image with size up to 2 MB. (Allow nulls)
--Height –  In meters. Real number precise up to 2 digits after floating point. (Allow nulls)
--Weight –  In kilograms. Real number precise up to 2 digits after floating point. (Allow nulls)
--Gender – Possible states are m or f. (Not null)
--Birthdate – (Not null)
--Biography – detailed biography of the person it can contain max allowed Unicode characters. (Allow nulls)

CREATE TABLE People(
	Id INT IDENTITY PRIMARY KEY NOT NULL,
	[Name] NVARCHAR(200) NOT NULL,
	Picture VARCHAR(MAX),
	Height DECIMAL(38,2),
	[Weight] DECIMAL(38,2),
	Gender CHAR(1) NOT NULL,
	Birthdate DATETIME NOT NULL,
    Biography NVARCHAR(MAX)
	
);
INSERT INTO People ([Name], Picture, Height, [Weight], Gender, Birthdate, Biography) 
VALUES
( 'Pesho', 'uaosnbo', 1.50, 56, 'm', '01/09/1992', 'some guy'),
( 'Gosho', 'cbauhda;j', 1.50, 56.3, 'm', '12/10/1992', 'some guy'),
( 'Sasho', NULL, 1.50, 56, 'f', '01/12/1992', 'some guy'),
( 'Tisho', 'uaosdaudsnbo', NULL, 56, 'm', '01/06/1992', 'some guy'),
( 'Misho', 'uaossdsanbo', 1.50, 56, 'f', '01/07/1992', NULL)

SELECT * FROM People
CREATE TABLE Users(
	Id BIGINT IDENTITY PRIMARY KEY NOT NULL,
	Username VARCHAR(30) NOT NULL,
    [Password] VARCHAR(26) NOT NULL,
	ProfilePicture VARCHAR(MAX),
	LastLoginTime DATETIME,
	IsDeleted BIT
);



INSERT INTO Users  VALUES
('TONI', 'kotio5', 'https://i.kinja-img.com/gawker-media/image/upload/t_original/ijsi5fzb1nbkbhxa2gc1.png', '10/02/1998', 0),
('Kosio', 'polly', 'sbdujpsbos', NULL, 1),
('Sasha', 'pass123', 'https://cnet3.cbsistatic.com/img/il6jwdZY19bL9QEDR5x6zNjd55Y=/0x404:828x1603/940x0/2020/05/18/ef3e4846-00d1-4b6b-8647-d876b73b6b3e/fb-avatar.jpg', '08/22/2005', 0),
('Branko', 'koko2', NULL, NULL, 1),
('Bianka', 'papi', 'fsdgdhdhsh', NULL, 0)

SELECT * FROM Users
--DELETE KEY
ALTER TABLE Users
DROP CONSTRAINT PK__Users__3214EC075AE86C22
--MAKE COMPOSITE KEY
ALTER TABLE Users
ADD CONSTRAINT PK_Id_Username PRIMARY KEY (Id, Username);
--ADD CHECK CONSTRAINT FOR LENGTH OF A CHAR FIELD
ALTER TABLE Users
ADD CHECK (LEN([Password]) > 3);
--SET DEFAULT VALUE
ALTER TABLE Users
ADD CONSTRAINT df_LastLogin DEFAULT GETDATE() FOR LastLoginTime
--DROP KEY AMD MAKE NEW ONE
ALTER TABLE Users
DROP CONSTRAINT PK_Id_Username
ALTER TABLE Users
ADD CONSTRAINT PK_Id PRIMARY KEY (Id)
ALTER TABLE Users
ADD CHECK (LEN(Username)>3);

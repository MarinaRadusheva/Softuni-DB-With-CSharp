CREATE DATABASE Movies
USE Movies
CREATE TABLE Directors (
	Id INT IDENTITY PRIMARY KEY NOT NULL,
	DirectorName NVARCHAR(90) NOT NULL,
	Notes NVARCHAR(MAX)
	)
INSERT INTO Directors VALUES
	('Spilberg', 'oscar winner'),
	('Tarantino', NULL),
	('w.Alan', 'vdaship'),
	('N.Iliev', 'bg'),
	('Mihalkov', 'rus');

CREATE TABLE Genres (
	Id INT IDENTITY PRIMARY KEY NOT NULL,
	GenreName VARCHAR(50) NOT NULL,
	Notes VARCHAR(MAX)
	)

INSERT INTO Genres VALUES
	('Comedy', 'funny'),
	('Drama', 'sad'),
	('Horror', 'scary'),
	('Sci-fi', 'interesting'),
	('Romantic', NULL)

CREATE TABLE Categories (
	Id INT IDENTITY PRIMARY KEY NOT NULL,
	CategoryName VARCHAR(50) NOT NULL,
	Notes VARCHAR(MAX)
	)

INSERT INTO Categories VALUES	
	('Oscar Winners', NULL),
	('Waste of time', 'super boring'),
	('Average', NULL),
	('Cartoon', 'for children'),
	('Favourite', 'best of all')

CREATE TABLE Movies (
	Id INT IDENTITY PRIMARY KEY NOT NULL, 
	Title NVARCHAR(100) NOT NULL,
	DirectorId INT NOT NULL, 
	CopyrightYear SMALLINT NOT NULL,
	[Length] TINYINT NOT NULL,
	GenreId INT NOT NULL,
	CategoryId INT NOT NULL,
	Rating REAL DEFAULT 0,
	Notes VARCHAR(MAX)
	)

INSERT INTO Movies VALUES
	('Avatar', 5, 2007, 186, 2, 1, 8.7, 'purple'),
	('Black Beauty', 4, 2020, 123, 1, 2, 8.0, 'based on novel'),
	('Beauty and the Beast', 3, 2017, 135, 2, 1, 8.7, NULL),
	('Armagedon', 1, 2001, 150, 2, 1, 6.5, 'end of world'),
	('Vicky Christina Barcelon', 2, 2010, 111, 3, 1, 8.7, 'some')

SELECT * FROM Movies
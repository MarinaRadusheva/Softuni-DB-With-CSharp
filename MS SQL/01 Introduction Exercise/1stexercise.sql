/* Create database*/
create database Minions
use Minions

/*Create tables*/
USE Minions
CREATE TABLE Minions(
	Id INT NOT NULL PRIMARY KEY ,
	[Name] VARCHAR(50) NOT NULL,
	Age INT);

CREATE TABLE Towns(
	Id INT NOT NULL PRIMARY KEY,
	[Name] VARCHAR(50) NOT NULL);

select* from Towns
ALTER TABLE Minions
ADD TownId int NOT NULL
select *from Minions

/*Alter Minions*/
ALTER TABLE Minions
ADD FOREIGN KEY (TownId) REFERENCES Towns(Id) 

/*Insert in both tables*/
INSERT INTO Towns ( Id, [Name])
VALUES
(1, 'Sofia'),
(2, 'Plovdiv'),
(3, 'Varna');

INSERT INTO Minions
	(Id, [Name], Age, TownId)
	VALUES
	(1, 'Kevin', 22, 1),
	(2, 'Bob', 15, 3),
	(3, 'Steward', NULL, 2);

/*Truncate Minions*/
TRUNCATE TABLE Minions

/*Drop All Tables
SELECT  'DROP TABLE [' + name + '];'
FROM    sys.tables*/
DROP TABLE [Minions];
DROP TABLE [Towns];

CREATE TABLE People
INSERT INTO Teachers VALUES
('Ruthanne','Bamb',	'84948 Mesta Junction',	'3105500146',	6),
('Gerrard',	'Lowin',	'370 Talisman Plaza',	'3324874824',	2),
('Merrile',	'Lambdin',	'81 Dahle Plaza',	'4373065154',	5),
('Bert',  	'Ivie',	'2 Gateway Circle',	'4409584510',	4)

INSERT INTO Subjects VALUES
('Geometry',	12),
('Health',	10),
('Drama	',7),
('Sports',	9)

--Make all grades 6.00, where the subject id is 1 or 2, if the grade is above or equal to 5.50
UPDATE StudentsSubjects
SET GRADE = 6.00 WHERE SubjectId IN (1,2) AND Grade>=5.50
SELECT * FROM StudentsSubjects WHERE SubjectId IN (1,2) AND Grade>=5.50

--Delete all teachers, whose phone number contains ‘72’.

DELETE FROM StudentsTeachers WHERE (TeacherId IN (SELECT Id FROM Teachers WHERE Phone LIKE '%72%'))
DELETE FROM Teachers WHERE Phone LIKE '%72%'

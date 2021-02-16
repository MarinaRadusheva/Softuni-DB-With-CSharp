--Select all students who are teenagers (their age is above or equal to 12). Order them by first name (alphabetically), then by last name (alphabetically). Select their first name, last name and their age.
SELECT FirstName, LastName, Age FROM Students WHERE AGE >=12
ORDER BY FirstName ASC, LastName ASC

--Select all students and the count of teachers each one has. 
SELECT S.FirstName, S.LastName, COUNT(T.TeacherId) FROM Students S
JOIN StudentsTeachers T ON S.Id = T.StudentId
GROUP BY S.Id, S.FirstName, S.LastName
ORDER BY S.LastName

--Find all students, who have not attended an exam. Select their full name (first name + last name).
--Order the results by full name (ascending).
SELECT FirstName + ' ' + LastName AS FULNAME FROM Students WHERE Id NOT IN(SELECT StudentId FROM StudentsExams)
ORDER BY FULNAME

----Find top 10 students, who have highest average grades from the exams.
--Format the grade, two symbols after the decimal point.
--Order them by grade (descending), then by first name (ascending), then by last name 

SELECT TOP(10) S.FirstName, S.LastName, CAST(AVG(E.Grade) AS DECIMAL(10,2)) AS GradeAVG FROM Students S
JOIN StudentsExams E ON S.Id = E.StudentId
GROUP BY S.Id, S.FirstName, S.LastName
ORDER BY GradeAVG DESC, S.FirstName ASC, S.LastName ASC

----Find all students who don’t have any subjects. Select their full name. The full name is combination of first name, middle name and last name. Order the result by full name
--NOTE: If the middle name is null you have to concatenate the first name and last name separated with single space.

SELECT S.FirstName +' '+ IIF(S.MiddleName IS NULL, '', S.MiddleName +' ')+S.LastName AS FullName FROM Students S
LEFT JOIN StudentsSubjects SS ON S.Id = SS.StudentId
WHERE SS.SubjectId IS NULL
ORDER BY FullName

--Find the average grade for each subject. Select the subject name and the average grade. 
--Sort them by subject id (ascending).

SELECT S.Name, AVG(SS.Grade) FROM Subjects S
JOIN StudentsSubjects SS ON S.Id = SS.SubjectId
GROUP BY S.Id, S.Name
ORDER BY S.Id ASC


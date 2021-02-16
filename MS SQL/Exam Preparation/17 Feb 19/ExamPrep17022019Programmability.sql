--Create a user defined function, named udf_ExamGradesToUpdate(@studentId, @grade), that receives a student id and grade.
--The function should return the count of grades, for the student with the given id, which are above the received grade and under the received grade with 0.50 added (example: you are given grade 3.50 and you have to find all grades for the provided student which are between 3.50 and 4.00 inclusive):
--If the condition is true, you must return following message in the format:
--“You have to update {count} grades for the student {student first name}”
--If the provided student id is not in the database the function should return “The student with provided id does not exist in the school!”
--If the provided grade is above 6.00 the function should return “Grade cannot be above 6.00!”

CREATE FUNCTION udf_ExamGradesToUpdate (@studentId INT, @grade DECIMAL(10,2))
RETURNS VARCHAR(MAX)
AS
BEGIN
	IF NOT EXISTS (SELECT * FROM Students WHERE Id = @studentId)
	RETURN 'The student with provided id does not exist in the school!'
	IF @grade>6
	RETURN 'Grade cannot be above 6.00!'
	DECLARE @count INT = (SELECT COUNT(Grade) FROM StudentsSubjects WHERE StudentId = @studentId AND (Grade>@grade AND Grade <= @grade+0.5))
	DECLARE @name NVARCHAR(MAX) = (SELECT FirstName FROM Students WHERE Id = @studentId)
	RETURN CONCAT('You have to update ', @count, ' grades for the student ', @name)

END
GO
SELECT dbo.udf_ExamGradesToUpdate(12, 6.20)
SELECT dbo.udf_ExamGradesToUpdate(12, 5.50)
SELECT dbo.udf_ExamGradesToUpdate(121, 5.50)

--Create a user defined stored procedure, named usp_ExcludeFromSchool(@StudentId), that receives a student id and attempts to delete the current student. A student will only be deleted if all of these conditions pass:
--If the student doesn’t exist, then it cannot be deleted. Raise an error with the message “This school has no student with the provided id!”
--If all the above conditions pass, delete the student and ALL OF HIS REFERENCES!

CREATE OR ALTER PROC usp_ExcludeFromSchool (@StudentId INT)
AS
	IF NOT EXISTS (SELECT * FROM Students WHERE Id = @StudentId)
		THROW 50001, 'This school has no student with the provided id!', 1
	DELETE FROM StudentsTeachers WHERE StudentId = @StudentId
	DELETE FROM StudentsSubjects WHERE StudentId = @StudentId
	DELETE FROM StudentsExams WHERE StudentId = @StudentId
	DELETE FROM Students WHERE Id = @StudentId
EXEC DBO.usp_ExcludeFromSchool 1
SELECT COUNT(*) FROM Students

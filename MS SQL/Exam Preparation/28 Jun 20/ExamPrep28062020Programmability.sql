--Create a user defined function with the name dbo.udf_GetColonistsCount(PlanetName VARCHAR (30)) that receives planet name and returns the count of all colonists sent to that planet.
CREATE FUNCTION dbo.udf_GetColonistsCount(@PlanetName VARCHAR (30))
RETURNS INT
AS
BEGIN
DECLARE @count INT = (SELECT COUNT(C.Id) FROM Planets P
JOIN Spaceports SP ON P.Id = SP.PlanetId
JOIN Journeys J ON SP.Id = J.DestinationSpaceportId
JOIN TravelCards TC ON J.Id = TC.JourneyId
JOIN Colonists C ON TC.ColonistId = C.Id
WHERE P.Name = @PlanetName)
RETURN @count
END

--Create a user defined stored procedure, named usp_ChangeJourneyPurpose(@JourneyId, @NewPurpose), that receives an journey id and purpose, and attempts to change the purpose of that journey. An purpose will only be changed if all of these conditions pass:
--If the journey id doesn’t exists, then it cannot be changed. Raise an error with the message “The journey does not exist!”
--If the journey has already that purpose, raise an error with the message “You cannot change the purpose!”
--If all the above conditions pass, change the purpose of that journey.

CREATE PROC usp_ChangeJourneyPurpose(@JourneyId INT, @NewPurpose VARCHAR(50))
AS
	IF NOT EXISTS (SELECT * FROM Journeys WHERE Id = @JourneyId)
	 THROW 50001, 'The journey does not exist!', 1;
	IF ((SELECT Purpose FROM Journeys WHERE Id = @JourneyId) = @NewPurpose)
	THROW 50002, 'You cannot change the purpose!', 1;
	UPDATE Journeys
	SET Purpose = @NewPurpose WHERE Id = @JourneyId

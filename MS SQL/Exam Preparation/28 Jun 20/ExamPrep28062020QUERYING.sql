--Extract from the database, all Military journeys in the format "dd-MM-yyyy". Sort the results ascending by journey start.
--Id JourneyStart JourneyEnd

SELECT Id, CONVERT(VARCHAR, JourneyStart, 103), CONVERT(VARCHAR, JourneyEnd, 103) FROM Journeys WHERE Purpose = 'MILITARY'
ORDER BY JourneyStart ASC

--Extract from the database all colonists, which have a pilot job. Sort the result by id, ascending.
-- ID, FULL NAME

SELECT C.Id, C.FirstName+' '+C.LastName FROM Colonists C
JOIN TravelCards T ON C.Id = T.ColonistId
WHERE T.JobDuringJourney = 'PILOT'
ORDER BY C.Id ASC

----Count all colonists that are on technical journey. SELECT Count
SELECT COUNT(*) FROM Colonists C
JOIN TravelCards T ON C.Id = T.ColonistId
JOIN Journeys J ON T.JourneyId = J.Id
WHERE J.Purpose = 'TECHNICAL'

--Extract from the database those spaceships, which have pilots, younger than 30 years old. In other words, 30 years from 01/01/2019. Sort the results alphabetically by spaceship name.
--Required Columns Name, Manufacturer

SELECT S.Name, S.Manufacturer FROM Spaceships S
JOIN Journeys J ON J.SpaceshipId = S.Id
JOIN TravelCards T ON T.JourneyId = J.Id
JOIN Colonists C ON C.Id = T.ColonistId
WHERE T.JobDuringJourney = 'PILOT' AND C.BirthDate>'01/01/1989'
ORDER BY S.Name ASC

--Extract from the database all planets’ names and their journeys count. Order the results by journeys count, descending and by planet name ascending.
--Required Columns PlanetName, JourneysCount

SELECT PL.Name, COUNT(J.Id) AS JourneyCount FROM Planets PL
JOIN Spaceports P ON PL.Id = P.PlanetId
JOIN Journeys J ON J.DestinationSpaceportId = P.Id 
GROUP BY PL.Id, PL.Name
ORDER BY JourneyCount DESC, PL.Name ASC

--Find all colonists and their job during journey with rank 2. Keep in mind that all the selected colonists with rank 2 must be the oldest ones. You can use ranking over their job during their journey.
--Required Columns  JobDuringJourney  FullName  JobRank

SELECT* FROM(SELECT T.JobDuringJourney, C.FirstName +' '+C.LastName AS FullName, DENSE_RANK() OVER (PARTITION BY T.JobDuringJourney ORDER BY C.BirthDate) AS JobRank FROM Colonists C
JOIN TravelCards T ON C.Id = T.ColonistId
JOIN Journeys J ON T.JourneyId = J.Id) AS K
WHERE JobRank =2
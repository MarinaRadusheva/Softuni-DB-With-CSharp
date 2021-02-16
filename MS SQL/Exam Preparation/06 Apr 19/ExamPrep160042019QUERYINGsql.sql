--Select all of the planes, which name contains "tr". Order them by id (ascending), name (ascending), seats (ascending) and range (ascending).

SELECT * FROM Planes WHERE [Name] LIKE '%TR%'
ORDER BY Id ASC, Name ASC, Seats ASC, Range ASC

--Select the total profit for each flight from database. Order them by total price (descending), flight id (ascending).
SELECT F.Id, SUM(T.Price) AS Price FROM Flights F
JOIN Tickets T ON F.Id = T.FlightId
GROUP BY F.Id
ORDER BY Price DESC, F.Id ASC

--Select the full name of the passengers with their trips (origin - destination). Order them by full name (ascending), origin (ascending) and destination (ascending).

SELECT CONCAT(P.FirstName, ' ', P.LastName) AS FullName, F.Origin, F.Destination FROM Passengers P
JOIN Tickets T ON P.Id = T.PassengerId
JOIN Flights F ON T.FlightId = F.Id
ORDER BY FullName ASC, Origin ASC, Destination ASC

--Select all people who don't have tickets. Select their first name, last name and age .Order them by age (descending), first name (ascending) and last name (ascending).
SELECT P.FirstName, P.LastName, P.Age FROM Passengers P
LEFT JOIN Tickets T ON P.Id = T.PassengerId
WHERE T.Id IS NULL
ORDER BY P.Age DESC, P.FirstName ASC, P.LastName ASC

--Select all passengers who have trips. Select their full name (first name – last name), plane name, trip (in format {origin} - {destination}) and luggage type. Order the results by full name (ascending), name (ascending), origin (ascending), destination (ascending) and luggage type (ascending).

SELECT CONCAT(P.FirstName, ' ', P.LastName) AS FullName,
	PL.[Name],
	CONCAT(F.Origin, ' - ', F.Destination) AS [Trip],
	LT.Type
FROM Passengers P
JOIN Tickets T ON P.Id = T.PassengerId
JOIN Flights F ON F.Id = T.FlightId
JOIN Planes PL ON F.PlaneId = PL.Id
JOIN Luggages L ON T.LuggageId = L.Id
JOIN LuggageTypes LT ON L.LuggageTypeId = LT.Id
ORDER BY FullName ASC, PL.Name ASC, F.Origin ASC, F.Destination ASC, LT.Type ASC

--Select all planes with their name, seats count and passengers count. Order the results by passengers count (descending), plane name (ascending) and seats (ascending) 

SELECT P.[Name], P.Seats, COUNT(T.Id) AS PassengerCount FROM Planes P
LEFT JOIN Flights F ON F.PlaneId = P.Id
LEFT JOIN Tickets T ON T.FlightId = F.Id
GROUP BY P.Id, P.[Name], P.Seats
ORDER BY PassengerCount DESC, P.[Name] ASC, P.Seats ASC


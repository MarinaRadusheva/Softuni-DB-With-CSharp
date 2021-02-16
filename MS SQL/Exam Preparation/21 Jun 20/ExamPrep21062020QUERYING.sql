--Select accounts whose emails start with the letter “e”. Select their first and last name, their birthdate in the format "MM-dd-yyyy", their city name, and their Email.
--Order them by city name (ascending)

SELECT FirstName, LastName, CONVERT(VARCHAR, BirthDate, 110), C.Name, A.Email FROM Accounts A
JOIN Cities C ON A.CityId = C.Id WHERE Email LIKE 'E%'
ORDER BY C.Name ASC

--Select all cities with the count of hotels in them. Order them by the hotel count (descending), then by city name. Do not include cities, which have no hotels in them.
SELECT C.Name, COUNT(H.Id) AS HOTELCOUNT FROM CITIES C JOIN Hotels H ON C.Id = H.CityId
GROUP BY C.Id, C.Name
ORDER BY HOTELCOUNT DESC, C.Name

--Find the longest and shortest trip for each account, in days. Filter the results to accounts with no middle name and trips, which are not cancelled (CancelDate is null).
--Order the results by Longest Trip days (descending), then by Shortest Trip (ascending).

SELECT Id, FullName, MAX(TripDays) AS [Longest], MIN(TripDays) AS [Shortest] FROM (SELECT A.Id,
	A.FirstName+' '+A.LastName AS FullName,
	DATEDIFF(DAY, ArrivalDate, ReturnDate) AS TripDays
FROM Accounts A
JOIN AccountsTrips ACT ON A.Id = ACT.AccountId
JOIN Trips T ON T.Id = ACT.TripId
WHERE A.MiddleName IS NULL AND T.CancelDate IS NULL) AS K
GROUP BY Id, FullName
ORDER BY Longest DESC, Shortest ASC

--Find the top 10 cities, which have the most registered accounts in them. Order them by the count of accounts (descending).

SELECT TOP (10) C.Id, C.Name, C.CountryCode, COUNT(A.Id) AS ACount FROM Cities C
JOIN Accounts A ON C.Id = A.CityId
GROUP BY C.Id, C.Name, C.CountryCode
ORDER BY ACount DESC

--Find all accounts, which have had one or more trips to a hotel in their hometown.
--Order them by the trips count (descending), then by Account ID.

SELECT Id, Email, [Name], COUNT(*) AS [Count] FROM (SELECT A.Id AS Id, A.Email AS Email, C.Name AS[Name] FROM Accounts A
JOIN AccountsTrips ATR ON A.Id = ATR.AccountId
JOIN Trips T ON T.Id = ATR.TripId
JOIN Rooms R ON R.Id = T.RoomId
JOIN Hotels H ON H.Id = R.HotelId
JOIN Cities C ON C.Id = H.CityId
WHERE A.CityId = H.CityId ) AS K
GROUP BY Id, Email, [Name]
ORDER BY [Count] DESC, Id

--Retrieve the following information about each trip:
--Trip ID
--Account Full Name
--From – Account hometown
--To – Hotel city
--Duration – the duration between the arrival date and return date in days. If a trip is cancelled, the value is “Canceled”
--Order the results by full name, then by Trip ID.

SELECT T.Id,
		CONCAT(A.FirstName, ' ', IIF(A.MiddleName IS NULL, '', A.MiddleName +' '), A.LastName) AS [FullName],
		C.Name AS [From],
		CT.Name AS [To],
		CASE WHEN T.CancelDate IS NOT NULL THEN 'Canceled'
		ELSE CAST((DATEDIFF(DAY, T.ArrivalDate, T.ReturnDate)) AS VARCHAR(50)) + ' days'
		END AS [Duration]
FROM Trips T
JOIN AccountsTrips ATR ON T.Id = ATR.TripId
JOIN Accounts A ON ATR.AccountId = A.Id
JOIN Cities C ON C.Id = A.CityId
JOIN Rooms R ON R.Id = T.RoomId
JOIN Hotels H ON H.Id = R.HotelId
JOIN Cities CT ON CT.Id = H.CityId
ORDER BY FullName,T.Id

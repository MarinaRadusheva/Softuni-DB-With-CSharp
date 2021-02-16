--Write a query that selects: CountryCode, MountainRange, PeakName, Elevation. Filter all peaks in Bulgaria with elevation over 2835. Return all the rows sorted by elevation in descending order.

SELECT MC.CountryCode, M.MountainRange, P.PeakName, P.Elevation 
	FROM Peaks P
	JOIN Mountains M ON P.MountainId = M.Id
	JOIN MountainsCountries MC ON M.Id=MC.MountainId
	WHERE MC.CountryCode='BG' AND P.Elevation>2835
	ORDER BY P.Elevation DESC

--Write a query that selects: CountryCode, MountainRanges. Filter the count of the mountain ranges in the United States, Russia and Bulgaria.
SELECT CountryCode, COUNT(MC.MountainId) AS MountainRanges FROM MountainsCountries MC
WHERE MC.CountryCode IN ('BG', 'RU', 'US') 
GROUP BY CountryCode

--Write a query that selects: CountryName, RiverName. Find the first 5 countries with or without rivers in Africa. Sort them by CountryName in ascending order

SELECT *  FROM CountriesRivers
SELECT TOP(5) C.CountryName, R.RiverName 
	FROM Countries C
	LEFT JOIN CountriesRivers CR ON C.CountryCode = CR.CountryCode
	LEFT JOIN Rivers R ON R.Id = CR.RiverId
	WHERE C.ContinentCode='AF'
	ORDER BY C.CountryName ASC

--Write a query that selects: ContinentCode, CurrencyCode, CurrencyUsage. Find all continents and their most used currency. Filter any currency that is used in only one country. Sort your results by ContinentCode.

SELECT ContinentCode, CurrencyCode, CurrencyUsage FROM
	(SELECT K.ContinentCode, 
		K.CurrencyCode, 
		COUNT(K.CurrencyCode) AS CurrencyUsage,
		DENSE_RANK() OVER (PARTITION BY K.ContinentCode ORDER BY COUNT(K.CurrencyCode) DESC) AS [Rank]
		FROM
			(SELECT CN.ContinentCode, C.CurrencyCode
					FROM Continents CN
					RIGHT JOIN Countries C ON C.ContinentCode = CN. ContinentCode
			) AS K
	GROUP BY K.ContinentCode, K.CurrencyCode) AS R
	WHERE Rank=1 AND CurrencyUsage>1
	ORDER BY ContinentCode 

--OPTIMISED
SELECT ContinentCode, 
		CurrencyCode, 
		CurrencyUsage 
		FROM
		(SELECT C.ContinentCode, 
				C.CurrencyCode, 
				COUNT(C.CurrencyCode) AS CurrencyUsage,
				DENSE_RANK() OVER (PARTITION BY C.ContinentCode ORDER BY COUNT(C.CurrencyCode) DESC) AS [Rank]
			FROM Countries C
			GROUP BY C.ContinentCode, C.CurrencyCode
		) AS K
	WHERE [Rank]=1 AND CurrencyUsage>1
	ORDER BY ContinentCode 

	

--Find all the count of all countries, which don’t have a mountain.


SELECT COUNT(C.ContinentCode) AS [Count]
	FROM Countries	C
	LEFT JOIN MountainsCountries M ON C.CountryCode = M.CountryCode	
	WHERE M.CountryCode IS NULL

--For each country, find the elevation of the highest peak and the length of the longest river, sorted by the highest peak elevation (from highest to lowest), then by the longest river length (from longest to smallest), then by country name (alphabetically). Display NULL when no data is available in some of the columns. Limit only the first 5 rows.

--SELECT C.CountryName, MAX(P.Elevation) AS HighestPeakElevation
--	FROM Peaks P
--	JOIN Mountains M ON P.MountainId = M.Id
--	JOIN MountainsCountries CM ON M.Id = CM.MountainId
--	JOIN Countries C ON C.CountryCode = CM.CountryCode
--	GROUP BY C.CountryName

--	SELECT * FROM Peaks P
--	LEFT JOIN MountainsCountries MC ON P.MountainId = MC.MountainId
--	LEFT JOIN Countries C ON C.CountryCode = MC.CountryCode
--	WHERE C.CountryCode = 'BG'

SELECT TOP(5) C.CountryName, MAX(P.Elevation) AS HighestPeakElevation, MAX(R.Length) AS LongestRiverLength
	FROM Countries C
	LEFT JOIN MountainsCountries MC ON C.CountryCode = MC. CountryCode
	LEFT JOIN Mountains M ON MC.MountainId = M.Id
	LEFT JOIN Peaks P ON P.MountainId = M.Id
	LEFT JOIN CountriesRivers RC ON C.CountryCode = RC.CountryCode
	LEFT JOIN Rivers R ON RC.RiverId = R.Id
	GROUP BY C.CountryName
	ORDER BY HighestPeakElevation DESC, LongestRiverLength DESC, CountryName ASC

--For each country, find the name and elevation of the highest peak, along with its mountain. When no peaks are available in some country, display elevation 0, "(no highest peak)" as peak name and "(no mountain)" as mountain name. When multiple peaks in some country have the same elevation, display all of them. Sort the results by country name alphabetically, then by highest peak name alphabetically. Limit only the first 5 rows.
WITH CTE_ALLPEAKS (Country, PeakName, Elevation, MountainRange, Ranked)  
AS (
	SELECT C.CountryName, P.PeakName, P.Elevation, M.MountainRange,
	DENSE_RANK() OVER (PARTITION BY C.CountryName ORDER BY P.Elevation DESC) AS RANKED
	FROM Countries C
	LEFT JOIN MountainsCountries MC ON MC.CountryCode = C.CountryCode
	LEFT JOIN Peaks P ON P.MountainId = MC.MountainId
	LEFT JOIN Mountains M ON M.Id = MC.MountainId
	)
SELECT TOP(5) I.Country, 
		CASE WHEN I.PeakName IS NULL THEN '(no highest peak)' ELSE I.PeakName END AS [Highest Peak Name],
		CASE WHEN I.Elevation IS NULL THEN 0 ELSE I.Elevation END AS [Highest Peak Elevation],
		CASE WHEN I.MountainRange IS NULL THEN '(no mountain)' ELSE I.MountainRange END AS [MountainRange]
		FROM CTE_ALLPEAKS AS I
		WHERE I.Ranked=1
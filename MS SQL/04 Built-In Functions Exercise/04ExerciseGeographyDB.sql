USE Geography
--Find all countries that holds the letter 'A' in their name at least 3 times (case insensitively), sorted by ISO code. Display the country name and ISO code. 
SELECT CountryName, IsoCode  
	FROM Countries
	WHERE CountryName LIKE '%A%A%A%'
	ORDER BY IsoCode

--Combine all peak names with all river names, so that the last letter of each peak name is the same as the first letter of its corresponding river name. Display the peak names, river names, and the obtained mix (mix should be in lowercase). Sort the results by the obtained mix.

SELECT PeakName, RiverName, LOWER(PeakName+SUBSTRING(RiverName,2,LEN(RiverName)-1)) AS [Mix] 
	FROM
	(SELECT P.PeakName, R.RiverName FROM Peaks P
	JOIN Rivers R ON RIGHT(P.PeakName,1) = LEFT(R.RiverName, 1) ) AS NAMES
	ORDER BY Mix;


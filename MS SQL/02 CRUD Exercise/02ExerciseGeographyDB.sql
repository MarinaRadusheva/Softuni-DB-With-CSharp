--Display all mountain peaks in alphabetical order.
SELECT PeakName FROM Peaks
	ORDER BY PeakName
--30 biggest countries by population from Europe. Display the country name and population. Sort the results by population (from biggest to smallest), then by country alphabetically.

SELECT TOP(30) CountryName, [Population] 
	FROM Countries 
	WHERE ContinentCode='eu'
	ORDER BY Population DESC, CountryName;

--Display the country name, country code and information about its currency: either "Euro" or "Not Euro". Sort the results by country name alphabetically.

SELECT CountryName, 
		CountryCode, 
		CASE WHEN CurrencyCode='EUR' THEN 'Euro' ELSE 'Not Euro' END AS [Currency]  
		FROM Countries
		ORDER BY CountryName;
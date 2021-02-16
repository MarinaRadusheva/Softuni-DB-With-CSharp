--USE Geography
SELECT M.MountainRange, P.PeakName, P.Elevation FROM Peaks P
JOIN Mountains M ON M.Id = P.MountainId
WHERE M.MountainRange='Rila'
ORDER BY P.Elevation DESC;

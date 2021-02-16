INSERT INTO Planes VALUES
('Airbus 336',	112,	5132),
('Airbus 330',	432,	5325),
('Boeing 369',	231,	2355),
('Stelt 297	',  254,	2143),
('Boeing 338',	165,	5111),
('Airbus 558',	387,	1342),
('Boeing 128',	345,	5541)

INSERT INTO LuggageTypes VALUES
('Crossbody Bag'),
('School Backpack'),
('Shoulder Bag')

--Make all flights to "Carlsbad" 13% more expensive.
UPDATE Tickets
SET Price*=1.13 WHERE FlightId = (SELECT F.Id FROM Flights  F 
JOIN Tickets T ON F.Id = T.FlightId WHERE Destination = 'Carlsbad')

SELECT T.Price FROM Flights F 
JOIN Tickets T ON F.Id = T.FlightId WHERE Destination = 'Carlsbad'

--Delete all flights to "Ayn Halagim".
DELETE FROM Tickets WHERE FlightId = (SELECT Id FROM Flights WHERE Destination = 'Ayn Halagim')

DELETE FROM Flights WHERE Destination = 'Ayn Halagim'

--Select all of the planes, which name contains "tr". Order them by id (ascending), name (ascending), seats (ascending) and range (ascending).

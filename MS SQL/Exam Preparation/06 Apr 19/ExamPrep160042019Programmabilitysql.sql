--Create a user defined function, named udf_CalculateTickets(@origin, @destination, @peopleCount) that receives an origin (town name), destination (town name) and people count.
--The function must return the total price in format "Total price {price}"
--If people count is less or equal to zero return – "Invalid people count!"
--If flight is invalid return – "Invalid flight!"
CREATE FUNCTION udf_CalculateTickets(@origin VARCHAR(50), @destination VARCHAR(50), @peopleCount INT)
RETURNS VARCHAR(MAX)
BEGIN
DECLARE @totalPrice DECIMAL(18,2)
IF(@peopleCount<=0)
	RETURN 'Invalid people count!'
ELSE IF NOT EXISTS (SELECT * FROM Flights WHERE Origin = @origin AND Destination = @destination)
	RETURN 'Invalid flight!'
ELSE
	BEGIN
	DECLARE @ticketPrice DECIMAL(18,2) = (SELECT T.Price FROM Flights F 
	JOIN Tickets T ON F.Id = T.FlightId  WHERE Origin = @origin AND Destination = @destination)
    END
SET @totalPrice = @peopleCount*@ticketPrice
RETURN CONCAT('Total price ', @totalPrice)
END

GO
SELECT dbo.udf_CalculateTickets('Kolyshley','Rancabolang', 33)
SELECT dbo.udf_CalculateTickets('Kolyshley','Rancabolang', -1)

--Create a user defined stored procedure, named usp_CancelFlights
--The procedure must cancel all flights on which the arrival time is before the departure time. Cancel means you need to leave the departure and arrival time empty.
CREATE PROC usp_CancelFlights
AS
UPDATE Flights
SET DepartureTime = NULL, ArrivalTime = NULL
WHERE ArrivalTime>DepartureTime

GO
EXEC DBO.usp_CancelFlights

SELECT * FROM Flights WHERE ArrivalTime<DepartureTime

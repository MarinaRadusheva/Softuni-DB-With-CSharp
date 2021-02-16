--Create a user defined function, named udf_GetAvailableRoom(@HotelId, @Date, @People), that receives a hotel ID, a desired date, and the count of people that are going to be signing up.
--The total price of the room can be calculated by using this formula:
--	(HotelBaseRate + RoomPrice) * PeopleCount
-- The function should find a suitable room in the provided hotel, based on these conditions:
-- The room must not be already occupied. A room is occupied if the date the customers want to book is between the arrival and return dates of a trip to that room and the trip is not canceled.
-- The room must be in the provided hotel.
--	The room must have enough beds for all the people.
--If any rooms in the desired hotel satisfy the customers’ conditions, find the highest priced room (by total price) of all of them and provide them with that room.
--The function must return a message in the format:
--	“Room {Room Id}: {Room Type} ({Beds} beds) - ${Total Price}”
--If no room could be found, the function should return “No rooms available”.
--112, '2011-12-17', 2
GO
CREATE FUNCTION udf_GetAvailableRoom(@HotelId INT, @Date DATE, @People INT)
RETURNS VARCHAR(MAX)
AS
BEGIN
IF NOT EXISTS (SELECT * FROM Rooms R
		JOIN Trips T ON R.Id = T.RoomId
		JOIN Hotels H ON H.Id = R.HotelId 
		WHERE (HotelId = @HotelId AND Beds>=@People) AND 
				NOT EXISTS (SELECT * FROM Trips T WHERE T.RoomId = R.Id AND (@Date BETWEEN T.ArrivalDate AND T.ReturnDate) AND T.CancelDate IS NULL))
RETURN 'No rooms available';
ELSE
DECLARE @output VARCHAR(MAX) = (SELECT TOP(1) 'Room ' + CAST(R.Id AS VARCHAR) +': ' + CAST(R.Type AS VARCHAR) + ' ('+ CAST(R.Beds AS VARCHAR) + ' beds) - $' + CAST(((H.BaseRate+R.Price)*@People) AS VARCHAR)
	FROM Rooms R
		JOIN Trips T ON R.Id = T.RoomId
		JOIN Hotels H ON H.Id = R.HotelId 
		WHERE (HotelId = @HotelId AND Beds>=@People) AND 
				NOT EXISTS (SELECT * FROM Trips T WHERE T.RoomId = R.Id AND (@Date BETWEEN T.ArrivalDate AND T.ReturnDate) AND T.CancelDate IS NULL)
ORDER BY (H.BaseRate+R.Price)*@People DESC)

RETURN @output

END
GO


SELECT dbo.udf_GetAvailableRoom(112, '2011-12-17', 2)
SELECT dbo.udf_GetAvailableRoom(94, '2015-07-26', 3)

--Create a user defined stored procedure, named usp_SwitchRoom(@TripId, @TargetRoomId), that receives a trip and a target room, and attempts to move the trip to the target room. A room will only be switched if all of these conditions are true:
-- If the target room ID is in a different hotel, than the trip is in, raise an exception with the message “Target room is in another hotel!”.
-- If the target room doesn’t have enough beds for all the trip’s accounts, raise an exception with the message “Not enough beds in target room!”.
--If all the above conditions pass, change the trip’s room ID to the target room ID.

CREATE PROC usp_SwitchRoom(@TripId INT, @TargetRoomId INT)
AS
	IF 
	(SELECT R.HotelId FROM Trips T
	JOIN Rooms R ON R.Id = T.RoomId
	WHERE T.Id= 10) != (SELECT HotelId FROM Rooms WHERE Id = @TargetRoomId)
	THROW 50001, 'Target room is in another hotel!', 1
	IF ( SELECT COUNT(AccountId) FROM AccountsTrips T
			WHERE T.TripId = @TripId) > (SELECT Beds FROM Rooms WHERE Id = @TargetRoomId)
	THROW 50002, 'Not enough beds in target room!', 1
	UPDATE Trips
	SET RoomId = @TargetRoomId WHERE Id = @TripId

EXEC usp_SwitchRoom 10, 11
SELECT RoomId FROM Trips WHERE Id = 10
EXEC usp_SwitchRoom 10, 7
EXEC usp_SwitchRoom 10, 8
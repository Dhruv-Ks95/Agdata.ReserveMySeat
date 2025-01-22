CREATE PROCEDURE [dbo].[GetAvailableSeatsOnDate]
    @BookingDate DATE
AS
BEGIN

    SELECT 
        s.SeatId,
        s.SeatNumber
    FROM [dbo].[Seats] s
    WHERE s.SeatId NOT IN (
        SELECT b.SeatId
        FROM [dbo].[Bookings] b
        WHERE CAST(b.BookingDate AS DATE) = @BookingDate
    )
    ORDER BY s.SeatNumber ASC;
END;
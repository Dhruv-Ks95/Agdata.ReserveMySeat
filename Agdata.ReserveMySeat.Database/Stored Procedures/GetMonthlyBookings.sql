CREATE PROCEDURE [dbo].[GetMonthlyBookings]
AS
BEGIN

    SELECT 
        BookingId,
        EmployeeId,
        SeatId,
        BookingDate
    FROM [dbo].[Bookings]
    WHERE BookingDate >= CAST(GETDATE() AS DATE)
      AND BookingDate < DATEADD(DAY, 30, CAST(GETDATE() AS DATE))
    ORDER BY BookingDate ASC;
END;
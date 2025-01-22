CREATE TABLE [dbo].[Bookings] (
    [BookingId] INT IDENTITY (1, 1) NOT NULL,
    [EmployeeId] INT NOT NULL,
    [SeatId] INT NOT NULL,
    [BookingDate] DATETIME2 (7) NOT NULL,
    CONSTRAINT [PK_Bookings] PRIMARY KEY CLUSTERED ([BookingId] ASC),
    CONSTRAINT [FK_Bookings_Employees_EmployeeId] FOREIGN KEY ([EmployeeId]) REFERENCES [dbo].[Employees] ([EmployeeId]) ON DELETE CASCADE,
    CONSTRAINT [FK_Bookings_Seats_SeatId] FOREIGN KEY ([SeatId]) REFERENCES [dbo].[Seats] ([SeatId]) ON DELETE CASCADE
);
namespace Agdata.ReserveMySeat.Domain.DTOs;
public record AddBookingRequestDto
{
    public int EmployeeId { get; init; }
    public int SeatId { get; init; }
    public DateTime BookingDate { get; init; }
}

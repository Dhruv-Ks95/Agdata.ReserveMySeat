namespace Agdata.ReserveMySeat.Domain.DTOs;
public record UpdateBookingRequestDto
{
    public int BookingId { get; init; }
    public int EmployeeId { get; init; }
    public int SeatId { get; init; }
    public DateTime BookingDate { get; init; }
}

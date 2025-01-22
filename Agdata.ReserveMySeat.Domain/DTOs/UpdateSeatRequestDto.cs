namespace Agdata.ReserveMySeat.Domain.DTOs;
public record UpdateSeatRequestDto
{
    public int SeatId { get; init; }
    public int SeatNumber { get; init; }
}

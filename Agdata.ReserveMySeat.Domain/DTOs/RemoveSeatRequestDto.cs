namespace Agdata.ReserveMySeat.Domain.DTOs;

public record RemoveSeatRequestDto
{
    public int SeatId { get; init; }
    public int SeatNumber { get; init; }
}

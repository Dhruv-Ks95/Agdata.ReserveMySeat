using Agdata.ReserveMySeat.Domain.DTOs;
namespace Agdata.ReserveMySeat.Domain.Entities;
public class Seat
{
    public int SeatId { get; private set; }
    public int SeatNumber { get; private set; }

    public Seat(AddSeatRequestDto addSeatRequestDto)
    {
        ValidateAddRequest(addSeatRequestDto);
        SeatNumber = addSeatRequestDto.SeatNumber;
    }

    private Seat() { }

    public void UpdateSeat(UpdateSeatRequestDto updateSeatRequestDto)
    {
        ValidateUpdateRequest(updateSeatRequestDto);
        SeatNumber = updateSeatRequestDto.SeatNumber;
    }

    public void DeleteSeat()
    {
        // Functionalities which might depend on this entity
    }

    private void ValidateAddRequest(AddSeatRequestDto addSeatRequestDto)
    {        
        if (addSeatRequestDto.SeatNumber <= 0)
        {
            throw new ArgumentException("Seat number must be a positive integer.");
        }
        if (addSeatRequestDto.SeatNumber > 150)
        {
            throw new ArgumentException("Exceeded seat number limit");
        }
    }

    private void ValidateUpdateRequest(UpdateSeatRequestDto updateSeatRequestDto)
    {
        if (updateSeatRequestDto.SeatNumber <= 0)
        {
            throw new ArgumentException("Seat number must be a positive integer.");
        }
        if (updateSeatRequestDto.SeatNumber > 150)
        {
            throw new ArgumentException("Exceeded seat number limit");
        }
    }
}

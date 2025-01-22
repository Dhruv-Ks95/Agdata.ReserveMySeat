using Agdata.ReserveMySeat.Application.Interfaces;
using Agdata.ReserveMySeat.Domain.DTOs;
using Agdata.ReserveMySeat.Domain.Entities;

namespace Agdata.ReserveMySeat.Application.Services;
public class SeatService : ISeatService
{
    private readonly ISeatRepository seatRepository;
    public SeatService(ISeatRepository seatRepository)
    {
        this.seatRepository = seatRepository;
    }

    public async Task<SeatDto> AddAsync(AddSeatRequestDto addSeatRequestDto)
    {
        if (addSeatRequestDto == null)
        {
            throw new ArgumentNullException(nameof(addSeatRequestDto), "Add request cannot be null.");
        }
        if (addSeatRequestDto.SeatNumber <= 0)
        {
            throw new ArgumentException("Invalid seat Number provided!");
        }
        var allSeats = await seatRepository.GetAllAsync();
        if (allSeats.Any(s => s.SeatNumber == addSeatRequestDto.SeatNumber))
        {
            throw new InvalidOperationException($"Seat with SeatNumber {addSeatRequestDto.SeatNumber} already exists!");
        }
        Seat seat = new Seat(addSeatRequestDto);
        return await seatRepository.AddAsync(seat);
    }

    public async Task<IEnumerable<SeatDto>> GetAllAsync()
    {
        return await seatRepository.GetAllAsync();
    }

    public async Task<SeatDto?> GetByIdAsync(int id)
    {
        if(id < 0)
        {
            throw new ArgumentException("Invalid seat Id");
        }
        var seatDto = await seatRepository.GetByIdAsync(id);
        if (seatDto == null)
        {
            throw new KeyNotFoundException("No seat found with seat id : " + id);
        }
        return seatDto;
    }

    public async Task<IEnumerable<SeatDto>> GetAvailableCountOnDateAsync(DateTime date)
    {
        if (date < DateTime.Today.Date || date > DateTime.Today.AddDays(29).Date)
        {
            throw new ArgumentException("Invalid date");
        }
        return await seatRepository.GetAvailableCountOnDateAsync(date);
    }

    public async Task<bool> UpdateAsync(UpdateSeatRequestDto updateSeatRequestDto)
    {
        if (updateSeatRequestDto == null)
        {
            throw new ArgumentNullException(nameof(updateSeatRequestDto), "Update request cannot be null.");
        }
        Seat? seat = await seatRepository.GetByIdAsync(updateSeatRequestDto.SeatId, updateSeatRequestDto.SeatNumber);
        if (seat == null)
        {
            throw new KeyNotFoundException($"Seat with SeatId {updateSeatRequestDto.SeatId} not found!");
        }
        var allSeats = await seatRepository.GetAllAsync();
        if (allSeats.Any(s => s.SeatNumber == updateSeatRequestDto.SeatNumber && s.SeatId != updateSeatRequestDto.SeatId))
        {
            throw new InvalidOperationException($"Seat with SeatNumber {updateSeatRequestDto.SeatNumber} already exists!");
        }
        seat.UpdateSeat(updateSeatRequestDto);
        return await seatRepository.UpdateAsync(seat);
    }

    public async Task<bool> RemoveAsync(RemoveSeatRequestDto removeSeatRequestDto)
    {
        if (removeSeatRequestDto == null)
        {
            throw new ArgumentNullException(nameof(removeSeatRequestDto), "Remove request cannot be null.");
        }
        if (removeSeatRequestDto.SeatId <= 0)
        {
            throw new ArgumentException("Invalid SeatId provided.");
        }
        Seat? seat = await seatRepository.GetByIdAsync(removeSeatRequestDto.SeatId, removeSeatRequestDto.SeatNumber);
        if (seat == null)
        {
            throw new KeyNotFoundException("Could not find seat to delete!");
        }
        seat.DeleteSeat();
        return await seatRepository.RemoveAsync(seat);
    }
}

using Agdata.ReserveMySeat.Domain.DTOs;
namespace Agdata.ReserveMySeat.Application.Interfaces;
public interface ISeatService
{
    Task<SeatDto> AddAsync(AddSeatRequestDto addSeatRequestDto);
    Task<SeatDto?> GetByIdAsync(int id);
    Task<IEnumerable<SeatDto>> GetAllAsync();
    Task<IEnumerable<SeatDto>> GetAvailableCountOnDateAsync(DateTime date);
    Task<bool> UpdateAsync(UpdateSeatRequestDto updateSeatRequestDto);
    Task<bool> RemoveAsync(RemoveSeatRequestDto removeSeatRequestDto);

}

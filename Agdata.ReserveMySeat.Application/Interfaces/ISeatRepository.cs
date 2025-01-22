using Agdata.ReserveMySeat.Domain.DTOs;
using Agdata.ReserveMySeat.Domain.Entities;

namespace Agdata.ReserveMySeat.Application.Interfaces;
public interface ISeatRepository
{
    Task<SeatDto> AddAsync(Seat seat);
    Task<SeatDto?> GetByIdAsync(int id);
    Task<Seat?> GetByIdAsync(int id,int number);
    Task<IEnumerable<SeatDto>> GetAllAsync();
    Task<IEnumerable<SeatDto>> GetAvailableCountOnDateAsync(DateTime date);    
    Task<bool> UpdateAsync(Seat seat);
    Task<bool> RemoveAsync(Seat seat);
}

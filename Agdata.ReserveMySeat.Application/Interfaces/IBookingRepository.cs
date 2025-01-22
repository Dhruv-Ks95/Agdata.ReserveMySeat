using Agdata.ReserveMySeat.Domain.DTOs;
using Agdata.ReserveMySeat.Domain.Entities;

namespace Agdata.ReserveMySeat.Application.Interfaces;
public interface IBookingRepository
{
    Task<BookingDto> AddAsync(Booking booking);
    Task<BookingDto?> GetByIdAsync(int id);
    Task<Booking?> GetByIdAsync(int id,DateTime date);
    Task<IEnumerable<BookingDto>> GetByUserAsync(int userId);
    Task<IEnumerable<BookingDto>> GetByDateAsync(DateTime date);
    Task<IEnumerable<BookingDto>> GetMonthlyAsync();
    Task<bool> UpdateAsync(Booking booking);
    Task<bool> RemoveAsync(Booking booking);
}

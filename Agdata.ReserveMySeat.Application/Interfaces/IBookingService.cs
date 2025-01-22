using Agdata.ReserveMySeat.Domain.DTOs;
namespace Agdata.ReserveMySeat.Application.Interfaces;
public interface IBookingService
{
    Task<BookingDto> AddAsync(AddBookingRequestDto addBookingRequestDto);
    Task<BookingDto> GetByIdAsync(int id);
    Task<IEnumerable<BookingDto>> GetByUserIdAsync(int userId);
    Task<IEnumerable<BookingDto>> GetByDateAsync(DateTime date);
    Task<IEnumerable<BookingDto>> GetMonthlyAsync();
    Task<bool> UpdateAsync(UpdateBookingRequestDto updateBookingRequestDto);
    Task<bool> RemoveAsync(RemoveBookingRequestDto removeBookingRequestDto);    
}

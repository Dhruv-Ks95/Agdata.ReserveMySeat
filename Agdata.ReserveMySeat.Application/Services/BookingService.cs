using Agdata.ReserveMySeat.Application.Interfaces;
using Agdata.ReserveMySeat.Domain.DTOs;
using Agdata.ReserveMySeat.Domain.Entities;
namespace Agdata.ReserveMySeat.Application.Services;
public class BookingService : IBookingService
{

    private readonly IBookingRepository bookingRepository;
    private readonly ISeatService seatService;
    private readonly IEmployeeService employeeService;
    public BookingService(IBookingRepository bookingRepository,ISeatService seatService,IEmployeeService employeeService)
    {
        this.bookingRepository = bookingRepository;
        this.seatService = seatService;
        this.employeeService = employeeService;
    }

    public async Task<BookingDto> AddAsync(AddBookingRequestDto addBookingRequestDto)
    {
        // Validate DTO
        if (addBookingRequestDto == null)
        {
            throw new ArgumentNullException(nameof(addBookingRequestDto), "Booking request cannot be null.");
        }
        // Validate existance of employee and seat
        var employee = await employeeService.GetByIdAsync(addBookingRequestDto.EmployeeId);
        if (employee == null)
        { 
            throw new KeyNotFoundException($"Employee with ID {addBookingRequestDto.EmployeeId} not found.");
        }
        var selectedseat = await seatService.GetByIdAsync(addBookingRequestDto.SeatId);
        if(selectedseat == null)
        {
            throw new KeyNotFoundException($"Seat with ID {addBookingRequestDto.SeatId} not found.");
        }
        // Check availability
        var availableSeats = await seatService.GetAvailableCountOnDateAsync(addBookingRequestDto.BookingDate);
        if (!availableSeats.Any(s => s.SeatId == addBookingRequestDto.SeatId))
        {
            throw new InvalidOperationException($"Seat ID {addBookingRequestDto.SeatId} is not available on the selected date.");
        }
        Booking booking = new Booking(addBookingRequestDto);
        BookingDto bookingDto = await bookingRepository.AddAsync(booking);
        return bookingDto;
    }

    public async Task<BookingDto> GetByIdAsync(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException("ID must be a positive number.");
        }
        var bookingDto = await bookingRepository.GetByIdAsync(id);
        if (bookingDto == null)
        {
            throw new KeyNotFoundException("Booking not Found");
        }
        return bookingDto;
    }    

    public async Task<IEnumerable<BookingDto>> GetByDateAsync(DateTime date)
    {
        if (date < DateTime.Today.Date || date > DateTime.Today.AddDays(29).Date)
        {
            throw new ArgumentException("Invalid date");
        }
        return await bookingRepository.GetByDateAsync(date);
    }

    public async Task<IEnumerable<BookingDto>> GetByUserIdAsync(int userId) 
    {
        if (userId <= 0)
        {
            throw new ArgumentException("User ID must be a positive number.");
        }
        var employee = await employeeService.GetByIdAsync(userId);
        if (employee == null)
        {
            throw new KeyNotFoundException($"Employee with ID {userId} not found.");
        }
        return await bookingRepository.GetByUserAsync(userId);
    }

    public async Task<IEnumerable<BookingDto>> GetMonthlyAsync()
    {
        return await bookingRepository.GetMonthlyAsync();
    }

    public async Task<bool> UpdateAsync(UpdateBookingRequestDto updateBookingRequestDto)
    {
        if (updateBookingRequestDto == null)
        {
            throw new ArgumentNullException(nameof(updateBookingRequestDto), "Update request cannot be null.");
        }
        Booking? booking = await bookingRepository.GetByIdAsync(updateBookingRequestDto.BookingId,updateBookingRequestDto.BookingDate);
        if(booking == null)
        {
            throw new KeyNotFoundException("Could not find booking to update!");
        }
        var employee = await employeeService.GetByIdAsync(updateBookingRequestDto.EmployeeId);
        if (employee == null)
        {
            throw new KeyNotFoundException($"Employee with ID {updateBookingRequestDto.EmployeeId} not found.");
        }
        var selectedseat = await seatService.GetByIdAsync(updateBookingRequestDto.SeatId);
        if (selectedseat == null)
        {
            throw new KeyNotFoundException($"Seat with ID {updateBookingRequestDto.SeatId} not found.");
        }
        // Check availability
        var availableSeats = await seatService.GetAvailableCountOnDateAsync(updateBookingRequestDto.BookingDate);
        if (!availableSeats.Any(s => s.SeatId == updateBookingRequestDto.SeatId))
        {
            throw new InvalidOperationException($"Seat ID {updateBookingRequestDto.SeatId} is not available on the selected date.");
        }
        booking.UpdateBooking(updateBookingRequestDto);
        return await bookingRepository.UpdateAsync(booking);
    }

    public async Task<bool> RemoveAsync(RemoveBookingRequestDto removeBookingRequestDto)
    {
        if (removeBookingRequestDto == null)
        {
            throw new ArgumentNullException(nameof(removeBookingRequestDto), "Remove request cannot be null.");
        }
        if (removeBookingRequestDto.BookingId <= 0)
        {
            throw new ArgumentException("Booking ID must be a positive number.");
        }
        Booking? booking = await bookingRepository.GetByIdAsync(removeBookingRequestDto.BookingId,removeBookingRequestDto.BookingDate);
        if(booking == null)
        {
            throw new KeyNotFoundException("Could not find booking to delete!");
        }
        booking.DeleteBooking();
        return await bookingRepository.RemoveAsync(booking);
    }
}

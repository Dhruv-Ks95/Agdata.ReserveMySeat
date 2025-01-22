using Agdata.ReserveMySeat.Application.Services;
using Agdata.ReserveMySeat.Application.Interfaces;
using Agdata.ReserveMySeat.Application.Tests.Mocks;
using Agdata.ReserveMySeat.Infrastructure.Repositories;
using Agdata.ReserveMySeat.Infrastructure.Mappings;
using AutoMapper;
using Agdata.ReserveMySeat.Domain.DTOs;
using FluentAssertions;
namespace Agdata.ReserveMySeat.Application.Tests;
public class BookingServiceTests
{

    private readonly IReserveMySeatDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly BookingRepository _bookingRepository;
    private readonly SeatRepository _seatRepository;
    private readonly EmployeeRepository _employeeRepository;
    private readonly SeatService _seatService;
    private readonly EmployeeService _employeeService;
    private readonly BookingService _bookingService;

    public BookingServiceTests()
    {
        _dbContext = MockDbContext.GetDbContextWithTestData();
        var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfiles>());
        _mapper = config.CreateMapper();

        _bookingRepository = new BookingRepository(_mapper, _dbContext);
        _seatRepository = new SeatRepository(_mapper, _dbContext);
        _employeeRepository = new EmployeeRepository(_mapper, _dbContext);
        _seatService = new SeatService(_seatRepository);
        _employeeService = new EmployeeService(_employeeRepository);
        _bookingService = new BookingService(_bookingRepository, _seatService, _employeeService);
    }

    [Fact]
    public async Task AddAsync_ShouldAddBooking_WhenValidRequest()
    {
        var request = new AddBookingRequestDto
        {
            EmployeeId = 2,
            SeatId = 1,
            BookingDate = DateTime.Today.AddDays(5)
        };
        var result = await _bookingService.AddAsync(request);
        result.Should().NotBeNull();
        result.EmployeeId.Should().Be(request.EmployeeId);
        result.SeatId.Should().Be(request.SeatId);
        result.BookingDate.Should().Be(request.BookingDate);
    }

    [Fact]
    public async Task AddAsync_ShouldThrowKeyNotFoundException_WhenEmployeeDoesNotExist()
    {
        var request = new AddBookingRequestDto
        {
            EmployeeId = 999,
            SeatId = 1,
            BookingDate = DateTime.Today.AddDays(5)
        };

        Func<Task> act = async () => await _bookingService.AddAsync(request);

        await act.Should().ThrowAsync<KeyNotFoundException>().WithMessage("Employee with EmployeeId: 999 not Found!");
    }    

    [Fact]
    public async Task AddAsync_ShouldThrowKeyNotFoundException_WhenSeatDoesNotExist()
    {
        var request = new AddBookingRequestDto
        {
            EmployeeId = 2,
            SeatId = 999, // Non-existent SeatId
            BookingDate = DateTime.Today.AddDays(5)
        };
        Func<Task> act = async () => await _bookingService.AddAsync(request);
        await act.Should().ThrowAsync<KeyNotFoundException>().WithMessage("No seat found with seat id : 999");
    }

    [Fact]
    public async Task AddAsync_ShouldThrowInvalidOperationException_WhenSeatNotAvailable()
    {
        var request = new AddBookingRequestDto
        {
            EmployeeId = 2,
            SeatId = 10, // Already booked
            BookingDate = DateTime.Today.AddDays(4)
        };
        Func<Task> act = async () => await _bookingService.AddAsync(request);
        await act.Should().ThrowAsync<InvalidOperationException>().WithMessage("Seat ID 10 is not available on the selected date.");
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnBooking_WhenValidId()
    {
        var bookingId = 1; 
        var result = await _bookingService.GetByIdAsync(bookingId);
        result.Should().NotBeNull();
        result.BookingId.Should().Be(bookingId);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldThrowArgumentException_WhenInvalidId()
    {
        var bookingId = -1;
        Func<Task> act = async () => await _bookingService.GetByIdAsync(bookingId);
        await act.Should().ThrowAsync<ArgumentException>().WithMessage("ID must be a positive number.");
    }

    [Fact]
    public async Task GetByIdAsync_ShouldThrowKeyNotFoundException_WhenBookingNotFound()
    {
        var bookingId = 999; // Non-existent BookingId
        Func<Task> act = async () => await _bookingService.GetByIdAsync(bookingId);
        await act.Should().ThrowAsync<KeyNotFoundException>().WithMessage("Booking not Found");
    }

    [Fact]
    public async Task GetByDateAsync_ShouldReturnBookings_WhenValidDate()
    {
        var date = DateTime.Today.AddDays(4);
        var result = await _bookingService.GetByDateAsync(date);
        result.Should().NotBeNull();
        result.Should().HaveCountGreaterThan(0);
        result.All(b => b.BookingDate == date).Should().BeTrue();
    }

    [Fact]
    public async Task GetByDateAsync_ShouldThrowArgumentException_WhenDateIsInvalid()
    {
        var date = DateTime.Today.AddDays(-1); // Past date
        Func<Task> act = async () => await _bookingService.GetByDateAsync(date);
        await act.Should().ThrowAsync<ArgumentException>().WithMessage("Invalid date");
    }

    [Fact]
    public async Task GetByUserIdAsync_ShouldReturnBookings_WhenValidUserId()
    {
        var validUserId = 2;
        var result = await _bookingService.GetByUserIdAsync(validUserId);
        result.Should().NotBeNull().And.HaveCountGreaterThan(0);
    }

    [Fact]
    public async Task GetByUserIdAsync_ShouldThrowArgumentException_WhenUserIdIsNonPositive()
    {
        var invalidUserId = -1;
        var act = async () => await _bookingService.GetByUserIdAsync(invalidUserId);
        await act.Should().ThrowAsync<ArgumentException>().WithMessage("User ID must be a positive number.");
    }

    [Fact]
    public async Task GetByUserIdAsync_ShouldThrowKeyNotFoundException_WhenUserDoesNotExist()
    {
        var nonExistentUserId = 999; 
        var act = async () => await _bookingService.GetByUserIdAsync(nonExistentUserId);
        await act.Should().ThrowAsync<KeyNotFoundException>().WithMessage("Employee with EmployeeId: 999 not Found!");
    }

    [Fact]
    public async Task GetMonthlyAsync_ShouldReturnBookings_WhenCalled()
    {
        var result = await _bookingService.GetMonthlyAsync();
        result.Should().NotBeNull().And.HaveCountGreaterThan(0);
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnTrue_WhenValidRequest()
    {
        var validUpdateRequest = new UpdateBookingRequestDto { BookingId = 1, EmployeeId = 2, SeatId = 3, BookingDate = DateTime.Today.AddDays(1) };
        var result = await _bookingService.UpdateAsync(validUpdateRequest);
        result.Should().BeTrue();
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrowArgumentNullException_WhenRequestIsNull()
    {
        UpdateBookingRequestDto nullRequest = null;
        var act = async () => await _bookingService.UpdateAsync(nullRequest);
        await act.Should().ThrowAsync<ArgumentNullException>().WithMessage("Update request cannot be null. (Parameter 'updateBookingRequestDto')");
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrowKeyNotFoundException_WhenBookingNotFound()
    {
        var invalidRequest = new UpdateBookingRequestDto { BookingId = 999, EmployeeId = 2, SeatId = 3, BookingDate = DateTime.Today.AddDays(1) };
        var act = async () => await _bookingService.UpdateAsync(invalidRequest);
        await act.Should().ThrowAsync<KeyNotFoundException>().WithMessage("Could not find booking to update!");
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrowKeyNotFoundException_WhenEmployeeNotFound()
    {
        var invalidRequest = new UpdateBookingRequestDto { BookingId = 1, EmployeeId = 999, SeatId = 3, BookingDate = DateTime.Today.AddDays(1) };
        var act = async () => await _bookingService.UpdateAsync(invalidRequest);
        await act.Should().ThrowAsync<KeyNotFoundException>().WithMessage("Could not find booking to update!");
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrowKeyNotFoundException_WhenSeatNotFound()
    {
        var invalidRequest = new UpdateBookingRequestDto { BookingId = 1, EmployeeId = 2, SeatId = 999, BookingDate = DateTime.Today.AddDays(1) };
        var act = async () => await _bookingService.UpdateAsync(invalidRequest);
        await act.Should().ThrowAsync<KeyNotFoundException>().WithMessage("No seat found with seat id : 999");
    }    

    [Fact]
    public async Task RemoveAsync_ShouldReturnTrue_WhenValidRequest()
    {
        var validRemoveRequest = new RemoveBookingRequestDto { BookingId = 1, BookingDate = DateTime.Today.AddDays(1) };
        var result = await _bookingService.RemoveAsync(validRemoveRequest);
        result.Should().BeTrue();
    }

    [Fact]
    public async Task RemoveAsync_ShouldThrowArgumentNullException_WhenRequestIsNull()
    {
        RemoveBookingRequestDto nullRequest = null;
        var act = async () => await _bookingService.RemoveAsync(nullRequest);
        await act.Should().ThrowAsync<ArgumentNullException>().WithMessage("Remove request cannot be null. (Parameter 'removeBookingRequestDto')");
    }

    [Fact]
    public async Task RemoveAsync_ShouldThrowArgumentException_WhenBookingIdIsNonPositive()
    {
        var invalidRequest = new RemoveBookingRequestDto { BookingId = 0, BookingDate = DateTime.Today.AddDays(1) };
        var act = async () => await _bookingService.RemoveAsync(invalidRequest);
        await act.Should().ThrowAsync<ArgumentException>().WithMessage("Booking ID must be a positive number.");
    }

    [Fact]
    public async Task RemoveAsync_ShouldThrowKeyNotFoundException_WhenBookingNotFound()
    {
        var invalidRequest = new RemoveBookingRequestDto { BookingId = 999, BookingDate = DateTime.Today.AddDays(1) };
        var act = async () => await _bookingService.RemoveAsync(invalidRequest);
        await act.Should().ThrowAsync<KeyNotFoundException>().WithMessage("Could not find booking to delete!");
    }

}
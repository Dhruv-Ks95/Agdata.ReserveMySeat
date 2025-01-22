using Agdata.ReserveMySeat.Application.Interfaces;
using Agdata.ReserveMySeat.Domain.DTOs;
using Agdata.ReserveMySeat.Domain.Entities;
using Agdata.ReserveMySeat.Infrastructure.Mappings;
using Agdata.ReserveMySeat.Infrastructure.Repositories;
using Agdata.ReserveMySeat.Infrastructure.Tests.Mocks;
using AutoMapper;
using FluentAssertions;

namespace Agdata.ReserveMySeat.Infrastructure.Tests;
public class BookingRepositoryTests
{

    private readonly IReserveMySeatDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IBookingRepository _bookingRepository;

    public BookingRepositoryTests()
    {
        _dbContext = MockDbContext.GetDbContextWithTestData();
        var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfiles>());
        _mapper = config.CreateMapper();
        _bookingRepository = new BookingRepository(_mapper, _dbContext);
    }

    [Fact]
    public async Task AddAsync_ShouldAddBooking()
    {
        var bookingdto = new AddBookingRequestDto { EmployeeId = 1, SeatId = 1, BookingDate = DateTime.Today};
        var booking = new Booking(bookingdto);
        var result = await _bookingRepository.AddAsync(booking);
        result.BookingId.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnBooking_WhenValidId()
    {
        var result = await _bookingRepository.GetByIdAsync(2);
        result.Should().NotBeNull();
        result.BookingId.Should().Be(2);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenBookingNotFound()
    {
        var result = await _bookingRepository.GetByIdAsync(999);
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetByIdAsync_WithDate_ShouldReturnBooking_WhenValidIdAndDate()
    {
        var date = DateTime.Today;
        var result = await _bookingRepository.GetByIdAsync(1, date);
        result.Should().NotBeNull();
        result.BookingDate.Should().Be(date);
    }

    [Fact]
    public async Task GetByIdAsync_WithDate_ShouldReturnNull_WhenBookingNotFound()
    {
        var date = DateTime.Today;
        var result = await _bookingRepository.GetByIdAsync(999, date);
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetByUserAsync_ShouldReturnBookings_WhenValidUserId()
    {
        var result = await _bookingRepository.GetByUserAsync(2);
        result.Should().NotBeEmpty();
    }

    [Fact]
    public async Task GetByUserAsync_ShouldReturnEmpty_WhenNoBookingsFound()
    {
        var result = await _bookingRepository.GetByUserAsync(999);
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetByDateAsync_ShouldReturnBookings_WhenValidDate()
    {
        var date = DateTime.Today;
        var result = await _bookingRepository.GetByDateAsync(date);
        result.Should().NotBeEmpty();
        result.All(b => b.BookingDate.Date == date).Should().BeTrue();
    }

    [Fact]
    public async Task GetByDateAsync_ShouldReturnEmpty_WhenNoBookingsOnDate()
    {
        var date = DateTime.Today.AddDays(20);
        var result = await _bookingRepository.GetByDateAsync(date);
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetMonthlyAsync_ShouldReturnMonthlyBookings()
    {
        var result = await _bookingRepository.GetMonthlyAsync();
        result.Should().NotBeEmpty();
        result.All(b => b.BookingDate >= DateTime.Today && b.BookingDate < DateTime.Today.AddDays(30)).Should().BeTrue();
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateBooking()
    {
        var existingBooking = await _bookingRepository.GetByIdAsync(1,DateTime.Today);
        existingBooking.Should().NotBeNull();

        var updateBookingDto = new UpdateBookingRequestDto
        {
            BookingId = 1,
            EmployeeId = 2,
            SeatId = 2,
            BookingDate = DateTime.Today
        };
        existingBooking.UpdateBooking(updateBookingDto);
        var result = await _bookingRepository.UpdateAsync(existingBooking);
        result.Should().BeTrue();
    }
    
    [Fact]
    public async Task RemoveAsync_ShouldRemoveBooking()
    {
        var existingBooking = await _bookingRepository.GetByIdAsync(1, DateTime.Today);
        existingBooking.Should().NotBeNull();

        var result = await _bookingRepository.RemoveAsync(existingBooking);
        result.Should().BeTrue();
    }
}
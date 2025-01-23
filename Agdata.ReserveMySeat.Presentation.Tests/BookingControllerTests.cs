using Agdata.ReserveMySeat.Application.Interfaces;
using Agdata.ReserveMySeat.Domain.DTOs;
using Agdata.ReserveMySeat.Presentation.Controllers;
using Microsoft.AspNetCore.Mvc;
using FluentAssertions;
using Moq;

namespace Agdata.ReserveMySeat.Presentation.Tests;
public class BookingControllerTests
{
    private readonly Mock<IBookingService> _mockBookingService;
    private readonly BookingController _controller;

    public BookingControllerTests()
    {
        _mockBookingService = new Mock<IBookingService>();
        _controller = new BookingController(_mockBookingService.Object);
    }

    [Fact]
    public async Task AddAsync_ShouldReturnOk_WhenValidRequest()
    {
        var addBookingRequest = new AddBookingRequestDto
        {
            EmployeeId = 1,
            SeatId = 1,
            BookingDate = DateTime.Today
        };

        var bookingDto = new BookingDto
        {
            BookingId = 1,
            EmployeeId = 1,
            SeatId = 1,
            BookingDate = DateTime.Today
        };

        _mockBookingService.Setup(s => s.AddAsync(addBookingRequest)).ReturnsAsync(bookingDto);

        var result = await _controller.AddAsync(addBookingRequest) as OkObjectResult;

        result.Should().NotBeNull();
        result.StatusCode.Should().Be(200);
        result.Value.Should().BeEquivalentTo(bookingDto);
    }

    [Fact]
    public async Task AddAsync_ShouldReturnBadRequest_WhenArgumentNullExceptionThrown()
    {
        var addBookingRequest = new AddBookingRequestDto
        {
            EmployeeId = 1,
            SeatId = 1,
            BookingDate = DateTime.Today
        };

        _mockBookingService.Setup(s => s.AddAsync(addBookingRequest)).ThrowsAsync(new ArgumentNullException());

        var result = await _controller.AddAsync(addBookingRequest) as BadRequestObjectResult;

        result.Should().NotBeNull();
        result.StatusCode.Should().Be(400);
        result.Value.Should().Be("Booking request cannot be null.");
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnOk_WhenValidId()
    {
        var bookingId = 1;
        var bookingDto = new BookingDto
        {
            BookingId = bookingId,
            EmployeeId = 1,
            SeatId = 1,
            BookingDate = DateTime.Today
        };

        _mockBookingService.Setup(s => s.GetByIdAsync(bookingId)).ReturnsAsync(bookingDto);

        var result = await _controller.GetByIdAsync(bookingId) as OkObjectResult;

        result.Should().NotBeNull();
        result.StatusCode.Should().Be(200);
        result.Value.Should().BeEquivalentTo(bookingDto);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNotFound_WhenKeyNotFoundExceptionThrown()
    {
        var bookingId = 1;

        _mockBookingService.Setup(s => s.GetByIdAsync(bookingId)).ThrowsAsync(new KeyNotFoundException());

        var result = await _controller.GetByIdAsync(bookingId) as NotFoundObjectResult;

        result.Should().NotBeNull();
        result.StatusCode.Should().Be(404);
        result.Value.Should().Be("Booking not found with provided BookingId");
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnNotFound_WhenKeyNotFoundExceptionThrown()
    {
        var updateRequest = new UpdateBookingRequestDto
        {
            BookingId = 1,
            EmployeeId = 1,
            SeatId = 2,
            BookingDate = DateTime.Today.AddDays(1)
        };

        _mockBookingService.Setup(s => s.UpdateAsync(updateRequest)).ThrowsAsync(new KeyNotFoundException());

        var result = await _controller.UpdateAsync(updateRequest) as NotFoundObjectResult;

        result.Should().NotBeNull();
        result.StatusCode.Should().Be(404);
        result.Value.Should().Be("No booking found with provided BookingId");
    }

    [Fact]
    public async Task RemoveAsync_ShouldReturnOk_WhenValidRequest()
    {
        var removeRequest = new RemoveBookingRequestDto
        {
            BookingId = 1
        };

        _mockBookingService.Setup(s => s.RemoveAsync(removeRequest)).ReturnsAsync(true);

        var result = await _controller.RemoveAsync(removeRequest) as OkObjectResult;

        result.Should().NotBeNull();
        result.StatusCode.Should().Be(200);
        result.Value.Should().Be(true);
    }

    [Fact]
    public async Task RemoveAsync_ShouldReturnNotFound_WhenKeyNotFoundExceptionThrown()
    {
        var removeRequest = new RemoveBookingRequestDto
        {
            BookingId = 1
        };

        _mockBookingService.Setup(s => s.RemoveAsync(removeRequest)).ThrowsAsync(new KeyNotFoundException());

        var result = await _controller.RemoveAsync(removeRequest) as NotFoundObjectResult;

        result.Should().NotBeNull();
        result.StatusCode.Should().Be(404);
        result.Value.Should().Be("No booking found with provided booking Id");
    }
}

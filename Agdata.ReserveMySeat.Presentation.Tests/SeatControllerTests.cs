using Agdata.ReserveMySeat.Application.Interfaces;
using Agdata.ReserveMySeat.Domain.DTOs;
using Agdata.ReserveMySeat.Presentation.Controllers;
using Microsoft.AspNetCore.Mvc;
using FluentAssertions;
using Moq;

namespace Agdata.ReserveMySeat.Presentation.Tests;
public class SeatControllerTests
{
    private readonly Mock<ISeatService> _mockSeatService;
    private readonly SeatController _controller;

    public SeatControllerTests()
    {
        _mockSeatService = new Mock<ISeatService>();
        _controller = new SeatController(_mockSeatService.Object);
    }

    [Fact]
    public async Task AddAsync_ShouldReturnOk_WhenValidRequest()
    {
        var addSeatRequest = new AddSeatRequestDto
        {
            SeatNumber = 120
        };

        var seatDto = new SeatDto
        {
            SeatId = 1,
            SeatNumber = 120
        };

        _mockSeatService.Setup(s => s.AddAsync(addSeatRequest)).ReturnsAsync(seatDto);

        var result = await _controller.AddAsync(addSeatRequest) as OkObjectResult;

        result.Should().NotBeNull();
        result.StatusCode.Should().Be(200);
        result.Value.Should().BeEquivalentTo(seatDto);
    }

    
    [Fact]
    public async Task UpdateAsync_ShouldReturnOk_WhenValidRequest()
    {
        var updateRequest = new UpdateSeatRequestDto
        {
            SeatId = 1,
            SeatNumber = 120
        };

        _mockSeatService.Setup(s => s.UpdateAsync(updateRequest)).ReturnsAsync(true);

        var result = await _controller.UpdateAsync(updateRequest) as OkObjectResult;

        result.Should().NotBeNull();
        result.StatusCode.Should().Be(200);
        result.Value.Should().Be("Seat successfully updated!");
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnNotFound_WhenSeatIdDoesNotExist()
    {
        var updateRequest = new UpdateSeatRequestDto
        {
            SeatId = 999,
            SeatNumber = 120
        };

        _mockSeatService.Setup(s => s.UpdateAsync(updateRequest)).ThrowsAsync(new KeyNotFoundException());

        var result = await _controller.UpdateAsync(updateRequest) as NotFoundObjectResult;

        result.Should().NotBeNull();
        result.StatusCode.Should().Be(404);
        result.Value.Should().Be("Seat not found with the given ID!");
    }
}


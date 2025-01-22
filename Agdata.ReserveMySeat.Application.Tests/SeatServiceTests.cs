using Agdata.ReserveMySeat.Application.Interfaces;
using Agdata.ReserveMySeat.Application.Services;
using Agdata.ReserveMySeat.Application.Tests.Mocks;
using Agdata.ReserveMySeat.Domain.DTOs;
using Agdata.ReserveMySeat.Infrastructure.Mappings;
using Agdata.ReserveMySeat.Infrastructure.Repositories;
using AutoMapper;
using FluentAssertions;

namespace Agdata.ReserveMySeat.Application.Tests;
public class SeatServiceTests
{
    private readonly IReserveMySeatDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly SeatRepository _seatRepository;
    private readonly SeatService _seatService;

    public SeatServiceTests()
    {
        _dbContext = MockDbContext.GetDbContextWithTestData();
        var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfiles>());
        _mapper = config.CreateMapper();
        _seatRepository = new SeatRepository(_mapper, _dbContext);
        _seatService= new SeatService(_seatRepository);
    }

    [Fact]
    public async Task AddAsync_ShouldAddSeat_WhenValidRequest()
    {
        var validRequest = new AddSeatRequestDto { SeatNumber = 100 };
        var result = await _seatService.AddAsync(validRequest);
        result.SeatNumber.Should().Be(100);
    }

    [Fact]
    public async Task AddAsync_ShouldThrowArgumentNullException_WhenRequestIsNull()
    {
        AddSeatRequestDto nullRequest = null;
        var act = async () => await _seatService.AddAsync(nullRequest);
        await act.Should().ThrowAsync<ArgumentNullException>().WithMessage("Add request cannot be null. (Parameter 'addSeatRequestDto')");
    }

    [Fact]
    public async Task AddAsync_ShouldThrowArgumentException_WhenSeatNumberIsInvalid()
    {
        var invalidRequest = new AddSeatRequestDto { SeatNumber = 0 };
        var act = async () => await _seatService.AddAsync(invalidRequest);
        await act.Should().ThrowAsync<ArgumentException>().WithMessage("Invalid seat Number provided!");
    }

    [Fact]
    public async Task AddAsync_ShouldThrowInvalidOperationException_WhenSeatNumberExists()
    {
        var duplicateRequest = new AddSeatRequestDto { SeatNumber = 1 }; 
        var act = async () => await _seatService.AddAsync(duplicateRequest);
        await act.Should().ThrowAsync<InvalidOperationException>().WithMessage("Seat with SeatNumber 1 already exists!");
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllSeats()
    {
        var result = await _seatService.GetAllAsync();
        result.Should().NotBeEmpty();
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnSeat_WhenValidId()
    {
        var result = await _seatService.GetByIdAsync(2);
        result.SeatId.Should().Be(2);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldThrowArgumentException_WhenIdIsInvalid()
    {
        var act = async () => await _seatService.GetByIdAsync(-1);
        await act.Should().ThrowAsync<ArgumentException>().WithMessage("Invalid seat Id");
    }

    [Fact]
    public async Task GetByIdAsync_ShouldThrowKeyNotFoundException_WhenSeatNotFound()
    {
        var act = async () => await _seatService.GetByIdAsync(999);
        await act.Should().ThrowAsync<KeyNotFoundException>().WithMessage("No seat found with seat id : 999");
    }

    [Fact]
    public async Task GetAvailableCountOnDateAsync_ShouldReturnSeats_WhenValidDate()
    {
        var validDate = DateTime.Today.AddDays(1);
        var result = await _seatService.GetAvailableCountOnDateAsync(validDate);
        result.Should().NotBeEmpty();
    }

    [Fact]
    public async Task GetAvailableCountOnDateAsync_ShouldThrowArgumentException_WhenDateIsInvalid()
    {
        var invalidDate = DateTime.Today.AddDays(-1);
        var act = async () => await _seatService.GetAvailableCountOnDateAsync(invalidDate);
        await act.Should().ThrowAsync<ArgumentException>().WithMessage("Invalid date");
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateSeat_WhenValidRequest()
    {
        var validRequest = new UpdateSeatRequestDto { SeatId = 1, SeatNumber = 11 };
        var result = await _seatService.UpdateAsync(validRequest);
        result.Should().BeTrue();
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrowArgumentNullException_WhenRequestIsNull()
    {
        UpdateSeatRequestDto nullRequest = null;
        var act = async () => await _seatService.UpdateAsync(nullRequest);
        await act.Should().ThrowAsync<ArgumentNullException>().WithMessage("Update request cannot be null. (Parameter 'updateSeatRequestDto')");
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrowKeyNotFoundException_WhenSeatNotFound()
    {
        var nonExistentRequest = new UpdateSeatRequestDto { SeatId = 999, SeatNumber = 10 };
        var act = async () => await _seatService.UpdateAsync(nonExistentRequest);
        await act.Should().ThrowAsync<KeyNotFoundException>().WithMessage("Seat with SeatId 999 not found!");
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrowInvalidOperationException_WhenSeatNumberExists()
    {
        var duplicateRequest = new UpdateSeatRequestDto { SeatId = 2, SeatNumber = 1 }; // Ensure SeatNumber 1 exists but belongs to a different seat
        var act = async () => await _seatService.UpdateAsync(duplicateRequest);
        await act.Should().ThrowAsync<InvalidOperationException>().WithMessage("Seat with SeatNumber 1 already exists!");
    }

    [Fact]
    public async Task RemoveAsync_ShouldRemoveSeat_WhenValidRequest()
    {
        var validRequest = new RemoveSeatRequestDto { SeatId = 5, SeatNumber = 5 };
        var result = await _seatService.RemoveAsync(validRequest);
        result.Should().BeTrue();
    }

    [Fact]
    public async Task RemoveAsync_ShouldThrowArgumentNullException_WhenRequestIsNull()
    {
        RemoveSeatRequestDto nullRequest = null;
        var act = async () => await _seatService.RemoveAsync(nullRequest);
        await act.Should().ThrowAsync<ArgumentNullException>().WithMessage("Remove request cannot be null. (Parameter 'removeSeatRequestDto')");
    }

    [Fact]
    public async Task RemoveAsync_ShouldThrowArgumentException_WhenSeatIdIsInvalid()
    {
        var invalidRequest = new RemoveSeatRequestDto { SeatId = 0, SeatNumber = 1 };
        var act = async () => await _seatService.RemoveAsync(invalidRequest);
        await act.Should().ThrowAsync<ArgumentException>().WithMessage("Invalid SeatId provided.");
    }

    [Fact]
    public async Task RemoveAsync_ShouldThrowKeyNotFoundException_WhenSeatNotFound()
    {
        var nonExistentRequest = new RemoveSeatRequestDto { SeatId = 999, SeatNumber = 1 };
        var act = async () => await _seatService.RemoveAsync(nonExistentRequest);
        await act.Should().ThrowAsync<KeyNotFoundException>().WithMessage("Could not find seat to delete!");
    }


}

using Agdata.ReserveMySeat.Application.Interfaces;
using Agdata.ReserveMySeat.Domain.DTOs;
using Agdata.ReserveMySeat.Domain.Entities;
using Agdata.ReserveMySeat.Infrastructure.Mappings;
using Agdata.ReserveMySeat.Infrastructure.Repositories;
using Agdata.ReserveMySeat.Infrastructure.Tests.Mocks;
using AutoMapper;
using FluentAssertions;

namespace Agdata.ReserveMySeat.Infrastructure.Tests;
public class SeatRepositoryTests
{
    private readonly IReserveMySeatDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly ISeatRepository _seatRepository;

    public SeatRepositoryTests()
    {
        _dbContext = MockDbContext.GetDbContextWithTestData();
        var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfiles>());
        _mapper = config.CreateMapper();
        _seatRepository = new SeatRepository(_mapper, _dbContext);
    }

    [Fact]
    public async Task AddAsync_ShouldAddSeat()
    {
        var addSeatRequest = new AddSeatRequestDto { SeatNumber = 25 };
        var seat = new Seat(addSeatRequest);
        var result = await _seatRepository.AddAsync(seat);
        result.Should().NotBeNull();
        result.SeatNumber.Should().Be(25);
        var addedSeat = await _seatRepository.GetByIdAsync(result.SeatId);
        addedSeat.Should().NotBeNull();
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllSeats()
    {
        var seats = await _seatRepository.GetAllAsync();
        seats.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnSeat_WhenExists()
    {
        var seatId = 1;
        var result = await _seatRepository.GetByIdAsync(seatId);
        result.Should().NotBeNull();
        result.SeatId.Should().Be(seatId);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenSeatDoesNotExist()
    {
        var seatId = 999;
        var result = await _seatRepository.GetByIdAsync(seatId);
        result.Should().BeNull();
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateSeat()
    {
        var seatId = 1;
        var seat = await _dbContext.Seats.FindAsync(seatId);
        seat.Should().NotBeNull();
        var updateRequest = new UpdateSeatRequestDto { SeatNumber = 30 };
        seat.UpdateSeat(updateRequest);
        var result = await _seatRepository.UpdateAsync(seat);
        result.Should().BeTrue();
        var updatedSeat = await _seatRepository.GetByIdAsync(seatId);
        updatedSeat.Should().NotBeNull();
        updatedSeat.SeatNumber.Should().Be(30);
    }

    [Fact]
    public async Task RemoveAsync_ShouldRemoveSeat()
    {
        var seatId = 1;
        var seat = await _dbContext.Seats.FindAsync(seatId);
        seat.Should().NotBeNull();
        var result = await _seatRepository.RemoveAsync(seat);
        result.Should().BeTrue();
        var deletedSeat = await _seatRepository.GetByIdAsync(seatId);
        deletedSeat.Should().BeNull();
    }

}

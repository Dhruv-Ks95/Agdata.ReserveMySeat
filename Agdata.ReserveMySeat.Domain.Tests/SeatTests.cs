using Agdata.ReserveMySeat.Domain.DTOs;
using Agdata.ReserveMySeat.Domain.Entities;
using FluentAssertions;

namespace Agdata.ReserveMySeat.Domain.Tests;
public class SeatTests
{
    [Fact]
    public void Constructor_ShouldCreateSeat_WhenValidRequestProvided()
    {
        var request = new AddSeatRequestDto
        {
            SeatNumber = 25
        };

        var seat = new Seat(request);

        seat.SeatNumber.Should().Be(25);
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentException_WhenSeatNumberIsZeroOrNegative()
    {
        var zeroRequest = new AddSeatRequestDto
        {
            SeatNumber = 0
        };

        var negativeRequest = new AddSeatRequestDto
        {
            SeatNumber = -5
        };

        Action actZero = () => new Seat(zeroRequest);
        Action actNegative = () => new Seat(negativeRequest);

        actZero.Should().Throw<ArgumentException>().WithMessage("Seat number must be a positive integer.");
        actNegative.Should().Throw<ArgumentException>().WithMessage("Seat number must be a positive integer.");
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentException_WhenSeatNumberExceedsLimit()
    {
        var request = new AddSeatRequestDto
        {
            SeatNumber = 151
        };

        Action act = () => new Seat(request);

        act.Should().Throw<ArgumentException>().WithMessage("Exceeded seat number limit");
    }

    [Fact]
    public void UpdateSeat_ShouldUpdateSeatNumber_WhenValidRequestProvided()
    {
        var initialRequest = new AddSeatRequestDto
        {
            SeatNumber = 25
        };

        var updateRequest = new UpdateSeatRequestDto
        {
            SeatNumber = 45
        };

        var seat = new Seat(initialRequest);
        seat.UpdateSeat(updateRequest);

        seat.SeatNumber.Should().Be(45);
    }

    [Fact]
    public void UpdateSeat_ShouldThrowArgumentException_WhenSeatNumberIsZeroOrNegative()
    {
        var initialRequest = new AddSeatRequestDto
        {
            SeatNumber = 25
        };

        var updateRequestZero = new UpdateSeatRequestDto
        {
            SeatNumber = 0
        };

        var updateRequestNegative = new UpdateSeatRequestDto
        {
            SeatNumber = -10
        };

        var seat = new Seat(initialRequest);

        Action actZero = () => seat.UpdateSeat(updateRequestZero);
        Action actNegative = () => seat.UpdateSeat(updateRequestNegative);

        actZero.Should().Throw<ArgumentException>().WithMessage("Seat number must be a positive integer.");
        actNegative.Should().Throw<ArgumentException>().WithMessage("Seat number must be a positive integer.");
    }

    [Fact]
    public void UpdateSeat_ShouldThrowArgumentException_WhenSeatNumberExceedsLimit()
    {
        var initialRequest = new AddSeatRequestDto
        {
            SeatNumber = 25
        };

        var updateRequest = new UpdateSeatRequestDto
        {
            SeatNumber = 151
        };

        var seat = new Seat(initialRequest);

        Action act = () => seat.UpdateSeat(updateRequest);

        act.Should().Throw<ArgumentException>().WithMessage("Exceeded seat number limit");
    }
}


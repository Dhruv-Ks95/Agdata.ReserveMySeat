using Agdata.ReserveMySeat.Domain.DTOs;
using Agdata.ReserveMySeat.Domain.Entities;
using FluentAssertions;

namespace Agdata.ReserveMySeat.Domain.Tests;
public class BookingTests
{
    [Fact]
    public void Constructor_ShouldCreateBooking_WhenValidRequestProvided()
    {
        var request = new AddBookingRequestDto
        {
            EmployeeId = 1,
            SeatId = 10,
            BookingDate = DateTime.Today.AddDays(5)
        };

        var booking = new Booking(request);

        booking.EmployeeId.Should().Be(1);
        booking.SeatId.Should().Be(10);
        booking.BookingDate.Should().Be(DateTime.Today.AddDays(5));
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentException_WhenEmployeeIdIsInvalid()
    {
        var request = new AddBookingRequestDto
        {
            EmployeeId = 0,
            SeatId = 10,
            BookingDate = DateTime.Today.AddDays(5)
        };

        Action act = () => new Booking(request);

        act.Should().Throw<ArgumentException>().WithMessage("Invalid employee Id");
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentException_WhenSeatIdIsInvalid()
    {
        var request = new AddBookingRequestDto
        {
            EmployeeId = 1,
            SeatId = 0,
            BookingDate = DateTime.Today.AddDays(5)
        };

        Action act = () => new Booking(request);

        act.Should().Throw<ArgumentException>().WithMessage("Invalid seat Id");
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentException_WhenBookingDateIsInThePast()
    {
        var request = new AddBookingRequestDto
        {
            EmployeeId = 1,
            SeatId = 10,
            BookingDate = DateTime.Today.AddDays(-1)
        };

        Action act = () => new Booking(request);

        act.Should().Throw<ArgumentException>().WithMessage("Invalid date");
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentException_WhenBookingDateIsBeyondAllowedRange()
    {
        var request = new AddBookingRequestDto
        {
            EmployeeId = 1,
            SeatId = 10,
            BookingDate = DateTime.Today.AddDays(30)
        };

        Action act = () => new Booking(request);

        act.Should().Throw<ArgumentException>().WithMessage("Invalid date");
    }

    [Fact]
    public void UpdateBooking_ShouldUpdateValues_WhenValidRequestProvided()
    {
        var initialRequest = new AddBookingRequestDto
        {
            EmployeeId = 1,
            SeatId = 10,
            BookingDate = DateTime.Today.AddDays(5)
        };

        var updateRequest = new UpdateBookingRequestDto
        {
            EmployeeId = 2,
            SeatId = 20,
            BookingDate = DateTime.Today.AddDays(10)
        };

        var booking = new Booking(initialRequest);
        booking.UpdateBooking(updateRequest);

        booking.EmployeeId.Should().Be(2);
        booking.SeatId.Should().Be(20);
        booking.BookingDate.Should().Be(DateTime.Today.AddDays(10));
    }

    [Fact]
    public void UpdateBooking_ShouldThrowArgumentException_WhenEmployeeIdIsInvalid()
    {
        var initialRequest = new AddBookingRequestDto
        {
            EmployeeId = 1,
            SeatId = 10,
            BookingDate = DateTime.Today.AddDays(5)
        };

        var updateRequest = new UpdateBookingRequestDto
        {
            EmployeeId = 0,
            SeatId = 10,
            BookingDate = DateTime.Today.AddDays(10)
        };

        var booking = new Booking(initialRequest);

        Action act = () => booking.UpdateBooking(updateRequest);

        act.Should().Throw<ArgumentException>().WithMessage("Invalid employee Id");
    }

    [Fact]
    public void UpdateBooking_ShouldThrowArgumentException_WhenSeatIdIsInvalid()
    {
        var initialRequest = new AddBookingRequestDto
        {
            EmployeeId = 1,
            SeatId = 10,
            BookingDate = DateTime.Today.AddDays(5)
        };

        var updateRequest = new UpdateBookingRequestDto
        {
            EmployeeId = 1,
            SeatId = 0,
            BookingDate = DateTime.Today.AddDays(10)
        };

        var booking = new Booking(initialRequest);

        Action act = () => booking.UpdateBooking(updateRequest);

        act.Should().Throw<ArgumentException>().WithMessage("Invalid seat Id");
    }

    
}

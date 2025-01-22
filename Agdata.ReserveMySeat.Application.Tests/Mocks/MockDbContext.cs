using Agdata.ReserveMySeat.Application.Interfaces;
using Agdata.ReserveMySeat.Domain.DTOs;
using Agdata.ReserveMySeat.Domain.Entities;
using Agdata.ReserveMySeat.Domain.Enums;
using Agdata.ReserveMySeat.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Agdata.ReserveMySeat.Application.Tests.Mocks;

public static class MockDbContext
{
    public static IReserveMySeatDbContext GetDbContextWithTestData()
    {

        var options = new DbContextOptionsBuilder<ReserveMySeatDbContext>().UseInMemoryDatabase(databaseName: "ReserveMySeatDbTest").Options;

        var dbContext = new ReserveMySeatDbContext(options);

        // Seed Employees
        var employees = new List<Employee>
        {
            new Employee(new AddEmployeeRequestDto { Name = "Admin", Email = "admin@company.com", Role = RoleType.Admin }),
            new Employee(new AddEmployeeRequestDto { Name = "Dhruv Ks", Email = "dhruv@company.com", Role = RoleType.User }),
            new Employee(new AddEmployeeRequestDto { Name = "Aradhya S", Email = "aradhya@company.com", Role = RoleType.User }),
            new Employee(new AddEmployeeRequestDto { Name = "Srijan D", Email = "srijan@company.com", Role = RoleType.User }),
            new Employee(new AddEmployeeRequestDto { Name = "Dileep R", Email = "dileep@company.com", Role = RoleType.User })
        };
        dbContext.Employees.AddRange(employees);

        // Seed Seats
        var seats = Enumerable.Range(1, 10).Select(i => new Seat(new AddSeatRequestDto { SeatNumber = i })).ToList();
        dbContext.Seats.AddRange(seats);

        // Seed Bookings
        DateTime bookingDate = DateTime.Now.Date.AddDays(4);
        var bookings = new List<Booking>
        {
            new Booking(new AddBookingRequestDto { EmployeeId = 2, BookingDate = bookingDate, SeatId = 10 }),
            new Booking(new AddBookingRequestDto { EmployeeId = 4, BookingDate = bookingDate, SeatId = 9 })
        };
        dbContext.Bookings.AddRange(bookings);
        dbContext.SaveChanges();
        return dbContext;
    }
}

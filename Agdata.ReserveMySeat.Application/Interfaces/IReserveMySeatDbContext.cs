using Agdata.ReserveMySeat.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
namespace Agdata.ReserveMySeat.Application.Interfaces;
public interface IReserveMySeatDbContext
{
    DbSet<Employee> Employees { get; set; }
    DbSet<Seat> Seats { get; set; }
    DbSet<Booking> Bookings { get; set; }
    DatabaseFacade Database { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

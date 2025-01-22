using Agdata.ReserveMySeat.Domain.Entities;
using Agdata.ReserveMySeat.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
namespace Agdata.ReserveMySeat.Infrastructure.Data;
public class ReserveMySeatDbContext : DbContext, IReserveMySeatDbContext
{
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Seat> Seats { get; set; }
    public DbSet<Booking> Bookings { get; set; }    

    public ReserveMySeatDbContext(DbContextOptions<ReserveMySeatDbContext> options): base(options)
    {

    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer("Data Source=INDLAP-G2382985;Initial Catalog=Agdata.ReserveMySeat.Database;Integrated Security=True;Connect Timeout=60;Encrypt=False;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {     
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ReserveMySeatDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

}

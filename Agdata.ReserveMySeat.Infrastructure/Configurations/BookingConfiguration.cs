using Agdata.ReserveMySeat.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Agdata.ReserveMySeat.Infrastructure.Configurations;
public class BookingConfiguration : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        builder.ToTable("Bookings");builder.HasKey(b => b.BookingId);
        builder.Property(b => b.BookingDate).IsRequired();
        builder.HasOne<Employee>().WithMany().HasForeignKey(b => b.EmployeeId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne<Seat>().WithMany().HasForeignKey(b => b.SeatId).OnDelete(DeleteBehavior.Cascade);
    }
}

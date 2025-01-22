using Agdata.ReserveMySeat.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agdata.ReserveMySeat.Infrastructure.Configurations;
public class SeatConfiguration : IEntityTypeConfiguration<Seat>
{
    public void Configure(EntityTypeBuilder<Seat> builder)
    {
        builder.ToTable("Seats");
        builder.HasKey(s => s.SeatId);
        builder.Property(s => s.SeatNumber).IsRequired();
    }
}

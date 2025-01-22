using Agdata.ReserveMySeat.Domain.Entities;
using Agdata.ReserveMySeat.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Agdata.ReserveMySeat.Infrastructure.Configurations;
public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.ToTable("Employees");
        builder.HasKey(e => e.EmployeeId);
        builder.Property(e => e.Name).IsRequired().HasMaxLength(100);
        builder.Property(e => e.Email).IsRequired().HasMaxLength(100);
        builder.Property(e => e.Role).IsRequired().HasMaxLength(50);
        builder.Property(e => e.Role).HasConversion(role => role.ToString(), role => (RoleType)Enum.Parse(typeof(RoleType), role));
    }
}

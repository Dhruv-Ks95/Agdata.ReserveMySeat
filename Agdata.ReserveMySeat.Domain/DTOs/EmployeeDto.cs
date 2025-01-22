using Agdata.ReserveMySeat.Domain.Enums;
namespace Agdata.ReserveMySeat.Domain.DTOs;
public record EmployeeDto
{
    public int EmployeeId { get; init; }
    public string Name { get; init; } = null!;
    public string Email { get; init; } = null!;
    public RoleType Role { get; init; }
}

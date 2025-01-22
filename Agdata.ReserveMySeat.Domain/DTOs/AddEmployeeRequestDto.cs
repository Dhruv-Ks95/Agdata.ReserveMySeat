using Agdata.ReserveMySeat.Domain.Enums;
namespace Agdata.ReserveMySeat.Domain.DTOs;
public record AddEmployeeRequestDto
{
    public string Name { get; init; } = null!;
    public string Email { get; init; } = null!;
    public RoleType Role { get; init; }
}

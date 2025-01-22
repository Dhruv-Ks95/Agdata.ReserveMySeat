namespace Agdata.ReserveMySeat.Domain.DTOs;
public record RemoveEmployeeRequestDto
{
    public int EmployeeId { get; init; }
    public string Email { get; init; } = null!;

}

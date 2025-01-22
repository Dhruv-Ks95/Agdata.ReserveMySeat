namespace Agdata.ReserveMySeat.Domain.DTOs;
public record RemoveBookingRequestDto
{
    public int BookingId { get; set; }
    public DateTime BookingDate { get; set; }

}

using Agdata.ReserveMySeat.Domain.DTOs;
namespace Agdata.ReserveMySeat.Domain.Entities;
public class Booking
{
    public int BookingId { get; private set; }
    public int EmployeeId { get; private set; }
    public int SeatId { get; private set; }
    public DateTime BookingDate { get; private set; }
    
    public Booking(AddBookingRequestDto addBookingRequestDto)
    {
        ValidateAddRequest(addBookingRequestDto);
        EmployeeId = addBookingRequestDto.EmployeeId;
        SeatId = addBookingRequestDto.SeatId;  
        BookingDate = addBookingRequestDto.BookingDate.Date;
    }

    private Booking() { }

    public void UpdateBooking(UpdateBookingRequestDto updateBookingRequestDto)
    {
        ValidateUpdateRequest(updateBookingRequestDto);
        EmployeeId = updateBookingRequestDto.EmployeeId;
        SeatId = updateBookingRequestDto.SeatId;
        BookingDate = updateBookingRequestDto.BookingDate.Date;
    }

    public void DeleteBooking()
    {
        // Some stuff that might be dependent on this booking 
    }

    private void ValidateAddRequest(AddBookingRequestDto addBookingRequestDto)
    {
        if (addBookingRequestDto.EmployeeId <= 0)
        {
            throw new ArgumentException("Invalid employee Id");
        }
        if (addBookingRequestDto.SeatId <= 0)
        {
            throw new ArgumentException("Invalid seat Id");
        }
        ValidateDate(addBookingRequestDto.BookingDate);
    }

    private void ValidateUpdateRequest(UpdateBookingRequestDto updateBookingRequestDto)
    {
        if (updateBookingRequestDto.EmployeeId <= 0)
        {
            throw new ArgumentException("Invalid employee Id");
        }
        if (updateBookingRequestDto.SeatId <= 0)
        {
            throw new ArgumentException("Invalid seat Id");
        }
        ValidateDate(BookingDate);
    }

    private void ValidateDate(DateTime date)
    {
        if(date < DateTime.Today.Date || date > DateTime.Today.AddDays(29).Date)
        {
            throw new ArgumentException("Invalid date");
        }
    }

}

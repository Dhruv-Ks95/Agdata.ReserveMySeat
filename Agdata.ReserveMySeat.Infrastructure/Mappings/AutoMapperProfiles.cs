using AutoMapper;
using Agdata.ReserveMySeat.Domain.Entities;
using Agdata.ReserveMySeat.Domain.DTOs;
namespace Agdata.ReserveMySeat.Infrastructure.Mappings;
public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<Employee, EmployeeDto>().ReverseMap();
        CreateMap<AddEmployeeRequestDto, Employee>().ReverseMap();
        CreateMap<RemoveEmployeeRequestDto, Employee>().ReverseMap();
        CreateMap<UpdateEmployeeRequestDto, Employee>().ReverseMap();        
        CreateMap<Seat, SeatDto>().ReverseMap();
        CreateMap<AddSeatRequestDto, Seat>().ReverseMap();
        CreateMap<UpdateSeatRequestDto, Seat>().ReverseMap();
        CreateMap<RemoveSeatRequestDto, Seat>().ReverseMap();
        CreateMap<Booking, BookingDto>().ReverseMap();
        CreateMap<AddBookingRequestDto, Booking>().ReverseMap();
        CreateMap<UpdateBookingRequestDto, Booking>().ReverseMap();
        CreateMap<RemoveBookingRequestDto, Booking>().ReverseMap();
    }
}

using Agdata.ReserveMySeat.Application.Interfaces;
using Agdata.ReserveMySeat.Domain.DTOs;
using Agdata.ReserveMySeat.Domain.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
namespace Agdata.ReserveMySeat.Infrastructure.Repositories;
public class BookingRepository : IBookingRepository
{
    private readonly IReserveMySeatDbContext dbContext;
    private readonly IMapper mapper;
    public BookingRepository(IMapper mapper, IReserveMySeatDbContext dbContext)
    {
        this.mapper = mapper;
        this.dbContext = dbContext;
    }

    public async Task<BookingDto> AddAsync(Booking booking)
    {
        await dbContext.Bookings.AddAsync(booking);
        await dbContext.SaveChangesAsync();
        return mapper.Map<BookingDto>(booking);
    }
   
    public async Task<BookingDto?> GetByIdAsync(int id)
    {
        var booking = await dbContext.Bookings.FindAsync(id);
        return mapper.Map<BookingDto?>(booking);
    }

    public async Task<Booking?> GetByIdAsync(int id, DateTime date)
    {
        var booking = await dbContext.Bookings.FindAsync(id);
        return booking;
    }

    public async Task<IEnumerable<BookingDto>> GetByUserAsync(int userId)
    {
        var bookings = await dbContext.Bookings.Where(b => b.EmployeeId == userId).ToListAsync();
        return mapper.Map<IEnumerable<BookingDto>>(bookings);
    }

    public async Task<IEnumerable<BookingDto>> GetByDateAsync(DateTime date)
    {
        var bookings = await dbContext.Bookings.Where(b => b.BookingDate == date.Date).ToListAsync();
        return mapper.Map<IEnumerable<BookingDto>>(bookings);
    }

    public async Task<IEnumerable<BookingDto>> GetMonthlyAsync()
    {
        if (dbContext.Database.IsInMemory())
        {
            var today = DateTime.Today;
            var thirtyDaysFromToday = today.AddDays(30);
            var localBookings = await dbContext.Bookings.Where(b => b.BookingDate.Date >= today.Date && b.BookingDate.Date < thirtyDaysFromToday.Date).OrderBy(b => b.BookingDate).ToListAsync();
            return mapper.Map<IEnumerable<BookingDto>>(localBookings);
        }
        var bookings = await dbContext.Bookings.FromSqlRaw("exec GetMonthlyBookings").ToListAsync();
        return mapper.Map<IEnumerable<BookingDto>>(bookings);
    }

    public async Task<bool> UpdateAsync(Booking booking)
    {
        dbContext.Bookings.Update(booking);
        var rowsAffected = await dbContext.SaveChangesAsync();
        return rowsAffected > 0;
    }

    public async Task<bool> RemoveAsync(Booking booking)
    {
        dbContext.Bookings.Remove(booking);
        var rowsAffected = await dbContext.SaveChangesAsync();
        return rowsAffected > 0;
    }
}

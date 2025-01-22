using Agdata.ReserveMySeat.Application.Interfaces;
using Agdata.ReserveMySeat.Domain.DTOs;
using Agdata.ReserveMySeat.Domain.Entities;
using AutoMapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
namespace Agdata.ReserveMySeat.Infrastructure.Repositories;
public class SeatRepository : ISeatRepository
{

    private readonly IReserveMySeatDbContext dbContext;
    private readonly IMapper mapper;
    public SeatRepository(IMapper mapper , IReserveMySeatDbContext dbContext)
    {
        this.mapper = mapper;
        this.dbContext = dbContext;
    }

    public async Task<SeatDto> AddAsync(Seat seat)
    {
        await dbContext.Seats.AddAsync(seat);
        await dbContext.SaveChangesAsync();
        return mapper.Map<SeatDto>(seat);
    }

    public async Task<IEnumerable<SeatDto>> GetAllAsync()
    {
        var seats = await dbContext.Seats.ToListAsync();
        return mapper.Map<IEnumerable<SeatDto>>(seats);
    }

    public async Task<IEnumerable<SeatDto>> GetAvailableCountOnDateAsync(DateTime date)
    {
        if (dbContext.Database.IsInMemory())
        {
            var availableSeats = await dbContext.Seats.Where(seat => !dbContext.Bookings.Any(b => b.SeatId == seat.SeatId && b.BookingDate.Date == date.Date)).OrderBy(seat => seat.SeatNumber).ToListAsync();
            return mapper.Map<IEnumerable<SeatDto>>(availableSeats);
        }
        var parameter = new SqlParameter("@BookingDate", date); 
        var seats = await dbContext.Seats.FromSqlRaw("EXEC dbo.GetAvailableSeatsOnDate @BookingDate", parameter).ToListAsync(); 
        return mapper.Map<IEnumerable<SeatDto>>(seats);
    }

    public async Task<SeatDto?> GetByIdAsync(int id)
    {
        var seat = await dbContext.Seats.FirstOrDefaultAsync(s => s.SeatId == id);
        return mapper.Map<SeatDto?>(seat);
    }

    public async Task<Seat?> GetByIdAsync(int id, int number)
    {
        var seat = await dbContext.Seats.FindAsync(id);
        return seat;
    }
    
    public async Task<bool> UpdateAsync(Seat seat)
    {
        dbContext.Seats.Update(seat);
        var rowsAffected = await dbContext.SaveChangesAsync();
        return rowsAffected > 0;
    }

    public async Task<bool> RemoveAsync(Seat seat)
    {
        dbContext.Seats.Remove(seat);
        var rowsAffected = await dbContext.SaveChangesAsync();
        return rowsAffected > 0;
    }
}

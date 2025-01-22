using Agdata.ReserveMySeat.Application.Interfaces;
using Agdata.ReserveMySeat.Application.Services;
using Agdata.ReserveMySeat.Infrastructure.Data;
using Agdata.ReserveMySeat.Infrastructure.Mappings;
using Agdata.ReserveMySeat.Infrastructure.Repositories;

namespace Agdata.ReserveMySeat.Presentation
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<IReserveMySeatDbContext, ReserveMySeatDbContext>();
            builder.Services.AddScoped<IEmployeeService, EmployeeService>();
            builder.Services.AddScoped<ISeatService, SeatService>();
            builder.Services.AddScoped<IBookingService, BookingService>();
            builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            builder.Services.AddScoped<IBookingRepository,BookingRepository>();
            builder.Services.AddScoped<ISeatRepository, SeatRepository>();
            builder.Services.AddAutoMapper(typeof(AutoMapperProfiles));

            var app = builder.Build();
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseHttpsRedirection();
            app.UseCors(options =>
            {
                options.AllowAnyOrigin();
                options.AllowAnyMethod();
                options.AllowAnyHeader();
            });
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}

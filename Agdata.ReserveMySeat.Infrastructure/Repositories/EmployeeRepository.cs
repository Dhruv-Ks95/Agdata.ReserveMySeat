using Agdata.ReserveMySeat.Application.Interfaces;
using Agdata.ReserveMySeat.Domain.DTOs;
using Agdata.ReserveMySeat.Domain.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Agdata.ReserveMySeat.Infrastructure.Repositories;
public class EmployeeRepository : IEmployeeRepository
{
    private readonly IReserveMySeatDbContext dbContext;
    private readonly IMapper mapper;

    public EmployeeRepository(IMapper mapper, IReserveMySeatDbContext dbContext)
    {
        this.mapper = mapper;
        this.dbContext = dbContext;
    }
    public async Task<EmployeeDto> AddAsync(Employee employee)
    {
        await dbContext.Employees.AddAsync(employee);
        await dbContext.SaveChangesAsync();
        return mapper.Map<EmployeeDto>(employee);
    }
    public async Task<IEnumerable<EmployeeDto>> GetAllAsync()
    {
        var employees = await dbContext.Employees.ToListAsync();
        return mapper.Map<IEnumerable<EmployeeDto>>(employees);
    }
    public async Task<EmployeeDto?> GetByIdAsync(int employeeId)
    {
        var employee = await dbContext.Employees.FindAsync(employeeId);
        return mapper.Map<EmployeeDto?>(employee);
    }
    public async Task<Employee?> GetByIdAsync(int employeeId, string employeeEmail)
    {
        return await dbContext.Employees.FindAsync(employeeId);
    }
    public async Task<EmployeeDto?> GetByEmailAsync(string email)
    {
        Employee? employee = await dbContext.Employees.FirstOrDefaultAsync(e => e.Email == email);
        return mapper.Map<EmployeeDto?>(employee);
    }
    public async Task<bool> UpdateAsync(Employee employee)
    {
        dbContext.Employees.Update(employee);
        var rowsAffected = await dbContext.SaveChangesAsync();
        return rowsAffected > 0;
    }
    public async Task<bool> RemoveAsync(Employee employee)
    {
        dbContext.Employees.Remove(employee);
        var rowsAffected = await dbContext.SaveChangesAsync();
        return rowsAffected > 0;
    }

}

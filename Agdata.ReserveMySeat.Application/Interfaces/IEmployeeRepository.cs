using Agdata.ReserveMySeat.Domain.DTOs;
using Agdata.ReserveMySeat.Domain.Entities;

namespace Agdata.ReserveMySeat.Application.Interfaces;
public interface IEmployeeRepository
{
    Task<EmployeeDto> AddAsync(Employee employee);
    Task<EmployeeDto?> GetByIdAsync(int employeeId);
    Task<Employee?> GetByIdAsync(int employeeId, string employeeEmail);
    Task<EmployeeDto?> GetByEmailAsync(string email);
    Task<IEnumerable<EmployeeDto>> GetAllAsync();
    Task<bool> UpdateAsync(Employee employee);
    Task<bool> RemoveAsync(Employee employee);
}

using Agdata.ReserveMySeat.Domain.DTOs;
namespace Agdata.ReserveMySeat.Application.Interfaces;
public interface IEmployeeService
{
    Task<EmployeeDto> AddAsync(AddEmployeeRequestDto addEmployeeRequestDto);
    Task<EmployeeDto> GetByIdAsync(int id);
    Task<IEnumerable<EmployeeDto>> GetAllAsync();
    Task<EmployeeDto> GetByEmailAsync(string email);
    Task<bool> UpdateAsync(UpdateEmployeeRequestDto updateEmployeeRequestDto); 
    Task<bool> RemoveAsync(RemoveEmployeeRequestDto removeEmployeeRequestDto);
}

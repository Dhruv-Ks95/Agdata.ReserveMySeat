using System.Text.RegularExpressions;
using Agdata.ReserveMySeat.Application.Interfaces;
using Agdata.ReserveMySeat.Domain.DTOs;
using Agdata.ReserveMySeat.Domain.Entities;
namespace Agdata.ReserveMySeat.Application.Services;
public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository employeeRepository;
    public EmployeeService(IEmployeeRepository employeeRepository)
    {
        this.employeeRepository = employeeRepository;
    }

    public async Task<EmployeeDto> AddAsync(AddEmployeeRequestDto addEmployeeRequestDto)
    {
        if (addEmployeeRequestDto == null)
        {
            throw new ArgumentNullException(nameof(addEmployeeRequestDto), "AddEmployeeRequestDto cannot be null.");
        }
        if (await IsEmailDuplicateAsync(addEmployeeRequestDto.Email))
        {
            throw new ArgumentException($"The email '{addEmployeeRequestDto.Email}' is already in use.");
        }
        Employee employee = new Employee(addEmployeeRequestDto);
        return await employeeRepository.AddAsync(employee);        
    }

    public async Task<IEnumerable<EmployeeDto>> GetAllAsync()
    {
        return await employeeRepository.GetAllAsync();        
    }

    public async Task<EmployeeDto> GetByIdAsync(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException("Invalid EmployeeId");
        }
        EmployeeDto? employeeDto = await employeeRepository.GetByIdAsync(id);
        if (employeeDto == null)
        {
            throw new KeyNotFoundException($"Employee with EmployeeId: {id} not Found!");
        }
        return employeeDto;
    }

    public async Task<EmployeeDto> GetByEmailAsync(string email)
    {
        ValidateEmail(email);
        EmployeeDto? employeeDto = await employeeRepository.GetByEmailAsync(email);
        if(employeeDto == null)
        {
            throw new KeyNotFoundException($"No Employee found with email - {email}!");
        }
        return employeeDto;
    }

    public async Task<bool> UpdateAsync(UpdateEmployeeRequestDto updateEmployeeRequestDto)
    {
        if (updateEmployeeRequestDto == null)
        {
            throw new ArgumentNullException(nameof(updateEmployeeRequestDto), "UpdateEmployeeRequestDto cannot be null.");
        }
        if (updateEmployeeRequestDto.EmployeeId <= 0)
        {
            throw new ArgumentException("Invalid EmployeeId");
        }
        Employee? employee = await employeeRepository.GetByIdAsync(updateEmployeeRequestDto.EmployeeId, updateEmployeeRequestDto.Name);
        if (employee == null)
        {
            throw new KeyNotFoundException($"Employee with EmployeeId{updateEmployeeRequestDto.EmployeeId} not Found!");
        }
        if (await IsEmailDuplicateAsync(updateEmployeeRequestDto.Email, updateEmployeeRequestDto.EmployeeId))
        {
            throw new ArgumentException($"The email '{updateEmployeeRequestDto.Email}' is already in use.");
        }
        employee.UpdateEmployee(updateEmployeeRequestDto);
        return await employeeRepository.UpdateAsync(employee);
    }

    public async Task<bool> RemoveAsync(RemoveEmployeeRequestDto removeEmployeeRequestDto)
    {
        if (removeEmployeeRequestDto.EmployeeId <= 0)
        {
            throw new ArgumentException("Invalid EmployeeId. It must be greater than 0.");
        }
        ValidateEmail(removeEmployeeRequestDto.Email);
        Employee? employee = await employeeRepository.GetByIdAsync(removeEmployeeRequestDto.EmployeeId, removeEmployeeRequestDto.Email);
        if (employee == null)
        {
            throw new KeyNotFoundException($"Employee with EmployeeId{removeEmployeeRequestDto.EmployeeId} not Found!");
        }
        employee.DeleteEmployee();
        return await employeeRepository.RemoveAsync(employee);
    }

    private void ValidateEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            throw new ArgumentException("Email cannot be blank.");
        }
        if (email.Length > 100)
        {
            throw new ArgumentException("Email must not exceed 100 characters.");
        }
        var emailRegex = @"^[^\s@]+@[^\s@]+\.[^\s@]+$";
        if (!Regex.IsMatch(email, emailRegex))
        {
            throw new ArgumentException("Invalid email format.");
        }
    }

    private async Task<bool> IsEmailDuplicateAsync(string email, int? employeeId = null)
    {
        if (employeeId == null) // means adding for first time
        {
            var existingEmployee = await employeeRepository.GetByEmailAsync(email);
            if (existingEmployee == null)
            {
                return false;
            }
            return true;
        }
        // means updating existing employee
        var existingEmp = await employeeRepository.GetByEmailAsync(email);
        if (existingEmp == null || existingEmp.EmployeeId == employeeId)
        {
            return false;
        }
        return true;
    }
}
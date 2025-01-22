using System.Text.RegularExpressions;
using Agdata.ReserveMySeat.Domain.DTOs;
using Agdata.ReserveMySeat.Domain.Enums;
namespace Agdata.ReserveMySeat.Domain.Entities;
public class Employee
{
    public int EmployeeId { get; private set; }
    public string Name { get; private set; } = null!;
    public string Email { get; private set; } = null!;
    public RoleType Role { get; private set; }

    public Employee(AddEmployeeRequestDto addEmployeeRequestDto)
    {
        ValidateAddRequest(addEmployeeRequestDto);
        Name = addEmployeeRequestDto.Name;
        Email = addEmployeeRequestDto.Email;
        Role = addEmployeeRequestDto.Role;
    }

    private Employee() { }

    public void UpdateEmployee(UpdateEmployeeRequestDto updateEmployeeRequestDto)
    {
        ValidateUpdateRequest(updateEmployeeRequestDto);
        Name = updateEmployeeRequestDto.Name;
        Email = updateEmployeeRequestDto.Email;
        Role = updateEmployeeRequestDto.Role;
    }

    public void DeleteEmployee()
    {
        // Functionalities which might depend on this entity
    }

    private void ValidateAddRequest(AddEmployeeRequestDto addEmployeeRequestDto)
    {
        ValidateName(addEmployeeRequestDto.Name);
        ValidateEmail(addEmployeeRequestDto.Email);
        ValidateRole(addEmployeeRequestDto.Role);
    }

    private void ValidateUpdateRequest(UpdateEmployeeRequestDto updateEmployeeRequestDto)
    {
        ValidateName(updateEmployeeRequestDto.Name);
        ValidateEmail(updateEmployeeRequestDto.Email);
        ValidateRole(updateEmployeeRequestDto.Role);
    }

    private void ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name cannot be blank.");
        }

        if (name.Length < 1 || name.Length > 50)
        {
            throw new ArgumentException("Name must be between 3 and 50 characters.");
        }
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

    private void ValidateRole(RoleType role)
    {
        if (!Enum.IsDefined(typeof(RoleType), role))
        {
            throw new ArgumentException("Invalid role type. Valid values are User(0) or Admin(1).");
        }
    }
}
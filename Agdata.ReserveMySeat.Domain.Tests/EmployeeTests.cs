using Agdata.ReserveMySeat.Domain.DTOs;
using Agdata.ReserveMySeat.Domain.Entities;
using Agdata.ReserveMySeat.Domain.Enums;
using FluentAssertions;

namespace Agdata.ReserveMySeat.Domain.Tests;
public class EmployeeTests
{
    [Fact]
    public void Constructor_ShouldCreateEmployee_WhenValidRequestProvided()
    {
        var request = new AddEmployeeRequestDto
        {
            Name = "Ravi",
            Email = "ravi@company.com",
            Role = RoleType.User
        };

        var employee = new Employee(request);

        employee.Name.Should().Be("Ravi");
        employee.Email.Should().Be("ravi@company.com");
        employee.Role.Should().Be(RoleType.User);
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentException_WhenNameIsBlank()
    {
        var request = new AddEmployeeRequestDto
        {
            Name = "",
            Email = "ravi@company.com",
            Role = RoleType.User
        };

        Action act = () => new Employee(request);

        act.Should().Throw<ArgumentException>().WithMessage("Name cannot be blank.");
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentException_WhenEmailIsInvalid()
    {
        var request = new AddEmployeeRequestDto
        {
            Name = "Ravi",
            Email = "invalid-email",
            Role = RoleType.User
        };

        Action act = () => new Employee(request);

        act.Should().Throw<ArgumentException>().WithMessage("Invalid email format.");
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentException_WhenEmailIsTooLong()
    {
        var request = new AddEmployeeRequestDto
        {
            Name = "Ravi",
            Email = new string('a', 101) + "@company.com",
            Role = RoleType.User
        };

        Action act = () => new Employee(request);

        act.Should().Throw<ArgumentException>().WithMessage("Email must not exceed 100 characters.");
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentException_WhenRoleIsInvalid()
    {
        var request = new AddEmployeeRequestDto
        {
            Name = "Ravi",
            Email = "ravi@company.com",
            Role = (RoleType)99 // Invalid role
        };

        Action act = () => new Employee(request);

        act.Should().Throw<ArgumentException>().WithMessage("Invalid role type. Valid values are User(0) or Admin(1).");
    }

    [Fact]
    public void UpdateEmployee_ShouldUpdateValues_WhenValidRequestProvided()
    {
        var initialRequest = new AddEmployeeRequestDto
        {
            Name = "Ravi",
            Email = "ravi@company.com",
            Role = RoleType.User
        };

        var updateRequest = new UpdateEmployeeRequestDto
        {
            Name = "Arjun",
            Email = "arjun@company.com",
            Role = RoleType.Admin
        };

        var employee = new Employee(initialRequest);
        employee.UpdateEmployee(updateRequest);

        employee.Name.Should().Be("Arjun");
        employee.Email.Should().Be("arjun@company.com");
        employee.Role.Should().Be(RoleType.Admin);
    }

    [Fact]
    public void UpdateEmployee_ShouldThrowArgumentException_WhenNameIsInvalid()
    {
        var initialRequest = new AddEmployeeRequestDto
        {
            Name = "Ravi",
            Email = "ravi@company.com",
            Role = RoleType.User
        };

        var updateRequest = new UpdateEmployeeRequestDto
        {
            Name = "",
            Email = "arjun@company.com",
            Role = RoleType.Admin
        };

        var employee = new Employee(initialRequest);

        Action act = () => employee.UpdateEmployee(updateRequest);

        act.Should().Throw<ArgumentException>().WithMessage("Name cannot be blank.");
    }

    [Fact]
    public void UpdateEmployee_ShouldThrowArgumentException_WhenEmailIsInvalid()
    {
        var initialRequest = new AddEmployeeRequestDto
        {
            Name = "Ravi",
            Email = "ravi@company.com",
            Role = RoleType.User
        };

        var updateRequest = new UpdateEmployeeRequestDto
        {
            Name = "Arjun",
            Email = "invalid-email",
            Role = RoleType.Admin
        };

        var employee = new Employee(initialRequest);

        Action act = () => employee.UpdateEmployee(updateRequest);

        act.Should().Throw<ArgumentException>().WithMessage("Invalid email format.");
    }

    [Fact]
    public void UpdateEmployee_ShouldThrowArgumentException_WhenRoleIsInvalid()
    {
        var initialRequest = new AddEmployeeRequestDto
        {
            Name = "Ravi",
            Email = "ravi@company.com",
            Role = RoleType.User
        };

        var updateRequest = new UpdateEmployeeRequestDto
        {
            Name = "Arjun",
            Email = "arjun@company.com",
            Role = (RoleType)99
        };

        var employee = new Employee(initialRequest);

        Action act = () => employee.UpdateEmployee(updateRequest);

        act.Should().Throw<ArgumentException>().WithMessage("Invalid role type. Valid values are User(0) or Admin(1).");
    }
}

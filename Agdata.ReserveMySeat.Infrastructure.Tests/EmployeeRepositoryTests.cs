using Agdata.ReserveMySeat.Application.Interfaces;
using Agdata.ReserveMySeat.Domain.DTOs;
using Agdata.ReserveMySeat.Domain.Entities;
using Agdata.ReserveMySeat.Domain.Enums;
using Agdata.ReserveMySeat.Infrastructure.Mappings;
using Agdata.ReserveMySeat.Infrastructure.Repositories;
using Agdata.ReserveMySeat.Infrastructure.Tests.Mocks;
using AutoMapper;
using FluentAssertions;

namespace Agdata.ReserveMySeat.Infrastructure.Tests;
public class EmployeeRepositoryTests
{
    private readonly IReserveMySeatDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IEmployeeRepository _employeeRepository;

    public EmployeeRepositoryTests()
    {
        _dbContext = MockDbContext.GetDbContextWithTestData();
        var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfiles>());
        _mapper = config.CreateMapper();
        _employeeRepository = new EmployeeRepository(_mapper, _dbContext);
    }

    [Fact]
    public async Task AddAsync_ShouldAddEmployee()
    {
        var addEmployeeRequest = new AddEmployeeRequestDto
        {
            Name = "Aarav",
            Email = "aarav@company.com",
            Role = RoleType.User
        };

        var newEmployee = new Employee(addEmployeeRequest);

        var result = await _employeeRepository.AddAsync(newEmployee);

        result.Should().NotBeNull();
        result.Name.Should().Be("Aarav");
        result.Email.Should().Be("aarav@company.com");
        result.Role.Should().Be(RoleType.User);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllEmployees()
    {
        var employees = await _employeeRepository.GetAllAsync();
        employees.Should().NotBeNull();
        employees.Should().HaveCountGreaterThan(0);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnEmployee_WhenEmployeeExists()
    {
        var result = await _employeeRepository.GetByIdAsync(1);

        result.Should().NotBeNull();
        result.Name.Should().NotBeNullOrWhiteSpace();
        result.Email.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenEmployeeDoesNotExist()
    {
        var result = await _employeeRepository.GetByIdAsync(999); // Non-existent ID
        result.Should().BeNull();
    }   

    [Fact]
    public async Task GetByEmailAsync_ShouldReturnNull_WhenEmailDoesNotExist()
    {
        var result = await _employeeRepository.GetByEmailAsync("nonexistent@example.com");

        result.Should().BeNull();
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateEmployee()
    {
        string mail = "sharma@comapny.com";
        var existingEmployee = await _employeeRepository.GetByIdAsync(1,mail);
        existingEmployee.Should().NotBeNull();

        var updateEmployeeRequest = new UpdateEmployeeRequestDto
        {
            EmployeeId = 1,
            Name = "Sharma",
            Email = "sharma@example.com",
            Role = RoleType.Admin
        };

        existingEmployee.UpdateEmployee(updateEmployeeRequest);

        var result = await _employeeRepository.UpdateAsync(existingEmployee);

        result.Should().BeTrue();

        var updatedEmployee = await _employeeRepository.GetByIdAsync(1);
        updatedEmployee.Name.Should().Be("Sharma");
        updatedEmployee.Email.Should().Be("sharma@example.com");
        updatedEmployee.Role.Should().Be(RoleType.Admin);
    }

    [Fact]
    public async Task RemoveAsync_ShouldRemoveEmployee()
    {
        string mail = "sharma@example.com";
        var existingEmployee = await _employeeRepository.GetByIdAsync(1,mail);
        existingEmployee.Should().NotBeNull();
        var result = await _employeeRepository.RemoveAsync(existingEmployee);
        result.Should().BeTrue();
        var deletedEmployee = await _employeeRepository.GetByIdAsync(1);
        deletedEmployee.Should().BeNull();
    }   

}

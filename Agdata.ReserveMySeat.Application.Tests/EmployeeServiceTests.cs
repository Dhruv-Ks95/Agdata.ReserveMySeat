using Agdata.ReserveMySeat.Application.Interfaces;
using Agdata.ReserveMySeat.Application.Services;
using Agdata.ReserveMySeat.Application.Tests.Mocks;
using Agdata.ReserveMySeat.Domain.DTOs;
using Agdata.ReserveMySeat.Infrastructure.Mappings;
using Agdata.ReserveMySeat.Infrastructure.Repositories;
using AutoMapper;
using FluentAssertions;

namespace Agdata.ReserveMySeat.Application.Tests;

public class EmployeeServiceTests
{
    private readonly IReserveMySeatDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly EmployeeRepository _employeeRepository;
    private readonly EmployeeService _employeeService;

    public EmployeeServiceTests()
    {
        _dbContext = MockDbContext.GetDbContextWithTestData();
        var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfiles>());
        _mapper = config.CreateMapper();
        _employeeRepository = new EmployeeRepository(_mapper, _dbContext);
        _employeeService = new EmployeeService(_employeeRepository);
    }

    [Fact]
    public async Task AddAsync_ShouldAddEmployee_WhenValidRequest()
    {
        var validRequest = new AddEmployeeRequestDto { Name = "chris", Email = "chris@company.com" };
        var result = await _employeeService.AddAsync(validRequest);
        result.Name.Should().Be("chris");
        result.Email.Should().Be("chris@company.com");
    }

    [Fact]
    public async Task AddAsync_ShouldThrowArgumentNullException_WhenRequestIsNull()
    {
        AddEmployeeRequestDto nullRequest = null;
        var act = async () => await _employeeService.AddAsync(nullRequest);
        await act.Should().ThrowAsync<ArgumentNullException>().WithMessage("AddEmployeeRequestDto cannot be null. (Parameter 'addEmployeeRequestDto')");
    }

    [Fact]
    public async Task AddAsync_ShouldThrowArgumentException_WhenEmailIsDuplicate()
    {
        var duplicateEmailRequest = new AddEmployeeRequestDto { Name = "akshat", Email = "dhruv@company.com" };
        var act = async () => await _employeeService.AddAsync(duplicateEmailRequest);
        await act.Should().ThrowAsync<ArgumentException>().WithMessage("The email 'dhruv@company.com' is already in use.");
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllEmployees()
    {
        var result = await _employeeService.GetAllAsync();
        result.Should().NotBeNullOrEmpty();
        result.Count().Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnEmployee_WhenValidId()
    {
        var result = await _employeeService.GetByIdAsync(3);
        result.Should().NotBeNull();
        result.EmployeeId.Should().Be(3);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldThrowArgumentException_WhenIdIsInvalid()
    {
        var act = async () => await _employeeService.GetByIdAsync(0);
        await act.Should().ThrowAsync<ArgumentException>().WithMessage("Invalid EmployeeId");
    }

    [Fact]
    public async Task GetByIdAsync_ShouldThrowKeyNotFoundException_WhenEmployeeDoesNotExist()
    {
        var act = async () => await _employeeService.GetByIdAsync(999); 
        await act.Should().ThrowAsync<KeyNotFoundException>().WithMessage("Employee with EmployeeId: 999 not Found!");
    }

    [Fact]
    public async Task GetByEmailAsync_ShouldReturnEmployee_WhenValidEmail()
    {
        var result = await _employeeService.GetByEmailAsync("dhruv@company.com");
        result.Should().NotBeNull();
        result.Email.Should().Be("dhruv@company.com");
    }

    [Fact]
    public async Task GetByEmailAsync_ShouldThrowArgumentException_WhenEmailIsInvalid()
    {
        var act = async () => await _employeeService.GetByEmailAsync("invalid-email");
        await act.Should().ThrowAsync<ArgumentException>().WithMessage("Invalid email format.");
    }

    [Fact]
    public async Task GetByEmailAsync_ShouldThrowKeyNotFoundException_WhenEmployeeDoesNotExist()
    {
        var act = async () => await _employeeService.GetByEmailAsync("nonexistent@company.com"); 
        await act.Should().ThrowAsync<KeyNotFoundException>().WithMessage("No Employee found with email - nonexistent@company.com!");
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateEmployee_WhenValidRequest()
    {
        var validRequest = new UpdateEmployeeRequestDto { EmployeeId = 1, Name = "Updated Name", Email = "updated@company.com" };
        var result = await _employeeService.UpdateAsync(validRequest);
        result.Should().BeTrue();
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrowArgumentNullException_WhenRequestIsNull()
    {
        UpdateEmployeeRequestDto nullRequest = null;
        var act = async () => await _employeeService.UpdateAsync(nullRequest);
        await act.Should().ThrowAsync<ArgumentNullException>().WithMessage("UpdateEmployeeRequestDto cannot be null. (Parameter 'updateEmployeeRequestDto')");
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrowArgumentException_WhenEmployeeIdIsInvalid()
    {
        var invalidRequest = new UpdateEmployeeRequestDto { EmployeeId = 0, Name = "Name", Email = "valid.email@example.com" };
        var act = async () => await _employeeService.UpdateAsync(invalidRequest);
        await act.Should().ThrowAsync<ArgumentException>().WithMessage("Invalid EmployeeId");
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrowKeyNotFoundException_WhenEmployeeNotFound()
    {
        var nonExistentRequest = new UpdateEmployeeRequestDto { EmployeeId = 999, Name = "Nonexistent", Email = "nonexistent.email@example.com" };
        var act = async () => await _employeeService.UpdateAsync(nonExistentRequest);
        await act.Should().ThrowAsync<KeyNotFoundException>().WithMessage("Employee with EmployeeId999 not Found!");
    }    

    [Fact]
    public async Task RemoveAsync_ShouldRemoveEmployee_WhenValidRequest()
    {
        var validRequest = new RemoveEmployeeRequestDto { EmployeeId = 1, Email = "valid.email@example.com" };
        var result = await _employeeService.RemoveAsync(validRequest);
        result.Should().BeTrue();
    }

    [Fact]
    public async Task RemoveAsync_ShouldThrowArgumentException_WhenEmployeeIdIsInvalid()
    {
        var invalidRequest = new RemoveEmployeeRequestDto { EmployeeId = 0, Email = "valid.email@example.com" };
        var act = async () => await _employeeService.RemoveAsync(invalidRequest);
        await act.Should().ThrowAsync<ArgumentException>().WithMessage("Invalid EmployeeId. It must be greater than 0.");
    }

    [Fact]
    public async Task RemoveAsync_ShouldThrowArgumentException_WhenEmailIsInvalid()
    {
        var invalidEmailRequest = new RemoveEmployeeRequestDto { EmployeeId = 1, Email = "invalid-email" };
        var act = async () => await _employeeService.RemoveAsync(invalidEmailRequest);
        await act.Should().ThrowAsync<ArgumentException>().WithMessage("Invalid email format.");
    }

    [Fact]
    public async Task RemoveAsync_ShouldThrowKeyNotFoundException_WhenEmployeeNotFound()
    {
        var nonExistentRequest = new RemoveEmployeeRequestDto { EmployeeId = 999, Email = "nonexistent@company.com" }; 
        var act = async () => await _employeeService.RemoveAsync(nonExistentRequest);
        await act.Should().ThrowAsync<KeyNotFoundException>().WithMessage("Employee with EmployeeId999 not Found!");
    }
}

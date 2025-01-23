using Agdata.ReserveMySeat.Application.Interfaces;
using Agdata.ReserveMySeat.Domain.DTOs;
using Agdata.ReserveMySeat.Domain.Enums;
using Agdata.ReserveMySeat.Presentation.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Agdata.ReserveMySeat.Presentation.Tests;
public class EmployeeControllerTests
{
    private readonly Mock<IEmployeeService> _mockEmployeeService;
    private readonly EmployeeController _controller;

    public EmployeeControllerTests()
    {
        _mockEmployeeService = new Mock<IEmployeeService>();
        _controller = new EmployeeController(_mockEmployeeService.Object);
    }

    [Fact]
    public async Task AddAsync_ShouldReturnOk_WhenValidRequest()
    {
        var addEmployeeRequest = new AddEmployeeRequestDto
        {
            Name = "Ravi",
            Email = "ravi@company.com",
            Role = RoleType.User
        };

        var employeeDto = new EmployeeDto
        {
            EmployeeId = 1,
            Name = "Ravi",
            Email = "ravi@company.com",
            Role = RoleType.User
        };

        _mockEmployeeService.Setup(s => s.AddAsync(addEmployeeRequest)).ReturnsAsync(employeeDto);

        var result = await _controller.AddAsync(addEmployeeRequest) as OkObjectResult;

        result.Should().NotBeNull();
        result.StatusCode.Should().Be(200);
        result.Value.Should().BeEquivalentTo(employeeDto);
    }

    [Fact]
    public async Task AddAsync_ShouldReturnBadRequest_WhenArgumentNullExceptionThrown()
    {
        var addEmployeeRequest = new AddEmployeeRequestDto
        {
            Name = "Ravi",
            Email = "ravi@company.com",
            Role = RoleType.User
        };

        _mockEmployeeService.Setup(s => s.AddAsync(addEmployeeRequest)).ThrowsAsync(new ArgumentNullException());

        var result = await _controller.AddAsync(addEmployeeRequest) as BadRequestObjectResult;

        result.Should().NotBeNull();
        result.StatusCode.Should().Be(400);
        result.Value.Should().Be("Request data is null or incomplete.");
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnOk_WhenValidId()
    {
        var employeeId = 1;
        var employeeDto = new EmployeeDto
        {
            EmployeeId = employeeId,
            Name = "Ravi",
            Email = "ravi@company.com",
            Role = RoleType.User
        };

        _mockEmployeeService.Setup(s => s.GetByIdAsync(employeeId)).ReturnsAsync(employeeDto);

        var result = await _controller.GetByIdAsync(employeeId) as OkObjectResult;

        result.Should().NotBeNull();
        result.StatusCode.Should().Be(200);
        result.Value.Should().BeEquivalentTo(employeeDto);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNotFound_WhenKeyNotFoundExceptionThrown()
    {
        var employeeId = 1;

        _mockEmployeeService.Setup(s => s.GetByIdAsync(employeeId)).ThrowsAsync(new KeyNotFoundException());

        var result = await _controller.GetByIdAsync(employeeId) as NotFoundObjectResult;

        result.Should().NotBeNull();
        result.StatusCode.Should().Be(404);
        result.Value.Should().Be("Employee not found with the provided Id.");
    }

    [Fact]
    public async Task GetByEmailAsync_ShouldReturnOk_WhenValidEmail()
    {
        var email = "ravi@company.com";
        var employeeDto = new EmployeeDto
        {
            EmployeeId = 1,
            Name = "Ravi",
            Email = email,
            Role = RoleType.User
        };

        _mockEmployeeService.Setup(s => s.GetByEmailAsync(email)).ReturnsAsync(employeeDto);

        var result = await _controller.GetByEmailAsync(email) as OkObjectResult;

        result.Should().NotBeNull();
        result.StatusCode.Should().Be(200);
        result.Value.Should().BeEquivalentTo(employeeDto);
    }

    [Fact]
    public async Task GetByEmailAsync_ShouldReturnNotFound_WhenKeyNotFoundExceptionThrown()
    {
        var email = "nonexistent@company.com";

        _mockEmployeeService.Setup(s => s.GetByEmailAsync(email)).ThrowsAsync(new KeyNotFoundException());

        var result = await _controller.GetByEmailAsync(email) as NotFoundObjectResult;

        result.Should().NotBeNull();
        result.StatusCode.Should().Be(404);
        result.Value.Should().Be("No employee found with the provided email.");
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnOk_WhenValidRequest()
    {
        var updateRequest = new UpdateEmployeeRequestDto
        {
            EmployeeId = 1,
            Name = "Ravi Kumar",
            Email = "ravi.kumar@company.com",
            Role = RoleType.Admin
        };

        _mockEmployeeService.Setup(s => s.UpdateAsync(updateRequest)).ReturnsAsync(true);

        var result = await _controller.UpdateAsync(updateRequest) as OkObjectResult;

        result.Should().NotBeNull();
        result.StatusCode.Should().Be(200);
        result.Value.Should().Be("Successfully updated!");
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnNotFound_WhenKeyNotFoundExceptionThrown()
    {
        var updateRequest = new UpdateEmployeeRequestDto
        {
            EmployeeId = 1,
            Name = "Ravi Kumar",
            Email = "ravi.kumar@company.com",
            Role = RoleType.Admin
        };

        _mockEmployeeService.Setup(s => s.UpdateAsync(updateRequest)).ThrowsAsync(new KeyNotFoundException());

        var result = await _controller.UpdateAsync(updateRequest) as NotFoundObjectResult;

        result.Should().NotBeNull();
        result.StatusCode.Should().Be(404);
        result.Value.Should().Be("Employee not found with the provided Id.");
    }

    [Fact]
    public async Task RemoveAsync_ShouldReturnOk_WhenValidRequest()
    {
        var removeRequest = new RemoveEmployeeRequestDto
        {
            EmployeeId = 1
        };

        _mockEmployeeService.Setup(s => s.RemoveAsync(removeRequest)).ReturnsAsync(true);

        var result = await _controller.RemoveAsync(removeRequest) as OkObjectResult;

        result.Should().NotBeNull();
        result.StatusCode.Should().Be(200);
        result.Value.Should().Be("Successfully deleted!");
    }

    [Fact]
    public async Task RemoveAsync_ShouldReturnNotFound_WhenKeyNotFoundExceptionThrown()
    {
        var removeRequest = new RemoveEmployeeRequestDto
        {
            EmployeeId = 1
        };

        _mockEmployeeService.Setup(s => s.RemoveAsync(removeRequest)).ThrowsAsync(new KeyNotFoundException());

        var result = await _controller.RemoveAsync(removeRequest) as NotFoundObjectResult;

        result.Should().NotBeNull();
        result.StatusCode.Should().Be(404);
        result.Value.Should().Be("Employee not found with the provided Id.");
    }
}


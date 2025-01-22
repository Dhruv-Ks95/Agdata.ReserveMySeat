using Agdata.ReserveMySeat.Application.Interfaces;
using Agdata.ReserveMySeat.Domain.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Agdata.ReserveMySeat.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EmployeeController : ControllerBase
{
    IEmployeeService employeeService;
    public EmployeeController(IEmployeeService employeeService)
    {
        this.employeeService = employeeService;
    }

    [HttpPost]
    [Route("add")]
    public async Task<IActionResult> AddAsync([FromBody] AddEmployeeRequestDto addEmployeeRequestDto)
    {
        try
        {
            var res = await employeeService.AddAsync(addEmployeeRequestDto);
            return Ok(res);
        }
        catch (ArgumentNullException ex)
        {
            Console.WriteLine(ex.Message);
            return BadRequest("Request data is null or incomplete.");
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine(ex.Message);
            return BadRequest("Invalid Details");
        }
    }

    [HttpGet]
    [Route("getById/{id:int}")]
    public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
    {
        try
        {
            var res = await employeeService.GetByIdAsync(id);
            return Ok(res);
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine(ex.Message);
            return BadRequest("Invalid Id");
        }
        catch (KeyNotFoundException ex)
        {
            Console.WriteLine(ex.Message);
            return NotFound("Employee not found with the provided Id.");
        }
    }

    [HttpGet]
    [Route("getByEmail/{email}")]
    public async Task<IActionResult> GetByEmailAsync([FromRoute] string email)
    {
        try
        {
            var res = await employeeService.GetByEmailAsync(email);
            return Ok(res);
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine(ex.Message);
            return BadRequest($"Invalid email: {ex.Message}");
        }
        catch (KeyNotFoundException ex)
        {
            Console.WriteLine(ex.Message);
            return NotFound("No employee found with the provided email.");
        }
    }

    [HttpGet]
    [Route("getAll")]
    public async Task<IActionResult> GetAllAsync()
    {
        var res = await employeeService.GetAllAsync();
        return Ok(res);        
    }

    [HttpPut]
    [Route("update")]
    public async Task<IActionResult> UpdateAsync([FromBody] UpdateEmployeeRequestDto updateEmployeeRequestDto)
    {
        try
        {
            var res = await employeeService.UpdateAsync(updateEmployeeRequestDto);
            if (res)
            {
                return Ok("Successfully updated!");
            }
            else
            {
                return BadRequest("Details could not be updated.");
            }
        }
        catch (ArgumentNullException ex)
        {
            Console.WriteLine(ex.Message);
            return BadRequest("Request data is null or incomplete.");
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine(ex.Message);
            return BadRequest("Invalid details");
        }
        catch (KeyNotFoundException ex)
        {
            Console.WriteLine(ex.Message);
            return NotFound("Employee not found with the provided Id.");
        }
    }

    [HttpDelete]
    [Route("delete")]
    public async Task<IActionResult> RemoveAsync([FromBody] RemoveEmployeeRequestDto removeEmployeeRequestDto)
    {
        try
        {
            var res = await employeeService.RemoveAsync(removeEmployeeRequestDto);
            if (res)
            {
                return Ok("Successfully deleted!");
            }
            else
            {
                return BadRequest("Employee could not be deleted.");
            }
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine(ex.Message);
            return BadRequest($"Invalid data: {ex.Message}");
        }
        catch (KeyNotFoundException ex)
        {
            Console.WriteLine(ex.Message);
            return NotFound("Employee not found with the provided Id.");
        }
    }

}

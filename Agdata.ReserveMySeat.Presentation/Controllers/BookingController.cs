using Agdata.ReserveMySeat.Application.Interfaces;
using Agdata.ReserveMySeat.Application.Services;
using Agdata.ReserveMySeat.Domain.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Agdata.ReserveMySeat.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BookingController : ControllerBase
{
    private readonly IBookingService bookingService;

    public BookingController(IBookingService bookingService)
    {
        this.bookingService = bookingService;
    }

    [HttpPost]
    [Route("add")]
    public async Task<IActionResult> AddAsync([FromBody] AddBookingRequestDto addBookingRequestDto)
    {
        try
        {
            var res = await bookingService.AddAsync(addBookingRequestDto);
            return Ok(res);
        }
        catch (ArgumentNullException ex)
        {
            Console.WriteLine(ex.Message, ex.StackTrace);
            return BadRequest("Booking request cannot be null.");
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine(ex.Message, ex.StackTrace);
            return BadRequest("Invalid Details");
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine(ex.Message, ex.StackTrace);
            return Conflict("Selected seat is not available");
        }
        catch (KeyNotFoundException ex)
        {
            Console.WriteLine(ex.Message, ex.StackTrace);
            return NotFound("Non existent employee/seat");
        }
    }

    [HttpGet]
    [Route("getById/{id:int}")]
    public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
    {
        try
        {
            var res = await bookingService.GetByIdAsync(id);
            return Ok(res);
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine(ex.Message, ex.StackTrace);
            return BadRequest("Invalid BookingId");
        }
        catch (KeyNotFoundException ex)
        {
            Console.WriteLine(ex.Message, ex.StackTrace);
            return NotFound("Booking not found with provided BookingId");
        }
    }

    [HttpGet]
    [Route("getByDate/{date}")]
    public async Task<IActionResult> GetByDateAsync(DateTime date)
    {
        try
        {
            var res = await bookingService.GetByDateAsync(date);
            return Ok(res);
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine(ex.Message, ex.StackTrace);
            return BadRequest("Invalid Date");
        }
    }

    [HttpGet]
    [Route("getByUserId/{id:int}")]
    public async Task<IActionResult> GetByUserIdAsync([FromRoute] int id)
    {
        try
        {
            var res = await bookingService.GetByUserIdAsync(id);
            return Ok(res);
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine(ex.Message, ex.StackTrace);
            return BadRequest("Invalid UserId");
        }
        catch (KeyNotFoundException ex)
        {
            Console.WriteLine(ex.Message, ex.StackTrace);
            return NotFound("No User found with provided UserId");
        }
    }

    [HttpGet]
    [Route("getMonthly")]
    public async Task<IActionResult> GetMonthlyAsync()
    {
        var res = await bookingService.GetMonthlyAsync();
        return Ok(res);
    }

    [HttpPut]
    [Route("update")]
    public async Task<IActionResult> UpdateAsync([FromBody] UpdateBookingRequestDto updateBookingRequestDto)
    {
        try
        {
            var res = await bookingService.UpdateAsync(updateBookingRequestDto);
            return Ok(res);
        }
        catch (ArgumentNullException ex)
        {
            Console.WriteLine(ex.Message, ex.StackTrace);
            return BadRequest("Update request cannot be null.");
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine(ex.Message, ex.StackTrace);
            return BadRequest("Invalid details provided");
        }
        catch (KeyNotFoundException ex)
        {
            Console.WriteLine(ex.Message, ex.StackTrace);
            return NotFound("No booking found with provided BookingId");
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine(ex.Message, ex.StackTrace);
            return BadRequest("Selected seat is not available!");
        }
    }

    [HttpDelete]
    [Route("delete")]
    public async Task<IActionResult> RemoveAsync([FromBody] RemoveBookingRequestDto removeBookingRequestDto)
    {
        try
        {
            var res = await bookingService.RemoveAsync(removeBookingRequestDto);
            return Ok(res);
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine(ex.Message, ex.StackTrace);
            return BadRequest("Invalid BookingId");
        }
        catch (KeyNotFoundException ex)
        {
            Console.WriteLine(ex.Message, ex.StackTrace);
            return NotFound("No booking found with provided booking Id");
        }
    }
}

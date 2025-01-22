using Agdata.ReserveMySeat.Application.Interfaces;
using Agdata.ReserveMySeat.Domain.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Agdata.ReserveMySeat.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeatController : ControllerBase
    {
        private readonly ISeatService seatService;
        public SeatController(ISeatService seatService)
        {
            this.seatService = seatService;
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> AddAsync([FromBody] AddSeatRequestDto addSeatRequestDto)
        {
            try
            {
                var res = await seatService.AddAsync(addSeatRequestDto);
                return Ok(res);
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest("Request body cannot be null!");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest("Invalid seat number provided!");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex.Message);
                return Conflict("Seat number already exists!");
            }
        }

        [HttpGet]
        [Route("getById/{id:int}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
        {
            try
            {
                var res = await seatService.GetByIdAsync(id);
                return Ok(res);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest("Invalid seat ID provided!");
            }
            catch (KeyNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound("Seat not found with the given ID!");
            }
        }

        [HttpGet]
        [Route("getAll")]
        public async Task<IActionResult> GetAllAsync()
        {
            var res = await seatService.GetAllAsync();
            return Ok(res);
        }

        [HttpGet]
        [Route("getAvailableByDate/{date}")]
        public async Task<IActionResult> GetAvailableCountOnDateAsync([FromRoute] DateTime date)
        {
            try
            {
                var res = await seatService.GetAvailableCountOnDateAsync(date);
                return Ok(res);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest("Invalid date provided!");
            }
        }

        [HttpPut]
        [Route("update")]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateSeatRequestDto updateSeatRequestDto)
        {
            try
            {
                var res = await seatService.UpdateAsync(updateSeatRequestDto);
                return Ok(res ? "Seat successfully updated!" : "Failed to update the seat!");
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest("Request body cannot be null!");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest("Invalid seat details provided!");
            }
            catch (KeyNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound("Seat not found with the given ID!");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex.Message);
                return Conflict("Seat number already exists for another seat!");
            }
        }

        [HttpDelete]
        [Route("delete")]
        public async Task<IActionResult> RemoveAsync([FromBody] RemoveSeatRequestDto removeSeatRequestDto)
        {
            try
            {
                var res = await seatService.RemoveAsync(removeSeatRequestDto);
                return Ok(res ? "Seat successfully removed!" : "Failed to remove the seat!");
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest("Request body cannot be null!");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest("Invalid seat details provided!");
            }
            catch (KeyNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound("Seat not found with the given ID!");
            }
        }

    }
}

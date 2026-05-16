using Microsoft.AspNetCore.Mvc;
using CineReserve.API.DTOs;
using CineReserve.API.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace CineReserve.API.Controllers
{
    [ApiController]
    [Route("api/bookings")]
    public class BookingController : ControllerBase
    {
        private readonly BookingService _service;

        public BookingController(BookingService service)
        {
            _service = service;
        }

        // ================= CREATE BOOKING =================
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Book(BookingRequestDto request)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userIdClaim == null)
                return Unauthorized("Invalid token");

            request.UserId = int.Parse(userIdClaim);

            var result = await _service.BookSeats(request);

            return Ok(new { bookingRef = result });
        }

        // ================= GET MY BOOKINGS =================
        [HttpGet("my")]
        [Authorize]
        public async Task<IActionResult> GetMyBookings()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var result = await _service.GetBookingsByUser(userId);

            return Ok(result);
        }

        // ================= GET ALL BOOKINGS (ADMIN) =================
        [HttpGet("all-bookings")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllBookingsForStaff()
        {
            // Fixed variable name from _bookingService to _service
            var adminList = await _service.GetAllBookings();
            return Ok(adminList);
        }

        // ================= GET BY ID =================
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetBookingById(id);

            if (result == null)
                return NotFound("Booking not found");

            return Ok(result);
        }

        // ================= CANCEL BOOKING =================
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> CancelBooking(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var result = await _service.CancelBooking(id, userId);

            if (!result)
                return BadRequest("Cannot cancel booking");

            return Ok("Booking cancelled successfully");
        }
    }
}
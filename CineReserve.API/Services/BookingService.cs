using CineReserve.API.Data;
using CineReserve.API.DTOs;
using CineReserve.API.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace CineReserve.API.Services
{
    public class BookingService
    {
        private readonly AppDbContext _context;

        public BookingService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<string> BookSeats(BookingRequestDto request)
        {
            using var transaction = await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable);

            try
            {
                var bookedSeats = await _context.TicketDetails
                    .Where(t => t.ShowtimeId == request.ShowtimeId)
                    .ToListAsync();

                foreach (var seat in request.Seats)
                {
                    if (bookedSeats.Any(b => b.RowNumber == seat.Row && b.SeatNumber == seat.Number))
                    {
                        throw new Exception("Conflict: One or more selected seats are already reserved.");
                    }
                }

                var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == request.UserId);
                if (user == null)
                    throw new Exception("User account not found.");

                decimal totalCost = 0;
                foreach (var seat in request.Seats)
                {
                    bool isVIP = seat.Row == "G" || seat.Row == "H";
                    totalCost += isVIP ? 200 : 120;
                }

                if (user.CreditBalance < totalCost)
                {
                    throw new Exception($"Insufficient balance. Total: ₹{totalCost}, Balance: ₹{user.CreditBalance}");
                }

                user.CreditBalance -= totalCost;

                var booking = new Booking
                {
                    UserId = request.UserId,
                    ShowtimeId = request.ShowtimeId,
                    TotalAmount = totalCost,
                    BookingReference = Guid.NewGuid().ToString().Substring(0, 8).ToUpper()
                };

                _context.Bookings.Add(booking);
                await _context.SaveChangesAsync();

                foreach (var seat in request.Seats)
                {
                    _context.TicketDetails.Add(new TicketDetail
                    {
                        BookingId = booking.BookingId,
                        ShowtimeId = request.ShowtimeId,
                        RowNumber = seat.Row,
                        SeatNumber = seat.Number,
                        Price = (seat.Row == "G" || seat.Row == "H") ? 200 : 120
                    });
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return booking.BookingReference;
            }
            catch (DbUpdateException)
            {
                await transaction.RollbackAsync();
                throw new Exception("Seat Already Reserved. Concurrency lock activated.");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<Booking>> GetBookingsByUser(int userId)
        {
            return await _context.Bookings
                .Include(b => b.TicketDetails)
                .Where(b => b.UserId == userId)
                .ToListAsync();
        }

        public async Task<List<Booking>> GetAllBookings()
        {
            return await _context.Bookings
                .Include(b => b.TicketDetails)
                .ToListAsync();
        }

        public async Task<Booking?> GetBookingById(int id)
        {
            return await _context.Bookings
                .Include(b => b.TicketDetails)
                .FirstOrDefaultAsync(b => b.BookingId == id);
        }

        public async Task<bool> CancelBooking(int bookingId, int userId)
        {
            var booking = await _context.Bookings
                .Include(b => b.TicketDetails)
                .FirstOrDefaultAsync(b => b.BookingId == bookingId);

            if (booking == null)
                return false;

            if (booking.UserId != userId)
                return false;

            _context.TicketDetails.RemoveRange(booking.TicketDetails);
            _context.Bookings.Remove(booking);

            await _context.SaveChangesAsync();

            return true;
        }
    }
}
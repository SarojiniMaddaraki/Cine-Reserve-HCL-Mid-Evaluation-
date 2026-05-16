using System.ComponentModel.DataAnnotations;
namespace CineReserve.API.Models
{
    public class Booking
    {
        [Key]
        public int BookingId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int ShowtimeId { get; set; }

        [Required]
        public decimal TotalAmount { get; set; }

        [Required]
        public required string BookingReference { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public ICollection<TicketDetail> TicketDetails { get; set; }
            = new List<TicketDetail>();
    }
}
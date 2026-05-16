using System.ComponentModel.DataAnnotations;
namespace CineReserve.API.Models
{
    public class TicketDetail
    {
        [Key]
        public int TicketId { get; set; }

        [Required]
        public int BookingId { get; set; }

        [Required]
        public int ShowtimeId { get; set; }

        [Required]
        [StringLength(1)]
        public required string RowNumber { get; set; }

        [Required]
        public int SeatNumber { get; set; }

        [Required]
        public decimal Price { get; set; }
    }
}

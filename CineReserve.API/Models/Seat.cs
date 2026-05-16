using System.ComponentModel.DataAnnotations;

namespace CineReserve.API.Models
{
    public class Seat
    {
        [Key]
        public int SeatId { get; set; }

        [Required]
        [StringLength(1)]
        public required string RowNumber { get; set; }

        [Required]
        public int SeatNumber { get; set; }

        public bool IsVIP { get; set; }
    }
}

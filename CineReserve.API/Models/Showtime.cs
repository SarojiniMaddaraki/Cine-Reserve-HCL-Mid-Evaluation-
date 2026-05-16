using System.ComponentModel.DataAnnotations;

namespace CineReserve.API.Models
{
    public class Showtime
    {
        [Key]
        public int ShowtimeId { get; set; }

        [Required]
        public int MovieId { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Range(50, 1000)]
        public decimal BasePrice { get; set; }
    }
}

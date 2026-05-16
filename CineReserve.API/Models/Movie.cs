using System.ComponentModel.DataAnnotations;

namespace CineReserve.API.Models
{
    public class Movie
    {
        [Key]
        public int MovieId { get; set; }

        [Required]
        [StringLength(100)]
        public required string Title { get; set; }

        [Required]
        public required string Genre { get; set; }

        [Range(30, 300)]
        public int Duration { get; set; }
    }
}

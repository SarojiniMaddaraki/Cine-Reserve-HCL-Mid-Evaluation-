
using System.ComponentModel.DataAnnotations;

namespace CineReserve.API.Models
{

    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [StringLength(100)]
        public required string Name { get; set; }

        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        public required string PasswordHash { get; set; }

        [Required]
        public required string Role { get; set; } // "User" or "Admin"

        public decimal CreditBalance { get; set; }
    }
}

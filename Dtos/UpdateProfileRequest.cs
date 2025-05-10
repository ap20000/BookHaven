using System.ComponentModel.DataAnnotations;

namespace BookHaven.DTOs
{
    public class UpdateProfileRequest
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string? Phone { get; set; }

        [MinLength(6)]
        public string Password { get; set; }
    }
}

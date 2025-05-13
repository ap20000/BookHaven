using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BookHaven.Options
{
    public class JwtOptions
    {
        public const string SectionName = "Jwt";

        [Required]
        public string Issuer { get; set; }

        [Required]
        public string Audience { get; set; }

        [Required]
        [MinLength(32)] // Ensures the secret is at least 32 characters
        public string Secret { get; set; }

        [Range(1, int.MaxValue)]
        public int ExpiryHours { get; set; }
    }
}

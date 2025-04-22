namespace BookHaven.DTOs
{
    public class RegisterRequest
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; } // "Member" or "Admin"
        public string? Phone { get; set; } // For Member only
    }
}

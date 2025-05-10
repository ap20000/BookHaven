namespace BookHaven.DTOs
{
    public class UpdateProfileRequest
    {
        public string Name { get; set; }
        public string? Phone { get; set; } // Only for Members
        public string Password { get; set; }
    }
}

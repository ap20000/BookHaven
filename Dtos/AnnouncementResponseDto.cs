namespace BookHaven.DTOs
{
    public class AnnouncementResponseDto
    {
        public int AnnouncementID { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string ApplicationUserName { get; set; } // Admin name
    }
}

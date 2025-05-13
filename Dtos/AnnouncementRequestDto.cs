namespace BookHaven.DTOs
{
    public class AnnouncementRequestDto
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}

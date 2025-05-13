namespace BookHaven.DTOs
{
    public class ReviewDto
    {
        public int ReviewID { get; set; }
        public string UserID { get; set; }
        public int Rating { get; set; }
        public string Username { get; set; }  // Added for username

        public string Comment { get; set; }
        public DateTime ReviewDate { get; set; }
        public int BookID { get; set; }  // Add this property

    }
}
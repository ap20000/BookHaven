namespace BookHaven.Entities
{
    public class Bookmark
    {
        public int BookmarkID { get; set; }

        public int BookID { get; set; }
        public Book Book { get; set; }

        public int MemberID { get; set; }
        public Member Member { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}

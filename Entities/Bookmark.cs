using BookHaven.Entities;

public class Bookmark
{
    public int BookmarkID { get; set; }

    public int BookID { get; set; }
    public Book Book { get; set; }

    public string ApplicationUserId { get; set; }
    public ApplicationUser ApplicationUser { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

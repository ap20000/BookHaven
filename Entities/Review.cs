using BookHaven.Entities;

public class Review
{
    public int ReviewID { get; set; }

    public string ApplicationUserId { get; set; }
    public ApplicationUser ApplicationUser { get; set; }

    public int BookID { get; set; }
    public Book Book { get; set; }

    public int Rating { get; set; }
    public string Comment { get; set; }
    public DateTime ReviewDate { get; set; }
}

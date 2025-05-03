using BookHaven.Entities;

public class Member : User
{
    public string Phone { get; set; }
    public string MembershipID { get; set; }
    public int TotalOrdersCount { get; set; }

    public ICollection<Order> Orders { get; set; }
    public ICollection<Review> Reviews { get; set; }
    public ICollection<Bookmark> Bookmarks { get; set; }
}

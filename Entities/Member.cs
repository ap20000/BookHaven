public class Member
{
    public int MemberID { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Phone { get; set; }
    public string MembershipID { get; set; }
    public int TotalOrdersCount { get; set; }

    public ICollection<Order> Orders { get; set; }
    public ICollection<Review> Reviews { get; set; }
    public ICollection<Bookmark> Bookmarks { get; set; }
}

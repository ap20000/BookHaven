public class Admin
{
    public int AdminID { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }

    public ICollection<Book> ManagedBooks { get; set; }
    public ICollection<Announcement> Announcements { get; set; }
}

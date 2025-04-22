public class Admin : User
{
    public ICollection<Book> ManagedBooks { get; set; }
    public ICollection<Announcement> Announcements { get; set; }
}

using BookHaven.Entities;

public class Announcement
{
    public int AnnouncementID { get; set; }
    public string Title { get; set; }
    public string Message { get; set; }

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public string ApplicationUserId { get; set; } // Admin User
    public ApplicationUser ApplicationUser { get; set; }
}


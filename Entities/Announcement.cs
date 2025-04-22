public class Announcement
{
    public int AnnouncementID { get; set; }
    public string Title { get; set; }
    public string Message { get; set; }

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public int CreatedBy { get; set; } // AdminID
    public Admin Admin { get; set; }
}

public class Publisher
{
    public int PublisherID { get; set; }
    public string Name { get; set; }

    public ICollection<Book> Books { get; set; }
}
